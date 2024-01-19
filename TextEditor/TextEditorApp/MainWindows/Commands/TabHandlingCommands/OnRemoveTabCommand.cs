using System;
using System.Linq;
using TextEditorApp.MainWindows.WinViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.Utils.DocumentFiles;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command handler for the remove tab action.
    /// </summary>
    internal class OnRemoveTabCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnRemoveTabCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The MainWinViewModel associated with the command.</param>
        public OnRemoveTabCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command logic when removing a tab.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // here it is not enough to work just with CallerViewModel.ActiveEditor because user can from GUI close any of the existing
            // tabs without really making them active

            // we respond only to right click - it is ok that we hijacked the right click for this purpose because we do not provide any context menu 
            // for tabs and we didn't find any easy way to add 'x' button to tabs
            if (parameter is MouseEventArgs args && args.Source is TabItem tabItem && args.RightButton == MouseButtonState.Pressed && tabItem.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                // we want to stop users to close unsaved tabs
                if (textEditor != null && textEditor.DocumentModel.IsSaved == false && textEditor.Text != string.Empty)
                {
                    // Prompt user for save confirmation
                    // only if the users cancels will we do an early return
                    if (!SaveOptionOnCloseEditorTab(textEditor.DocumentModel)) return;
                }

                CallerViewModel.MainTabControl.Items.Remove(tabItem);
                CallerViewModel.FileCount--;

                // we want to aviod having none editors open - so just like in Notepad++ if the last tab is closed, a new one will be opened automatically
                if(CallerViewModel != null && CallerViewModel.FileCount <= 0)
                {
                    CallerViewModel.NewFileCommand.Execute(CallerViewModel);
                }
            }
        }

        /// <summary>
        /// Displays a message box prompting the user to save the document before closing the tab.
        /// </summary>
        /// <param name="docModel">The DocumentFiles_Model associated with the tab.</param>
        /// <returns>True if the user chooses to save or not save, False if the user cancels the operation.</returns>
        private bool SaveOptionOnCloseEditorTab(DocumentFiles_Model docModel)
        {
            MessageBoxResult result = MessageBox.Show("You have unsaved changes in your file. Do you wish to save file " + docModel.FileName, "Save?", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    CallerViewModel.SaveFileCommand.Execute(CallerViewModel);
                    return true;
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return false;
        }
    }
}
