using System;
using Go.Models;
using Go.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace Go.Views
{
    public partial class OptionsPopup : PopupPage
    {
        public OptionsPopup(Occurrence occurrence, Type type)
        {
            InitializeComponent();

            BindingContext = new OptionsPopupViewModel(occurrence, type);
        }
    }
}