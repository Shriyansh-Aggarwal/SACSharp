using System.Reflection;
using SACSharp.Rules;
using SACSharp.Core;
using SACSharp.Models;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] != "scan" || args.Length < 2)
        {
            Console.WriteLine("Usage: sacsharp scan /path/to/project-directory");
            return;
        }

        string path = args[1];
        var csFiles = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
        var ruleInterface = typeof(IRule);
        var rules = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => ruleInterface.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            .Select(t => (IRule)Activator.CreateInstance(t)!)
            .ToList();

        foreach (var file in csFiles)
        {
            var findings = Scanner.ScanFile(file, rules);
            foreach (var finding in findings)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"[{finding.RuleId}] ");

                Console.ForegroundColor = finding.Severity switch
                {
                    Severity.Low => ConsoleColor.White,
                    Severity.Medium => ConsoleColor.Yellow,
                    Severity.High => ConsoleColor.DarkYellow,
                    Severity.Critical => ConsoleColor.Red,
                    _ => ConsoleColor.Gray
                };
                Console.WriteLine(finding.Message);

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($" -> {finding.FilePath}:{finding.LineNumber}");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"    Resolution: {finding.Resolution}");

                Console.ResetColor();
            }
        }
    }
}
