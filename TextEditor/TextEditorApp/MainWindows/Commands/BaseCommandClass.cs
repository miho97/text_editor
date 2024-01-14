using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal abstract class BaseCommandClass : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public BaseCommandClass(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true; 
        }

        public abstract void Execute(object? parameter);
    }
}
