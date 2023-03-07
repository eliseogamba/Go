using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class OccurrencePage : ContentPage
    {
        public OccurrencePage(Occurrence Occurrence)
        {
            InitializeComponent();

            BindingContext = new OccurrenceViewModel(Occurrence, map);
        }
    }
}