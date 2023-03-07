using System;
using System.Windows.Input;
using Go.Helpers;
using Go.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private string _Username;
        private string _Name;
        private string _Lastname;
        private string _Email;
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

        public string Name
        {
            get => _Name;
            set => SetValue(ref _Name, value);
        }

        public string Lastname
        {
            get => _Lastname;
            set => SetValue(ref _Lastname, value);
        }

        public string Email
        {
            get => _Email;
            set => SetValue(ref _Email, value);
        }

        public string Password
        {
            get => _Password;
            set => SetValue(ref _Password, value);
        }

        public ICommand SignUpCommand { get; private set; }

        public SignUpViewModel()
        {
            SignUpCommand = new Command(SignUp);
        }

        private async void SignUp()
        {
            if(string.IsNullOrEmpty(Username))
            {
                DisplayMessage("Debes ingresar un nombre de usuario", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                DisplayMessage("Debes ingresar tu nombre", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Lastname))
            {
                DisplayMessage("Debes ingresar tu apellido", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Email) || !Validations.ValidEmail(Email))
            {
                DisplayMessage("Debes ingresar un email válido", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                DisplayMessage("Debes ingresar tu clave", "Espera");
                return;
            }

            IsLoading = true;

            var SignUpData = await GoService.SignUp(URL, Username, Name, Lastname, Email, Encription.Encrypt(Password));

            if (SignUpData.StatusCode == System.Net.HttpStatusCode.Created)
            {
                DisplayMessage("Tu usuario se ha creado con exito", "¡Listo!");
                Back();
            }
            else
            {
                if (SignUpData.ErrorData != null)
                {
                    DisplayMessage(SignUpData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos crear tu usuario, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
    }
}