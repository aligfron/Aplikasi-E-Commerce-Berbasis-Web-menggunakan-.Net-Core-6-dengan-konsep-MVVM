using Newtonsoft.Json;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class HomeModel
    {

        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiurl;

        public HomeModel(IConfiguration _config)
        {
            apiurl = _config["ApiUrl"];
        }

        public List<VMTblCoba>? getAllCoba()
        {
            List<VMTblCoba>? dataCoba =  null;
            try
            {
                dataCoba = JsonConvert.DeserializeObject<List<VMTblCoba>>(httpClient.GetStringAsync(apiurl+"Home").Result);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return dataCoba;
        }
    }
}
