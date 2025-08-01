using SpecAPI.Models;
using System.Text;

namespace SpecAPI.Services;

public static class ResultExporter
{
    public static string ToMarkdown(List<ApiTestResult> results)
    {
        var sb = new StringBuilder();
        sb.AppendLine("| Test | Status | Time (ms) | Passed |");
        sb.AppendLine("|------|--------|-----------|--------|");
        foreach (var r in results)
        {
            sb.AppendLine($"| {r.Name} | {r.StatusCode} | {r.ResponseTimeMs} | {(r.Passed ? "✅" : "❌")} |");
        }
        return sb.ToString();
    }

    public static string ToHtml(List<ApiTestResult> results)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<table border='1'><tr><th>Test</th><th>Status</th><th>Time (ms)</th><th>Passed</th></tr>");
        foreach (var r in results)
        {
            sb.AppendLine($"<tr><td>{r.Name}</td><td>{r.StatusCode}</td><td>{r.ResponseTimeMs}</td><td>{(r.Passed ? "✅" : "❌")}</td></tr>");
        }
        sb.AppendLine("</table>");
        return sb.ToString();
    }
}
