using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Controllers
{
    //V#100 FORMULARIO TIPO DE CUENTAS 
    //Controlador que va a servir para manejar las multiples cuentas que puede tener
    //un usuario
    public class TiposCuentasController : Controller
    {

        #region //V#109 Comunicandonos con la Base de datos - Connection Strings (Realizando un prueba de conexion)
        /// <summary>
        /// Campo para asignar la cadena de conexion
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Constructor que contiene la Interfaz IConfiguration.
        /// </summary>
        /// <param name="configuration">Recibe o obtiene la Cadena de conexión de los perfiles de configuración</param>
        public TiposCuentasController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection"); //Igual que en proveedores de configuración
        }
        #endregion

        /// <summary>
        /// Todas las peticiones Get(Muestra la vista)
        /// </summary>
        /// <returns>Una vista</returns>
        [HttpGet]
        public IActionResult Crear()
        {
            //Abrimos la conexión (pasamos la cadena de conexion obtenuda en el constructor)
            using (var connection = new SqlConnection(connectionString))
            {
                //Ejecutamos una consulta a la bd con la palabra Query(Importa y utiliza Dapper)
                var query = connection.Query("SELECT 1").FirstOrDefault();
            }

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

