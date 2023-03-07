using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Go.Models;
using Go.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Go.ViewModels
{
    public class EditOccurrenceViewModel : ViewModelBase
    {
        private string _pathPhoto = string.Empty;

        private bool _IsLoading;
        private Occurrence _Occurrence;
        private Type _Type;
        private string _PhotoName;
        private List<Category> _Categories;
        private Category _SelectedCategory;
        private bool _IsSameDay;
        private TimeSpan _TimeStart;
        private TimeSpan _TimeEnd;
        private List<Country> _Countries;
        private Country _SelectedCountry;
        private List<Province> _Provinces;
        private Province _SelectedProvince;
        private List<Models.Location> _Locations;
        private Models.Location _SelectedLocation;

        public bool IsLoading
        {
            get => _IsLoading;
            set => SetValue(ref _IsLoading, value);
        }

        public Occurrence Occurrence
        {
            get => _Occurrence;
            set => SetValue(ref _Occurrence, value);
        }

        public Type Type
        {
            get => _Type;
            set => SetValue(ref _Type, value);
        }

        public string PhotoName
        {
            get => _PhotoName;
            set => SetValue(ref _PhotoName, value);
        }

        public List<Category> Categories
        {
            get => _Categories;
            set => SetValue(ref _Categories, value);
        }

        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set => SetValue(ref _SelectedCategory, value);
        }

        public bool IsSameDay
        {
            get => _IsSameDay;
            set => SetValue(ref _IsSameDay, value);
        }

        public TimeSpan TimeStart
        {
            get => _TimeStart;
            set => SetValue(ref _TimeStart, value);
        }

        public TimeSpan TimeEnd
        {
            get => _TimeEnd;
            set => SetValue(ref _TimeEnd, value);
        }

        public List<Country> Countries
        {
            get => _Countries;
            set => SetValue(ref _Countries, value);
        }

        public Country SelectedCountry
        {
            get => _SelectedCountry;
            set
            {
                if (value == null || value.Id == _SelectedCountry?.Id)
                    return;

                _SelectedCountry = value;
                OnPropertyChanged(nameof(_SelectedCountry));

                Task.Run(LoadProvinces);
            }
        }

        public List<Province> Provinces
        {
            get => _Provinces;
            set => SetValue(ref _Provinces, value);
        }

        public Province SelectedProvince
        {
            get => _SelectedProvince;
            set
            {
                if (value == null || value.Id == _SelectedProvince?.Id)
                    return;

                _SelectedProvince = value;
                OnPropertyChanged(nameof(SelectedProvince));

                Task.Run(LoadLocations);
            }
        }

        public List<Models.Location> Locations
        {
            get => _Locations;
            set => SetValue(ref _Locations, value);
        }

        public Models.Location SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                if (value == null || value.Id == _SelectedLocation?.Id)
                    return;

                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        public ICommand PickPhotoCommand { get; private set; }
        public ICommand EditCommand { get; private set; }

        public EditOccurrenceViewModel(Occurrence ocurrence, Type type, List<Category> categories, List<Country> countries, List<Province> provinces, List<Models.Location> locations)
        {
            Categories = categories;
            Countries = countries;
            Provinces = provinces;
            Locations = locations;

            Occurrence = ocurrence;
            Type = type;

            if (Occurrence.DatetimeStart.HasValue)
            {
                TimeStart = new TimeSpan(Occurrence.DatetimeStart.Value.Hour, Occurrence.DatetimeStart.Value.Minute, Occurrence.DatetimeStart.Value.Second);
            }

            if (Occurrence.DatetimeEnd.HasValue)
            {
                TimeStart = new TimeSpan(Occurrence.DatetimeEnd.Value.Hour, Occurrence.DatetimeEnd.Value.Minute, Occurrence.DatetimeEnd.Value.Second);
            }

            if(Occurrence.DatetimeStart == Occurrence.DatetimeEnd)
            {
                IsSameDay = true;
            }

            SelectedCategory = Categories.Find(x => x.Id == Occurrence.Category.Id);

            _SelectedCountry = Countries.Find(x => x.Id == Occurrence.Location.Province.Country.Id);
            _SelectedProvince = Provinces.Find(x => x.Id == Occurrence.Location.Province.Id);
            _SelectedLocation = Locations.Find(x => x.Id == Occurrence.Location.Id);

            PhotoName = Occurrence.Photo.Split('/').Last();

            PickPhotoCommand = new Command(PickPhoto);
            EditCommand = new Command(EditOccrrence);
        }

        private async void LoadProvinces()
        {
            IsLoading = true;

            var ProvinceData = await GoService.GetProvinces(URL, SelectedCountry.Id);

            if (ProvinceData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Provinces = ProvinceData.Data;
            }
            else
            {
                if (ProvinceData.ErrorData != null)
                {
                    DisplayMessage(ProvinceData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos recuperar las provincias, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }

        private async void LoadLocations()
        {
            IsLoading = true;

            var LocationData = await GoService.SearchingForLocations(URL, ProvinceID: SelectedProvince.Id);

            if (LocationData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Locations = LocationData.Data;
            }
            else
            {
                if (LocationData.ErrorData != null)
                {
                    DisplayMessage(LocationData.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos recuperar las ciudades, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }

        private async void PickPhoto()
        {
            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.MaxWidthHeight, MaxWidthHeight = 500 }).ConfigureAwait(true);

            if (file == null)
                return;

            Stream stream = file.GetStream();
            file.Dispose();

            _pathPhoto = ((FileStream)stream).Name;

            PhotoName = _pathPhoto.Split('/').Last();
        }

        private async void EditOccrrence()
        {
            Tuple<bool, string> occurrenceValidation;

            if (Type == typeof(Event))
            {
                Occurrence.DatetimeStart = new DateTime(Occurrence.DatetimeStart.Value.Year, Occurrence.DatetimeStart.Value.Month, Occurrence.DatetimeStart.Value.Day, TimeStart.Hours, TimeStart.Minutes, TimeStart.Seconds);

                if (IsSameDay)
                {
                    Occurrence.DatetimeEnd = Occurrence.DatetimeStart;
                }
                else
                {
                    Occurrence.DatetimeEnd = new DateTime(Occurrence.DatetimeEnd.Value.Year, Occurrence.DatetimeEnd.Value.Month, Occurrence.DatetimeEnd.Value.Day, TimeEnd.Hours, TimeEnd.Minutes, TimeEnd.Seconds);
                }

                occurrenceValidation = Occurrence.Validate(false, IsSameDay);
            }
            else
            {
                occurrenceValidation = Occurrence.Validate(false);
            }

            if (!occurrenceValidation.Item1)
            {
                DisplayMessage(occurrenceValidation.Item2, "Espera");
                return;
            }

            if (Occurrence.DatetimeEnd < Occurrence.DatetimeStart)
            {
                DisplayMessage("La fecha de fin debe ser luego de la fecha de inicio", "Espera");
            }

            if (SelectedCategory is null)
            {
                DisplayMessage("Debes seleccionar una categoría", "Espera");
                return;
            }

            if (SelectedCountry is null)
            {
                DisplayMessage("Debes seleccionar un país", "Espera");
                return;
            }

            if (SelectedProvince is null)
            {
                DisplayMessage("Debes seleccionar una provincina", "Espera");
                return;
            }

            if (SelectedLocation is null)
            {
                DisplayMessage("Debes seleccionar una ciudad", "Espera");
                return;
            }

            IsLoading = true;

            Response CreateResponse;

            if (Type == typeof(Event))
            {
                CreateResponse = await GoService.EditEvent(URL, Occurrence, _pathPhoto, SelectedLocation.Id, SelectedCategory.Id, Token);
            }
            else
            {
                CreateResponse = await GoService.EditActivity(URL, Occurrence, _pathPhoto, SelectedLocation.Id, SelectedCategory.Id, Token);
            }

            if (CreateResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DisplayMessage("Modificado con éxito", "Éxito!");
                Back();
            }
            else
            {
                if (CreateResponse.ErrorData != null)
                {
                    DisplayMessage(CreateResponse.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos crear el evento, intentalo nuevamente", "Tuvimos un problema");
                }
            }

            IsLoading = false;
        }
    }
}