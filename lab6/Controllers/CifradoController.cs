using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace lab6.Controllers
{ 

    public class CifradoController : ControllerBase
    {

        [HttpGet]
        [Route("cipher/getPublicKey")]
        public ActionResult<string> Insertar(int key)
        {
            if (ModelState.IsValid)
            {
                RSA.RSA Llaves = new RSA.RSA();
                 
                var json = JsonConvert.SerializeObject(Llaves.Llaves);
                return json;


            }
            return BadRequest(ModelState);
        }




    }
}
