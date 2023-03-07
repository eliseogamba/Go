using System.Windows.Input;
using Go.Services;
using Go.Views;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class ForgetPasswordViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private string _Email;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public string Email
        {
            get => _Email;
            set => SetValue(ref _Email, value);
        }

        public ICommand ContinueCommand { get; private set; }

        public ForgetPasswordViewModel()
        {
            ContinueCommand = new Command(Continue);
        }

        private async void Continue()
        {
            if(string.IsNullOrEmpty(Email))
            {
                DisplayMessage("Debes ingresar un email", "Espera");
                return;
            }

            IsLoading = true;

            var ForgetPasswordData = await GoService.ForgetPassword(URL, Email);

            if(ForgetPasswordData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DisplayMessage("Te enviamos un código a tu email para reestablecer tu clave", "Un paso mas");
                NavigateTo(new ResetPasswordPage(Email));
            }
            else
            {
                if(ForgetPasswordData.ErrorData != null)
                {
                    DisplayMessage(ForgetPasswordData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos enviarte un emai, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
    }
}