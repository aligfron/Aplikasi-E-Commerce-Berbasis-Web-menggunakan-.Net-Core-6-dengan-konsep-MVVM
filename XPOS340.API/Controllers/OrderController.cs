using DataAccess;
using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace XPOS340.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private DAOrder order;
        public OrderController(XPOS340Context _db) {
            order = new DAOrder(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                VMResponse<List<VMTblTOrder>> response = await Task.Run(() => order.GetByFilter(""));
                if (response.data != null && response.data.Count > 0)
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
        public async Task<ActionResult> GetByFilter(string? filter)
        {
            try
            {
                return (filter != null)
                    ? Ok(await Task.Run(() => order.GetByFilter(filter)))
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
                VMResponse<VMTblTOrder> response = await Task.Run(() => order.GetById(id));
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
        public async Task<ActionResult> Create(VMTblTOrder data)
        {
            try
            {
                return Created("api/Category", await Task.Run(() => order.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTblTOrder data)
        {
            try
            {
                VMResponse<VMTblTOrder> response = await Task.Run(() => order.Update(data));
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

    }
}
