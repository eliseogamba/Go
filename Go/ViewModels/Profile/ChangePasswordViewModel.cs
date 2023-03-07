using System;
using System.Windows.Input;
using Go.Helpers;
using Go.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private bool _IsLoading;
        private string _OldPassword;
        private string _NewPassword;
        private string _RepeatPassword;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public string OldPassword
        {
            get => _OldPassword;
            set => SetValue(ref _OldPassword, value);
        }

        public string NewPassword
        {
            get => _NewPassword;
            set => SetValue(ref _NewPassword, value);
        }

        public string RepeatPassword
        {
            get => _RepeatPassword;
            set => SetValue(ref _RepeatPassword, value);
        }

        public ICommand ChangePasswordCommand { get; private set; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new Command(ChangePassword);
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                DisplayMessage("Debes ingresar tu clave anterior", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                DisplayMessage("Debes ingresar tu nueva clave", "Espera");
                return;
            }

            if (string.IsNullOrEmpty(RepeatPassword))
            {
                DisplayMessage("Debes ingresar tu nueva clave otra vez", "Espera");
                return;
            }

            if(!NewPassword.Equals(RepeatPassword))
            {
                DisplayMessage("Las claves no coinciden", "Espera");
                return;
            }

            IsLoading = true;

            var ChangePasswordData = await GoService.ChangePassword(URL, Encription.Encrypt(OldPassword), Encription.Encrypt(NewPassword), Token);

            if (ChangePasswordData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DisplayMessage("Se ha cambiado tu clave con éxito", "¡Listo!");
                Back();
            }
            else
            {
                if (ChangePasswordData.ErrorData != null)
                {
                    DisplayMessage(ChangePasswordData.ErrorData.Message, "Tuvimos un problema");
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