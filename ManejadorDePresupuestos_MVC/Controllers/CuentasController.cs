using AutoMapper;
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
        //V#131 Utilizando AutoMapper (Inyectando IMapper de AutoMapper)
        private readonly IMapper mapper;

        //V#125 Formulario de Cuentas (Inyectando dependencias 2 hasta ahora repoTC y serUs)
        //V#126 Insertar Cuenta (Inyectando el servicio RepositorioCuentas)
        //V#131 Utilizando AutoMapper (Inyectando IMapper de AutoMapper)
        /// <summary>
        /// Inyección de dependencias.
        /// </summary>
        /// <param name="repositorioTiposCuentas">Apunta a la interfaz y antes al repo.</param>
        /// <param name="servicioUsuarios">Apunta a la interfaz y luego al sevicion Obtener usuarioId.</param>
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas, IMapper mapper)
        {
            //V#125 Formulario de Cuentas (Luego de inyectar dependencias 2 hasta ahora repoTC y serUs)
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
            //V#126 Insertar Cuenta (Inyectando el servicio RepositorioCuentas)
            this.repositorioCuentas = repositorioCuentas;
            //V#131 Utilizando AutoMapper (Inyectando IMapper de AutoMapper)
            this.mapper = mapper;
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
            if (!ModelState.IsValid)
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


        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Creando el action(httpget) Editar)
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            //Obtenemos el usarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //obtenemos el método ObtenerPorIdCuenta (Mandamos los parametros que espera)
            //de CuentViewModel
            var cuenta = await repositorioCuentas.ObtenerPorIdCuenta(id, usuarioId);

            //Si no existen registros o el usuarioId no corresponde al Id (cuenta es nulo)
            if (cuenta is null)
            {
                //Va a redirigir a pág de error
                RedirectToAction("NoEncontrado", "Home");
            }

            //Se construye el modelo que va a recibir/esperar la vista
            //Los datos que va a obtener se van a asignar a ese mismo modelo
            //var modelo = new DropDownCuentaViewModel()
            //{
            //    Id = cuenta.Id,
            //    Nombre = cuenta.Nombre,
            //    TipoCuentaId = cuenta.TipoCuentaId,
            //    Balance = cuenta.Balance,
            //    Descripcion = cuenta.Descripcion
            //};

            //SUSTITUYE A LO DE ARRIBA CON AM
            //V#131 Utilizando AutoMapper (Utilizando AutoMapper para mapear)
            //nuevoMapeo<Hacia donde voy a mappearDDCVM>(cuentaorigenCuentaVM)
            //La ventaja de utilizar AutoMapper es que evitamos que se olviden escribir
            //o asignar campos y es menos código
            var modelo = mapper.Map<DropDownCuentaViewModel>(cuenta);


            //Llenamos el dropDowList
            modelo.TiposCuentas = await ObtenerTiposCuentasPorUsuarioId(usuarioId);

            //devolvemos la vista
            return View(modelo);
        }

        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Creando el action(httppost) Editar)
        [HttpPost]
        public async Task<IActionResult> Editar(DropDownCuentaViewModel dropDownCuentaViewModel)
        {
            //Obtenenmos el id 
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtenemos la cuenta pasandole el id y el usuarioId
            var cuenta = await repositorioCuentas.ObtenerPorIdCuenta(dropDownCuentaViewModel.Id, usuarioId);

            //Si la cuenta es nulo redirecciona a otra pag
            if (cuenta is null) { RedirectToAction("NoEncontrado", "Home"); }

            //obtiene el tipocuenta por tipoCuentaId y usuario id
            var tipoCuenta = await repositorioCuentas.ObtenerPorIdCuenta(dropDownCuentaViewModel.TipoCuentaId, usuarioId);

            //si tipo cuenta es nulo redirige a otra pag
            if (tipoCuenta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }

            //Una vez obtenidos los datos se utiliza el modelo para 
            await repositorioCuentas.Actualizar(dropDownCuentaViewModel);

            //Se redirige al index luego de una actualización exitosa
            return RedirectToAction("Index");
        }

        //V#131 Borrando Cuentas (Creando action HttpGet para obtener el registro a borrar)
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            //Obtenemos el usuarioId
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Va a servir para validar si la cuenta corresponde al id y usuarioId
            var cuenta = await repositorioCuentas.ObtenerPorIdCuenta(id, usuarioId);

            //Valida si el usuario existe si es nulo o no y redirige si es nulo
            if (cuenta is null) { RedirectToAction("NoEncontrado", "Home"); }

            //Enviamos la cuenta del usuario a la vista
            return View(cuenta);
        }

        //V#131 Borrando Cuentas (Creando el action HttpPost para eliminar)
        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            //Obtiene el id del usuario
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene los registros que correspondan a id y usuarioId
            var cuenta = await repositorioCuentas.ObtenerPorIdCuenta(id, usuarioId);

            //Verifica si cuenta es nulo redirige a pag de error
            if (cuenta is null) { RedirectToAction("NoEncontrado", "Error"); }

            //si cuenta no es nulo se ejecuta el método borrar y se le pasa el id del registro (lo req el método)
            await repositorioCuentas.Borrar(id);

            //Si se elimina exitosamente se redirige al usuario al Index
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

