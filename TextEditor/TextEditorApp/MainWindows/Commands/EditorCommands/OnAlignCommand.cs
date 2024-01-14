using TextEditorApp.Common.Enums;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnAlignCommand : BaseCommandClass
    {
        public OnAlignCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            if(CallerViewModel.ActiveTextEditor != null && parameter is CustomHorizontalTextAlignment align)
            {
                CallerViewModel.ActiveTextEditor.TextAlignment = align;
            }
        }
    }
}
