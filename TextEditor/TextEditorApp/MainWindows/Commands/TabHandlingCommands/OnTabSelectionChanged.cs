using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command handler for the tab selection changed event.
    /// </summary>
    internal class OnTabSelectionChanged : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnTabSelectionChanged"/> class.
        /// </summary>
        /// <param name="callerViewModel">The MainWinViewModel associated with the command.</param>
        public OnTabSelectionChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command logic when the tab selection changes.
        /// </summary>
        

        // we can rely on UpdateWindow function to do the work
        public override void Execute(object? parameter)
        {
            CallerViewModel.UpdateWindow();
        }
    }
}
