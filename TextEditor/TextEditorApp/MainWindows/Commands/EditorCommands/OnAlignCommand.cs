using RoslynPad.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Common.Enums;
using TextEditorApp.Controls.ControlsModels;
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
            //if (CallerViewModel.MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel
            //    && parameter is CustomHorizontalTextAlignment align)
            //{
            //    var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
            //    if (textEditor != null)
            //    {
            //        textEditor.DocumentModel.TextAlignment = align;
            //        textEditor.TextAlignment = align;
            //    }
            //}

            if(CallerViewModel.ActiveTextEditor != null && parameter is CustomHorizontalTextAlignment align)
            {
                CallerViewModel.ActiveTextEditor.TextAlignment = align;
            }
        }
    }
}
