using Newtonsoft.Json;
using System.Net;
using System.Text;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class CategoryModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMCategory>>? apiResponse;
        private string jsonData;

        HttpContent content;

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

        public async Task<VMResponse<VMTblMCategory>?> UpdateAsync(VMTblMCategory data)
        {
            VMResponse<VMTblMCategory>? apiResponse = new VMResponse<VMTblMCategory>();
            try
            {
                //manggil api update proses
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>?>
                    (await httpClient.PutAsync($"{apiurl}Category", content).Result.Content.ReadAsStringAsync());

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

        public async Task<VMResponse<VMTblMCategory>?> CreateAsync(VMTblMCategory data)
        {
            VMResponse<VMTblMCategory>? apiResponse = new VMResponse<VMTblMCategory>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>?>(
                    await httpClient.PostAsync($"{apiurl}Category", content).Result.Content.ReadAsStringAsync()
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
        public async Task<VMResponse<VMTblMCategory>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTblMCategory>? apiResponse = new VMResponse<VMTblMCategory>();
            try
            {

                /*apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>?>(
                    await httpClient.DeleteAsync($"{apiurl}Category/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );*/
                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMCategory>?>(
                    await httpClient.DeleteAsync($"{apiurl}Category?id={id}&userId={userId}").Result.Content.ReadAsStringAsync()
                    );

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
            catch (Exception ex)
            {
                Console.WriteLine($"CategoryModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }
    }
}
