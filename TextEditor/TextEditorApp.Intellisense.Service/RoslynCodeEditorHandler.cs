using Microsoft.CodeAnalysis;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextEditorApp.Controls.ControlsModels;

namespace TextEditorApp.Intellisense.Service
{
    public class RoslynCodeEditorHandler
    {
        private CustomRoslynHost _host;

        public RoslynCodeEditorHandler()
        {
            InitializeRoslynHost();
        }

        private void InitializeRoslynHost()
        {
            _host = new CustomRoslynHost(additionalAssemblies: new[]
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
        }
        public async Task InitializeRoslynCodeEditorAsync(CustomTextEditorModel roslynCodeEditor)
        {
            var workingDirectory = Directory.GetCurrentDirectory();
            var documentId = await roslynCodeEditor.InitializeAsync(_host, new ClassificationHighlightColors(),
            workingDirectory, string.Empty, SourceCodeKind.Script).ConfigureAwait(true);
        }
    }
}
