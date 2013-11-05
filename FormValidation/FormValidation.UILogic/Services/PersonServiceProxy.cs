using FormValidation.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FormValidation.UILogic.Services
{
    public class PersonServiceProxy : IPersonServiceProxy
    {
        private string _personBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Person/", Constants.ServerAddress);
 
        // Get all CommonDataTypes
        public async Task<CrudResult> GetPeopleAsync() {
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync(string.Format("{0}", _personBaseUrl));
                response.EnsureSuccessStatusCode();
                CrudResult crudResult = await response.Content.ReadAsAsync<CrudResult>();
                return crudResult;
            }
        }

        // Get a CommonDataType by Id        
        public async Task<CrudResult> GetPersonAsync(int personId) {
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync(string.Format("{0}{1}", _personBaseUrl, personId.ToString()));
                response.EnsureSuccessStatusCode();
                CrudResult crudResult = await response.Content.ReadAsAsync<CrudResult>();
                return crudResult;
            }
        }

        // Create a new CommonDataType
        public async Task<CrudResult> CreatePersonAsync(Person person) {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() }) {
                using (var httpClient = new HttpClient(handler)) {
                    string postUrl = string.Format("{0}", _personBaseUrl);
                    var response = await httpClient.PostAsJsonAsync<Person>(postUrl, person);
                    await response.EnsureSuccessWithValidationSupportAsync();
                    CrudResult crudResult = await response.Content.ReadAsAsync<CrudResult>();
                    return crudResult;
                }
            }
        }

        // Update an existing CommonDataType
        public async Task<CrudResult> UpdatePersonAsync(Person person) {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() }) {
                using (var httpClient = new HttpClient()) {
                    string putUrl = string.Format("{0}{1}", _personBaseUrl, person.Id.ToString());
                    var response = await httpClient.PutAsJsonAsync<Person>(putUrl, person);
                    await response.EnsureSuccessWithValidationSupportAsync();
                    CrudResult crudResult = await response.Content.ReadAsAsync<CrudResult>();
                    return crudResult;
                }
            }
        }

        // Delete an existing CommonDataType
        public async Task<CrudResult> DeletePersonAsync(int personId) {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() }) {
                using (var httpClient = new HttpClient()) {
                    string deleteUrl = string.Format("{0}{1}", _personBaseUrl, personId.ToString());
                    var response = await httpClient.DeleteAsync(deleteUrl);
                    await response.EnsureSuccessWithValidationSupportAsync();
                    CrudResult crudResult = await response.Content.ReadAsAsync<CrudResult>();
                    return crudResult;
                }
            }
        }
    }


    // Create a server-side error for testing
    //throw new HttpRequestException("GetCommonDataTypesAsync failed. Check log for details.");

    // Create a server-side error for testing
    //commonDataTypeId = 10001;

    // Create a server-side error for testing
    //throw new HttpRequestException("CreateCommonDataTypeAsync failed. Check log for details.");

    // Create a server-side error for testing
    //throw new HttpRequestException("UpdateCommonDataTypeAsync failed. Check log for details.");

    // Create a server-side error for testing
    //throw new HttpRequestException("DeleteCommonDataTypeAsync failed. Check log for details.");
}
