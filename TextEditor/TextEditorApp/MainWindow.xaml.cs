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

            TabItem newTab = new TabItem();
            newTab.Header = "Untitled" + (++fileCount) + ".txt";
            newTab.PreviewMouseRightButtonDown += TabItem_PreviewMouseRightButtonDown;

            DockPanel panel = new DockPanel();

            TextBlock statusBar = new TextBlock();
            statusBar.Text = "Status bar for " + newTab.Header;
            DockPanel.SetDock(statusBar, Dock.Bottom);

            TextBox textBox = new TextBox();
            textBox.IsReadOnly = false;
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            textBox.AcceptsReturn = true;
            textBox.AcceptsTab = true;
            DockPanel.SetDock(textBox, Dock.Top);

            panel.Children.Add(statusBar);
            panel.Children.Add(textBox);

            newTab.Content = panel;

            MainTabControl.Items.Add(newTab);

            MainTabControl.SelectedItem = newTab;

            textBox.Focus();
        }

        private void TabItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TabItem tabItem && e.RightButton == MouseButtonState.Pressed)
            {
                MainTabControl.Items.Remove(tabItem);
                fileCount--;
            }
        }
    }
}
