using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
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

       
        private void Consolas_click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    textEditor.FontFamily = new FontFamily("Consolas");
                }
            }
        }

        private void Courier_new_click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    textEditor.FontFamily = new FontFamily("Courier new");
                }
            }
        }
        private void Roboto_mono_click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    textEditor.FontFamily = new FontFamily("Roboto mono");
                }
            }
        }

        private void ApplyFontSize(int fontSize)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if (textEditor != null)
                {
                    textEditor.FontSize = fontSize;
                }
            }
        }
        private void Font12_click(object sender, RoutedEventArgs e)
        {
            ApplyFontSize(12);
        }
        private void Font16_click(object sender, RoutedEventArgs e)
        {
            ApplyFontSize(16);
        }
        private void Font20_click(object sender, RoutedEventArgs e)
    
        {
            ApplyFontSize(20);
        }

        /*
        private void FontSizePopup_click(object sender, RoutedEventArgs e)
        {
            FontSizePopup.IsOpen = true;

            // Delay focus to ensure the popup is fully open
            Dispatcher.BeginInvoke((Action)(() =>
            {
                FontSizeInput.Focus();
            }), DispatcherPriority.Input);
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

        // Otvaranje posebnog prozora za Find/Replace
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

                    findReplaceWindow.FindAndHighlight(); // Prikaži poseban prozor za Find/Replace

                    // Reset prozora nakon zatvaranja
                    findReplaceWindow = null;
                }
            }
        }

    }
}
