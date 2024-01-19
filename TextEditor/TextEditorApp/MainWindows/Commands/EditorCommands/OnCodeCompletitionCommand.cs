using System.Windows.Controls.Ribbon;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using TextEditorApp.Intellisense.Service;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command class for handling code completion, extending the <see cref="BaseCommandClass"/>.
    /// </summary>
    internal class OnCodeCompletitionCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnCodeCompletitionCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public OnCodeCompletitionCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to handle code completion.
        /// </summary>

        // if toggle button responsible for enabling the code copletiotion is enabled we will start the asynchronous initialization of the code
        // analysis services
        // when the toggle is turned off we will kill the provider responsible for providing the copletition
        public override async void Execute(object? parameter)
        {
            var textEditor = CallerViewModel.ActiveTextEditor;
            if (parameter is RoutedEventArgs args && args.Source is RibbonToggleButton toggleButton && textEditor != null)
            {
                if (toggleButton.IsChecked == true)
                {
                    var roslynHandler = new RoslynCodeEditorHandler();
                    await roslynHandler.InitializeRoslynCodeEditorAsync(textEditor);
                }
                else
                {
                    textEditor.SyntaxHighlighting = null;
                    textEditor.CompletionProvider = null;
                }
            }
        }
    }
}
