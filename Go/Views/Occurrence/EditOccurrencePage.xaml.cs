using System;
using System.Collections.Generic;
using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class EditOccurrencePage : ContentPage
    {
        public EditOccurrencePage(Occurrence occurrence, Type type, List<Category> categories, List<Country> countries, List<Province> provinces, List<Location> locations)
        {
            InitializeComponent();

            BindingContext = new EditOccurrenceViewModel(occurrence, type, categories, countries, provinces, locations);
        }
    }
}