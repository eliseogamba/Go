using System;
using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class LocationsPage : ContentPage
    {
        public LocationsPage(Action<Location> LocationPicked)
        {
            InitializeComponent();

            BindingContext = new LocationsViewModel(LocationPicked);
        }
    }
}