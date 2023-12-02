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
        //V#141 DropDown Cascada (Creando propiedad 04.05)
        private readonly IRepositorioCategorias repositorioCategorias;
        //V#142 Insertando la Transacción (IAction Crear Transaccion HttpPost min 02.40)
        private readonly IRepositorioTransacciones repositorioTransacciones;

        public TransaccionesController(IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas, IRepositorioCategorias repositorioCategorias, IRepositorioTransacciones repositorioTransacciones) 
        {
            //V#138 Creando Transacciones (Asignando a campo  09.30)
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
            //V#141 DropDown Cascada (Creando propiedad 04.05)
            this.repositorioCategorias = repositorioCategorias;
            //V#142 Insertando la Transacción (IAction Crear Transaccion HttpPost min 02.40)
            this.repositorioTransacciones = repositorioTransacciones;
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

            //V#141 DropDown Cascada (DropDown dependiente de otro Categoría depende de TOp min 08.08)
            //(Se coloco por defecto TipoOperacionId = Gastos)
            modelo.Categorias = await ObtenerCategorias1(usuarioId, modelo.TipoOperacionId);

            //Retornamos a la vista el modelo.
            return View(modelo);
        }


        //V#142 Insertando la Transacción (IAction Crear Transaccion HttpPost min 00.50)
        [HttpPost]
        public async Task<IActionResult> Crear(TransaccionCreacionViewModel modelo)
        {
            //Obtener usuarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Si el modelo no es valido por x o y razon
            if(!ModelState.IsValid)
            {
                //Cuentas obtiene las cuentas que correspondel al usuarioId
                modelo.Cuentas = await ObtenerCuentas(usuarioId);

                //Obtiene las categgorias que recibe el usuarioId y tipoOperacion enum
                modelo.Categorias = await ObtenerCategorias1(usuarioId, modelo.TipoOperacionId); //TipoOperación ya contiene un valor por defecto = 2 por ahora

                //Retorna el modelo nuevamente
                return View(modelo);
            }

            //obtiene la cuenta por usuario id y cuenta
            var cuenta = await repositorioCuentas.ObtenerPorIdCuenta(modelo.CuentaId, usuarioId);

            //Verifica si la cuenta corresponde al usuarioid
            if(cuenta is null) { RedirectToAction("NoEncontrado", "Home"); }


            //Obtiene la categoria segun idcategoria y usuarioId
            var categoria = await repositorioCategorias.ObtenerPorIdCategoria(modelo.CategoriaId, usuarioId);

            //Verifica si categoria es nulo
            if (categoria is null) { RedirectToAction("NoEncontrado", "Home"); }

            //El campo usuarioId del modelo sera igual al usuario id obtenido
            modelo.UsuarioId = usuarioId;   

            //Verifia si la propiedad tipo operación es igual a un gasto
            if(modelo.TipoOperacionId == TipoOperacionEnum.Gastos) 
            {
                //Si es un gasto se multiplicara por -1 para tener el mismo valor pero en negativo
                modelo.Monto *= -1;
            }

            //Ejecutamos la accion crear y le pasamos el modelo con los datos que recibe
            await repositorioTransacciones.Crear(modelo);

            //Redirige a la vista Index
            return RedirectToAction("Index");
        }


        //V#138 Creando Transacciones (Método privado min 11.10)
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId)
        {
            //Obtiene las cuentas que correspondan al usuarioId
            var cuentas = await repositorioCuentas.Buscar(usuarioId);

            //Retorna una proyección
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        //V#141 DropDown Cascada (Método privado para cargar los datos de categoria en un dropdown 04.10)
        //Obtener seguna usuarioId y TipoOperacion seleccionado
        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias1(int usuarioId, TipoOperacionEnum tipoOperacionId)
        {
            //Asignación inecesaria ya fue declarado en el método abajo
            //var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtenemos las categorias que correspondan al UsuarioId y TipoOperacion (se muestran segun tipoOp)
            //Se mandan los parametros esperados
            var categorias = await repositorioCategorias.ObtenerPorUsuarioIdyTipoOperacion(usuarioId, tipoOperacionId);
            //Devuelve un mapeo en un selectlistitem de nombre y categoria
            return categorias.Select(x => new SelectListItem(x.NombreCategoria, x.Id.ToString()));
        }


        //V#141 DropDown Cascada (Creando la acción de la función  Js 03.10)
        //End Point o la acción de transacciones (ÓbtenerCategorias desde el cuerpo )
        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias2([FromBody] TipoOperacionEnum tipoOperacionEnum)
        {
            //Obtenemos el usuario id
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtenemos el Drop según usuarioId y TipoOperacion
            var categorias = await ObtenerCategorias1(usuarioId, tipoOperacionEnum);

            //Retorna una respuesta exitosa enviando el modelo(lo recibira el script en la vista crear min 06.26)
            return Ok(categorias);
        }

    }
}
