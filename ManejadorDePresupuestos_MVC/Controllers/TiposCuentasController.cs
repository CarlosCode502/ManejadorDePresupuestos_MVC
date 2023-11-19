using ManejadorDePresupuestos_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#100 FORMULARIO TIPO DE CUENTAS 
    //Controlador que va a servir para manejar las multiples cuentas que puede tener
    //un usuario
    public class TiposCuentasController : Controller
    {
        /// <summary>
        /// Todas las peticiones Get(Muestra la vista)
        /// </summary>
        /// <returns>Una vista</returns>
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Crear(TipoCuentaViewModel tipoCuentaViewModel)
        {
            //V#101 VALIDANDO EL FORMULARIO( Nunca confiar en la data que envía el usuario)
            //Si el modelo no es valido retornar el tipo cuenta
            if (!ModelState.IsValid)
            {
                //De esta manera se devuelve lo que ha escrito el usuario
                return View(tipoCuentaViewModel);
            }

            return View();
        }
    }
}

