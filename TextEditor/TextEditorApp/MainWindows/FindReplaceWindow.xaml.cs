using ICSharpCode.AvalonEdit;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace TextEditorApp
{
    /// <summary>
    /// Interaction logic for FindReplaceWindow.xaml
    /// </summary>
    public partial class FindReplaceWindow : Window
    {
        public string SearchText { get; private set; } = string.Empty;
        public string ReplaceText { get; private set; } = string.Empty;
        public bool MatchCase { get; private set; }
        public bool NextClicked { get; private set; }
        public bool PreviousClicked { get; private set; }
        public bool FindNext { get; private set; }
        private bool replacementMade = false;
        public bool CircularSearch { get; private set; }
        private int totalMatches = 0;
        private int currentMatchIndex = 0;


        private readonly TextEditor textEditor;
        private int currentIndex = -1;

        public FindReplaceWindow(TextEditor textEditor)
        {
            InitializeComponent();
            this.textEditor = textEditor;
            UpdateHighlighting();
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            FindNext = chkFindNext.IsChecked ?? false;

            if (FindNext)
            {
                replacementMade = FindAndReplaceNext();
            }
            else
            {
                ReplaceSelectedText();
            }
        }

        private void ReplaceSelectedText()
        {
            if (textEditor != null && textEditor.SelectionLength > 0)
            {
                textEditor.Document.Replace(textEditor.SelectionStart, textEditor.SelectionLength, ReplaceText);
            }
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtFind.Text;
            string replaceText = txtReplace.Text;

            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            string editorText = textEditor.Text;

            string newText = MatchCase
                ? editorText.Replace(searchText, replaceText)
                : Regex.Replace(editorText, searchText, replaceText, RegexOptions.IgnoreCase);

            textEditor.Document.Text = newText;
         }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;
            FindNext = chkFindNext.IsChecked ?? false;

            NextClicked = true;

            UpdateHighlighting();

            if (FindNext && chkFindNext.IsChecked == true)
            {
                ReplaceTextInTextEditor();
            }
        }

        private void ReplaceTextInTextEditor()
        {
            if (textEditor != null)
            {
                int selectionStart = textEditor.SelectionStart;
                int selectionLength = textEditor.SelectionLength;

                if (selectionLength > 0)
                {
                    textEditor.Document.Replace(selectionStart, selectionLength, ReplaceText);

                    // Move the cursor to the end of the replaced text
                    textEditor.Select(selectionStart + ReplaceText.Length, 0);
                }
            }
        }

        private bool FindAndReplaceNext()
        {
            if (textEditor != null)
            {
                string searchText = SearchText;
                StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

                if (currentIndex != -1)
                {
                    textEditor.Select(currentIndex, searchText.Length);
                    textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);

                    ReplaceTextInTextEditor();

                    return true; //Found and replaced, continue searching
                }
                else
                {
                    MessageBox.Show("No more matches found", "Replace");
                    currentIndex = -1; // Reset for the next search
                    return false; //No more matches found, stop searching
                }
            }

            return false;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            SearchText = txtFind.Text;
            ReplaceText = txtReplace.Text;
            MatchCase = chkMatchCase.IsChecked ?? false;

            PreviousClicked = true;

            UpdateHighlighting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Delay setting DialogResult until the window is loaded
            DialogResult = NextClicked; // Set DialogResult based on whether "Next" button was clicked
        }

        public bool FindAndHighlight()
        {
            ShowDialog();

            return NextClicked;
        }

        private void UpdateHighlighting()
        {
            if (textEditor != null)
            {
                HighlightMatches();
            }
        }

        private void HighlightMatches()
        {
            if (textEditor != null)
            {
                textEditor.TextArea.TextView.LineTransformers.Clear();

                if (NextClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

                    // Wrap around to the beginning if not found
                    if (currentIndex == -1)
                    {
                        currentIndex = textEditor.Text.IndexOf(searchText, 0, comparison);
                    }

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentIndex = -1; 
                    }

                    NextClicked = false; // Reset for the next iteration
                }
                else if (PreviousClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    currentIndex = textEditor.Text.LastIndexOf(searchText, Math.Max(0, currentIndex - 1), comparison);

                    // Wrap around to the end if not found
                    if (currentIndex == -1)
                    {
                        currentIndex = textEditor.Text.LastIndexOf(searchText, textEditor.Text.Length - 1, comparison);
                    }

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentIndex = textEditor.Text.Length; // Reset for the next search
                    }

                    PreviousClicked = false; // Reset for the next iteration
                }
            }
        }
    }
}