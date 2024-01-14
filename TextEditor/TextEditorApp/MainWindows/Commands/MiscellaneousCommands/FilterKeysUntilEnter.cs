using System;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command handler for filtering keys in a ComboBox until the Enter key is pressed.
    /// </summary>
    internal class FilterKeysUntilEnter : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterKeysUntilEnter"/> class.
        /// </summary>
        /// <param name="callerViewModel">The MainWinViewModel associated with the command.</param>
        public FilterKeysUntilEnter(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command logic for filtering keys in a ComboBox until the Enter key is pressed.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // this command reacts to every key down on combobox that is responsible for setting the font size.
            // However, we only want to trigger a certain behaviour when Enter is pressed because until then user can input any number.
            if (parameter is KeyboardEventArgs args && args.Source is ComboBox combo &&  args.KeyboardDevice.IsKeyDown(Key.Enter))
            {
                // we stop any further handling of enter
                args.Handled = true;
                if(!string.IsNullOrEmpty(combo.Text))
                {
                    // OnChangeFonr size should take over frome here
                    CallerViewModel.OnChangeFontSize.Execute(combo.Text);
                }
            }
        }
    }
}
