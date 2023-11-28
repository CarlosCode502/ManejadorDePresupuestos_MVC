using ManejadorDePresupuestos_MVC.Models;
using ManejadorDePresupuestos_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#133 Creando Categorias (Creando el Controlador Categorias min 04:50)
    public class CategoriasController : Controller
    {
        //V#133 Creando Categorias (Inyectando los servicios IRepoCat y IServUsuario min 05:50)
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServicioUsuarios servicioUsuarios;

        //V#133 Creando Categorias (Inyectando los servicios IRepoCat y IServUsuario min 05:50)
        public CategoriasController(IRepositorioCategorias repositorioCategorias, IServicioUsuarios servicioUsuarios) 
        {
            //V#133 Creando Categorias (Inyectando los servicios IRepoCat y IServUsuario min 05:50)
            this.repositorioCategorias = repositorioCategorias;
            this.servicioUsuarios = servicioUsuarios;
        }

        //V#135 Indice de Categorias (Creando método Mostrar 01.50)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Obtener el usuarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener las categorias de ese usuarioId
            var categorias = await repositorioCategorias.Obtener(usuarioId);

            //Devuelve el modelo hacia la vista
            return View(categorias);
        }

        //V#133 Creando Categorias (Creando el action HttpGet Crear Categorias min 04:50)
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        //V#133 Creando Categorias (Creando el action HttpPost Crear Categorias min 06:35)
        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaViewModel categoriaViewModel)
        {
            //Verifica si el modelo no es válido 
            if (!ModelState.IsValid)
            {
                //Si no redirige los mismos datos hasta que sea válido
                return View(categoriaViewModel);
            }

            //Obtiene el usuarioId del servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene el usuarioId de la categoria y le pasa el del servicio
            categoriaViewModel.UsuarioId = usuarioId;

            //Ejecuta el método Crear pasandole el modelo completado
            await repositorioCategorias.Crear(categoriaViewModel);

            //Si todo es correcto redirige al index
            return RedirectToAction("Index");
        }
    }
}
