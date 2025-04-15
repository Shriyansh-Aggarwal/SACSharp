using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SACSharp.Models;

namespace SACSharp.Rules
{
    public class XSSInjection : IRule
    {
        public string Id => "SAST80";
        public string Name => "Cross-Site Scripting (CWE-80)";
        public string Description => "Detects unsafe reflection of user input in HTML or script contexts.";
        public string Resolution => "Always encode output when rendering user input to HTML. Use frameworks or sanitization libraries.";

        public List<Finding> Analyze(SyntaxTree tree, string filePath)
        {
            var findings = new List<Finding>();
            var root = tree.GetRoot();

            var invocationExprs = root.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var invocation in invocationExprs)
            {
                var invocationStr = invocation.ToString().ToLower();

                if (invocationStr.Contains("response.write") || invocationStr.Contains("httpcontext.current.response.write"))
                {
                    if (invocationStr.Contains("request") || invocationStr.Contains("input") || invocationStr.Contains("textbox"))
                    {
                        var line = invocation.GetLocation().GetLineSpan().StartLinePosition.Line + 1;

                        findings.Add(new Finding
                        {
                            RuleId = Id,
                            FilePath = filePath,
                            LineNumber = line,
                            Message = "Potential XSS: Writing raw user input directly to response.",
                            Resolution = Resolution,
                            Severity = Severity.High
                        });
                    }
                }
            }

            return findings;
        }
    }
}
