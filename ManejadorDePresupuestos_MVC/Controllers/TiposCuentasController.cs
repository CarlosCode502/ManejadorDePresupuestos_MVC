using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using ManejadorDePresupuestos_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#100 FORMULARIO TIPO DE CUENTAS 
    //Controlador que va a servir para manejar las multiples cuentas que puede tener
    //un usuario
    public class TiposCuentasController : Controller
    {

        #region Campos creados luego de Inyección de dependencias
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        //V#116 Evitando repetir código.
        private readonly IServicioUsuarios servicioUsuarios;
        #endregion

        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Inyectando el servicio repo)
        //V#116 Evitando repetir código (Inyectando/ utilizando el servicio usuarioId)
        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            //V#110 Insertando un Tipo de Cuenta en la Base de Datos.
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            //V#116 Evitando repetir código.
            this.servicioUsuarios = servicioUsuarios;
        }


        #region //V# 115 Listado Tipos Cuentas (Creando la action mostrar/obtener)
        /// <summary>
        /// Obtiene un listado de elementos y los manda a la vista.
        /// </summary>
        /// <returns>Un listado.</returns>
        public async Task<IActionResult> Index() //Utilizamos un indice cuando deseamos mostrar un listado de elementos
        {
            //Asignamos el id del usuario al que vamos a consultar
            //var usuarioId = 2;

            //V#116 Evitando repetir código. (utilizando el servicio usuarioId)
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Asigna el resultado del método Obtener a una variable para mandarlo a la vista
            var obtenerTiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);

            //Se manda a la vista
            return View(obtenerTiposCuentas);
        }
        #endregion


        #region //V#109 Comunicandonos con la Base de datos - Connection Strings (Realizando un prueba de conexion)
        ///// <summary>
        ///// Campo para asignar la cadena de conexion
        ///// </summary>
        //private readonly string connectionString;

        ///// <summary>
        ///// Constructor que contiene la Interfaz IConfiguration.
        ///// </summary>
        ///// <param name="configuration">Recibe o obtiene la Cadena de conexión de los perfiles de configuración</param>
        //public TiposCuentasController(IConfiguration configuration)
        //{
        //    connectionString = configuration.GetConnectionString("DefaultConnection"); //Igual que en proveedores de configuración
        //}
        #endregion

        /// <summary>
        /// Todas las peticiones Get(Muestra la vista)
        /// </summary>
        /// <returns>Una vista</returns>
        [HttpGet]
        public IActionResult Crear()
        {
            ////V#109 Comunicandonos con la Base de datos - Connection Strings
            ////Abrimos la conexión (pasamos la cadena de conexion obtenuda en el constructor)
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    //Ejecutamos una consulta a la bd con la palabra Query(Importa y utiliza Dapper)
            //    var query = connection.Query("SELECT 1").FirstOrDefault(); //SELECT 1 devuelve un 1
            //}

            return View();
        }

        #region //Antes de V#112 Aplicando la programación Asíncrona
        //[HttpPost]
        //public IActionResult Crear(TipoCuentaViewModel tipoCuentaViewModel)
        //{
        //    //V#101 VALIDANDO EL FORMULARIO( Nunca confiar en la data que envía el usuario)
        //    //Si el modelo no es valido retornar el tipo cuenta 
        //    if (!ModelState.IsValid)
        //    {
        //        //De esta manera se devuelve lo que ha escrito el usuario
        //        return View(tipoCuentaViewModel);
        //    }

        //    //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Inyectando el servicio repo)
        //    //ANTES DE AGREGAR SE DEBIA CREAR UN USUARIO contra abcd
        //    tipoCuentaViewModel.UsuarioId = 1;//Vamos a crear un usuario desde aquí
        //    repositorioTiposCuentas.Crear(tipoCuentaViewModel);//Accedemos al repositorio y metodo crear luego pasamos el modelo

        //    return View();
        //} 
        #endregion

        //V#112 Aplicando la programación Asíncrona
        //Task<> (Debe devolver un valor)
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuentaViewModel tipoCuentaViewModel)
        {
            //V#101 VALIDANDO EL FORMULARIO( Nunca confiar en la data que envía el usuario)
            //Si el modelo no es valido retornar el tipo cuenta 
            if (!ModelState.IsValid)
            {
                //De esta manera se devuelve lo que ha escrito el usuario
                return View(tipoCuentaViewModel);
            }

            //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Inyectando el servicio repo)
            //ANTES DE AGREGAR SE DEBIA CREAR UN USUARIO contra abcd
            //tipoCuentaViewModel.UsuarioId = 2;//Vamos a crear un usuario desde aquí

            //V#116 Evitando repetir código. (utilizando el servicio usuarioId)
            tipoCuentaViewModel.UsuarioId = servicioUsuarios.ObtenerUsuarioID();


            #region //V# 113 Validaciones personalizadas a Nivel del Controlador 
            //(Para evitar que un usuario pueda registrar 2 veces la misma cuenta)
            //Obtiene el resultado de la validación a nivel de controlador
            //Retorna verdadero(si existe) y falso (si no existe)
            var yaExisteTipoCuenta =
                await repositorioTiposCuentas.Existe(tipoCuentaViewModel.Nombre, tipoCuentaViewModel.UsuarioId);

            //Si es verdadero entonces se agrega un error
            if (yaExisteTipoCuenta)
            {
                //Se crea el msj de error en base al modelo (a nivel de campo)
                ModelState.AddModelError(nameof(tipoCuentaViewModel.Nombre), //Se especifica el campo
                    $"El nombre {tipoCuentaViewModel.Nombre} ya existe."); //Msj personalizado 

                //Si ya existe, lógicamente no se podrá agregar por lo que se vuelve a retornar el valor ingresado
                return View(tipoCuentaViewModel);
            }
            //Si no existe salta y se crea el registro
            #endregion

            //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Inyectando el servicio repo)
            await repositorioTiposCuentas.Crear(tipoCuentaViewModel);//Accedemos al repositorio y metodo crear luego pasamos el modelo

            //return View();

            //V# 115 Listado Tipos Cuentas (Redirigiendo al usuario)
            return RedirectToAction("Index");   
        }


        //V#117 Actualizando Tipos Cuentas (Creando el action Editar )
        /// <summary>
        /// Accion para editar TipoCuenta.
        /// </summary>
        /// <param name="id">Recibe el id del TipoCuenta</param>
        /// <returns>El Id y usuarioId si TipoCuenta no es nulo.</returns>
        [HttpGet]        
        public async Task<ActionResult> Editar(int id)
        {
            //Obtiene el usuarioId a través del servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene el tipo cuenta solo si el usuario es el mismo que el del id
            var tipoCuentaId = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            //Si el tipoCuenta no es igual al id del usuario este es nulo
            if(tipoCuentaId is null)
            {
                //SI es nulo retornar a una pag de error
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Si no Asigna el nuevo Nombre de TipoCuenta(Se actualiza)
            return View(tipoCuentaId);
        }

        //V#117 Actualizando Tipos Cuentas (Creando el action Editar POST)
        /// <summary>
        /// Action que recibe el modelo TipoCuentas y permite modificar el Nombre.
        /// </summary>
        /// <param name="tipoCuentaViewModel">Recibe el modelo TipoCuenta.</param>
        /// <returns>Modifica el registro y retorna al index en caso de error a pag especifica.</returns>
        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuentaViewModel tipoCuentaViewModel)
        {
            //Obtenemos el id del usuario por medio del servicio
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Verificamos si el usuario existe
            var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuentaViewModel.Id, usuarioId);

            //Si el tipo cuenta no existe sera nulo
            if (tipoCuentaExiste is null)
            {
                //Redirigimos al usuasrio a una pag de error
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuentas.Actualizar(tipoCuentaViewModel);
            return RedirectToAction("Index");
        }

        //V#118 Borrando tipos cuentas
        /// <summary>
        /// Action que obtiene y valida si el usuario tiene permisos o existe ese id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>El modelo hacia la vista</returns>
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            //Obtenenmos el usuario Id
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtenemos el repoTipoCuenta para ver si existe o no tiene permisos
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            //Verifica si existe el tipoCuenta 
            if(tipoCuenta is null)
            {
                //Si no existe o no tiene permiso lo redirige a pag de error
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Envia el modelo a la vista
            return View(tipoCuenta);
        }

        //V#118 Borrando tiposCuentas
        /// <summary>
        /// Elimina el registro TipoCuenta segun id
        /// </summary>
        /// <param name="id">Recibe el id del registro</param>
        /// <returns>Una vista</returns>
        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            //Obtenemos el id del usuario
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene el tipoCuenta según id y usuario id
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            //Valida si existe el tipoCuenta o tiene permisos el user
            if(tipoCuenta is null)
            {
                //Si es nulo lo redirige a la pag de error
                return RedirectToAction("NoEncontrado", "Home");
            }

            //obtiene el método borrar y le manda el id
            await repositorioTiposCuentas.Borrar(id);

            //Luego de borrar lo redirige a la vista
            return RedirectToAction("Index");
        }


        //V#114 Validaciones personalizadas con JavaScript utilizando Remote (CreandoAction)
        //Por medio de una accion httpget validamos si el usuario contiene un TipoCuenta ya registrado
        //Y genera un texto por medio de Json el cual recibira Remote de JS con el msj de error para
        //Validar en tiempo real
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            //Asignaar el id del usuario temporalmente
            //var usuarioId = 2;

            //V#116 Evitando repetir código. (utilizando el servicio usuarioId)
            var usuarioId = servicioUsuarios.ObtenerUsuarioID();

            //Obtiene si existe o no (true o false)
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            //Si yaExisteTipoCuenta
            if (yaExisteTipoCuenta)
            {
                //Va a retornar un objeto de tipo Json con el msj de erro
                //JSon permite establecer comunicación con c# (Serializarlo y transmititrlo)
                //Es un formato para representar una cadena de texto
                return Json($"El nombre {nombre} ya existe");
            }

            //Retorna un json
            return Json(true);
        }
    }
}

