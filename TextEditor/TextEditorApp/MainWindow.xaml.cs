using System;
using System.Collections.Generic;
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
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TextEditorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public  int fileCount = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {

            // tabItem function called on every raise of event Preview...
            // += for possible list of events
            TabItem newTab = new TabItem();
            newTab.Header = "Untitled" + (++fileCount) + ".txt";
            newTab.PreviewMouseRightButtonDown += TabItem_PreviewMouseRightButtonDown;

            DockPanel panel = new DockPanel();

            TextBlock statusBar = new TextBlock();
            statusBar.Text = "Status bar for " + newTab.Header;
            DockPanel.SetDock(statusBar, Dock.Bottom);

            var textEditor = new TextEditor();
            textEditor.IsReadOnly = false;
            textEditor.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            textEditor.ShowLineNumbers = true;
            //textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            DockPanel.SetDock(textEditor, Dock.Top);

            panel.Children.Add(statusBar);
            panel.Children.Add(textEditor);

            newTab.Content = panel;

            MainTabControl.Items.Add(newTab);

            MainTabControl.SelectedItem = newTab;

            textEditor.Focus();
        }

        private void TabItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TabItem tabItem && e.RightButton == MouseButtonState.Pressed)
            {
                MainTabControl.Items.Remove(tabItem);
                fileCount--;
            }
        }

        private void EnableHighlighting_click(object sender, RoutedEventArgs e)
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<TextEditor>().FirstOrDefault();
                if( textEditor != null)
                {
                    textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");
                }
            }
        }
    }
}
