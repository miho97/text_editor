using Microsoft.CodeAnalysis.Diagnostics;
using RoslynPad.Roslyn;
using System.Collections.Immutable;
using System.Reflection;

namespace TextEditorApp.Intellisense.Service
{
    public class CustomRoslynHost : RoslynHost
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
