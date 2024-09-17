using Newtonsoft.Json;
using System.Net;
using System.Text;
using XPOS240.ViewModel;

namespace XPOS340.web.Models
{
    public class ProductModel
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly string? apiurl;
        private VMResponse<List<VMTblMProduct>>? apiResponse;
        private readonly IWebHostEnvironment webHostEnv;
        private readonly string imageFolder;
        private string jsonData;
        HttpContent content;



        public ProductModel(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            apiurl = _config["ApiUrl"];
            webHostEnv = _webHostEnv;
            imageFolder = _config["ImageFolder"];
        }
        private string UploadFile(IFormFile? imageFile)
        {
            string uniqueFileName = string.Empty;
            if (imageFile != null)
            {
                uniqueFileName = $"{Guid.NewGuid()}-{imageFile.FileName}";
                using (FileStream fileStream = new FileStream(
                    $"{webHostEnv.WebRootPath}\\{imageFolder}\\{uniqueFileName}", FileMode.CreateNew
                    ))
                {
                    imageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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
                throw new Exception(ex.Message);
            }
            return dataCoba;
        }
        public async Task<VMTblMProduct?> getById(int id)
        {
            VMTblMProduct? dataCoba = null;
            try
            {

                VMResponse<VMTblMProduct>? apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>>
                    (await httpClient.GetStringAsync(apiurl + "Product/" + id));



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
                Console.WriteLine($"Product.GetById : {ex.Message}");
            }
            return dataCoba;
        }
        public async Task<VMResponse<VMTblMProduct>?> CreateAsync(VMTblMProduct data)
        {
            VMResponse<VMTblMProduct>? apiResponse = new VMResponse<VMTblMProduct>();
            try
            {
                //file upload
                if (data.ImageFile != null)
                {
                    data.Image = UploadFile(data.ImageFile);
                    data.ImageFile = null;
                }


                jsonData = JsonConvert.SerializeObject(data);
                content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
                    await httpClient.PostAsync($"{apiurl}Product", content).Result.Content.ReadAsStringAsync()
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

        private bool DeleteOldImage(string oldImageFileName)
        {
            try
            {
                oldImageFileName = $"{webHostEnv.WebRootPath}\\{imageFolder}\\{oldImageFileName}";
                if (File.Exists(oldImageFileName))
                {
                    File.Delete(oldImageFileName);
                }
                else
                {
                    throw new ArgumentException("Product Api could");
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }

        public async Task<VMResponse<VMTblMProduct>?> DeleteAsync(int id, int userId)
        {
            VMResponse<VMTblMProduct>? apiResponse = new VMResponse<VMTblMProduct>();
            try
            {
                VMTblMProduct? existingdata = await getById(id);
                string? filename = existingdata?.Image;
                
                    apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
                    await httpClient.DeleteAsync($"{apiurl}Product/{id}/{userId}").Result.Content.ReadAsStringAsync()
                    );
                    /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
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
                        if (filename != null)
                        {
                            DeleteOldImage(filename);
                        }
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VariantModel.GetbyId: {ex.Message}");
            }
            return apiResponse;
        }

        internal async Task<VMResponse<VMTblMProduct>> UpdateAsync(VMTblMProduct data)
        {
            VMResponse<VMTblMProduct>? apiResponse = new VMResponse<VMTblMProduct>();
            try
            {
                if (data.ImageFile != null)
                {
                    if (data.Image != null)
                    {
                        DeleteOldImage(data.Image);
                    }
                    data.Image = UploadFile(data.ImageFile);
                    data.ImageFile = null;

                    //manggil api update proses
                    jsonData = JsonConvert.SerializeObject(data);
                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
                        await httpClient.PutAsync($"{apiurl}Product", content).Result.Content.ReadAsStringAsync()
                        );
                    /* apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
                         await httpClient.DeleteAsync($"{apiurl}Category?id={id}&userId={userId}").Result.Content.ReadAsStringAsync()
                         );*/
                }
                else
                {
                    jsonData = JsonConvert.SerializeObject(data);
                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    apiResponse = JsonConvert.DeserializeObject<VMResponse<VMTblMProduct>?>(
                        await httpClient.PutAsync($"{apiurl}Product", content).Result.Content.ReadAsStringAsync()
                        );
                }
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
