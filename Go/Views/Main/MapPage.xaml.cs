using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            BindingContext = new MapViewModel(map, popup);
        }
    }
}