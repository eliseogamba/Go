using System;
using Go.Models;
using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage(Occurrence occurrence, Type type)
        {
            InitializeComponent();

            BindingContext = new StatisticsViewModel(occurrence, type);
        }
    }
}