using System.Windows.Input;
using Go.Common;
using Go.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class OnboardingViewModel : ViewModelBase
    {
        private bool _FirstOnboardingVisible;
        private bool _SecondOnboardingVisible;

        public bool FirstOnboardingVisible
        {
            get => _FirstOnboardingVisible;
            set => SetValue(ref _FirstOnboardingVisible, value);
        }

        public bool SecondOnboardingVisible
        {
            get => _SecondOnboardingVisible;
            set => SetValue(ref _SecondOnboardingVisible, value);
        }

        public ICommand ShowOnboadringTwoCommand { get; set; }
        public ICommand StartAppCommand { get; set; }

        public OnboardingViewModel()
        {
            FirstOnboardingVisible = true;

            Preferences.Set(Constants.OnbordednKey, true);

            ShowOnboadringTwoCommand = new Command(ShowOnboadringTwo);
            StartAppCommand = new Command(StartApp);
        }

        private void ShowOnboadringTwo()
        {
            FirstOnboardingVisible = false;
            SecondOnboardingVisible = true;
        }

        private void StartApp()
        {
            NavigateTo(new SetLocationPage());
        }
    }
}