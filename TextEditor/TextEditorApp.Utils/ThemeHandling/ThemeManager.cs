using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextEditorApp.Utils.ThemeHandling
{
    /// <summary>
    /// The ThemeManager class provides functionality to change the theme within our application.
    /// </summary>
    public class ThemeManager
    {
        /// <summary>
        /// The ChangeTheme method modifies the current application theme based on the provided URI path of a resource.
        /// The new theme is applied to the main application window.
        /// </summary>
        /// <param name="theme">URI path to the ResourceDictionary resource defining the desired theme.</param>
        public static void ChangeTheme(Uri theme)
        {
            // Create a new ResourceDictionary from the given URI path
            ResourceDictionary Theme = new ResourceDictionary() { Source = theme };


            // Retrieve the main application window
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow != null)
            {
                // Replace current resources with the new theme resources
                // Since default ResourceDict is defined in MainWindow and not in app we must clear it from the Window
                mainWindow.Resources.MergedDictionaries.Clear();
                mainWindow.Resources.MergedDictionaries.Add(Theme);
            }
        }
    }
}
