using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab6.Controllers
{ 

    public class CifradoController : ControllerBase
    {

        [HttpGet]
        [Route("cipher/getPublicKey")]
        public ActionResult Insertar(int key)
        {
            if (ModelState.IsValid)
            {
                RSA.RSA Llave = new RSA.RSA();

            }
            return BadRequest(ModelState);
        }




    }
}
