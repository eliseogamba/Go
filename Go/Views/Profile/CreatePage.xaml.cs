using System.Collections.Generic;
using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class CreatePage : ContentPage
    {
        public CreatePage(List<Category> Categories, List<Country> Countries)
        {
            InitializeComponent();

            BindingContext = new CreateViewModel(Categories, Countries);
        }
    }
}