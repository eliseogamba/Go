using System;
using System.Collections.Generic;
using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class FilterPage : ContentPage
    {
        public FilterPage(Action<TimeRange, List<Category>> ApplyFiltersCallback, bool HideWhen = false)
        {
            InitializeComponent();

            BindingContext = new FilterViewModel(ApplyFiltersCallback, HideWhen);
        }
    }
}