using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SACSharp.Models;

namespace SACSharp.Rules
{
    public class SQLInjection : IRule
    {
        public string Id => "SAST89";
        public string Name => "SQL Injection (CWE-89)";
        public string Description => "Detects code vulnerable to SQL Injection attacks.";
        public string Resolution => "Use parameterized queries or ORM tools to safely construct SQL commands.";

        public List<Finding> Analyze(SyntaxTree tree, string filePath)
        {
            var findings = new List<Finding>();
            var root = tree.GetRoot();

            var binaryExpressions = root.DescendantNodes().OfType<BinaryExpressionSyntax>();

            foreach (var expr in binaryExpressions)
            {
                if (expr.IsKind(SyntaxKind.AddExpression))
                {
                    var left = expr.Left.ToString().ToLower();
                    var right = expr.Right.ToString().ToLower();

                    if ((left.Contains("select") || left.Contains("insert") || left.Contains("update") || left.Contains("delete")) &&
                        (right.Contains("request") || right.Contains("input") || right.Contains("user") || right.Contains("textbox")))
                    {
                        var line = expr.GetLocation().GetLineSpan().StartLinePosition.Line + 1;

                        findings.Add(new Finding
                        {
                            RuleId = Id,
                            FilePath = filePath,
                            LineNumber = line,
                            Message = "Possible SQL Injection: SQL string concatenated with user input.",
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
