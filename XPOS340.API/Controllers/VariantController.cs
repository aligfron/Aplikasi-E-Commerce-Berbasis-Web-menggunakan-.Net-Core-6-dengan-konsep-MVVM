using DataAccess;
using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace XPOS340.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VariantController : ControllerBase
    {
        private DAVariant variant;
        public VariantController(XPOS340Context _db) { 
            variant = new DAVariant(_db);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTblMVariant>> response = await Task.Run(() => variant.GetByFilter(""));
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
                return (filter != null)
                    ? Ok(await Task.Run(() => variant.GetByFilter(filter)))
                    : BadRequest("Category name or description must be.... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                VMResponse<VMTblMVariant> response = await Task.Run(() => variant.GetById(id));
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
        public async Task<ActionResult> Create(VMTblMVariant data)
        {
            try
            {
                return Created("api/Variant", await Task.Run(() => variant.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTblMVariant data)
        {
            try
            {
                VMResponse<VMTblMVariant> response = await Task.Run(() => variant.update(data));
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
                VMResponse<VMTblMVariant> response = await Task.Run(() => variant.delete(id, userId));
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
