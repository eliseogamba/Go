using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Go.Common;
using Go.Services;
using Go.ViewModels;
using Xunit;

namespace GoTest
{
    public class CreateViewModelUnitTest
    {
        [Fact]
        public async void LoadProvincesTest()
        {
            var URL = $"{Constants.TestingBaseURL}/api/v1";

            var CategoryData = await GoService.GetCategories(URL);
            var CountryData = await GoService.GetCountries(URL);

            if (CategoryData.StatusCode != System.Net.HttpStatusCode.OK || CountryData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var viewModel = new CreateViewModel(CategoryData.Data, CountryData.Data);

            viewModel.SelectedCountry = CountryData.Data.First();

            Assert.True(await viewModel.LoadProvinces());
        }

        [Fact]
        public async void LoadLocationsTest()
        {
            var URL = $"{Constants.TestingBaseURL}/api/v1";

            var CategoryData = await GoService.GetCategories(URL);
            var CountryData = await GoService.GetCountries(URL);
            
            if (CategoryData.StatusCode != System.Net.HttpStatusCode.OK || CountryData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var ProvinceData = await GoService.GetProvinces(URL, CountryData.Data.First().Id);

            if (ProvinceData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var viewModel = new CreateViewModel(CategoryData.Data, CountryData.Data);

            viewModel.SelectedProvince = ProvinceData.Data.First();

            Assert.True(await viewModel.LoadLocations());
        }

        [Fact]
        public async void CreateEventTest()
        {
            var URL = $"{Constants.TestingBaseURL}/api/v1";

            var CategoryData = await GoService.GetCategories(URL);
            var CountryData = await GoService.GetCountries(URL);

            if (CategoryData.StatusCode != System.Net.HttpStatusCode.OK || CountryData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var ProvinceData = await GoService.GetProvinces(URL, CountryData.Data.First().Id);

            if (ProvinceData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var LocationData = await GoService.SearchingForLocations(URL, ProvinceID: ProvinceData.Data.First().Id);

            if (LocationData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var viewModel = new CreateViewModel(CategoryData.Data, CountryData.Data);

            viewModel.Token = "ed4bb4885a8532c532110689c406c7107d0ceb89";

            viewModel.KindSelected = "Evento";
            viewModel.Occurrence.DatetimeStart = DateTime.Now.AddDays(1);
            viewModel.Occurrence.DatetimeEnd = DateTime.Now.AddDays(2);
            viewModel.Occurrence.Title = Utils.RandomString(10);
            viewModel.Occurrence.Description = Utils.RandomString(10);
            viewModel.Occurrence.Street = Utils.RandomString(10);
            viewModel.Occurrence.StreetNumber = Utils.RandomString(10);
            viewModel.Occurrence.Place = Utils.RandomString(10);
            viewModel.PhotoName = Utils.RandomString(10);
            viewModel.SelectedCategory = CategoryData.Data.First();
            viewModel.SelectedCountry = CountryData.Data.First();
            viewModel.SelectedProvince = ProvinceData.Data.First();
            viewModel.SelectedLocation = LocationData.Data.First();
            viewModel._pathPhoto = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.png");

            Assert.True(await viewModel.Create());
        }

        [Fact]
        public async void CreateActivityTest()
        {
            var URL = $"{Constants.TestingBaseURL}/api/v1";

            var CategoryData = await GoService.GetCategories(URL);
            var CountryData = await GoService.GetCountries(URL);

            if (CategoryData.StatusCode != System.Net.HttpStatusCode.OK || CountryData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var ProvinceData = await GoService.GetProvinces(URL, CountryData.Data.First().Id);

            if (ProvinceData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            var LocationData = await GoService.SearchingForLocations(URL, ProvinceID: ProvinceData.Data.First().Id);

            if (LocationData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.False(false);
                return;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = new List<string>(assembly.GetManifestResourceNames());

            var viewModel = new CreateViewModel(CategoryData.Data, CountryData.Data);

            viewModel.Token = "ed4bb4885a8532c532110689c406c7107d0ceb89";

            viewModel.KindSelected = "Actividad";
            viewModel.Occurrence.Title = Utils.RandomString(10);
            viewModel.Occurrence.Description = Utils.RandomString(10);
            viewModel.Occurrence.Street = Utils.RandomString(10);
            viewModel.Occurrence.StreetNumber = Utils.RandomString(10);
            viewModel.Occurrence.Place = Utils.RandomString(10);
            viewModel.PhotoName = Utils.RandomString(10);
            viewModel.SelectedCategory = CategoryData.Data.First();
            viewModel.SelectedCountry = CountryData.Data.First();
            viewModel.SelectedProvince = ProvinceData.Data.First();
            viewModel.SelectedLocation = LocationData.Data.First();
            viewModel._pathPhoto = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.png");

            Assert.True(await viewModel.Create());
        }
    }
}