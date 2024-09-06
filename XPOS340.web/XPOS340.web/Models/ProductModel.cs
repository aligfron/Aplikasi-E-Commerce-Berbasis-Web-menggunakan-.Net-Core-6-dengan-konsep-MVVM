using Newtonsoft.Json;
using System.Net;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class ProductModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMProduct>>? apiResponse;

        public ProductModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }

        public async Task<List<VMTblMProduct>>? getByFilter(string? filter)
        {
            List<VMTblMProduct>? dataCoba = null;
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblMProduct>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Product")
                    : await httpClient.GetStringAsync(apiurl + "Product/GetBy/" + filter));
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
    }
}
