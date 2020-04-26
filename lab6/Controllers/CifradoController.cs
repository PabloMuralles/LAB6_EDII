using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace lab6.Controllers
{ 

    public class CifradoController : ControllerBase
    {

        [HttpGet]
        [Route("cipher/getPublicKey")]
        public ActionResult<string> Insertar( int key)
        {
            if (ModelState.IsValid)
            {
                RSA.RSA Llaves = new RSA.RSA();
                 
                var json = JsonConvert.SerializeObject(Llaves.Llaves);
                return json;


            }
            return BadRequest(ModelState);
        }

        //[HttpPost]
        //[Route("cipher/caesar2")]
        //public ActionResult Insertar([FromBody] RSA.Informacion info)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var contraseña = RSA.Descifrar.Instance.DecifrarContraseña(info.contraseña);
        //        return Ok();
        //    }
        //    return BadRequest(ModelState);
        //}

        [HttpPost]
        [Route("cipher/caesar2")]
        public ActionResult Cipher([FromBody] Informacion Info)
        {
            if (ModelState.IsValid)
            {
              
                RSA.Descifrar.Instance.CifrarDocumento(Info.RutaArchivo, Info.NombreArchivo, Info.RutaLlave);
                return Ok();

            }
            return BadRequest(ModelState);


            
     

        }

        [HttpPost]
        [Route("Decipher/caesar2")]
        public ActionResult Decipher([FromBody] Informacion Info)
        {
            if (ModelState.IsValid)
            {

                RSA.Descifrar.Instance.DescifrarDocumentos(Info.RutaArchivo, Info.NombreArchivo, Info.RutaLlave);

                
                return Ok();

            }
            return BadRequest(ModelState);




            return Ok();

        }


    }
}
