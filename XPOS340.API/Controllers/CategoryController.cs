﻿using Microsoft.AspNetCore.Mvc;
using XPOS340.DataModel;
using DataAccess;
using XPOS240.ViewModel;

namespace XPOS340.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public DACategory category;
        public CategoryController(XPOS340Context _db) { 
            category = new DACategory(_db);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                return Ok(await Task.Run(() => category.GetByFilter("")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.GetAll: "+ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("[action]/{filter?}")]
        public async Task<ActionResult> GetBy(string? filter)
        {
            try
            {
                return (filter != null) 
                    ? Ok(await Task.Run(() => category.GetByFilter(filter))) 
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
                return Ok(await Task.Run(() => category.GetById(id)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.GetByID " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(VMTblMCategory data)
        {
            try
            {
                return Created("api/Category", await Task.Run(() => category.Create(data)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Create " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMTblMCategory data)
        {
            try
            {
                return Ok( await Task.Run(() => category.Update(data)));
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
                return Ok(await Task.Run(() => category.Delete(id, userId)));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CategoryController.Delete " + ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
