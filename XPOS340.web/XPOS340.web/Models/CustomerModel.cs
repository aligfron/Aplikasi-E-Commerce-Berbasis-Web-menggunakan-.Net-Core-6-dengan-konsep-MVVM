using Newtonsoft.Json;
using System.Net;
using System.Text;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace XPOS340.web.Models
{
    public class CustomerModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMCustomer>>? apiResponse;
        private string jsonData;

        HttpContent content;

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
               throw new Exception (ex.Message);
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
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>?>(
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
        public async Task<VMTblMCustomer?> getById(int id)
        {
            VMTblMCustomer? dataCoba = null;
            try
            {

                VMResponse<VMTblMCustomer>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>>(await httpClient.GetStringAsync(apiurl + "Customer/" + id));



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
                Console.WriteLine($"CategoryModel.GetById : {ex.Message}");
            }
            return dataCoba;
        }

        public async Task<VMResponse<VMTblMCustomer>?> UpdateAsync(VMTblMCustomer data)
        {
            VMResponse<VMTblMCustomer>? apiResponse = new VMResponse<VMTblMCustomer>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>?>
                    (await httpClient.PutAsync($"{apiurl}Customer", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.message);
                    }
                }
                else
                {
                    throw new Exception("category api could not be reached");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"CategoryModel.GetbyId: {e.Message}");

            }
            return apiResponse;
        }

        public async Task<VMResponse<VMTblMCustomer>?> CreateAsync(VMTblMCustomer data)
        {
            VMResponse<VMTblMCustomer>? apiResponse = new VMResponse<VMTblMCustomer>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>?>(
                    await httpClient.PostAsync($"{apiurl}Customer", content).Result.Content.ReadAsStringAsync()
                    );

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode != HttpStatusCode.Created)
                    {
                        throw new Exception(apiResponse.message);
                    }

                }
                else
                {
                    throw new Exception("category api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
        public async Task<VMTblMCustomer>? getByEmail(string email)
        {
            VMTblMCustomer? dataCoba = null;
            try
            {

                HttpResponseMessage apiResponse = 
                   await httpClient.GetAsync(apiurl + "Customer/GetByEmail/" + email);

                if (apiResponse != null)
                {
                    if (apiResponse.StatusCode == HttpStatusCode.OK)
                    {
                       VMResponse<VMTblMCustomer> ? apires = JsonConvert.DeserializeObject<VMResponse<VMTblMCustomer>?>(
                            apiResponse.Content.ReadAsStringAsync().Result) ;
                        dataCoba = apires!.data;
                    }
                    else
                    {
                        throw new Exception($"{apiResponse.StatusCode}-{apiResponse.Content.ReadAsStringAsync().Result}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dataCoba;
        }
        
            
    }
}
