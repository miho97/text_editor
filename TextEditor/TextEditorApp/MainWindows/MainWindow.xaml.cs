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
using TextEditorApp.Utils.BrowserModels;
using System.Windows.Controls.Ribbon;
using TextEditorApp.Utils.StaticModels;
using TextEditorApp.MainWindows.Commands;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Reflection;
using System.Collections.Immutable;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.ConstrainedExecution;
using CefSharp;
using CefSharp.Wpf;
using TextEditorApp.MainWindows.Behaviours;

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
                    //await InitializeRoslynCodeEditorAsync(roslynCodeEditor);
                }
            }
        }

        private void OnLanguageChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox languageCombobox)
            {
                var SelectedLanguage = languageCombobox.SelectedItem as string;
                // Pristup RoslynCodeEditor-u unutar StackPanel-a
                if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        textEditor.SyntaxHighlighting =  HighlightingManager.Instance.GetDefinition(SelectedLanguage);
                    }
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
            var _host = new CustomRoslynHost(additionalAssemblies: new[]
        {
                    Assembly.Load("RoslynPad.Roslyn.Windows"),
                    Assembly.Load("RoslynPad.Editor.Windows")
                }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
        {
            typeof(object).Assembly,
            typeof(System.Text.RegularExpressions.Regex).Assembly,
            typeof(Enumerable).Assembly,
        }));

            roslynCodeEditor.ShowLineNumbers = true;

            var workingDirectory = Directory.GetCurrentDirectory();
            var documentId = await roslynCodeEditor.InitializeAsync(_host, new ClassificationHighlightColors(),
            workingDirectory, string.Empty, SourceCodeKind.Script).ConfigureAwait(true);
            //await roslynCodeEditor.InitializeAsync(roslynHost, new ClassificationHighlightColors(), workingDirectory, "", SourceCodeKind.Script);
        }

        private async void Intellisese_Handler(object sender, RoutedEventArgs e)
        {
            if (sender is RibbonToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            await InitializeRoslynCodeEditorAsync(textEditor);
                        }
                    }
                }
                else
                {
                    if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                    {
                        var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                        if (textEditor != null)
                        {
                            textEditor.CompletionProvider = null;
                        }
                    }
                }
            }
        }

        private class CustomRoslynHost : RoslynHost
        {
            private bool _addedAnalyzers;

            public CustomRoslynHost(IEnumerable<Assembly>? additionalAssemblies = null, RoslynHostReferences? references = null, ImmutableArray<string>? disabledDiagnostics = null) : base(additionalAssemblies, references, disabledDiagnostics)
            {
            }

            protected override IEnumerable<AnalyzerReference> GetSolutionAnalyzerReferences()
            {
                if (!_addedAnalyzers)
                {
                    _addedAnalyzers = true;
                    return base.GetSolutionAnalyzerReferences();
                }

                return Enumerable.Empty<AnalyzerReference>();
            }
        }


    }
}
