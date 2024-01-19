using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TextEditorApp.Common;
using TextEditorApp.MainWindows.Commands;
using TextEditorApp.Utils.StaticModels;
using System.ComponentModel;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Reactive.Linq;
using TextEditorApp.Controls.ControlsModels;
using TextEditorApp.Common.Enums;
using System.Windows;
using Microsoft.CodeAnalysis;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Text;
using System.Xml;
using Color = System.Windows.Media.Color;

namespace TextEditorApp.MainWindows.WinViewModels
{
    internal class MainWinViewModel : ViewModelBase
    {
        // private members
        private TabControl? _MainTabControl;
        private ComboBox? _FontSizeComboBox;
        private bool _IsShowLineNumbers;
        private bool _IsCodeCompletitionEnabled;
        private bool _IsPrimCodeCompletionEnabled;
        private FontFamilyModel? _ChosenFontFamily;
        private CustomHorizontalTextAlignment _HorizontalTextAlignment;
        private CustomTextEditorModel? _activeTextEditor;
        private bool _IsBrowserEnabled;
        private FontSizeListModel? _selectedFontSize;
        private LanguageViewModel? _ChosenLanguage;
        private int _MainIndentationSize = 8;
        private bool _tabsToSpaces;
        private Color _selColor;
        private int _FileCount;
        private bool _IsCursive = false;
        private bool _IsBolded = false;
        private bool _IsThemeChangeEnabled = false;
        private List<string> _DocumentLanguages = new List<string> { "None" };
        private List<int> FontSizeList => new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72  };
        private ObservableCollection<FontSizeListModel> _fontSizes = new ObservableCollection<FontSizeListModel>(); 
        private ObservableCollection<FontFamilyModel> _fontFamilies = new ObservableCollection<FontFamilyModel>();

        // Constructor

        public MainWinViewModel(TabControl _MainTabControl, ComboBox _FontCombo)
        {
            MainTabControl = _MainTabControl;
            FontSizeComboBox = _FontCombo;

            RegisterCustomHighlighting();
            Init();
            UpdateWindow();
        }

        // public getters - setters
        // all are verbose enough so we won't document them one by one

        public bool IsThemeChangeEnabled
        {
            get { return _IsThemeChangeEnabled; }
            set
            {
                _IsThemeChangeEnabled = value;
                OnPropertyChanged(nameof(IsThemeChangeEnabled));
            }

        }

        public string MainIndentationSize
        {
            get { return _MainIndentationSize.ToString(); }
            set
            {
                _MainIndentationSize = int.Parse(value);
                ActiveTextEditor.IndentationSize = _MainIndentationSize;
                OnPropertyChanged(nameof(MainIndentationSize));
            }
        }

        public bool IsCursive
        {
            get { return _IsCursive; }
            set
            {
                _IsCursive = value;
                OnPropertyChanged(nameof(IsCursive));
            }
        }

        public bool IsBolded
        {
            get { return _IsBolded; }
            set
            {
                if(_IsBolded != value)
                {
                    _IsBolded = value;
                    OnPropertyChanged(nameof(IsBolded));
                }
            }
        }

        public Color SelectedFontColor
        {
            get { return _selColor; }
            set
            {
                _selColor = value;
                OnPropertyChanged(nameof(SelectedFontColor));
            }
        }

        public bool MainTabsToSpaces
        {
            get { return _tabsToSpaces; }
            set
            {
                _tabsToSpaces = value;
                ActiveTextEditor.ConvertTabsToSpaces = _tabsToSpaces;
                OnPropertyChanged(nameof(MainTabsToSpaces));
            }
        }
        public CustomHorizontalTextAlignment HorizontalTextAlignment
        {
            get { return _HorizontalTextAlignment; }
            set
            {
                _HorizontalTextAlignment = value;
                OnPropertyChanged(nameof(HorizontalTextAlignment));
            }
        }

        public bool IsBrowserEnabled
        {
            get => _IsBrowserEnabled;
            set
            {
                _IsBrowserEnabled = value;
                OnPropertyChanged(nameof(IsBrowserEnabled));
            }
        }

        public FontFamilyModel ChosenFontFamily
        {
            get { return _ChosenFontFamily ?? (_ChosenFontFamily = new FontFamilyModel()); }
            set
            {
                _ChosenFontFamily = value;
                OnPropertyChanged(nameof(ChosenFontFamily));
            }
        }

        // main currently active text editor
        public CustomTextEditorModel ActiveTextEditor
        {
            get { return _activeTextEditor ?? (_activeTextEditor = new CustomTextEditorModel()); }
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

        public bool IsPrimCodeCompletionEnabled
        {
            get { return _IsPrimCodeCompletionEnabled; }
            set
            {
                _IsPrimCodeCompletionEnabled = value;
                OnPropertyChanged(nameof(IsPrimCodeCompletionEnabled));
            }
        }

        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        public TabControl MainTabControl
        {
            get { return _MainTabControl ?? (_MainTabControl = new TabControl()); }
            set
            {
                _MainTabControl = value;
                OnPropertyChanged(nameof(_MainTabControl));
            }
        }

        public ComboBox FontSizeComboBox
        {
            get { return _FontSizeComboBox ?? (_FontSizeComboBox = new ComboBox()); }
            set
            {
                _FontSizeComboBox = value;
                OnPropertyChanged(nameof(_FontSizeComboBox));
            }
        }

        public FontSizeListModel SelectedFontSize
        {
            get { return _selectedFontSize ?? (_selectedFontSize = new FontSizeListModel()); }
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }
        public LanguageViewModel ChosenLanguage
        {
            get { return _ChosenLanguage ?? (_ChosenLanguage = new LanguageViewModel()); }
            set
            {
                _ChosenLanguage = value;
                OnPropertyChanged(nameof(ChosenLanguage));
            }
        }

        public List<string> DocumentLanguages
        {
            get { return _DocumentLanguages; }
            set
            {
                _DocumentLanguages = value;
                OnPropertyChanged(nameof(DocumentLanguages));
            }
        }

        public ObservableCollection<FontSizeListModel> FontSizes
        {
            get { return _fontSizes; }
            set
            {
                _fontSizes = value;
                OnPropertyChanged(nameof(FontSizes));
            }
        }

        public ObservableCollection<FontFamilyModel> FontFamilies
        {
            get { return _fontFamilies; }
            set
            {
                _fontFamilies = value;
                OnPropertyChanged(nameof(FontFamilies));
            }
        }
        

        //public helper functions

        // extension to OnWindowClose handler for Main Win - in practice use to check if there are some unsaved files
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (CancelMainWinClosing()) e.Cancel = true;
        }

        // main funtion that handles updates on the window upon switching tabs ond or big UI changes such as theme change
        public void UpdateWindow()
        {
            if (MainTabControl.SelectedItem is TabItem selectedTab && selectedTab.Content is DockPanel dockPanel)
            {
                var textEditor = dockPanel.Children.OfType<CustomTextEditorModel>().FirstOrDefault();
                if (textEditor != null)
                {
                    // properties related to tabs indentation are global so text editor must take them from the win/app model
                    textEditor.IndentationSize = int.Parse(MainIndentationSize);
                    textEditor.ConvertTabsToSpaces = MainTabsToSpaces;

                    // dark/white theme is also global for the app
                    textEditor.IsDarkModeEnabled = IsThemeChangeEnabled;
                    textEditor.UpdateVisualMode(
                        IsThemeChangeEnabled ? CustomTextEditorModel.Mode.dark : CustomTextEditorModel.Mode.white
                    );

                    // some properties that are displayed in the app ribbon are dependant on the tab/editor/document so 
                    // window needs to read them from the editor's model
                    ChosenLanguage = textEditor.DocumentModel.DocumentLanguage;
                    SelectedFontSize = textEditor.DocumentModel.FontSize;
                    IsShowLineNumbers = textEditor.IsShowNumbersEnabled;
                    ChosenFontFamily = textEditor.DocumentModel.DocumentFontFamily;
                    HorizontalTextAlignment = textEditor.DocumentModel.TextAlignment;
                    IsCodeCompletitionEnabled = textEditor.IsIntellisenseEnabled;
                    IsPrimCodeCompletionEnabled = textEditor.IsPrimIntellisenseEnabled;
                    SelectedFontColor = CustomTextEditorModel.ForegroundToColor(textEditor.Foreground);
                    IsBolded = (textEditor.FontWeight == FontWeights.Bold);
                    IsCursive = (textEditor.FontStyle == FontStyles.Italic);
                }
            }
        }

        // private helper functions


        // initialization of the main window and it's model - used only in constructor
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

        // used upon closing to check if there are some unsaved files and the prompt the user if so
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

        // used to prompt the user upon closing if there are some unsaved files
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

        // aux function used to integrate 2 custom snytax languages
        private void RegisterCustomHighlighting()
        {
            string relativePath = "..\\..\\..\\..\\TextEditorApp.Utils\\CustomSyntax.xsdh";
            string filePath = Path.GetFullPath(relativePath);
            string xshdContent;
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                xshdContent = reader.ReadToEnd();
            }
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            using (XmlReader xshdReader = XmlReader.Create(new StringReader(xshdContent), settings))
            {
                xshdReader.Read();
                HighlightingManager.Instance.RegisterHighlighting("CustomSyntax", new string[0], HighlightingLoader.Load(xshdReader, HighlightingManager.Instance));
            }

            // grapf language - similar to cypher

            relativePath = "..\\..\\..\\..\\TextEditorApp.Utils\\GraphSyntax.xsdh";
            filePath = Path.GetFullPath(relativePath);
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                xshdContent = reader.ReadToEnd();
            }
            settings.DtdProcessing = DtdProcessing.Ignore;
            using (XmlReader xshdReader = XmlReader.Create(new StringReader(xshdContent), settings))
            {
                xshdReader.Read();
                HighlightingManager.Instance.RegisterHighlighting("GraphSyntax", new string[0], HighlightingLoader.Load(xshdReader, HighlightingManager.Instance));
            }

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

        private ICommand? _OnFontColorChange;
        public ICommand OnFontColorChange => _OnFontColorChange ??= new OnFontColorChange(this);

        private ICommand? _OnBoldChanged;
        public ICommand OnBoldChanged => _OnBoldChanged ??= new OnBoldChanged(this);

        private ICommand? _OnItalicChanged;
        public ICommand OnItalicChanged => _OnItalicChanged ??= new OnItalicChanged(this);

        /// Theme Commands
        private ICommand? _OnThemeChange;
        public ICommand OnThemeChange => _OnThemeChange ??= new OnThemeChange(this);

        //// Editor related commands

        private ICommand? _OnLanguageChanged;
        public ICommand OnLanguageChanged => _OnLanguageChanged ??= new OnLanguageChanged(this);

        private ICommand? _OnCodeCompletitionCommand;
        public ICommand OnCodeCompletitionCommand => _OnCodeCompletitionCommand ??= new OnCodeCompletitionCommand(this);

        private ICommand? _OnPrimCodeCompletitionCommand;
        public ICommand OnPrimCodeCompletitionCommand => _OnPrimCodeCompletitionCommand ??= new OnPrimCodeCompletitionCommand(this);

        private ICommand? _OnShowLineNumbersChanged;
        public ICommand OnShowLineNumbersChanged => _OnShowLineNumbersChanged ??= new OnShowLineNumbersChanged(this);

        private ICommand? _OnAlignCommand;
        public ICommand OnAlignCommand => _OnAlignCommand ??= new OnAlignCommand(this);

        private ICommand? _OnFindClickCommand;
        public ICommand OnFindClickCommand => _OnFindClickCommand ??= new OnFindClickCommand(this);

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
