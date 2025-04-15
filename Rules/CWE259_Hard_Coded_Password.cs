using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SACSharp.Models;

namespace SACSharp.Rules
{
    public class HardcodedPassword : IRule
    {
        public string Id => "SAST259";
        public string Name => "Hardcoded Password (CWE-259)";
        public string Description => "Detects hardcoded password assignments.";
        public string Resolution => "Avoid storing passwords directly in code.";

        public List<Finding> Analyze(SyntaxTree tree, string filePath)
        {
            var findings = new List<Finding>();
            var root = tree.GetRoot();

            var variableDeclarations = root.DescendantNodes().OfType<VariableDeclaratorSyntax>();

            foreach (var varDecl in variableDeclarations)
            {
                var name = varDecl.Identifier.Text.ToLower();
                var value = varDecl.Initializer?.Value;

                if ((name.Contains("password") || name.Contains("pwd")) &&
                    value is LiteralExpressionSyntax literal &&
                    literal.IsKind(SyntaxKind.StringLiteralExpression))
                {
                    var line = varDecl.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    findings.Add(new Finding
                    {
                        RuleId = Id,
                        FilePath = filePath,
                        LineNumber = line,
                        Message = $"Hardcoded password in variable '{varDecl.Identifier.Text}'",
                        Resolution = Resolution,
                        Severity = Severity.Critical
                    });
                }
            }

            var literals = root.DescendantNodes().OfType<LiteralExpressionSyntax>();
            foreach (var literal in literals)
            {
                var valueText = literal.Token.ValueText.ToLower();

                if (valueText.Contains("pass") || valueText.Contains("pwd"))
                {
                    var line = literal.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    findings.Add(new Finding
                    {
                        RuleId = Id,
                        FilePath = filePath,
                        LineNumber = line,
                        Message = $"Suspicious hardcoded string: \"{literal.Token.ValueText}\"",
                        Resolution = Resolution,
                        Severity = Severity.High
                    });
                }
            }

            return findings;
        }
    }
}
