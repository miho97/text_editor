using TextEditorApp.Utils.StaticModels;
using System.Windows.Media;
using TextEditorApp.Common;
using TextEditorApp.Common.Enums;

namespace TextEditorApp.Utils.DocumentFiles
{
    public class DocumentFiles_Model : ViewModelBase
    {
        private string? _filePath;
        public string FilePath
        {
            get
            {
                return _filePath!;
            }

            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        private string? _fileName;
        public string FileName
        {
            get
            {
                return _fileName!;
            }

            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        public DocumentFiles_Model()
        {
            _documentLanguage = new LanguageViewModel("None", true);
            _fileName = "Untitled.txt";
            IsSaved = true;
        }

        private string _content = string.Empty;

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        private FontSizeListModel _fontSize = new FontSizeListModel(12, true);

        public FontSizeListModel FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
        private FontFamilyModel _documentFontFamily = new FontFamilyModel(new FontFamily("Times New Roman"), true);

        public FontFamilyModel DocumentFontFamily
        {
            get => _documentFontFamily;
            set
            {
                _documentFontFamily = value;
                OnPropertyChanged(nameof(DocumentFontFamily));
            }
        }
        private LanguageViewModel _documentLanguage;

        public LanguageViewModel DocumentLanguage
        {
            get => _documentLanguage;
            set
            {
                _documentLanguage = value;
                OnPropertyChanged(nameof(DocumentLanguage));
            }
        }
        private CustomHorizontalTextAlignment _textAlignment = CustomHorizontalTextAlignment.Left;

        public CustomHorizontalTextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                _textAlignment = value;
                OnPropertyChanged(nameof(TextAlignment));
            }
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private bool _isSaved;

        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                _isSaved = value;
                if (!IsSaved)
                {
                    if (_fileName != null && !_fileName.EndsWith("*"))
                    {
                        _fileName += "*";
                        OnPropertyChanged(nameof(FileName));
                    }
                }
                else
                {
                    if (_fileName != null &&  _fileName.EndsWith("*"))
                    {
                        _fileName = _fileName.Substring(0, _fileName.Length - 1);
                        OnPropertyChanged(nameof(FileName));
                    }
                }
                OnPropertyChanged(nameof(IsSaved));
            }
        }

    }
}
