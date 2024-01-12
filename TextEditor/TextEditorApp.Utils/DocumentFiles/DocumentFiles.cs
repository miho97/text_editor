using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextEditorApp.Utils.DocumentFiles
{
    public static class DocumentFiles
    {

        public static List<string> suggestedWords = new List<string> { "class", "if", "else", "for", "while", "int", "string" };
        static List<string> basicTypes = new List<string> { "class", "if", "else", "for", "while", "int", "string" };
        //protected static bool IsExistDirectory(string directoryPath)
        //{
        //    return Directory.Exists(directoryPath);
        //}

        //protected static void CreateDirectory(string directoryPath)
        //{
        //    Directory.CreateDirectory(directoryPath);
        //}

        //protected static string GetDirectoryPath(string filePath)
        //{
        //    return Path.GetDirectoryName(filePath)!;
        //}

        //public void WriteToFile(DocumentFiles_Model model)
        //{
        //    string directoryPath = GetDirectoryPath(model.FilePath)!;

        //    if (!IsExistDirectory(directoryPath))
        //    {
        //        CreateDirectory(directoryPath);
        //    }

        //    File.WriteAllText(model.FilePath, model.Content as string);
        //}
    }
}
