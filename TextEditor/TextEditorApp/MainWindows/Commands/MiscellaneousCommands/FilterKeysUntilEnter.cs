using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class FilterKeysUntilEnter : ICommand
    {
        protected readonly MainWinViewModel CallerViewModel;

        public FilterKeysUntilEnter(MainWinViewModel callerViewModel)
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
            if (parameter is KeyboardEventArgs args && args.Source is ComboBox combo &&  args.KeyboardDevice.IsKeyDown(Key.Enter))
            {
                args.Handled = true;
                if(!string.IsNullOrEmpty(combo.Text))
                {
                    CallerViewModel.OnChangeFontSize.Execute(combo.Text);
                }
            }
        }
    }
}
