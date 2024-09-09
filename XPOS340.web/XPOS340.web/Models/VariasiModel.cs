using Newtonsoft.Json;
using System.Net;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class VariasiModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMVariant>>? apiResponse;

        public VariasiModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }

        public async Task<List<VMTblMVariant>>? getByFilter(string? filter)
        {
            List<VMTblMVariant>? dataCoba = null;
            try
            {

                apiResponse = JsonConvert.DeserializeObject<VMResponse<List<VMTblMVariant>>?>(
                    (string.IsNullOrEmpty(filter))
                    ? await httpClient.GetStringAsync(apiurl + "Variant")
                    : await httpClient.GetStringAsync(apiurl + "Variant/GetBy/" + filter));
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
                Console.WriteLine($" : {ex.Message}");
            }
            return dataCoba;
        }


    }
}
