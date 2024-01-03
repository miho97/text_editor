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

namespace TextEditorApp.MainWindows.WinViewModels
{
    internal class MainWinViewModel : ViewModelBase
    {

        private FontSizeListModel? _FontSizeModel;
        public FontSizeListModel FontSizeModel => _FontSizeModel ??= new FontSizeListModel();

        private TabControl? _MainTabControl;
        private ComboBox? _FontSizeComboBox;
        //private RibbonComboBox _FontSizeComboboxOuter;
        private string trenutniUnos = "";


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
            //FontSizeComboboxOuter = _FontSizeComboboxOuter;

            FontSizeComboBox.SelectionChanged += (sender, args) =>
            {
                OnChangeFontSize?.Execute(args);
            };
            foreach (var fontFamily in Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList())
            {
                FontFamilies.Add(new FontFamilyModel(fontFamily, true));
            };
        }

        private void FontSizeComboBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public List<int> FontSizeList => new List<int> { 12, 16, 20 };

        public List<FontSizeListModel> FontSizeListObjects { get; set; } = new List<FontSizeListModel>
        {
            new FontSizeListModel { FontSize = 12, IsEnabled = true },
            new FontSizeListModel { FontSize = 16, IsEnabled = true },
            new FontSizeListModel { FontSize = 20, IsEnabled = true }
        };

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

        


    }
}
