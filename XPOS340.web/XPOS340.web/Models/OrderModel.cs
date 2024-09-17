using Newtonsoft.Json;
using System.Net;
using System.Text;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class OrderModel
    {
        private readonly string apiurl;
        private string jsonData;
        private readonly HttpClient httpClient = new HttpClient();
        private HttpContent content;


        public OrderModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }
        public async Task<VMResponse<VMTblTOrder>?> SaveAsync(VMTblTOrder data)
        {
            VMResponse<VMTblTOrder>? apiResponse = new VMResponse<VMTblTOrder>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblTOrder>?>(
                    await httpClient.PostAsync($"{apiurl}Order", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode != HttpStatusCode.OK)
                    {
                     
                        throw new Exception(apiResponse.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order Model Save Async {ex.Message}");

            }
            return apiResponse;
        }
        public async Task<List<VMTblTOrder>>? getByFilter(string? filter)
        {
            List<VMTblTOrder>? dataCoba = null;
            try
            {

                VMResponse<List< VMTblTOrder>>? apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblTOrder>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Order")
                    : await httpClient.GetStringAsync(apiurl + "Order/GetByFilter/" + filter));
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
        public async Task<VMTblTOrder?> GetById(int id)
        {
            VMTblTOrder? data = null;
            try
            {
                VMResponse<VMTblTOrder>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblTOrder>>(
                    
                    await httpClient.GetStringAsync(apiurl + "Order/" + id));
               
                if (apiResponse != null)
                {
                    if (apiResponse.statusCode == HttpStatusCode.OK)
                    {
                        data = apiResponse.data;
                    }
                    else
                    {
                        throw new Exception(apiResponse.message);
                    }
                }
            }
            catch
                (Exception ex)
            {
                    Console.WriteLine($"CategoryModel.GetAll : {ex.Message}");
                }
            return data;
        }

        internal async Task<VMResponse<VMTblTOrder>?> UpdateAsync(VMTblTOrder data)
        {
            VMResponse<VMTblTOrder>? apiResponse = new VMResponse<VMTblTOrder>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblTOrder>?>(
                    await httpClient.PutAsync($"{apiurl}Order", content).Result.Content.ReadAsStringAsync());

                if (apiResponse != null)
                {
                    if (apiResponse.statusCode != HttpStatusCode.OK)
                    {

                        throw new Exception(apiResponse.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order Model Save Async {ex.Message}");

            }
            return apiResponse;
        }
    }
}
