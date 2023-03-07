using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Go.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class OptionsPopupViewModel : ViewModelBase
    {
        protected static List<Category> _categories;
        protected static List<Country> _countries;
        protected static List<Province> _provinces;
        protected static List<Location> _locations;

        private bool _IsLoading;
        private Occurrence _occurrence;
        private readonly Type _type;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public Occurrence Occurrence
        {
            get => _occurrence;
            set => SetValue(ref _occurrence, value);
        }

        public ICommand StatusCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand StatisticsCommand { get; private set; }

        public OptionsPopupViewModel(Occurrence occurrence, Type type)
        {
            _occurrence = occurrence;
            _type = type;

            StatusCommand = new Command(Status);
            EditCommand = new Command(Edit);
            StatisticsCommand = new Command(Statistics);
        }

        private async void Status()
        {
            bool status;
            string title;
            string description;

            if(Occurrence.Active)
            {
                status = false;
                title = "Desactivar";
                description = "¿Seguro que deseas desactivar?";
            }
            else
            {
                status = true;
                title = "Activar";
                description = "¿Seguro que deseas activar?";
            }

            if (await DisplayQuestion(title, description))
            {
                IsLoading = true;

                if(_type == typeof(Event))
                {
                    var EventStatus = await GoService.ChangeEventStatus(URL, _occurrence, status, Token);

                    if (EventStatus.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _occurrence.Active = status;

                        DisplayMessage(string.Empty, "¡Hecho!");
                    }
                    else
                    {
                        if (EventStatus.ErrorData != null)
                        {
                            DisplayMessage(EventStatus.ErrorData.Message, "Tuvimos un problema");
                        }
                        else
                        {
                            DisplayMessage("No pudimos cambiar el estado, intentalo nuevamente", "Tuvimos un problema");
                        }
                    }
                }
                else
                {
                    var ActivitiyStatus = await GoService.ChangeActivityStatus(URL, _occurrence, status, Token);

                    if (ActivitiyStatus.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _occurrence.Active = status;

                        DisplayMessage(string.Empty, "¡Hecho!");
                    }
                    else
                    {
                        if (ActivitiyStatus.ErrorData != null)
                        {
                            DisplayMessage(ActivitiyStatus.ErrorData.Message, "Tuvimos un problema");
                        }
                        else
                        {
                            DisplayMessage("No pudimos cambiar el estado, intentalo nuevamente", "Tuvimos un problema");
                        }
                    }
                }
            }

            IsLoading = false;

            await PopupNavigation.Instance.PopAsync();
        }

        private async void Edit()
        {
            IsLoading = true;

            if (_categories is null)
            {
                var CategoryData = await GoService.GetCategories(URL);

                if (CategoryData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _categories = CategoryData.Data;
                }
                else
                {
                    if (CategoryData.ErrorData != null)
                    {
                        DisplayMessage(CategoryData.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos recuperar las categorías, intentalo nuevamente", "Tuvimos un problema");
                    }

                    return;
                }
            }

            if(_countries is null)
            {
                var CountryData = await GoService.GetCountries(URL);

                if (CountryData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _countries = CountryData.Data;
                }
                else
                {
                    if (CountryData.ErrorData != null)
                    {
                        DisplayMessage(CountryData.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos recuperar los paises, intentalo nuevamente", "Tuvimos un problema");
                    }

                    return;
                }
            }

            if (_provinces is null || _provinces.First().Country.Id != _occurrence.Location.Province.Country.Id)
            {
                var ProviceData = await GoService.GetProvinces(URL, _occurrence.Location.Province.Country.Id);

                if (ProviceData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _provinces = ProviceData.Data;
                }
                else
                {
                    if (ProviceData.ErrorData != null)
                    {
                        DisplayMessage(ProviceData.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos recuperar las provincias, intentalo nuevamente", "Tuvimos un problema");
                    }

                    return;
                }
            }

            if (_locations is null || _locations.First().Province.Id != _occurrence.Location.Province.Id)
            {
                var LocationData = await GoService.SearchingForLocations(URL, ProvinceID: _occurrence.Location.Province.Id);

                if (LocationData.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _locations = LocationData.Data;
                }
                else
                {
                    if (LocationData.ErrorData != null)
                    {
                        DisplayMessage(LocationData.ErrorData.Message, "Tuvimos un problema");
                    }
                    else
                    {
                        DisplayMessage("No pudimos recuperar las localidades, intentalo nuevamente", "Tuvimos un problema");
                    }

                    return;
                }
            }

            await PopupNavigation.Instance.PopAsync();

            NavigateTo(new EditOccurrencePage(_occurrence, _type, _categories, _countries, _provinces, _locations));

            IsLoading = false;
        }

        private async void Statistics()
        {
            await PopupNavigation.Instance.PopAsync();

            NavigateTo(new StatisticsPage(_occurrence, _type));
        }
    }
}