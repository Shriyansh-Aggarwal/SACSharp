using Microsoft.CodeAnalysis.CSharp;
using SACSharp.Models;
using SACSharp.Rules;

namespace SACSharp.Core
{
    public static class Scanner
    {
        public static List<Finding> ScanFile(string filePath, List<IRule> rules)
        {
            var findings = new List<Finding>();
            var code = File.ReadAllText(filePath);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            foreach (var rule in rules)
            {
                var ruleFindings = rule.Analyze(syntaxTree, filePath);
                findings.AddRange(ruleFindings);
            }

            return findings;
        }
    }
}
