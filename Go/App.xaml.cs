using DLToolkit.Forms.Controls;
using Go.Common;
using Go.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ExportFont("SfProBold.otf", Alias = "SfProBold")]
[assembly: ExportFont("SfProRegular.otf", Alias = "SfProRegular")]
[assembly: ExportFont("SfProLight.otf", Alias = "SfProLight")]
namespace Go
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            FlowListView.Init();

            if (Preferences.Get(Constants.OnbordednKey, false))
            {
                if(Preferences.Get(Constants.LocationKey, null) is null)
                {
                    MainPage = new NavigationPage(new SetLocationPage())
                    {
                        BarBackgroundColor = Color.White
                    };
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage())
                    {
                        BarBackgroundColor = Color.White
                    }; 
                }
            }
            else
            {
                MainPage = new NavigationPage(new OnboardingPage())
                {
                    BarBackgroundColor = Color.White
                };
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}