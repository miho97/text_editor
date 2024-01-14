using TextEditorApp.Common.Enums;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling the alignment command, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnAlignCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnAlignCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnAlignCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the alignment command, setting the text alignment of the active text editor.
        /// </summary>
        public override void Execute(object? parameter)
        {
            if(CallerViewModel.ActiveTextEditor != null && parameter is CustomHorizontalTextAlignment align)
            {
                CallerViewModel.ActiveTextEditor.TextAlignment = align;
            }
        }
    }
}
