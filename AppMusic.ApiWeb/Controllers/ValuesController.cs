using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppMusic.ApiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*ControllerBase nos convierte la clase en Controller*/
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "valor1", "valor2" };
        }
        [HttpGet("{id}")]
        public ActionResult<string> Post(int id)
        {
            return "value";
        }
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
        [HttpPut("{id}")]

        public void Put(int id , [FromBody] string value)
        {

        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

    }
}
