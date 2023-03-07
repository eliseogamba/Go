using System.Windows.Input;
using Go.Common;
using Xamarin.Forms;

namespace Go.Models
{
    public class Category : Observable
    {
        private bool _IsSelected;

        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public bool IsSelected
        {
            get => _IsSelected;
            set => SetValue(ref _IsSelected, value);
        }

        public ICommand SelectionToogleCommand { get; private set; }

        public Category()
        {
            SelectionToogleCommand = new Command(SelectionToogle);
        }

        private void SelectionToogle()
        {
            IsSelected = !IsSelected;
        }
    }
}