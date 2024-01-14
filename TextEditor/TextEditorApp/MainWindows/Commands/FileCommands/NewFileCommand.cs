using System.Windows.Controls;
using System.Windows.Data;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.MainWindows.WinViewModels;

namespace TextEditorApp.MainWindows.Commands
{
    /// <summary>
    /// Command for creating and adding a new file tab to the main window.
    /// </summary>
    internal class NewFileCommand : BaseCommandClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewFileCommand"/> class.
        /// </summary>
        /// <param name="callerViewModel">The view model associated with the main window.</param>
        public NewFileCommand(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        /// <summary>
        /// Executes the command to create and add a new file tab.
        /// </summary>
        public override void Execute(object? parameter)
        {
            // we are creating new tab that will be added to tab Control
            // inside of the tab everything will be inside of the dock panel

            // this could be put into a few different classes but it is called only here
            CallerViewModel.FileCount++;
            TabItem newTab = new TabItem();
            DockPanel panel = new DockPanel();

            newTab.PreviewMouseRightButtonDown += (sender, args) =>
            {
                CallerViewModel.OnRemoveTabCommand?.Execute(args);
            };

            // we are adding new text Editor to tab's panel
            var textEditor = new CustomTextEditorModel();
            DockPanel.SetDock(textEditor, Dock.Top);
            panel.Children.Add(textEditor);
            textEditor.DockParent = panel; // we need to remember the visual parent of the editor

            // we are adding new text status bar to tab's panel
            TextBlock statusBar = new TextBlock();
            DockPanel.SetDock(statusBar, Dock.Bottom);
            panel.Children.Add(statusBar);
            

            // we are adding created panel content to new tab
            newTab.Content = panel;

            // we are binding tab header to file name of the document inside the Editor
            // this ensures that any changes to the filename will be visible in the tab's header 
            var binding = new Binding(nameof(textEditor.DocumentModel.FileName));
            binding.Source = textEditor.DocumentModel;
            newTab.SetBinding(HeaderedContentControl.HeaderProperty, binding);

            // TODO - bind status bar text to something from the document model
            statusBar.Text = "Status bar for " + newTab.Header;


            // adding the newly created editor to Main Tab
            CallerViewModel.MainTabControl.Items.Add(newTab);
            CallerViewModel.MainTabControl.SelectedItem = newTab;
            CallerViewModel.ActiveTextEditor = textEditor;

            textEditor.Focus();

        }
    }
}
