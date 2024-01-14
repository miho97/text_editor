using RoslynPad.Editor;
using System.Linq;
using System.Windows.Controls;
using TextEditorApp.MainWindows.WinViewModels;
using TextEditorApp.Utils.StaticModels;

namespace TextEditorApp.MainWindows.Commands
{
    internal class OnFontFamilyChanged : BaseCommandClass
    {
        public OnFontFamilyChanged(MainWinViewModel callerViewModel) : base(callerViewModel) { }

        public override void Execute(object? parameter)
        {
            if (parameter is SelectionChangedEventArgs args && args.Source is ComboBox combo && combo.SelectedValue is FontFamilyModel fontFamily)
            {
                if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<RoslynCodeEditor>().FirstOrDefault();
                    if (textEditor != null)
                    {
                        textEditor.FontFamily = fontFamily.Fontfamily;
                    }
                }
            }
        }
    }
}
