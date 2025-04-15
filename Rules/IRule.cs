using Microsoft.CodeAnalysis;
using SACSharp.Models;

namespace SACSharp.Rules
{
    public interface IRule
    {
        string Id { get; }
        string Name { get; }
        string Description { get;}
        string Resolution { get;}

        List<Finding> Analyze(SyntaxTree syntaxTree, string filePath);
    }
}
