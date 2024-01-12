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

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (findReplaceWindow == null)
            {
                findReplaceWindow = new FindReplaceWindow(currentTextEditor);
            }

            if (findReplaceWindow.ShowDialog() == true)
            {
                string searchText = findReplaceWindow.SearchText;
                string replaceText = findReplaceWindow.ReplaceText;
                bool findNext = findReplaceWindow.ReplaceNext;

                StringComparison comparison = findReplaceWindow.MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                int index = textEditor.Text.IndexOf(searchText, comparison);

                if (index != -1)
                {
                    textEditor.Document.Replace(index, searchText.Length, replaceText);

                    if (findNext)
                    {
                        // Find the next occurrence if Find Next is checked
                        index = textEditor.Text.IndexOf(searchText, index + replaceText.Length, comparison);
                        if (index != -1)
                        {
                            textEditor.Select(index, searchText.Length);
                            textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(index).LineNumber);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Text not found", "Replace");
                }

                // Reset findReplaceWindow after using it
                findReplaceWindow = null;
            }
        }


    }
}
