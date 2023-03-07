using System.Windows.Input;
using Go.Common;
using Go.Helpers;
using Go.Services;
using Go.Views;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private string _Username;
        private string _Password;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public string Username
        {
            get => _Username;
            set => SetValue(ref _Username, value);
        }

        public string Password
        {
            get => _Password;
            set => SetValue(ref _Password, value);
        }

        public ICommand ForgetPasswordCommand { get; private set; }
        public ICommand SignUpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            ForgetPasswordCommand = new Command(ForgetPassword);
            SignUpCommand = new Command(SignUp);
            LoginCommand = new Command(Login);
        }

        private void ForgetPassword()
        {
            NavigateTo(new ForgetPasswordPage());
        }

        private void SignUp()
        {
            NavigateTo(new SignUpPage());
        }

        private async void Login()
        {
            if(string.IsNullOrEmpty(Username))
            {
                DisplayMessage("Debes ingresar tu nombre de usuario", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                DisplayMessage("Debes ingresar tu clave", "Espera");
                return;
            }

            IsLoading = true;

            var LoginData = await GoService.Login(URL, Username, Encription.Encrypt(Password));

            if (LoginData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Xamarin.Essentials.Preferences.Set(Constants.TokenKey, LoginData.Data.User.Token);

                MessagingCenter.Send<ViewModelBase>(this, "TokenUpdated");
            }
            else
            {
                if (LoginData.ErrorData != null)
                {
                    DisplayMessage(LoginData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos logearte, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
    }
}