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
    /// Abstract base class for implementing ICommand, providing a common structure for command classes.
    /// Serves as a base class for all ou our commands.
    /// </summary>
    internal abstract class BaseCommandClass : ICommand
    {
        /// <summary>
        /// The MainWinViewModel instance that invokes the command.
        /// </summary>
        protected readonly MainWinViewModel CallerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandClass"/> class.
        /// </summary>
        /// <param name="callerViewModel">The ViewModel that invokes the command.</param>
        public BaseCommandClass(MainWinViewModel callerViewModel)
        {
            CallerViewModel = callerViewModel;
        }

        public event EventHandler? CanExecuteChanged;

        // for our use cases it is ok if all of the commands are enabled
        public bool CanExecute(object? parameter)
        {
            return true; 
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>

        // every command will implement just Execute method
        public abstract void Execute(object? parameter);
    }
}
