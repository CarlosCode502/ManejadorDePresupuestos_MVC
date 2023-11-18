using Microsoft.AspNetCore.Mvc;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#100 FORMULARIO TIPO DE CUENTAS 
    //Controlador que va a servir para manejar las multiples cuentas que puede tener
    //un usuario
    public class TiposCuentasController : Controller
    {
        public IActionResult Crear()
        {
            return View();
        }
    }
}
