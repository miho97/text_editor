using RoslynPad.Editor;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TextEditorApp.Controls.ControlsModels
{
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
