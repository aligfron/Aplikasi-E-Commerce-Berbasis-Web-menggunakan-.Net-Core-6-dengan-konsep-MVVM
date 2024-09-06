using Newtonsoft.Json;
using System.Net;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class CategoryModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMCategory>>? apiResponse;

        public CategoryModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }

        public async Task<List<VMTblMCategory>>? getAll()
        {
            List<VMTblMCategory>? dataCoba = null;
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblMCategory>>>(await httpClient.GetStringAsync(apiurl + "Category"));

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
        public async Task<List<VMTblMCategory>?> getByFilter(string? filter)
        {
            List<VMTblMCategory>? dataCoba = null;
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblMCategory>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Category")
                    : await httpClient.GetStringAsync(apiurl + "Category/GetBy/" + filter));


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
        public async Task<VMTblMCategory?> getById(int id)
        {
            VMTblMCategory? dataCoba = null;
            try
            {

                VMResponse<VMTblMCategory>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>>(await httpClient.GetStringAsync(apiurl + "Category/" + id));
                  


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
    }
}
