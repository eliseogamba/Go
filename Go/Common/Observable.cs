using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Go.Common
{
    public class Observable : INotifyPropertyChanged
    {
        public void SetValue<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            variable = value;
            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}