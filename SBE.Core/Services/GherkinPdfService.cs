using SBE.Core.Models;
using SBE.Core.OutputGenerators.Html;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SBE.Core.Services
{
    internal sealed class GherkinPdfService
    {
        readonly GherkinHtmlService gherkinHtmlService;
        readonly PdfService pdfService;
        readonly string projectName;
        readonly string branchName;
        readonly string buildNumber;
        readonly OutputFilter filter;

        internal GherkinPdfService(string buildNumber, string projectName, string branchName, OutputFilter filter)
        {
            this.buildNumber = buildNumber;
            this.projectName = projectName;
            this.branchName = branchName;
            this.filter = filter;
            this.gherkinHtmlService = GherkinHtmlService.CreateForPdf();
            pdfService = new PdfService();
        }

        internal byte[] GeneratePdf(SbeEpic[] epics)
        {
            var html = GetHtmlForFeatures(epics);

            SetHeader();
            SetCss();
            RegisterImages();

            return pdfService.RenderPdf(html);
        }

        private void SetHeader()
        {
            pdfService.SetHeader(30, $"<p>{projectName} {branchName} {buildNumber} {DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}</p><hr/>");
            pdfService.SetFooter(30, "<hr/><p>Sida @page@ av @pages@</p>");
        }

        private void RegisterImages()
        {
            pdfService.RegisterImage("glyphicon-remove-sign", "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");
            pdfService.RegisterImage("glyphicon-exclamation-sign", "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");
            pdfService.RegisterImage("glyphicon-ok-sign", "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");
            pdfService.RegisterImage("glyphicon-minus-sign", "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");
            pdfService.RegisterImage("adjust", "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");
        }

        private void SetCss()
        {
            var css = @"* { font-size: 12px; }
            .scenario { padding-top: 20px; }
            .steps { padding-top: 10px; }
            .example { padding-top: 10px; }
            .feature .feature-title img { height:16px; width:16px; }
            .feature .feature-title td.text { font-weight: bold; font-size: 20px; padding-top:6px; }
            .scenario .title img { height:16px; width:16px; }
            .scenario .title td.text { font-weight: bold; font-size: 16px; padding-top:6px; }
            .scenario { page-break-inside:avoid; }
            .example { page-break-inside:avoid; }
            table.examples {
                border-collapse: collapse;
                border-spacing: 0;
                border-top-color: black;
                border-top-width: 1px;
                border-top-style: solid;
                border-left-color: black;
                border-left-width: 1px;
                border-left-style: solid;
            }
            table.examples th, table.table-bordered th, table.table-xml th { 
                font-weight:bold; 
            }
            table.examples th, table.examples td {
                border-collapse: collapse;
                border-spacing: 0;
                border-right-color: black;
                border-right-width: 1px;
                border-right-style: solid;
                border-bottom-color: black;
                border-bottom-width: 1px;
                border-bottom-style: solid;
                padding: 6px 4px 4px 4px;
            }
            table.table-bordered {
                border-collapse: collapse;
                border-spacing: 0;
                border-top-color: black;
                border-top-width: 1px;
                border-top-style: solid;
                border-left-color: black;
                border-left-width: 1px;
                border-left-style: solid;
            }
            table.table-bordered th, table.table-bordered td {
                border-collapse: collapse;
                border-spacing: 0;
                border-right-color: black;
                border-right-width: 1px;
                border-right-style: solid;
                border-bottom-color: black;
                border-bottom-width: 1px;
                border-bottom-style: solid;
                padding: 6px 4px 4px 4px;
            }
            table.table-xml { border: solid 1px black; }
            table.table-xml th {
                border-bottom-color: black;
                border-bottom-width: 1px;
                border-bottom-style: solid; 
            }
            table.table-xml th, table.table-xml td {
                padding-top: 2px;
            }
            table.examples img { height:12px; width:12px; }";

            pdfService.SetCss(css);
        }

        private string GetHtmlForFeatures(SbeEpic[] epics)
        {
            var features = GetFilteredFeatures(epics);

            if (!features.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            sb.Append(gherkinHtmlService.FeatureHtml(features.First()));

            foreach (var feature in features.Skip(1))
            {
                sb.AppendLine("<div style=\"page-break-before:always;\"></div>");
                sb.Append(gherkinHtmlService.FeatureHtml(feature));
            }

            return sb.ToString();
        }

        private SbeFeature[] GetFilteredFeatures(SbeEpic[] epics)
        {
            return epics.SelectMany(e => e.GetFeatures())
                                    .Where(f => filter?.IncludeFeature(f) ?? true)
                                    .ToArray();
        }
    }
}