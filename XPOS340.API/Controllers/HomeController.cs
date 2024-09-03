using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XPOS240.ViewModel;
using XPOS340.DataModel;

namespace XPOS340.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private DAHome Home;
        List<VMTblCoba> data = new List<VMTblCoba>();
        public HomeController(XPOS340Context _db)
        {
            Home = new DAHome(_db);
        }

        [HttpGet]
        public List<VMTblCoba> GetAll() => Home.GetByFilter("");

        [HttpGet("[action]/{id?}")]
        public VMTblCoba GetByID(int id) => Home.GetById(id);
        

        [HttpGet("[action]/{filter?}")]
        public List<VMTblCoba> GetByFilter(string filter) => Home.GetByFilter(filter);
        
        [HttpPost]
        public List<VMTblCoba> create(VMTblCoba input)=> Home.create(input);

        [HttpPut]
        public VMTblCoba? Update(VMTblCoba input) => Home.Update(input);
        

        [HttpDelete("{id?}")]
        public List<VMTblCoba> Delete(int id) => Home.Delete(id);

    }
}
