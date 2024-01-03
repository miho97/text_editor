﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Common;
using TextEditorApp.MainWindows.Commands;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace TextEditorApp.MainWindows.WinViewModels
{
    internal class MainWinViewModel : ViewModelBase
    {
        private TabControl? _MainTabControl;

        private int _FileCount;

        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        public TabControl MainTabControl
        {
            get { return _MainTabControl; }
            set
            {
                _MainTabControl = value;
                OnPropertyChanged(nameof(_MainTabControl));
            }
        }
        public MainWinViewModel(TabControl _MainTabControl) 
        {
            MainTabControl = _MainTabControl;
        }

        
        private ICommand? _saveFileAsCommand;
        public ICommand SaveFileAsCommand => _saveFileAsCommand ??= new SaveFileAsCommand(this);

        private ICommand? _OpenFileCommand;
        public ICommand OpenFileCommand => _OpenFileCommand ??= new OpenFileCommand(this);

        private ICommand? _OnRemoveTabCommand;
        public ICommand OnRemoveTabCommand => _OnRemoveTabCommand ??= new OnRemoveTabCommand(this);

        private ICommand? _OnEnableHighlighting;
        public ICommand OnEnableHighlighting => _OnEnableHighlighting ??= new OnEnableHighlighting(this);

        private ICommand? _OnDisableHighlighting;
        public ICommand OnDisableHighlighting => _OnDisableHighlighting ??= new OnDisableHighlighting(this);

        private ICommand? _NewFileCommand;
        public ICommand NewFileCommand => _NewFileCommand ??= new NewFileCommand(this);


    }
}