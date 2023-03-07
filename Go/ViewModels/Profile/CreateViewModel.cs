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
    public class CreateViewModel : ViewModelBase
    {
        public string _pathPhoto = string.Empty;

        private bool _IsLoading;
        private Occurrence _Occurrence;
        private string _PhotoName;
        private List<Category> _Categories;
        private Category _SelectedCategory;
        private string _KindSelected;
        private bool _DatetimeDataVisible;
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

        public string KindSelected
        {
            get => _KindSelected;
            set
            {
                _KindSelected = value;
                OnPropertyChanged(nameof(KindSelected));

                if(value.Equals("Evento"))
                {
                    DatetimeDataVisible = true;
                }
                else
                {
                    DatetimeDataVisible = false;
                }
            }
        }

        public bool DatetimeDataVisible
        {
            get => _DatetimeDataVisible;
            set => SetValue(ref _DatetimeDataVisible, value);
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
                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        public ICommand PickPhotoCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }

        public CreateViewModel(List<Category> Categories, List<Country> Countries)
        {
            this.Categories = Categories;
            this.Countries = Countries;

            Occurrence = new Occurrence();

            PickPhotoCommand = new Command(PickPhoto);
            CreateCommand = new Command(async () => await Create());
        }

        public async Task<bool> LoadProvinces()
        {
            IsLoading = true;

            var ProvinceData = await GoService.GetProvinces(URL, SelectedCountry.Id);

            IsLoading = false;

            if (ProvinceData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Provinces = ProvinceData.Data;

                return true;
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

                return false;
            }
        }

        public async Task<bool> LoadLocations()
        {
            IsLoading = true;

            var LocationData = await GoService.SearchingForLocations(URL, ProvinceID: SelectedProvince.Id);

            IsLoading = false;

            if (LocationData.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Locations = LocationData.Data;

                return true;
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

                return false;
            }
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

        public async Task<bool> Create()
        {
            Tuple<bool, string> occurrenceValidation;

            if (KindSelected.Equals("Evento"))
            {
                Occurrence.DatetimeStart = new DateTime(Occurrence.DatetimeStart.Value.Year, Occurrence.DatetimeStart.Value.Month, Occurrence.DatetimeStart.Value.Day, TimeStart.Hours, TimeStart.Minutes, TimeStart.Seconds);

                if(IsSameDay)
                {
                    Occurrence.DatetimeEnd = Occurrence.DatetimeStart;
                }
                else
                {
                    Occurrence.DatetimeEnd = new DateTime(Occurrence.DatetimeEnd.Value.Year, Occurrence.DatetimeEnd.Value.Month, Occurrence.DatetimeEnd.Value.Day, TimeEnd.Hours, TimeEnd.Minutes, TimeEnd.Seconds);
                }

                occurrenceValidation = Occurrence.Validate(true, IsSameDay);
            }
            else
            {
                occurrenceValidation = Occurrence.Validate(false);
            }
            
            if(!occurrenceValidation.Item1)
            {
                DisplayMessage(occurrenceValidation.Item2, "Espera");
                return false;
            }

            if (string.IsNullOrEmpty(PhotoName))
            {
                DisplayMessage("Debes seleccionar una foto", "Espera");
                return false;
            }

            if (SelectedCategory is null)
            {
                DisplayMessage("Debes seleccionar una categoría", "Espera");
                return false;
            }

            if (SelectedCountry is null)
            {
                DisplayMessage("Debes seleccionar un país", "Espera");
                return false;
            }

            if (SelectedProvince is null)
            {
                DisplayMessage("Debes seleccionar una provincina", "Espera");
                return false;
            }

            if (SelectedLocation is null)
            {
                DisplayMessage("Debes seleccionar una ciudad", "Espera");
                return false;
            }

            IsLoading = true;

            Response CreateResponse;

            if(KindSelected.Equals("Evento"))
            {
                CreateResponse = await GoService.CreateEvent(URL, Occurrence, _pathPhoto, SelectedLocation.Id, SelectedCategory.Id, Token);
            }
            else
            {
                CreateResponse = await GoService.CreateActivity(URL, Occurrence, _pathPhoto, SelectedLocation.Id, SelectedCategory.Id, Token);
            }

            IsLoading = false;

            if (CreateResponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                DisplayMessage("Creado con éxito", "Éxito!");
                Back();

                return true;
            }
            else
            {
                if (CreateResponse.ErrorData != null)
                {
                    DisplayMessage(CreateResponse.ErrorData.Message, "Tuvimos un problema");
                }
                else
                {
                    DisplayMessage("No pudimos crearlo, intentalo nuevamente", "Tuvimos un problema");
                }

                return false;
            }
        }
    }
}