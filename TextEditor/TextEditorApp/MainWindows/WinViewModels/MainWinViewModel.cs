using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using TextEditorApp.Common;
using TextEditorApp.MainWindows.Commands;
using TextEditorApp.Utils.StaticModels;
using System.ComponentModel;
using System.Windows.Media;
using System.Drawing;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Reactive.Linq;
using RoslynPad.Editor;
using TextEditorApp.Controls.ControlsModels;
using System.Windows.Automation.Text;
using TextEditorApp.Common.Enums;
using System.Windows;
using Microsoft.CodeAnalysis;
using RoslynPad.Roslyn;
using System.IO;
using System.Reflection;
using TextEditorApp.Intellisense.Service;

namespace TextEditorApp.MainWindows.WinViewModels
{
    internal class MainWinViewModel : ViewModelBase
    {

        private TabControl? _MainTabControl;
        private ComboBox? _FontSizeComboBox;
        private bool _IsShowLineNumbers;
        private bool _IsCodeCompletitionEnabled;
        private FontFamilyModel _ChosenFontFamily;
        private CustomHorizontalTextAlignment _HorizontalTextAlignment;
        private CustomTextEditorModel _activeTextEditor;

        public CustomHorizontalTextAlignment HorizontalTextAlignment
        {
            get { return _HorizontalTextAlignment; }
            set
            {
                _HorizontalTextAlignment = value;
                OnPropertyChanged(nameof(HorizontalTextAlignment));
            }
        }

        public FontFamilyModel ChosenFontFamily
        {
            get { return _ChosenFontFamily; }
            set
            {
                _ChosenFontFamily = value;
                OnPropertyChanged(nameof(ChosenFontFamily));
            }
        }

        public CustomTextEditorModel ActiveTextEditor
        {
            get { return _activeTextEditor; }
            set
            {
                _activeTextEditor = value;
                OnPropertyChanged(nameof(ActiveTextEditor));
            }
        }

        public bool IsShowLineNumbers
        {
            get { return _IsShowLineNumbers; }
            set
            {
                _IsShowLineNumbers = value;
                OnPropertyChanged(nameof(IsShowLineNumbers));
            }
        }

        public bool IsCodeCompletitionEnabled
        {
            get { return _IsCodeCompletitionEnabled; }
            set
            {
                _IsCodeCompletitionEnabled = value;
                OnPropertyChanged(nameof(IsCodeCompletitionEnabled));
            }
        }


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

        public ComboBox FontSizeComboBox
        {
            get { return _FontSizeComboBox; }
            set
            {
                _FontSizeComboBox = value;
                OnPropertyChanged(nameof(_FontSizeComboBox));
            }
        }
        public MainWinViewModel(TabControl _MainTabControl, ComboBox _FontCombo)
        {
            MainTabControl = _MainTabControl;
            FontSizeComboBox = _FontCombo;

            Init();

            UpdateWindow();
        }

        private void Init()
        {
            foreach (var fontFamily in Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList())
            {
                FontFamilies.Add(new FontFamilyModel(fontFamily, true));
            };
            foreach (var fontSize in FontSizeList)
            {
                FontSizes.Add(new FontSizeListModel(fontSize, true));
            };

            foreach (var textLanguage in HighlightingManager.Instance.HighlightingDefinitions)
            {
                DocumentLanguages.Add(textLanguage.Name);
            }
            _FileCount = 0;
            NewFileCommand.Execute(this);
        }

        public void UpdateWindow()
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    ChosenLanguage = textEditor.DocumentModel.DocumentLanguage;
                    SelectedFontSize = textEditor.DocumentModel.FontSize;
                    IsShowLineNumbers = textEditor.IsShowNumbersEnabled;
                    ChosenFontFamily = textEditor.DocumentModel.DocumentFontFamily;
                    HorizontalTextAlignment = textEditor.DocumentModel.TextAlignment;
                    IsCodeCompletitionEnabled = textEditor.IsIntellisenseEnabled;
                }
            }
        }

        private List<int> FontSizeList => new List<int> { 12, 16, 20 };

        private FontSizeListModel _selectedFontSize;

        public FontSizeListModel SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }

        private LanguageViewModel _ChosenLanguage;

        public LanguageViewModel ChosenLanguage
        {
            get { return _ChosenLanguage; }
            set
            {
                _ChosenLanguage = value;
                OnPropertyChanged(nameof(ChosenLanguage));
            }
        }

        private List<string> _DocumentLanguages = new List<string> { "None"};

        public List<string> DocumentLanguages
        {
            get { return _DocumentLanguages; }
            set
            {
                _DocumentLanguages = value;
                OnPropertyChanged(nameof(DocumentLanguages));
            }
        }

        private ObservableCollection<FontSizeListModel> _fontSizes = new ObservableCollection<FontSizeListModel>();

        public ObservableCollection<FontSizeListModel> FontSizes
        {
            get { return _fontSizes; }
            set
            {
                _fontSizes = value;
                OnPropertyChanged(nameof(FontSizes));
            }
        }

        private ObservableCollection<FontFamilyModel> _fontFamilies = new ObservableCollection<FontFamilyModel>();

        public ObservableCollection<FontFamilyModel> FontFamilies
        {
            get { return _fontFamilies; }
            set
            {
                _fontFamilies = value;
                OnPropertyChanged(nameof(FontFamilies));
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if ( CancelMainWinClosing() ) e.Cancel = true;
        }

        private bool CancelMainWinClosing()
        {
            foreach (var tab in MainTabControl.Items.OfType<TabItem>())
            {
                if (tab.Content is DockPanel dockPanel)
                {
                    var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                    if (textEditor != null && textEditor.DocumentModel.IsSaved == false)
                    {
                        return CancelMainWinDialogOptions();
                    }
                }
            }
            return false;
        }

        private bool CancelMainWinDialogOptions()
        {
            MessageBoxResult result = MessageBox.Show("You've got some unsaved doucuments. Are you sure you want to quit before saving them? " +
                "If you do so all of the changes will be erased. ", "Quit", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return false;
                case MessageBoxResult.No:
                    return true;
                case MessageBoxResult.Cancel:
                    return true;
            }
            return false;
        }


        /// 
        ///  Commands section
        /// 
        /// 


        /// Font commands


        private ICommand? _OnChangeFontSize;
        public ICommand OnChangeFontSize => _OnChangeFontSize ??= new OnChangeFontSize(this);

        private ICommand? _OnFontFamilyChanged;
        public ICommand OnFontFamilyChanged => _OnFontFamilyChanged ??= new OnFontFamilyChanged(this);

        //// Editor related commands

        private ICommand? _OnLanguageChanged;
        public ICommand OnLanguageChanged => _OnLanguageChanged ??= new OnLanguageChanged(this);

        private ICommand? _OnCodeCompletitionCommand;
        public ICommand OnCodeCompletitionCommand => _OnCodeCompletitionCommand ??= new OnCodeCompletitionCommand(this);

        private ICommand? _OnShowLineNumbersChanged;
        public ICommand OnShowLineNumbersChanged => _OnShowLineNumbersChanged ??= new OnShowLineNumbersChanged(this);

        private ICommand? _OnAlignCommand;
        public ICommand OnAlignCommand => _OnAlignCommand ??= new OnAlignCommand(this);

        /// Clipboard commands

        private ICommand? _OnPasteCommand;
        public ICommand OnPasteCommand => _OnPasteCommand ??= new OnPasteCommand(this);

        private ICommand? _OnCopyCommand;
        public ICommand OnCopyCommand => _OnCopyCommand ??= new OnCopyCommand(this);

        private ICommand? _OnCutCommand;
        public ICommand OnCutCommand => _OnCutCommand ??= new OnCutCommand(this);


        /// File manipulation commands

        private ICommand? _saveFileAsCommand;
        public ICommand SaveFileAsCommand => _saveFileAsCommand ??= new SaveFileAsCommand(this);

        private ICommand? _SaveFileCommand;
        public ICommand SaveFileCommand => _SaveFileCommand ??= new SaveFileCommand(this);

        private ICommand? _OpenFileCommand;
        public ICommand OpenFileCommand => _OpenFileCommand ??= new OpenFileCommand(this);

        private ICommand? _PrintCommand;
        public ICommand PrintCommand => _PrintCommand ??= new PrintCommand(this);

        private ICommand? _NewFileCommand;
        public ICommand NewFileCommand => _NewFileCommand ??= new NewFileCommand(this);


        /// Commands for adding and removing tabs or documents

        private ICommand? _OnRemoveTabCommand;
        public ICommand OnRemoveTabCommand => _OnRemoveTabCommand ??= new OnRemoveTabCommand(this);

        private ICommand? _OnTabSelectionChanged;
        public ICommand OnTabSelectionChanged => _OnTabSelectionChanged ??= new OnTabSelectionChanged(this);


        /// misc commands

        private ICommand? _FilterKeysUntilEnter;
        public ICommand FilterKeysUntilEnter => _FilterKeysUntilEnter ??= new FilterKeysUntilEnter(this);






    }
}
