using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Go.Common;
using Go.Models;
using Newtonsoft.Json;

namespace Go.Services
{
    public class GoService
    {
        public static async Task<Response<List<Location>>> SearchingForLocations(string BaseURL, string Text = null, int? ProvinceID = null)
        {
            var Response = new Response<List<Location>>();

            try
            {
                var Url = $"{BaseURL}/locations/";

                if(!string.IsNullOrEmpty(Text))
                {
                    Url += $"?name={Text}";
                }

                if(ProvinceID.HasValue)
                {
                    Url += $"?provinceID={ProvinceID.Value}";
                }

                var Request = await Requests.HttpGet(Url);

                Response.StatusCode = Request.StatusCode;

                if(Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<List<Location>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Event>>> GetEvents(string BaseURL, long Location, int Page, string Categories, DateTime? Start, DateTime? End, string Text, string Token)
        {
            var Response = new Response<Page<Event>>();

            try
            { 
                var Url = $"{BaseURL}/events/?page={Page}&location={Location}";

                if(!string.IsNullOrEmpty(Categories))
                {
                    Url += $"&categories=[{Categories}]";
                }

                if(Start.HasValue && End.HasValue)
                {
                    Url += $"&datetimeStart={Start.Value.ToString("yyyy-MM-dd")}&datetimeEnd={End.Value.ToString("yyyy-MM-dd")}";
                }

                if (!string.IsNullOrEmpty(Text))
                {
                    Url += $"&search={Text}";
                }

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Event>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Activity>>> GetActivities(string BaseURL, long Location, int Page, string Categories, string Text)
        {
            var Response = new Response<Page<Activity>>();

            try
            {
                var Url = $"{BaseURL}/activities/?page={Page}&location={Location}";

                if (!string.IsNullOrEmpty(Categories))
                {
                    Url += $"&categories=[{Categories}]";
                }

                if (!string.IsNullOrEmpty(Text))
                {
                    Url += $"&search={Text}";
                }

                var Request = await Requests.HttpGet(Url);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Activity>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> ForgetPassword(string BaseURL, string Email)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    email = Email
                };

                var Url = $"{BaseURL}/user/forgetPassword/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> ResetPassword(string BaseURL, string Email, string Password, string Code)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    email = Email,
                    password = Password,
                    code = Code
                };

                var Url = $"{BaseURL}/user/resetPassword/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> SignUp(string BaseURL, string Username, string Name, string Lastname, string Email, string Password)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    username = Username,
                    firstName = Name,
                    lastName = Lastname,
                    email = Email,
                    password = Password
                };

                var Url = $"{BaseURL}/user/signUp/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<LoginResponse>> Login(string BaseURL, string Username, string Password)
        {
            var Response = new Response<LoginResponse>();

            try
            {
                var RequestData = new
                {
                    username = Username,
                    password = Password
                };

                var Url = $"{BaseURL}/user/login/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<LoginResponse>(await Request.Content.ReadAsStringAsync());
                }
                else
                {
                   try
                   {
                        Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                    }
                   catch { }
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> ChangePassword(string BaseURL, string OldPassword, string NewPassword, string Token)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    oldPassword = OldPassword,
                    newPassword = NewPassword
                };

                var Url = $"{BaseURL}/user/changePassword/";

                var Request = await Requests.HttpPost(Url, RequestData, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Event>>> GetOwnedEvents(string BaseURL, int Page, string Token)
        {
            var Response = new Response<Page<Event>>();

            try
            {
                var Url = $"{BaseURL}/events/owned/?page={Page}";

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Event>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Activity>>> GetOwnedActivities(string BaseURL, int Page, string Token)
        {
            var Response = new Response<Page<Activity>>();

            try
            {
                var Url = $"{BaseURL}/activities/owned/?page={Page}";

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Activity>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Event>>> GetFavoriteEvents(string BaseURL, int Page, string Token)
        {
            var Response = new Response<Page<Event>>();

            try
            {
                var Url = $"{BaseURL}/events/favorites/?page={Page}";

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Event>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<Page<Activity>>> GetFavoriteActivities(string BaseURL, int Page, string Token)
        {
            var Response = new Response<Page<Activity>>();

            try
            {
                var Url = $"{BaseURL}/activities/favorites/?page={Page}";

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<Page<Activity>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<List<Category>>> GetCategories(string BaseURL)
        {
            var Response = new Response<List<Category>>();

            try
            {
                var Url = $"{BaseURL}/categories/";

                var Request = await Requests.HttpGet(Url);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<List<Category>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<List<Country>>> GetCountries(string BaseURL)
        {
            var Response = new Response<List<Country>>();

            try
            {
                var Url = $"{BaseURL}/countries/";

                var Request = await Requests.HttpGet(Url);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<List<Country>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<List<Province>>> GetProvinces(string BaseURL, int countryID)
        {
            var Response = new Response<List<Province>>();

            try
            {
                var Url = $"{BaseURL}/provinces/?countryID={countryID}";

                var Request = await Requests.HttpGet(Url);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<List<Province>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> CreateEvent(string BaseURL, Occurrence Occurrence, string Photo, long LocationId, long CategoryID, string Token)
        {
            var Response = new Response();

            try
            {
                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Occurrence.Title), "title" },
                    { new StringContent(Occurrence.Description), "description" },
                    { new StringContent(Occurrence.DatetimeStart.Value.ToString("dd/MM/yyyy")), "datetimeStart" },
                    { new StringContent(Occurrence.DatetimeEnd.Value.ToString("dd/MM/yyyy")), "datetimeEnd" },
                    { new StringContent(Occurrence.Street), "street" },
                    { new StringContent(Occurrence.StreetNumber), "streetNumber" },
                    { new StringContent(Occurrence.Place), "place" },
                    { new StringContent(LocationId.ToString()), "locationID" },
                    { new StringContent(CategoryID.ToString()), "categoryID" }
                };

                var imageContent = new ByteArrayContent(new MemoryStream(Utils.ImageToBinary(Photo)).ToArray());

                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "photo",
                    FileName = Guid.NewGuid() + ".jpeg"
                };

                data.Add(imageContent, "photo");

                var Url = $"{BaseURL}/events/";

                var Request = await Requests.HttpPostFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> CreateActivity(string BaseURL, Occurrence Occurrence, string Photo, long LocationId, long CategoryID, string Token)
        {
            var Response = new Response();

            try
            {
                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Occurrence.Title), "title" },
                    { new StringContent(Occurrence.Description), "description" },
                    { new StringContent(Occurrence.Street), "street" },
                    { new StringContent(Occurrence.StreetNumber), "streetNumber" },
                    { new StringContent(LocationId.ToString()), "locationID" },
                    { new StringContent(CategoryID.ToString()), "categoryID" }
                };

                var imageContent = new ByteArrayContent(new MemoryStream(Utils.ImageToBinary(Photo)).ToArray());

                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "photo",
                    FileName = Guid.NewGuid() + ".jpeg"
                };

                data.Add(imageContent, "photo");

                var Url = $"{BaseURL}/activities/";

                var Request = await Requests.HttpPostFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddEventToFavorities(string BaseURL, long EventID, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/events/favorites/{EventID}/";

                var Request = await Requests.HttpPost(Url, token: Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> RemoveEventFromFavorities(string BaseURL, long EventID, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/events/favorites/{EventID}/";

                var Request = await Requests.HttpDelete(Url, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddActivityToFavorities(string BaseURL, long ActivityID, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/activities/favorites/{ActivityID}/";

                var Request = await Requests.HttpPost(Url, token: Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> RemoveActivityFromFavorities(string BaseURL, long ActivityID, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/activities/favorites/{ActivityID}/";

                var Request = await Requests.HttpDelete(Url, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch { }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> EditEvent(string BaseURL, Occurrence Occurrence, string Photo, long LocationId, long CategoryID, string Token)
        {
            var Response = new Response();

            try
            {
                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Occurrence.Title), "title" },
                    { new StringContent(Occurrence.Description), "description" },
                    { new StringContent(Occurrence.DatetimeStart.Value.ToString("dd/MM/yyyy")), "datetimeStart" },
                    { new StringContent(Occurrence.DatetimeEnd.Value.ToString("dd/MM/yyyy")), "datetimeEnd" },
                    { new StringContent(Occurrence.Street), "street" },
                    { new StringContent(Occurrence.StreetNumber), "streetNumber" },
                    { new StringContent(Occurrence.Place), "place" }
                };

                if (Occurrence.Location.Id != LocationId)
                {
                    data.Add(new StringContent(LocationId.ToString()), "locationID");
                }

                if (Occurrence.Category.Id != CategoryID)
                {
                    data.Add(new StringContent(CategoryID.ToString()), "categoryID");
                }

                if (!string.IsNullOrEmpty(Photo))
                {
                    var imageContent = new ByteArrayContent(new MemoryStream(Utils.ImageToBinary(Photo)).ToArray());

                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "photo",
                        FileName = Guid.NewGuid() + ".jpeg"
                    };

                    data.Add(imageContent, "photo");
                }

                var Url = $"{BaseURL}/events/{Occurrence.Id}/";

                var Request = await Requests.HttpPatchFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> EditActivity(string BaseURL, Occurrence Occurrence, string Photo, long LocationId, long CategoryID, string Token)
        {
            var Response = new Response();

            try
            {
                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Occurrence.Title), "title" },
                    { new StringContent(Occurrence.Description), "description" },
                    { new StringContent(Occurrence.Street), "street" },
                    { new StringContent(Occurrence.StreetNumber), "streetNumber" }
                };

                if (Occurrence.Location.Id != LocationId)
                {
                    data.Add(new StringContent(LocationId.ToString()), "locationID");
                }

                if (Occurrence.Category.Id != CategoryID)
                {
                    data.Add(new StringContent(CategoryID.ToString()), "categoryID");
                }

                if (!string.IsNullOrEmpty(Photo))
                {
                    var imageContent = new ByteArrayContent(new MemoryStream(Utils.ImageToBinary(Photo)).ToArray());

                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "photo",
                        FileName = Guid.NewGuid() + ".jpeg"
                    };

                    data.Add(imageContent, "photo");
                }

                var Url = $"{BaseURL}/activities/{Occurrence.Id}/";

                var Request = await Requests.HttpPatchFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response<List<Occurrence>>> GetMapData(string BaseURL, long Location, string Token)
        {
            var Response = new Response<List<Occurrence>>();

            try
            {
                var Url = $"{BaseURL}/map/?location={Location}";

                var Request = await Requests.HttpGet(Url, Token);

                Response.StatusCode = Request.StatusCode;

                if (Request.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Response.Data = JsonConvert.DeserializeObject<List<Occurrence>>(await Request.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> ChangeEventStatus(string BaseURL, Occurrence Occurrence, bool Status, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/events/{Occurrence.Id}/";

                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Status.ToString()), "active" }
                };

                var Request = await Requests.HttpPatchFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> ChangeActivityStatus(string BaseURL, Occurrence Occurrence, bool Status, string Token)
        {
            var Response = new Response();

            try
            {
                var Url = $"{BaseURL}/activities/{Occurrence.Id}/";

                MultipartFormDataContent data = new MultipartFormDataContent("---8d0f01e6b3b5dafaaadaada")
                {
                    { new StringContent(Status.ToString()), "active" }
                };

                var Request = await Requests.HttpPatchFiles(Url, data, Token);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddEventView(string BaseURL, long eventID)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    eventID
                };

                var Url = $"{BaseURL}/events/addView/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddEventOpen(string BaseURL, long eventID)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    eventID
                };

                var Url = $"{BaseURL}/events/addOpen/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddActivityView(string BaseURL, long activityID)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    activityID
                };

                var Url = $"{BaseURL}/activities/addView/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }

        public static async Task<Response> AddActivityOpen(string BaseURL, long activityID)
        {
            var Response = new Response();

            try
            {
                var RequestData = new
                {
                    activityID
                };

                var Url = $"{BaseURL}/activities/addOpen/";

                var Request = await Requests.HttpPost(Url, RequestData);

                Response.StatusCode = Request.StatusCode;

                try
                {
                    Response.ErrorData = JsonConvert.DeserializeObject<ErrorData>(await Request.Content.ReadAsStringAsync());
                }
                catch
                {
                }
            }
            catch
            {
                Response.StatusCode = System.Net.HttpStatusCode.Forbidden;
            }

            return Response;
        }
    }
}