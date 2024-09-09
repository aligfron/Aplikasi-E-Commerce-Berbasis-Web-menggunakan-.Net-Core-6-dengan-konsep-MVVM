using Newtonsoft.Json;
using System.Net;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class CustomerModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMCustomer>>? apiResponse;

        public CustomerModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }

        public async Task<List<VMTblMCustomer>>? getByFilter(string? filter)
        {
            List<VMTblMCustomer>? dataCoba = null;
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblMCustomer>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Customer")
                    : await httpClient.GetStringAsync(apiurl + "Customer/GetBy/" + filter));

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode == HttpStatusCode.OK)
                    {
                        dataCoba = apiResponse.data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryModel.GetAll : {ex.Message}");
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMTblMCustomer>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTblMCustomer>? apiResponse = new VMResponse<VMTblMCustomer>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>?>(
                    await httpClient.DeleteAsync($"{apiurl}Customer/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>?>(
                     await httpClient.DeleteAsync($"{apiurl}Category?id={id}&userId={userId}").Result.Content.ReadAsStringAsync()
                     );*/

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(apiResponse.message);
                    }

                }
                else
                {
                    throw new Exception("variant api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VariantModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
    }
}
