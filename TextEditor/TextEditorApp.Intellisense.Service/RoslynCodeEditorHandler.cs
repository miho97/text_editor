using Microsoft.CodeAnalysis;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System.IO;
using System.Reflection;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.Intellisense.Service
{
    /// <summary>
    /// Handles the interaction between the Roslyn code editor and the application.
    /// </summary>
    public class RoslynCodeEditorHandler 
    {
        private CustomRoslynHost _host;
        private DocumentId? documentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynCodeEditorHandler"/> class.
        /// </summary>
        public RoslynCodeEditorHandler()
        {
            _host = InitializeRoslynHost();
        }

        /// <summary>
        /// Initializes the Roslyn host with the required assemblies.
        /// </summary>
        /// <returns>The initialized CustomRoslynHost.</returns>
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

        /// <summary>
        /// Initializes the Roslyn code editor asynchronously.
        /// </summary>
        /// <param name="roslynCodeEditor">The CustomTextEditorModel representing the Roslyn code editor.</param>
        public async Task InitializeRoslynCodeEditorAsync(CustomTextEditorModel roslynCodeEditor)
        {
            var workingDirectory = Directory.GetCurrentDirectory();
            documentId = await roslynCodeEditor.InitializeAsync(_host, new ClassificationHighlightColors(),
            workingDirectory, roslynCodeEditor.Text, SourceCodeKind.Regular).ConfigureAwait(true);
        }
    }
}
