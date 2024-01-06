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
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Reflection;

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

        private async void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Pristup StackPanel-u unutar ScrollViewer-a
            if (sender is RoslynCodeEditor editor)
            {
                // Pristup RoslynCodeEditor-u unutar StackPanel-a
                var roslynCodeEditor = editor as RoslynCodeEditor;

                if (roslynCodeEditor != null)
                {
                    await InitializeRoslynCodeEditorAsync(roslynCodeEditor);
                }
            }
        }

        private async Task InitializeRoslynCodeEditorAsync(RoslynCodeEditor roslynCodeEditor)
        {
            var roslynHost = new RoslynHost(additionalAssemblies: new[]
            {
            Assembly.Load("RoslynPad.Roslyn.Windows"),
            Assembly.Load("RoslynPad.Editor.Windows"),
        });

            roslynCodeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("HTML");
            roslynCodeEditor.ShowLineNumbers = true;

            var workingDirectory = Directory.GetCurrentDirectory();
            await roslynCodeEditor.InitializeAsync(roslynHost, new ClassificationHighlightColors(), workingDirectory, "", SourceCodeKind.Script);
        }


    }
}
