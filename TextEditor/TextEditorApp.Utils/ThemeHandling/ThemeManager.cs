using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextEditorApp.Utils.ThemeHandling
{
    public class ThemeManager
    {
        public static void ChangeTheme(Uri theme)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = theme };

            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Resources.MergedDictionaries.Clear();
                mainWindow.Resources.MergedDictionaries.Add(Theme);
            }

        }
    }
}
