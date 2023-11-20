using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        //NO ES CORRECTO MANEJAR LA LOGICA DENTRO DEL CONSTRUCTOR YA QUE ESTAMOS VIOLENTANDO
        //EL PRINCIPIO DE RESPONSABILIDAD UNICA 

        //A este campo se le va a poder asignar la cadena de conexión
        private readonly string connectionString;

        /// <summary>
        /// Obtiene la cadena de conexión del appsettings (perfiles de configuración)
        /// </summary>
        /// <param name="configuration"></param>
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            //Cadena de conexión obtenida a través de la IConfig desde Perfiles de Config.
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region //Antes de V#112 Aplicando la programación Asíncrona
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
        ///// <summary>
        ///// Permite crear un TipoCuenta en la BD.
        ///// </summary>
        ///// <param name="tipoCuentaViewModel">Recibe el modelo de TipoCuentaViewModel</param>
        //public void Crear(TipoCuentaViewModel tipoCuentaViewModel) //Ctrl + . para agregar al IRepositorio
        //{
        //    //Abre una nueva conexión
        //    using var connection = new SqlConnection(connectionString);

        //    //Consulta que obtiene el id(int) del registro
        //    //Dapper -> QuerySingle (Realiza un query que va a obtener un solo resultado). 
        //    var id = connection.QuerySingle<int>($@"INSERT INTO Tbl_TiposCuentas_Sys (Nombre, UsuarioId, Orden)
        //                                            Values (@Nombre, @UsuarioId, 0);
        //                                            SELECT SCOPE_IDENTITY();", tipoCuentaViewModel);
        //    //SCOPE_IDENTITY() -> Trae el id del registro creado

        //    //Asigna el id obtenido al campo id del modelo tipo cuenta 
        //    tipoCuentaViewModel.Id = id;
        //}
        #endregion

        //V#112 Aplicando la programación Asíncrona(Es útil cuando existe una comunicación con externos)
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
        /// <summary>
        /// Permite crear un TipoCuenta en la BD.
        /// </summary>
        /// <param name="tipoCuentaViewModel">Recibe el modelo de TipoCuentaViewModel</param>
        public void Crear(TipoCuentaViewModel tipoCuentaViewModel) //Ctrl + . para agregar al IRepositorio
        {
            //Abre una nueva conexión
            using var connection = new SqlConnection(connectionString);

            //Consulta que obtiene el id(int) del registro
            //Dapper -> QuerySingle (Realiza un query que va a obtener un solo resultado). 
            var id = connection.QuerySingle<int>
                ($@"INSERT INTO Tbl_TiposCuentas_Sys (Nombre, UsuarioId, Orden)
                    Values (@Nombre, @UsuarioId, 0);
                    SELECT SCOPE_IDENTITY();", tipoCuentaViewModel); //SCOPE_IDENTITY() -> Trae el id del registro creado

            //Asigna el id obtenido al campo id del modelo tipo cuenta 
            tipoCuentaViewModel.Id = id;
        }
    }
}
