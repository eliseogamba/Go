using System;
using System.Windows.Input;
using Go.Helpers;
using Go.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private string _Email;
        private string _Password;
        private string _RepeatPassword;
        private string _Code;

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

        public string Password
        {
            get => _Password;
            set => SetValue(ref _Password, value);
        }

        public string RepeatPassword
        {
            get => _RepeatPassword;
            set => SetValue(ref _RepeatPassword, value);
        }

        public string Code
        {
            get => _Code;
            set => SetValue(ref _Code, value);
        }

        public ICommand ResetPasswordCommand { get; private set; }

        public ResetPasswordViewModel(string Email)
        {
            this.Email = Email;

            ResetPasswordCommand = new Command(ResetPassword);
        }

        private async void ResetPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                DisplayMessage("Debes ingresar tu nueva clave", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(RepeatPassword))
            {
                DisplayMessage("Debes ingresar repetir tu nueva clave", "Espera");
                return;
            }

            if(!Password.Equals(RepeatPassword))
            {
                DisplayMessage("Las claves deben coincidir", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(Code))
            {
                DisplayMessage("Debes el código", "Espera");
                return;
            }

            IsLoading = true;

            var ResetPasswordData = await GoService.ResetPassword(URL, Email, Encription.Encrypt(Password), Code);

            if (ResetPasswordData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DisplayMessage("Se ha cambiado tu clave con éxito", "¡Listo!");
                PopToRoot();
            }
            else
            {
                if (ResetPasswordData.ErrorData != null)
                {
                    DisplayMessage(ResetPasswordData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos cambiar tu clave, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
    }
}