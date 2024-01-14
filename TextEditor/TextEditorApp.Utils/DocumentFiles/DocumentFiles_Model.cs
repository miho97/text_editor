using TextEditorApp.Utils.StaticModels;
using System.Windows.Media;
using TextEditorApp.Common;
using TextEditorApp.Common.Enums;

namespace TextEditorApp.Utils.DocumentFiles
{

    /// <summary>
    /// Model class representing a document file with properties related to its content, styling, and file management.
    /// This class in practice represents all important informations related to document user is working on such as path on the user disc, font size, etc.
    /// </summary>
    /// 

    // since all of the properties, setters and getter are verbose enough we will only explain those that are not straightforward
    public class DocumentFiles_Model : ViewModelBase
    {
        private string? _filePath;
        private string? _fileStatus;
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

        public string FileStatus
        {
            get
            {
                return _fileStatus!;
            }

            set
            {
                _fileStatus = value;
                OnPropertyChanged(nameof(FileStatus));
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFiles_Model"/> class with default values.
        /// </summary>
        public DocumentFiles_Model()
        {
            _documentLanguage = new LanguageViewModel("None", true);
            _fileName = "Untitled.txt";
            FileStatus = "Status bar for : " + FileName;
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

        /// <summary>
        /// Gets or sets the font size used in the current document.
        /// Refers to  <see cref="FontSizeListModel"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the font used in the current document.
        /// Refers to  <see cref="FontFamilyModel"/>.
        /// </summary>

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

        /// <summary>
        /// Gets or sets the language used for text highlighting.
        /// Refers to  <see cref="LanguageViewModel"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the horizontal text alignment for the document.
        /// Default value: <see cref="CustomHorizontalTextAlignment.Left"/>.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the document is saved.
        /// Default value: true.
        /// </summary>
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                _isSaved = value;

                // If the document is not saved, append "*" to the file name to indicate unsaved changes.
                if (!IsSaved)
                {
                    if (_fileName != null && !_fileName.EndsWith("*"))
                    {
                        _fileName += "*";
                        OnPropertyChanged(nameof(FileName));
                    }
                }

                // If the document is saved, remove "*" from the file name.
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
