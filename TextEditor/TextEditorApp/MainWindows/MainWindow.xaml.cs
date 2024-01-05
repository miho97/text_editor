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
using System.Windows.Controls.Ribbon;
using TextEditorApp.Utils.StaticModels;
using TextEditorApp.MainWindows.Commands;

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


    }
}
