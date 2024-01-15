using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextEditorApp.MainWindows.Commands
{
    class ThemeManager
    {
        public static void ChangeTheme(Uri theme)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = theme };

            var app = Application.Current;
            app.Resources.MergedDictionaries.Clear();
            app.Resources.MergedDictionaries.Add(Theme);

        }
    }
}