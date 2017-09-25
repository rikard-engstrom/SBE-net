using Gherkin;
using Gherkin.Ast;
using SBE.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SBE.Core.OutputGenerators.Html
{
    internal sealed class GherkinHtmlService
    {
        readonly string imageTemplate;

        private sealed class ExtendedExamples
        {
            public string Keyword { get; private set; }
            public string[] Headers { get; private set; }
            public ExtendedTableRow[] Rows { get; private set; }

            internal static ExtendedExamples Create(Examples examples)
            {
                var extended = new ExtendedExamples
                {
                    Keyword = examples.Keyword,
                    Headers = examples.TableHeader.Cells.Select(x => x.Value).ToArray(),
                };

                extended.Rows = examples.TableBody.Select(row => ExtendedTableRow.Create(row, extended.Headers)).ToArray();

                return extended;
            }

            internal static void MatchResult(ExtendedExamples examples, ScenarioDefinition scenario, ICollection<SbeScenario> results)
            {
                foreach (var row in examples.Rows)
                {
                    var rowTitle = row.GetTitle(scenario.Name);
                    var result = results.FirstOrDefault(x => x.Title.Equals(rowTitle, StringComparison.OrdinalIgnoreCase));
                    row.Result = result.Outcome;
                }
            }
        }

        private sealed class ExtendedTableRow
        {
            public string[] Values { get; private set; }
            public string[] Headers { get; private set; }
            public Outcome Result { get; internal set; }

            internal static ExtendedTableRow Create(TableRow row, string[] headers)
            {
                var extendedRow = new ExtendedTableRow
                {
                    Values = row.Cells.Select(x => x.Value).ToArray(),
                    Headers = headers
                };

                return extendedRow;
            }

            internal string GetTitle(string name)
            {
                var nameValuePairs = new List<string>();

                for (int i = 0; i < Headers.Length; i++)
                {
                    nameValuePairs.Add(string.Concat(Headers[i].ToLower(), ":", Values[i]));
                }

                return $"{name} ({string.Join(", ", nameValuePairs.ToArray())})";
            }
        }

        internal static GherkinHtmlService CreateForPdf()
        {
            return new GherkinHtmlService("<img src=\"{0}\" />");
        }

        private GherkinHtmlService(string imageTemplate)
        {
            this.imageTemplate = imageTemplate;
        }

        internal string FeatureHtml(SbeFeature feature)
        {
            var parser = new Parser();
            using (var reader = new StringReader(feature.FeatureText))
            {
                var doc = parser.Parse(reader);
                return FeatureHtml(doc, feature);
            }
        }

        internal string FeatureHtml(GherkinDocument document, SbeFeature feature)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<div class=\"feature\">");

            TagsHtml(sb, document.Feature.Tags);

            sb.AppendLine($@"<table class=""feature-title""><tr><td>{GenerateStatusImageTag(feature.GetOutcome())}</td>
                <td class=""text"">{document.Feature.Keyword}: {HtmlEncode(document.Feature.Name)}</td></tr></table>");
            sb.AppendLine($"<p>{AddLineBreaks(HtmlEncode(document.Feature.Description))}</p>");

            GenerateHtmlForParsedFeature(document, feature, sb);

            sb.AppendLine("</div>");

            return sb.ToString();
        }

        private void GenerateHtmlForParsedFeature(GherkinDocument document, SbeFeature feature, StringBuilder sb)
        {
            foreach (var child in document.Feature.Children)
            {
                if (child is Background)
                {
                    BackgroundHtml(child as Background, sb);
                }
                else if (child is ScenarioDefinition)
                {
                    ScenarioHtml(child as ScenarioDefinition, sb, feature.Scenarios);
                }
                else
                {
                    throw new NotSupportedException($"Type {child.GetType().FullName} is not a supported child of a feature.");
                }
            }
        }

        private static void TagsHtml(StringBuilder sb, IEnumerable<Tag> tags)
        {
        }

        private static void BackgroundHtml(Background background, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"background\">");
            sb.AppendLine($"{background.Keyword}:<br/>");
            sb.AppendLine($"<p>{AddLineBreaks(HtmlEncode(background.Description))}</p>");
            StepsHtml(sb, background.Steps);
            sb.AppendLine("</div>");
        }

        private void ScenarioHtml(ScenarioDefinition scenario, StringBuilder sb, ICollection<SbeScenario> results)
        {
            var result = results.FirstOrDefault(x => x.Title == scenario.Name)?.Outcome ?? Outcome.Inconclusive;

            var outline = scenario as ScenarioOutline;
            var examples = outline?.Examples.Select(ExtendedExamples.Create).ToArray();
            examples?.ToList().ForEach(x => ExtendedExamples.MatchResult(x, scenario, results));

            if (outline != null)
            {
                result = GetOutlineResultFromExamples(examples);
            }

            sb.AppendLine("<div class=\"scenario\">");
            sb.AppendLine($"<table class=\"title\"><tr><td>{GenerateStatusImageTag(result)}</td><td class=\"text\">{scenario.Keyword}: {HtmlEncode(scenario.Name)}</td></tr></table>");
            sb.AppendLine($"<p>{AddLineBreaks(HtmlEncode(scenario.Description))}</p>");

            StepsHtml(sb, scenario.Steps);

            if (outline != null)
            {
                ExamplesHtml(sb, examples);
            }

            sb.AppendLine("</div>");
        }

        private static Outcome GetOutlineResultFromExamples(ExtendedExamples[] examples)
        {
            var results = examples.SelectMany(x => x.Rows.Select(r => r.Result)).Distinct().ToArray();
            return OutcomeService.CombineOutcomes(results);
        }

        private string GenerateStatusImageTag(Outcome status)
        {
            string statusString = null;

            switch (status)
            {
                case Outcome.Passed:
                    statusString = "glyphicon-ok-sign";
                    break;
                case Outcome.Failed:
                    statusString = "glyphicon-exclamation-sign";
                    break;
                case Outcome.PartlyPassed:
                    statusString = "glyphicon-adjust";
                    break;
                default:
                    statusString = "glyphicon-minus-sign";
                    break;
            }

            return string.Format(imageTemplate, statusString);
        }

        private static void StepsHtml(StringBuilder sb, IEnumerable<Step> steps)
        {
            sb.AppendLine("<p class=\"steps\">");

            foreach (var step in steps)
            {
                StepHtml(sb, step);
            }

            sb.AppendLine("</p>");
        }

        private static void StepHtml(StringBuilder sb, Step step)
        {
            sb.AppendLine($"{step.Keyword} {HtmlEncode(step.Text)}<br/>");
            if (step.Argument is DataTable)
            {
                GenerateStepArgumentTable(sb, step.Argument as DataTable);
            }
        }

        private static void GenerateStepArgumentTable(StringBuilder sb, DataTable table)
        {
            var cssClass = TableService.IsXmlTable(table) ? "table-xml" : "table-bordered";
            TableService.BeginTable(table, cssClass, sb);
            TableService.WriteRows(table, sb);
            TableService.EndTable(sb);
        }

        private void ExamplesHtml(StringBuilder sb, ExtendedExamples[] examples)
        {
            foreach (var example in examples)
            {
                sb.AppendLine("<div class=\"example\">");
                sb.AppendLine($"{example.Keyword}:");
                sb.AppendLine("<table class=\"examples\" styles=\"border: solid 1px #000\">");
                TableService.Header(example.Headers, sb);
                GenerateTableBodyHtml(example.Rows, sb);
                sb.AppendLine("</table></div>");
            }
        }

        private void GenerateTableBodyHtml(ExtendedTableRow[] tableBody, StringBuilder sb)
        {
            sb.AppendLine("<tbody>");

            foreach (var row in tableBody)
            {
                sb.Append($"<tr><td>{GenerateStatusImageTag(row.Result)}</td>");

                foreach (var cell in row.Values)
                {
                    if (string.IsNullOrWhiteSpace(cell))
                    {
                        sb.Append($"<td>&nbsp;</td>");
                    }
                    else
                    {
                        sb.Append($"<td>{cell}</td>");
                    }
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody>");
        }

        private static string HtmlEncode(string text)
        {
            return WebUtility.HtmlEncode(text);
        }

        private static string AddLineBreaks(string text)
        {
            return text?.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        }
    }
}