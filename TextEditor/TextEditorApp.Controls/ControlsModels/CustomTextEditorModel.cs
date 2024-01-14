using System.Windows.Controls;
using TextEditorApp.Common;
using TextEditorApp.Common.Enums;
using TextEditorApp.Utils.DocumentFiles;
using System.Windows;
using Microsoft.CodeAnalysis;
using System.Windows.Documents;
using TextEditorApp.Controls.CodeCompletion;
using ICSharpCode.AvalonEdit.Rendering;
using System.ComponentModel;

namespace TextEditorApp.Controls.ControlsModels
{

    /// <summary>
    /// Represents a custom text editor model that extends the CustomTextEditorBaseModel class.
    /// </summary>
 

    // this will be the model base for our Text Editor - the only reason we are using RoslynCodeEditor and not just simple RichTextBox is because
    // we want to expand our app with addiotional use ceses such as syntax highlighting for multiple languages, advanced code completion for C# and .NET
    // enabling/disabling code lines etc. Basic operations that are mentioned in the project description can all be done using the same logic just with RichTextBox
    // control from WPF
    public class CustomTextEditorModel : CustomTextEditorBaseModel
    {
        private DocumentFiles_Model _document;
        private DockPanel? _dockParent;
        private List<string> usedVariables;
        private int _indentetionSize = 4;
        private bool _tabsToSpaces = false;

        // used to remeber if the primitive code completion in enabled or disabled
        // here it is important because we use it to disable Adorner for code completion
        private bool uglyDirtyCompletionDisabler = false;
       

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextEditorModel"/> class.
        /// </summary>
        public CustomTextEditorModel() : base()
        {
            usedVariables = new List<string>();
            _document = new DocumentFiles_Model();

            this.Loaded += (sender, e) =>
            {
                this.Height = DockParent.ActualHeight * 0.98;
                UpdateStatusBar();
            };

            base.Options.IndentationSize = IndentationSize;
            base.Options.ConvertTabsToSpaces = ConvertTabsToSpaces;


            this.SizeChanged += (sender, e) =>
            {
                this.Height = DockParent.ActualHeight * 0.98;
            };

            // setting a few view properties
            base.ClipToBounds = true;
            base.IsBraceCompletionEnabled = true;
            base.ShowLineNumbers = false;
            base.IsReadOnly = false;
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            // adding a few functionalities to event handler that gets triggered on every 
            base.TextChanged += (sender, args) =>
            {
                DocumentModel.IsSaved = false;
                UpdateCurrentWord();
                CheckForNewVariable();
                ExecuteCodeCompletion();
            };

            base.TextArea.Caret.PositionChanged += (sender, args) =>
            {
                UpdateStatusBar();
            };
        }

        private CustomHorizontalTextAlignment _textAlignment = CustomHorizontalTextAlignment.Left;

        public CustomHorizontalTextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                
                _textAlignment = value;
                base.TextArea.TextView.HorizontalAlignment = ((HorizontalAlignment)Enum.ToObject(typeof(HorizontalAlignment), (int)value));
                OnPropertyChanged(nameof(TextAlignment));
            }
        }

        /// <summary>
        /// Gets or sets the model <see cref="DocumentFiles_Model"/> representing the document associated with the text editor.
        /// </summary>
        public DocumentFiles_Model DocumentModel
        {
            get => _document;
            init
            {
                _document = value;
                OnPropertyChanged(nameof(DocumentModel));
            }
        }

        private string _currentWord = string.Empty;

        public string CurrentWord
        {
            get { return _currentWord; }
            set
            {
                if (_currentWord != value)
                {
                    _currentWord = value;
                    OnPropertyChanged(nameof(CurrentWord));
                }
            }
        }

        public int IndentationSize
        {
            get { return _indentetionSize; }
            set
            {
                if (_indentetionSize != value)
                {
                    _indentetionSize = value;
                    base.Options.IndentationSize = _indentetionSize;
                    OnPropertyChanged(nameof(IndentationSize));
                }
            }
        }

        [DefaultValue(false)]
        /// <summary>
        /// Gets or sets if the tabs are converted to spaces in text.
        /// </summary>
        public bool ConvertTabsToSpaces
        {
            get { return _tabsToSpaces; }
            set
            {
                if (_tabsToSpaces != value)
                {
                    _tabsToSpaces = value;
                    base.Options.ConvertTabsToSpaces = _tabsToSpaces;
                    OnPropertyChanged(nameof(ConvertTabsToSpaces));
                }
            }
        }

        /// <summary>
        /// Gets or sets the dock pandel that is the visual parent of the Editor.
        /// </summary>
        public DockPanel DockParent
        {
            get { return _dockParent ?? (_dockParent = new DockPanel()); }
            set
            {
                if (_dockParent != value)
                {
                    _dockParent = value;
                    OnPropertyChanged(nameof(DockParent));
                }
            }
        }

        private bool _isShowNumbersEnabled;

        /// <summary>
        /// Gets or sets NumbersShown property.
        /// </summary>
        public bool IsShowNumbersEnabled
        {
            get => _isShowNumbersEnabled;
            set
            {
                _isShowNumbersEnabled = value;
                base.ShowLineNumbers = value;
                OnPropertyChanged(nameof(IsShowNumbersEnabled));
            }
        }
        private bool _isIntellisenseEnabled;

        /// <summary>
        /// Gets or sets the advanced code completion property.
        /// </summary>
        public bool IsIntellisenseEnabled
        {
            get => _isIntellisenseEnabled;
            set
            {
                _isIntellisenseEnabled = value;
                OnPropertyChanged(nameof(IsIntellisenseEnabled));
            }
        }

        /// <summary>
        /// Gets or sets the primitive code completion property.
        /// </summary>
        public bool IsPrimIntellisenseEnabled
        {
            get => !uglyDirtyCompletionDisabler;
            set
            {
                uglyDirtyCompletionDisabler = !value;
                OnPropertyChanged(nameof(IsPrimIntellisenseEnabled));
            }
        }

        /// <summary>
        /// Private function used to update the current active word in the editor. 
        /// </summary>
        /// 

        // basic algorithm - we look from the cursor to the beginning of the document, skip all of the whitespaces until we find an active word 
        private void UpdateCurrentWord()
        {
            int cursorPosition = base.SelectionStart;
            string text = base.Text;

            if (text == string.Empty) CurrentWord = string.Empty;

            int startOfWord = cursorPosition - 1;

            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            int endOfWord = cursorPosition;

            while (endOfWord < text.Length && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord++;
            }

            string currentWord = text.Substring(startOfWord + 1, endOfWord - startOfWord - 1);
            CurrentWord = currentWord;
        }

        /// <summary>
        /// Private function used to insert a chosen word instead of the active one.
        /// </summary>


        // In a nutshell we remove the current active word within the cursor and insert the selected word
        private void InsertSelectedWord(string selectedWord)
        {
            if (string.IsNullOrEmpty(selectedWord)) return;

            int cursorIndex = base.SelectionStart;
            string text = base.Text;

            int startOfWord = cursorIndex - 1;
            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            int endOfWord = cursorIndex;
            while (endOfWord < text.Length && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord++;
            }

            // calls to Remove/Insert will againt trigger OnTextChanged so we want to temporarily disable the completion Adorner
            var activeCompFilter = uglyDirtyCompletionDisabler;
            uglyDirtyCompletionDisabler = true;

            base.Text = text.Remove(startOfWord + 1, endOfWord - startOfWord - 1).Insert(startOfWord + 1, selectedWord + " " );

            base.SelectionStart = startOfWord + 2 + selectedWord.Length;

            uglyDirtyCompletionDisabler = activeCompFilter;
        }

        // private function called OnTextChanged event. Usef to determine if a user has specified a new variable so we can add it to the 
        // usedVariables list. 
        // Since it would be too much to write a full lexer we decided to 'call' a variable everything that comes before '=' sign.
        // So in case user writes 'private string MyPrivateString = "some_text";' we would have a new variable MyPrivateString and 
        // it will be added to usedVariables
        // For the obvious reasons we only want to remember the variable once
        private void CheckForNewVariable()
        {
            if (CurrentWord != "=" || base.Text == string.Empty) return;

             int cursorIndex = base.SelectionStart;
            string text = base.Text;

            int endOfWord = cursorIndex - 2;
            var currentChar = text[endOfWord];
            if (!char.IsLetterOrDigit(currentChar) && !char.IsWhiteSpace(currentChar)) return;
            while (endOfWord >= 0 && !char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord--;
            }

            while (endOfWord >= 0 && char.IsWhiteSpace(text[endOfWord]))
            {
                endOfWord--;
            }

            int startOfWord = endOfWord;
            while (startOfWord >= 0 && !char.IsWhiteSpace(text[startOfWord]))
            {
                startOfWord--;
            }

            if (startOfWord >= 0)
            {
                string variableName = text.Substring(startOfWord + 1, endOfWord - startOfWord);
                if(!usedVariables.Contains(variableName))
                {
                    usedVariables.Add(variableName);
                }
            }

        }

        // private helper function that is used to determine which keywords and variables should be presented in the code completion combobox
        //  here we use a string query to find all of the possible matches in usedVariables and in CSKeywords.AllKeywords
        private List<string> GetMatchingKeywords()
        {
            if (CurrentWord == string.Empty)  {
                RemoveAllAdorners();
                return new List<string>(); ;
            }
            return usedVariables
                .Concat(CSKeywords.AllKeywords)
                .Where(w => w.StartsWith(CurrentWord, StringComparison.OrdinalIgnoreCase) || w.Contains(CurrentWord, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // handler that ckecks if we have some possible matches for our current word and calls the Adorner
        private void ExecuteCodeCompletion()
        {
            if (uglyDirtyCompletionDisabler) return;
            if (GetMatchingKeywords() is List<string> keywords && keywords.Count > 0 && DockParent is Panel panel)
            {
                AddKeywordsAdorner(keywords);
            }
        }

        // function that handles the logic of creation and adding the Adorner to the view
        private void AddKeywordsAdorner(List<string> keywords)
        {
            // first we remove all of the Adorner if such exists which includes previous code suggestions
            RemoveAllAdorners();

            // here we determine the position of the Adorner
            // we want it to be right below the current letx meaning the caret position but it is needed to convert to position of the carret to 
            // coordinate values
            var caretPosition = base.TextArea.Caret.Position;
            var caretVisualPosition = base.TextArea.TextView.GetVisualPosition(caretPosition, VisualYPosition.LineTop);

            // a new CustomCompletionAdorner is created and added to the Adorner layer of the text editor control
            var area = this;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(area);
            var comboBoxAdorner = new CustomCompletionAdorner(area, caretVisualPosition, CreateComboForAdorner(keywords));
            adornerLayer.Add(comboBoxAdorner);
        }

        // basically a small custom combobox class packed in a function
        // this combobox will contain all of the code suggestions 
        // and will render inside of the Adorner
        private ComboBox CreateComboForAdorner(List<string> keywords)
        {
            var comboBox = new ComboBox();
            comboBox.ItemsSource = keywords;
            comboBox.IsEnabled = true;

            // if the user selects something from the list of suggested words that word will be inserted, 
            // combobox will close, Adorner will be destroyed and text editor will again get focused
            comboBox.SelectionChanged += (sender, args) =>
            {
                if (comboBox.SelectedValue != null)
                {
                    InsertSelectedWord(comboBox.SelectedValue.ToString() ?? string.Empty);
                    comboBox.IsDropDownOpen = false;
                    comboBox.IsEnabled = false;
                    RemoveAllAdorners();
                    this.Focus();
                    this.TextArea.Focus();
                }
            };

            return comboBox;
        }

        // helper function used to remove all adorners in the adorner layer of the text editor control 
        private void RemoveAllAdorners()
        {
            var area = this;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(area);
            Adorner[] adorners = adornerLayer.GetAdorners(area);

            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }

        // updates status bar at the bottom of the page
        private void UpdateStatusBar()
        {
            this.DocumentModel.FileStatus = "Status bar for: " +  this.DocumentModel.FileName + "\t" + "length: " + this.Text.Length + " lines: " + this.LineCount + "\t\t" +
                 " pos : " + this.TextArea.Caret.Location;
        }
    }
}
