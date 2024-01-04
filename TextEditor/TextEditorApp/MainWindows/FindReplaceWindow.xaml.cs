using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                // Replace the selected text
                textEditor.Document.Replace(textEditor.SelectionStart, textEditor.SelectionLength, ReplaceText);
            }
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            // Get the search and replace strings
            string searchText = txtFind.Text;
            string replaceText = txtReplace.Text;

            // Set up the StringComparison based on MatchCase
            StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            // Get the entire text content of the editor
            string editorText = textEditor.Text;

            // Perform a case-sensitive or case-insensitive replace all
            string newText = MatchCase
                ? editorText.Replace(searchText, replaceText)
                : Regex.Replace(editorText, searchText, replaceText, RegexOptions.IgnoreCase);

            // Replace the entire content of the editor with the modified text
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

            // Update the highlighting without closing the window
            UpdateHighlighting();

            // Replace the found text if "Replace" button is checked
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
                    // Replace the selected text
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

                // Start searching from the current index + 1
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

            // Update the highlighting without closing the window
            UpdateHighlighting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Delay setting DialogResult until the window is loaded
            DialogResult = NextClicked; // Set DialogResult based on whether "Next" button was clicked
        }

        public bool FindAndHighlight()
        {
            // Show the window as a dialog
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

                    // Start searching from the current index + 1
                    currentIndex = textEditor.Text.IndexOf(searchText, currentIndex + 1, comparison);

                    if (currentIndex != -1)
                    {
                        textEditor.Select(currentIndex, searchText.Length);
                        textEditor.ScrollToLine(textEditor.Document.GetLineByOffset(currentIndex).LineNumber);
                    }
                    else
                    {
                        MessageBox.Show("No more matches found", "Find");
                        currentIndex = -1; // Reset for the next search
                    }

                    NextClicked = false; // Reset for the next iteration
                }
                else if (PreviousClicked)
                {
                    string searchText = SearchText;
                    StringComparison comparison = MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                    // Search in reverse order, starting from the current index - 1
                    currentIndex = textEditor.Text.LastIndexOf(searchText, Math.Max(0, currentIndex - 1), comparison);

                    if (currentIndex >= 0)
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