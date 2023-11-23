using ManejadorDePresupuestos_MVC.Models;
using ManejadorDePresupuestos_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#125 Formulario de Cuentas (Creando el controlador Cuentas)

    /// <summary>
    /// Clase que contiene los mútliples actions de la tabla Cuentas
    /// </summary>
    public class CuentasController : Controller
    {
        //V#125 Formulario de Cuentas (Luego de inyectar dependencias 2 hasta ahora repoTC y serUs)
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;
        //V#126 Insertar Cuenta (Inyectando el servicio RepositorioCuentas)
        private readonly IRepositorioCuentas repositorioCuentas;

        //V#125 Formulario de Cuentas (Inyectando dependencias 2 hasta ahora repoTC y serUs)
        //V#126 Insertar Cuenta (Inyectando el servicio RepositorioCuentas)
        /// <summary>
        /// Inyección de dependencias.
        /// </summary>
        /// <param name="repositorioTiposCuentas">Apunta a la interfaz y antes al repo.</param>
        /// <param name="servicioUsuarios">Apunta a la interfaz y luego al sevicion Obtener usuarioId.</param>
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas) 
        {
            //V#125 Formulario de Cuentas (Luego de inyectar dependencias 2 hasta ahora repoTC y serUs)
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
            //V#126 Insertar Cuenta (Inyectando el servicio RepositorioCuentas)
            this.repositorioCuentas = repositorioCuentas;
        }


        //V#127 Indice de Cuentas - Query (Creando action de Index)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Obtenemos el usuarioid
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Contiene un INNER JOIN Entre la tabla tiposCuentas y Cuentas (las obtiene por el id) 
            var cuentasConTipoCuenta = await repositorioCuentas.Buscar(usuarioId);

            //Construir el modelo
            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta) //Agrupa por el mismo tipoCuenta
                .Select(grupo => new IndiceCuentasViewModel
                {
                    TipoCuenta = grupo.Key, //Es igual al valor de tipoCuenta
                    CuentasIndice = grupo.AsEnumerable() //Obteniendo el IEnumerable del tipo cuentas
                    //Balance ya no se coloca ya que este se suma en el modeloIndiceCuentas 
                }).ToList();

            //Mandamos el modelo a la vista de tipo (IndiceCuentasViewModel)
            return View(modelo);
        }


        //V#125 Formulario de Cuentas (Creando el Controlador y Action Cuentas)
        [HttpGet]
        public async Task<IActionResult> Crear() 
        {
            //Obtener el Id del usuario desde el servicio.
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener los tiposCuentas según el usuarioID obtenido.
            //V#126 Insertar Cuenta (Centralizando un método para obtener los Tipos Cuentas por usuarioId)
            //SE COMENTO LUEGO DE 126
            //var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);

            //Se asigna el modelo del la clase que hereda de CuentaVM
            var modelo = new DropDownCuentaViewModel();

            //Modelo contiene el DropDLCuentaVM se accede al DPL a través de la propiedad (Nombre de este TiposCuentas)
            //Select hace el mapeo de tipos cuentas y crea un nuevo DropDL con los datos de este
            //(x.Nombre, x.Id.ToString() Primero el texto luego el valor (El texto debe ver el usuario)
            //modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            //V#126 Insertar Cuenta (Centralizando un método para obtener los Tipos Cuentas por usuarioId)
            //Se modifico luego de V#126
            modelo.TiposCuentas = await ObtenerTiposCuentasPorUsuarioId(usuarioId);

            //Se manda el modelo a la vista
            return View(modelo); 
        }

        //V#126 Insertar Cuenta (Action Crear)
        [HttpPost]
        public async Task<IActionResult> Crear(DropDownCuentaViewModel dropDownCuentaViewModel)
        {
            //Obtener el usuarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtener el tipoCuenta por tipoCuentaId y usuarioId(Anteriormente obtenido)
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(dropDownCuentaViewModel.TipoCuentaId, usuarioId);

            //Valida si tipoCuenta es nulo (ya que si es nulo el tipoCUenta no corresponde al usuario id o viceversa)
            if (tipoCuenta is null)
            {
                //Si es nulo lo redirige a otra pág
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Si el tipoCuenta no es nulo (Se verifica si el usuario no es válido)
            if(!ModelState.IsValid)
            {
                //Obtener los tipos cuentas del usuario para cargar la vista
                dropDownCuentaViewModel.TiposCuentas = await ObtenerTiposCuentasPorUsuarioId(usuarioId);

                //Si el modelo no es válido le mandamos el SelectList a la vista
                //Se vuelve a cargar
                return View(dropDownCuentaViewModel);
            }

            //Si el usuario es valido se crea la cuenta donde recibe el dropdown
            await repositorioCuentas.Crear(dropDownCuentaViewModel);
            return RedirectToAction("Index");
        }


        //V#126 Insertar Cuenta (Centralizando un método para obtener los Tipos Cuentas por usuarioId)
        //Esto ya fue creado en el Action Crear(HttpGet) V#125 ahora pasara a ser un método
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentasPorUsuarioId(int usuarioId)
        {
            //Obtiene los TipoCuenta por usuarioId
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);

            //Modelo contiene el DropDLCuentaVM se accede al DPL a través de la propiedad (Nombre de este TiposCuentas)
            //Select hace el mapeo de tipos cuentas y crea un nuevo DropDL con los datos de este
            //(x.Nombre, x.Id.ToString() Primero el texto luego el valor (El texto debe ver el usuario)
            //modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

            //Mapea los tiposCuentas para el UsuarioId y los carga en el SelectListItem (Texto = Nombre y valor = Id)
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}

//V#126 Insertar Cuenta