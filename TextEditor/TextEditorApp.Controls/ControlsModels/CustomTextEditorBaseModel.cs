using RoslynPad.Editor;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TextEditorApp.Controls.ControlsModels
{
    /// <summary>
    /// Represents a custom base model for a text editor that extends the <see cref="RoslynCodeEditor"/> class
    /// and implements the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    /// 

    // this will be the base for our Text Editor - the only reason we are using RoslynCodeEditor and not just simple RichTextBox is because
    // we want to expand our app with addiotional use ceses such as syntax highlighting for multiple languages, advanced code completion for C# and .NET
    // enabling/disabling code lines etc. Basic operations that are mentioned in the project description can all be done using the same logic just with RichTextBox
    // control from WPF
    public class CustomTextEditorBaseModel : RoslynCodeEditor, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            if (name != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
