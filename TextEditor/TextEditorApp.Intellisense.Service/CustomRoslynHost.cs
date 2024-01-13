using Microsoft.CodeAnalysis.Diagnostics;
using RoslynPad.Roslyn;
using System.Collections.Immutable;
using System.Reflection;

namespace TextEditorApp.Intellisense.Service
{
    /// <summary>
    /// Custom implementation of the <see cref="RoslynHost"/> class, allowing the addition of analyzers to the solution.
    /// Main purpose of  <see cref="CustomRoslynHost"/> is to provide with services used to analyse and manipulate with C# and .NET code.
    /// </summary>
    /// 

    // In our case this host is used in the 'expanded' use-case where we provide users with out-of-the-box code completion option for C# code in our editor application.
    public class CustomRoslynHost : RoslynHost
    {
        private bool _addedAnalyzers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRoslynHost"/> class.
        /// </summary>
        /// <param name="additionalAssemblies">Additional assemblies to be included.</param>
        /// <param name="references">References for the Roslyn host.</param>
        /// <param name="disabledDiagnostics">Disabled diagnostics for the Roslyn host.</param>
        public CustomRoslynHost(IEnumerable<Assembly>? additionalAssemblies = null, RoslynHostReferences? references = null, ImmutableArray<string>? disabledDiagnostics = null) 
            : base(additionalAssemblies, references, disabledDiagnostics)
        {
        }

        /// <summary>
        /// Overrides the base method to provide custom solution analyzer references.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{AnalyzerReference}"/> of AnalyzerReference objects.</returns>
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
