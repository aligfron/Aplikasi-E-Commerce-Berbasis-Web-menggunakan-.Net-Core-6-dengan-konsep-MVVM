using Newtonsoft.Json;
using System.Net;
using System.Text;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class VariantModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiurl;

        private string jsonData;

        HttpContent content;
        public VariantModel(IConfiguration _config)
        {
            apiurl = $"{_config["ApiUrl"]}Variant";
        }

        public async Task<List<VMTblMVariant>?> GetByFilter(string? filter)
        {
            List<VMTblMVariant>? data = null;

            try
            {
                VMResponse<List<VMTblMVariant>>? response = JsonConvert.DeserializeObject<VMResponse<List<VMTblMVariant>>?>(
                        await httpClient.GetStringAsync(
                                (string.IsNullOrEmpty(filter)) ? $"{apiurl}" : $"{apiurl}/GetBy/{filter}"
                            )
                    );

                if (response != null)
                {
                    if (response.statusCode == HttpStatusCode.OK)
                    {
                        data = response.data;
                    }
                    else
                    {
                        throw new Exception(response.message);
                    }
                }
                else
                {
                    throw new Exception("Variant API cannot be reached!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError at VariantModel.GetByFilter: {ex.Message}");
            }

            return data;
        }

        public async Task<VMTblMVariant?> getById(int id)
        {
            VMTblMVariant? dataCoba = null;
            try
            {

                VMResponse<VMTblMVariant>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMVariant>>(await httpClient.GetStringAsync(apiurl + "Variant/" + id));



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
                Console.WriteLine($"Variant.GetById : {ex.Message}");
            }
            return dataCoba;
        }

        public async Task<VMResponse<VMTblMVariant>?> CreateAsync(VMTblMVariant data)
        {
            VMResponse<VMTblMVariant>? apiResponse = new VMResponse<VMTblMVariant>();
            try
            {
                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMVariant>?>(
                    await httpClient.PostAsync($"{apiurl}Variant", content).Result.Content.ReadAsStringAsync()
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
                    throw new Exception("variant api could not be reached");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"variantModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }


        public async Task<VMResponse<VMTblMVariant>?> DeleteAsync(int id, int userid)
        {
            VMResponse<VMTblMVariant>? apiResponse = new VMResponse<VMTblMVariant>();
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMVariant>?>(
                    await httpClient.DeleteAsync($"{apiurl}Variant/{id}/{userid}").Result.Content.ReadAsStringAsync()
                    );
                /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMVariant>?>(
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
