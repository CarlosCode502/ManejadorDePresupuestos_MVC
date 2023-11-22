using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        //Task (es como un void asincrono)
        //V#110 Insertando un Tipo de Cuenta en la Base de Datos (Creando el Repositorio)
        /// <summary>
        /// Permite crear un TipoCuenta en la BD.
        /// </summary>
        /// <param name="tipoCuentaViewModel">Recibe el modelo de TipoCuentaViewModel</param>
        public async Task Crear(TipoCuentaViewModel tipoCuentaViewModel) //Ctrl + . para agregar al IRepositorio
        {
            //Abre una nueva conexión
            using var connection = new SqlConnection(connectionString);

            //Consulta que obtiene el id(int) del registro
            //Dapper -> QuerySingle (Realiza un query que va a obtener un solo resultado). 
            var id = await connection.QuerySingleAsync<int>
                //Insertar un tipo cuenta
                (@"INSERT INTO Tbl_TiposCuentas_Sys (Nombre, UsuarioId, Orden)
                    Values (@Nombre, @UsuarioId, 0);
                    SELECT SCOPE_IDENTITY();", tipoCuentaViewModel); //SCOPE_IDENTITY() -> Trae el id del registro creado

            //Asigna el id obtenido al campo id del modelo tipo cuenta 
            tipoCuentaViewModel.Id = id;
        }

        //V# 113 Validaciones personalizadas a Nivel del Controlador 
        //Creando un método que valide si existe un TipoCuenta para determinado usuario
        /// <summary>
        /// Método asíncrono que valida si un usario ya tiene 1 TipoCuenta
        /// </summary>
        /// <param name="nombre">Se asigna el nombre de la BD</param>
        /// <param name="usuarioId">Se asigna el usuarioId de la BD</param>
        /// <returns>Un booleano  (true o false), 1 si existe y 0 si no</returns>
        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            //Obtenemos y abre la cadena de conexión
            using var connection2 = new SqlConnection(connectionString);

            //Se crea la consulta para traer al PrimerElementoOPordefecto que se encuentre (Dapper)
            //QueryFirstOrDefaultAsync Obtiene el primero que encuentre o un valor por defecto(igual a 0)
            var existe = await connection2.QueryFirstOrDefaultAsync<int>
                //Select 1 obtiene el primer registro de la tabla cuando nombre y usuarioId correspondan
                (@"SELECT 1 
                FROM Tbl_TiposCuentas_Sys
                WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;", new { nombre, usuarioId });
            //Funciona muy bien para ver la existencia de un registro sin traer el registro(para mostrarlo)

            //Como el método recibe un boleano se le envia si es (true -> 1 o false -> 0)
            return existe == 1;
        }

        //V# 115 Listado Tipos Cuentas (Creando el método para mostrar)
        /// <summary>
        /// Recibe un IEnumerable con el Modelo TipoCuenta (Un listado de elementos)
        /// </summary>
        /// <param name="usuarioId">Recibe el usuarioId.</param>
        /// <returns>Una consulta con el usuarioId como parámetro.</returns>
        public async Task<IEnumerable<TipoCuentaViewModel>> Obtener(int usuarioId)//Ctrl+. para enviar al IRepositorio
        {
            //Abrimos la conexión
            using var connection = new SqlConnection(connectionString);

            //Retorna una consulta de la tabla TiposCuentas
            return await connection.QueryAsync<TipoCuentaViewModel>
                //Consulta donde pasamos como parametro el usuarioId
                //Obtiene todos los registros de la tabla TiposCuentas cuando usuarioId sea determinado
                (@"SELECT Id, Nombre, UsuarioId, Orden
                FROM Tbl_TiposCuentas_Sys
                WHERE UsuarioId = @UsuarioId
                ORDER BY Orden ", //V#121 Aplicando Mutliples Queries a la Base de Datos (Modificando el OrderBy)
                new { usuarioId });
        }


        //V#117 Actualizando Tipos Cuentas (Método simple para actualizar un registro TipoCuenta no retorna nada)
        /// <summary>
        /// Permite modificar / actualizar un registro TipoCuenta según el Id.
        /// </summary>
        /// <param name="tipoCuentaViewModel">El modelo de TipoCuenta.</param>
        /// <returns>No retorna por eso el ExecuteAsync.</returns>
        public async Task Actualizar(TipoCuentaViewModel tipoCuentaViewModel)
        {
            //Cadena de conexión
            using var connection = new SqlConnection(connectionString);

            //Permite modificar el TipoCuenta
            await connection.ExecuteAsync //Permite ejecutar un query que no va a retornar nada
                //Actualizar de la tabla TiposCuentas el campo Nombre Cuando el Id corresponda
                (@"UPDATE Tbl_TiposCuentas_Sys
                SET Nombre = @Nombre
                WHERE Id = @Id", tipoCuentaViewModel); //Tenía Error aquí (pasaba id y usuarioId en vez del modelo)
        }

        //V#117 Actualizando Tipos Cuentas (Para que el usuario pueda consultar por id) (Este si retorna)
        /// <summary>
        /// Para evitar que el usuario pueda consultar otros cuentas que no le corresponde.
        /// </summary>
        /// <param name="id">Recibe el id.</param>
        /// <param name="usuarioId">Recibe el usuarioId.</param>
        /// <returns>El primer elemento o un valor por defecto.</returns>
        public async Task<TipoCuentaViewModel> ObtenerPorId(int id, int usuarioId)
        {
            //Cadena de conexión
            using var connection = new SqlConnection(connectionString);

            //Obtiene el primer registro o un valor por defecto
            return await connection.QueryFirstOrDefaultAsync<TipoCuentaViewModel>
                //Obtiene algunos campos de la tabla TiposCuentas cuando el Id y UsuarioId correspondan
                (@"SELECT Id, Nombre, Orden
                FROM Tbl_TiposCuentas_Sys
                WHERE Id = @Id AND UsuarioId = @UsuarioId", new {id, usuarioId});
        }

        //V#118 Borrando tipos de cuentas (Método que permitira borrar un registro TipoCuenta)
        /// <summary>
        /// Permite borrar/eliminar un registro TipoCuenta según Id.
        /// </summary>
        /// <param name="id">Recibe el id.</param>
        /// <returns>Nada</returns>
        public async Task Borrar(int id)
        {
            //Obtenenmos la conexión
            using var connection = new SqlConnection (connectionString);

            //Como es un método void (por eso Task Borrar) no retornara nada            
            await connection.ExecuteAsync
                //Consulta que permitira eliminar el registro TipoCuenta según el id(por eso lo recibe)
                (@"DELETE Tbl_TiposCuentas_Sys WHERE Id = @Id", new { id }); //necesita el new ya que es un parametro espec
        }

        //V#121 Aplicando Mutliples Queries a la Base de Datos (Creando el método para ordenar)
        /// <summary>
        /// Método que recibe el modelo y reordena los registros de la tipoCuenta segun el nuevo orden.
        /// </summary>
        /// <param name="tipoCuentaViewModels">El modelo tipoCuenta</param>
        /// <returns>EL modelo con el nuevo orden.</returns>
        public async Task Ordenar(IEnumerable<TipoCuentaViewModel> tipoCuentaViewModels)
        {
            //Almacena la consulta de actualizar el orden según el id del registro
            var query = "UPDATE Tbl_TiposCuentas_Sys SET Orden = @Orden WHERE Id = @Id";

            //Obtenemos la conexión
            using var connection = new SqlConnection(connectionString);

            //Dapper nos ayuda en estos casos 
            //Ya que entiende que para cada registro de la tabla se ejecutará una nueva consulta con estos parametros
            await connection.ExecuteAsync(query, tipoCuentaViewModels);
        }

    }
}