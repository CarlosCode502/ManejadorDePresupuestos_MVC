using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#126 Insertar Cuenta     
    /// <summary>
    /// Contiene los distintos métodos para realizar la diferentes consultas a la BD de la tabla Cuentas.
    /// </summary>
    public class RepositorioCuentas : IRepositorioCuentas
    {
        //Se crea un campo para asignar la cadena de conexión y poder aperturarla luego.
        private readonly string connectionString;

        //V#126 Insertar Cuenta (Extraer la cadena de conexión de los servidores de configuración)
        //Pasar como parámetro la IConfiguration para obtener datos de los sev de conf 
        public RepositorioCuentas(IConfiguration configuration)
        {
            //Obtiene la cadena de conexion
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// //V#126 Insertar Cuenta (Método Crear/Insertar)
        /// </summary>
        /// <param name="cuentaViewModel">Recibe el modelo</param>
        /// <returns>EL id con los datos</returns>
        public async Task Crear(CuentaViewModel cuentaViewModel)
        {
            //Abre la conexión
            using var connection = new SqlConnection(connectionString);

            //QuerySingleAsync crea una sola consulta hacía la cadena de conexión
            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Tbl_Cuentas_Sys (Nombre, TipoCuentaId, Balance, Descripcion)
                    VALUES (@Nombre, @TipoCuentaId, @Balance, @Descripcion);
                    SELECT SCOPE_IDENTITY();", cuentaViewModel); //Obtiene el id (Olvide mandarle el modelo)

            //El id lo manda al modelo
            //SI ES DINAMIC ES PORQUE NO SE INICIALIZO (Se debía poner <int>)
            cuentaViewModel.Id = id;
        }


        //V#127 Indice de Cuentas - Query (Creando método Buscar)
        /// <summary>
        /// Obtiene un INNER JOIN entre 2 tablas
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CuentaViewModel>> Buscar(int usuarioId)
        {
            //Establece la conexión
            using var connection = new SqlConnection(connectionString);

            //Mostrar el listado de cuentas del usuario
            //Calculo de tipo cuenta de los distintos balances
            return await connection.QueryAsync<CuentaViewModel>
                //Cuando existe un campo con el mismo nombre en ambas tablas se debe especificar a que campo se refiere
                //Obtiene la union de algunos campos de una tabla con otra
                //El modelo cuenta no contiene un campo de nombre TipoCueta por lo que se debe crear
                (@"SELECT Tbl_Cuentas_Sys.Id, Tbl_Cuentas_Sys.Nombre, Balance, tc.Nombre AS TipoCuenta
                    FROM Tbl_Cuentas_Sys
                    INNER JOIN Tbl_TiposCuentas_Sys tc
                    ON tc.Id = Tbl_Cuentas_Sys.TipoCuentaId
                    WHERE tc.UsuarioId = @UsuarioId
                    ORDER BY tc.Orden", new {usuarioId}); //Pasamos usuarioId que es el que espera para la BD
        }


        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Creando el modelo ObtenerPorId la Cuenta)
        //Creando el método Obtener cuenta por id
        /// <summary>
        /// Obtiene la cuenta por Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        public async Task<CuentaViewModel> ObtenerPorIdCuenta(int id, int usuarioId)
        {
            //Establece la conexión
            using var connection = new SqlConnection(connectionString);

            //Obtiene el primer valor que coincida o un valor por defecto se le pasa el id y usuarioId
            return await connection.QueryFirstOrDefaultAsync<CuentaViewModel>
                (@"SELECT Tbl_Cuentas_Sys.Id, Tbl_Cuentas_Sys.Nombre, Balance, TipoCuentaId, Descripcion
                    FROM Tbl_Cuentas_Sys
                    INNER JOIN Tbl_TiposCuentas_Sys tc
                    ON tc.Id = Tbl_Cuentas_Sys.TipoCuentaId
                    WHERE tc.UsuarioId = @UsuarioId AND Tbl_Cuentas_Sys.Id = @Id",
                    new {id, usuarioId}); //Pasamos usuarioId que es el que espera para la BD
        }


        //V#130 Editando Cuentas - Agregando Íconos a la Aplicación (Creando el método actualizar Cuenta)
        public async Task Actualizar(DropDownCuentaViewModel dropDownCuentaViewModel)
        {
            //Abrimos la conexión
            using var connection = new SqlConnection(connectionString);

            //Ejecuta la consulta pero no devuelve ningun valor
            await connection.ExecuteAsync
                (@"UPDATE Tbl_Cuentas_Sys
                    SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion, TipoCuentaId = @TipoCuentaId
                    WHERE Id = @Id", dropDownCuentaViewModel);
        }
    }
}