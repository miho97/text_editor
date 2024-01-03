using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace TextEditorApp.Utils.WriteToFiles
{
    public class WriteToFiles_Model
    {
        private readonly string? _filePath;
        public string FilePath
        {
            get
            {
                // error handling?

                return _filePath!;
            }

            init => _filePath = value;
        }

        public string Content { get; init; } = string.Empty;
    }
}
