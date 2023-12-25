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

            // objekt newTab pri svakom raisenjau Preview....  poziva TabItem... funkciju
            // += jer nadodajemo na listu evenata, subscribera moze biti vise
            TabItem newTab = new TabItem();
            newTab.Header = "Untitled" + (++fileCount) + ".txt";
            newTab.PreviewMouseRightButtonDown += TabItem_PreviewMouseRightButtonDown;

            DockPanel panel = new DockPanel();

            TextBlock statusBar = new TextBlock();
            statusBar.Text = "Status bar for " + newTab.Header;
            DockPanel.SetDock(statusBar, Dock.Bottom);

            TextEditor text_editor = new TextEditor();
            text_editor.IsReadOnly = false;
            text_editor.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            DockPanel.SetDock(text_editor, Dock.Top);

            panel.Children.Add(statusBar);
            panel.Children.Add(text_editor);

            newTab.Content = panel;

            MainTabControl.Items.Add(newTab);

            MainTabControl.SelectedItem = newTab;

            text_editor.Focus();
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
            textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");

        }
    }
}
