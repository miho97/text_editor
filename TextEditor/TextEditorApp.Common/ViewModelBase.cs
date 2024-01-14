using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace TextEditorApp.Common
{
    /// <summary>
    /// A base class for view models that implements the INotifyPropertyChanged interface.
    /// </summary>
    /// 

    // since we are trying to build this app using the MVVM design it is probable that we will have many view models
    // that will need to automatically notify the UI when some data is changed. That is the reason we created this base class that can be inherited if needed
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed. Defaults to the calling member's name.</param>
        public void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            if (name != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
