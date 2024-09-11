using DataAccess;
using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace XPOS340.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private DAProduct product;
        public ProductController(XPOS340Context _db) {
            product = new DAProduct(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTblMProduct>> response = await Task.Run(() => product.GetByFilter(""));
                if (response.data.Count > 0)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.GetAll: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    throw new ArgumentNullException("No Filter Provided");
                }
                return (filter != null)
                    ? Ok(await Task.Run(() => product.GetByFilter(filter)))
                    : BadRequest("Category name or description must be.... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMTblMProduct?> response = await Task.Run(() => product.GetById(id));
                if (response.data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.message);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.GetByID " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Create(VMTblMProduct data)
        {
            try
            {
                return Created("api/Product", await Task.Run(() => product.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTblMProduct data)
        {
            try
            {
                VMResponse<VMTblMProduct?> response = await Task.Run(() => product.Update(data));
                if (response.data != null)
                {
                    return Ok(response);
                }
                else
                {
                    Console.WriteLine(response.message);
                    return NoContent();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Update " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id?}/{userId?}")]
        public async Task<ActionResult> Delete(int id, int userId)
        {
            try
            {
                VMResponse<VMTblMProduct> response = await Task.Run(() => product.Delete(id, userId));
                if (response.data != null) { return Ok(response); }
                else
                {
                    Console.WriteLine(response.message);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Delete " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
