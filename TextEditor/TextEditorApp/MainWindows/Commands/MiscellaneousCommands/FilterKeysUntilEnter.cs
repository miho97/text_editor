using System;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class FilterKeysUntilEnter : BaseCommandClass
    {
        public FilterKeysUntilEnter(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
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
