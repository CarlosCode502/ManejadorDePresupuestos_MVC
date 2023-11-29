using ManejadorDePresupuestos_MVC.Models;
using ManejadorDePresupuestos_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#138 Creando Transacciones (Creando TransaccionesController 09.30)
    public class TransaccionesController : Controller
    {
        //V#138 Creando Transacciones (Creando Campos 09.30)
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;

        public TransaccionesController(IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas) 
        {
            //V#138 Creando Transacciones (Asignando a campo  09.30)
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
        }

        public IActionResult Index()
        {
            return View();
        }

        //V#138 Creando Transacciones (Action CrearTransaccion min 09.55)
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            //Obtener usuarioId 
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene ambos SelectListItem
            var modelo = new TransaccionCreacionViewModel();

            //El modelo contien los SLI accedemos al de nombre Cuentas y le pasamos el método privado que obtiene
            //todas las cuentas por el usuarioId por (Texto y Valor)
            modelo.Cuentas = await ObtenerCuentas(usuarioId);

            //Retornamos a la vista el modelo.
            return View(modelo);
        }

        //V#138 Creando Transacciones (Método privado min 11.10)
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            //Obtiene las cuentas que correspondan al usuarioId
            var cuentas = await repositorioCuentas.Buscar(usuarioId);

            //Retorna una proyección
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }


    }
}
