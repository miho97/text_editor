using System;
using System.Windows.Input;
using TextEditorApp.Common.Enums;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnAlignCommand : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public OnAlignCommand(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(CallerViewModel.ActiveTextEditor != null && parameter is CustomHorizontalTextAlignment align)
            {
                CallerViewModel.ActiveTextEditor.TextAlignment = align;
            }
        }
    }
}
