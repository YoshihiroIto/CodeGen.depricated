using System;
using System.IO;

namespace CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var configFilePath = args[0];

            var configDirPath = Path.GetDirectoryName(configFilePath);
            var config = Utf8Json.JsonSerializer.Deserialize<Config>(File.ReadAllBytes(configFilePath));

            if (string.IsNullOrEmpty(configDirPath) == false)
                Directory.SetCurrentDirectory(configDirPath);

            foreach (var keyword in config.Keywords)
            {
                Console.WriteLine($"Input {keyword}.");

                Environment.SetEnvironmentVariable(keyword, Console.ReadLine());
            }

            foreach (var p in config.Processes)
            {
                switch (p.Kind)
                {
                    case ProcessKind.Template:

                        var template = Environment.ExpandEnvironmentVariables(File.ReadAllText(p.TemplateFile));

                        var outputPath = Environment.ExpandEnvironmentVariables(p.OutputFile);
                        var outputDir = Path.GetDirectoryName(outputPath);

                        if (string.IsNullOrEmpty(outputDir) == false)
                            Directory.CreateDirectory(outputDir);

                        File.WriteAllText(outputPath, template);

                        break;

                    case ProcessKind.Patch:
                        var targetFile = Environment.ExpandEnvironmentVariables(p.TargetFile);
                        var targetFileText = File.ReadAllText(targetFile);

                        var input = Environment.ExpandEnvironmentVariables(p.Input);

                        targetFileText = targetFileText.Replace(p.Tag, input);

                        File.WriteAllText(targetFile, targetFileText);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}