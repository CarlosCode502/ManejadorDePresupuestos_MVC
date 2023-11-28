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

        //V#136 Editar Categorías (Action Editar HttPost min 02.45)
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            //Obtiene el usuarioId del servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene las categorias según id y UsuarioId
            var categoria = await repositorioCategorias.ObtenerPorIdCategoria(id, usuarioId); 

            //Si categoria es nulo no existen o no son válidos
            if (categoria is null)
            {
                //Redirige a pag de error
                RedirectToAction("NoEncontrado", "Home");
            }

            //Envia el modelo a la vista
            return View(categoria);
        }

        //V#136 Editar Categorías (Método Editar HttpPost min 03.40)
        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaViewModel categoriaViewModel)
        {
            //Verificamos si el modelo no es valido entonces retornamos la vista con los datos erroneos ingresados
            if(!ModelState.IsValid)
            {
                return View(categoriaViewModel);
            }

            //Obtiene el usuarioId del servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene las categorias según id y UsuarioId
            var categoria = await repositorioCategorias.ObtenerPorIdCategoria(categoriaViewModel.Id, usuarioId);

            //Si categoria es nulo no existen o no son válidos
            if (categoria is null)
            {
                //Redirige a pag de error
                RedirectToAction("NoEncontrado", "Home");
            }

            //El id del usuario categoria le pasamos el usuarioid obtenido
            categoriaViewModel.UsuarioId = usuarioId;

            //Ejecutamos el método Actualizar pasandole los nuevos valores del modelo
            await repositorioCategorias.Actualizar(categoriaViewModel);

            //Retornamos a la vista nuevamente
            return RedirectToAction("Index");
        }


        //V#137 Borrar Categorias (Creando action httpget borrar categoria min 01.50)
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            //Obtenemos el id
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener la categoria por id y usuarioId
            var categoria = await repositorioCategorias.ObtenerPorIdCategoria(id, usuarioId);

            //Verifica si categoria es nulo retorna a pag de error
            if(categoria is null) { RedirectToAction("NoEncontrado","Home"); }

            //Manda el modelo con datos a la vista
            return View(categoria);
        }

        //V#137 Borrar Categorias (Creando action httpPost borrar categoria min 02.30)
        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id)
        {
            //UsuarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener la categoria por id y usuarioId
            var categoria = await repositorioCategorias.ObtenerPorIdCategoria(id, usuarioId);

            //Verifica si categoria es nulo retorna a pag de error
            if (categoria is null) { RedirectToAction("NoEncontrado", "Home"); }

            //Accede al método para borrar un registro de categorias pasandole el Id correspondiente
            await repositorioCategorias.Borrar(id);

            //Redirige a la vista index
            return RedirectToAction("Index");
        }
    }
}