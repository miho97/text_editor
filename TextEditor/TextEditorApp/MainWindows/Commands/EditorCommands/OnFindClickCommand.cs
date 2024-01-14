using TextEditorApp.Dialogs;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnFindClickCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnFindClickCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnFindClickCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the find/replace command.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if (CallerViewModel.ActiveTextEditor != null )
            {

                var findReplaceWindow = new FindReplaceDialog(CallerViewModel.ActiveTextEditor);//new FindReplaceWindow(CallerViewModel.ActiveTextEditor);
                findReplaceWindow.FindAndHighlight(); 
                findReplaceWindow = null;

            }
        }
    }
}
