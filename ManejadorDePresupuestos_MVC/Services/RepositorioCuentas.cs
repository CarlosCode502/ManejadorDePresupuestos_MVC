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

    }
}
