using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiDB.Database;
using MultiDB.MSSQLDatabase;
using MultiDB.MySQLDatabase;

namespace MultiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiDBController : ControllerBase
    {
        private List<IApplicationDBContext> applicationDBContexts;
        public MultiDBController(IMSSQLApplicationDBContext mssql, IMySQLApplicationDBContext mysql)
        {          
            applicationDBContexts = new List<IApplicationDBContext>
            {
                mysql,
                mssql
            };
        }
        [HttpGet]
        public IActionResult Get()  
        {
            return Ok("Hello world");
        }
        [HttpPost]
        public IActionResult Post(string name)
        {
            var result = new Dictionary<string, string>();
            Parallel.ForEach(applicationDBContexts, (db) => 
            {
                var  profile = db.Profiles.Add(new DatabaseModels.Profile { Name = name }).Entity;
                db.SaveChanges();
                result.Add($"{db.GetType().Name} Id: { profile.Id}",profile.Name);
            });            
            return Ok(result);
        }
    }
}
