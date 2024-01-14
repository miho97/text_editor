using Microsoft.CodeAnalysis;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System.IO;
using System.Reflection;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.Intellisense.Service
{
    public class RoslynCodeEditorHandler 
    {
        private CustomRoslynHost _host;
        private DocumentId? documentId;

        public RoslynCodeEditorHandler()
        {
            _host = InitializeRoslynHost();
        }

        private CustomRoslynHost InitializeRoslynHost()
        {
            var host = new CustomRoslynHost(additionalAssemblies: new[]
                        {
                            Assembly.Load("RoslynPad.Roslyn.Windows"),
                            Assembly.Load("RoslynPad.Editor.Windows")
                        }, RoslynHostReferences.NamespaceDefault.With(assemblyReferences: new[]
                                {
                                    typeof(object).Assembly,
                                    typeof(System.Text.RegularExpressions.Regex).Assembly,
                                    typeof(Enumerable).Assembly,
                                }
                            )
                        );
            return host;
        }
        public async Task InitializeRoslynCodeEditorAsync(CustomTextEditorModel roslynCodeEditor)
        {
            var workingDirectory = Directory.GetCurrentDirectory();
            documentId = await roslynCodeEditor.InitializeAsync(_host, new ClassificationHighlightColors(),
            workingDirectory, roslynCodeEditor.Text, SourceCodeKind.Regular).ConfigureAwait(true);
        }
    }
}
