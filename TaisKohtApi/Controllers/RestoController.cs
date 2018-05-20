using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace ASPNetCoreReact.Controllers
{
    [Route("api/[controller]")]
    public class RestoController : Controller
    {
       
        [HttpGet("[action]")]
        public IEnumerable<Resto> List() {
            return Enumerable.Range(1, 15)
            .Select(index => new Resto {
                Name = "Some X name",
                Address = "Some X address",
                Rating ="Some X Rating"
            });
        }
        public class Resto
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Rating { get; set; }
        
        }
    }
}