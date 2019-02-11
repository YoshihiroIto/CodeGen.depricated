namespace CodeGen
{
    public class Config
    {
        public string[] Keywords { get; set; }

        public Process[] Processes { get; set; }
    }

    public class Process
    {
        public ProcessKind Kind { get; set; }

        // Template
        public string TemplateFile { get; set; }
        public string OutputFile { get; set; }

        // Patch
        public string TargetFile { get; set; }
        public string Tag { get; set; }
        public string Input { get; set; }
    }

    public enum ProcessKind
    {
        Template,
        Patch
    }
}