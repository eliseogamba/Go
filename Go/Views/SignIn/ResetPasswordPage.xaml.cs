using Go.ViewModels;
using Xamarin.Forms;

namespace Go.Views
{
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage(string Email)
        {
            InitializeComponent();

            BindingContext = new ResetPasswordViewModel(Email);
        }
    }
}