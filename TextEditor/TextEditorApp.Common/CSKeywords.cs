namespace TextEditorApp.Common
{
    public static class CSKeywords
    {
        public static readonly List<string> AllKeywords = new List<string>
        {
            // C# keywords
            "abstract", "as", "base", "bool", "break", "byte",
            "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double",
            "else", "enum", "event", "explicit", "extern", "false",
            "finally", "fixed", "float", "for", "foreach", "goto",
            "if", "implicit", "in", "int", "interface", "internal",
            "is", "lock", "long", "namespace", "new", "null",
            "object", "operator", "out", "override", "params", "private",
            "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc", "static", "string",
            "struct", "switch", "this", "throw", "true", "try",
            "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
            "using", "var", "virtual", "void", "volatile", "while",

            // .NET keywords
            "System", "Collections", "Generic", "Linq", "Text", "Threading", "Tasks",

            // ASP.NET keywords
            "Web", "Mvc",

            // LINQ keywords
            "Expressions",

            // Entity Framework keywords
            "Data", "Entity",

            // ASP.NET Core keywords
            "Microsoft", "AspNetCore",

            // ASP.NET keywords
            "Identity",

            // WPF keywords
            "Windows", "Controls",

            // WinForms keywords
            "Windows", "Forms",

            // ADO.NET keywords
            "Data", "SqlClient",

            // System namespaces
            "System.IO", "System.Net", "System.Text", "System.Diagnostics", "System.Linq",

            // System.Windows namespaces
            "System.Windows", "System.Windows.Controls", "System.Windows.Media", "System.Windows.Shapes",

            // Additional namespaces
            "Microsoft.Win32", "System.Reflection", "System.Globalization"
        };
    }
}
