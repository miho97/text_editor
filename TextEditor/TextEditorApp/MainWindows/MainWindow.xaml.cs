using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TextEditorApp.MainWindows.WinViewModels;


namespace TextEditorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWinViewModel(MainTabControl, FontSizeComboBox);
        }

        private void Browser_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void FontSizeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.Key == Key.Enter)
            {
                if (int.TryParse(FontSizeInput.Text, out int newSize) && (newSize>0))
                {
                    ApplyFontSize(newSize);
                }
                FontSizePopup.IsOpen = false;
            }
        }
        private void FontSizePopup_LostFocus(object sender, RoutedEventArgs e)
        {
            FontSizePopup.IsOpen = false;
        }
        */
        public static FindReplaceWindow? findReplaceWindow;
        private TextEditor? currentTextEditor;

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                currentTextEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (currentTextEditor != null)
                {
                    if (findReplaceWindow == null)
                    {
                        findReplaceWindow = new FindReplaceWindow(currentTextEditor);
                    }

                    findReplaceWindow.FindAndHighlight(); // Show the window as a dialog

                    // Reset findReplaceWindow after using it
                    findReplaceWindow = null;
                }
            }
        }
    }
}
