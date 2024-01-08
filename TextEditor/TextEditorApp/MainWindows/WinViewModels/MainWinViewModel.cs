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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using TextEditorApp.Utils.StaticModels;
using System.ComponentModel;
using System.Windows.Media;
using System.Drawing;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Reactive.Linq;

namespace TextEditorApp.MainWindows.WinViewModels
{
    internal class MainWinViewModel : ViewModelBase
    {

        private FontSizeListModel? _FontSizeModel;
        //public FontSizeListModel FontSizeModel => _FontSizeModel ??= new FontSizeListModel();

        private TabControl? _MainTabControl;
        private ComboBox? _FontSizeComboBox;
        //private RibbonComboBox _FontSizeComboboxOuter;


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

            foreach (var fontFamily in Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList())
            {
                FontFamilies.Add(new FontFamilyModel(fontFamily, true));
            };
            foreach (var fontSize in FontSizeList)
            {
                FontSizes.Add(new FontSizeListModel(fontSize, true));
            };

            foreach (var textLanguage in HighlightingManager.Instance.HighlightingDefinitions )
            {
                DocumentLanguages.Add(textLanguage.Name);
            }
        }

        private void FontSizeComboBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private List<int> FontSizeList => new List<int> { 12, 16, 20 };

        private FontSizeListModel _selectedFontSize = new FontSizeListModel(12.0, true);

        public FontSizeListModel SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                _selectedFontSize = value;
                OnPropertyChanged(nameof(SelectedFontSize));
            }
        }

        public LanguageViewModel ChosenLanguage = new LanguageViewModel("None", true);

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







        private ICommand? _OnChangeFontSize;
        public ICommand OnChangeFontSize => _OnChangeFontSize ??= new OnChangeFontSize(this);

        private ICommand? _OnFontFamilyChanged;
        public ICommand OnFontFamilyChanged => _OnFontFamilyChanged ??= new OnFontFamilyChanged(this);

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

        private ICommand? _FilterKeysUntilEnter;
        public ICommand FilterKeysUntilEnter => _FilterKeysUntilEnter ??= new FilterKeysUntilEnter(this);

        private ICommand? _PrintCommand;
        public ICommand PrintCommand => _PrintCommand ??= new PrintCommand(this);

        private ICommand? _OnShowLineNumbersChanged;
        public ICommand OnShowLineNumbersChanged => _OnShowLineNumbersChanged ??= new OnShowLineNumbersChanged(this);

        private ICommand? _OnBrowserCommand;
        public ICommand OnBrowserCommand => _OnBrowserCommand ??= new OnBrowserCommand(this);




    }
}
