using Gherkin.Ast;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SBE.Core.OutputGenerators.Html
{
    static class TableService
    {
        internal static bool IsXmlTable(DataTable table)
        {
            return string.Concat(table.Rows.First().Cells.Select(x => x.Value.ToLower())) == "xml";
        }

        internal static void BeginTable(DataTable table, string cssClass, StringBuilder sb)
        {
            sb.AppendLine($"<table class=\"{cssClass}\" styles=\"border: solid 1px #000\">");
            Header(table.Rows.First().Cells.Select(x => x.Value), sb);
            sb.AppendLine("<thead><tr>");

            foreach (var cell in table.Rows.First().Cells)
            {
                sb.Append($"<th>{WebUtility.HtmlEncode(cell.Value)}</th>");
            }

            sb.AppendLine("</tr></thead>");
        }

        internal static void WriteRows(DataTable table, StringBuilder sb)
        {
            var rows = table.Rows.Skip(1).Select(row => row.Cells.Select(cell => WebUtility.HtmlEncode(cell.Value).ForceSpaceIfEmpty()).ToArray()).ToArray();

            sb.AppendLine("<tbody>");
            foreach (var row in rows)
            {
                sb.Append($"<tr>{row.Select(cell => $"<td>{cell}</td>")}</tr>");
            }

            sb.AppendLine("</tbody>");
        }

        internal static void EndTable(StringBuilder sb)
        {
            sb.AppendLine("</table>");
        }

        internal static void Header(IEnumerable<string> headers, StringBuilder sb)
        {
            sb.Append("<thead><tr><th></th>");

            foreach (var header in headers)
            {
                sb.Append($"<th>{header}</th>");
            }

            sb.AppendLine("</tr></thead>");
        }

        internal static string ForceSpaceIfEmpty(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "&nbsp;" : text;
        }
    }
}
