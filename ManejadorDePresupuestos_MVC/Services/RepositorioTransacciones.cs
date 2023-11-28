using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#138 Creando Transacciones (RepositorioTrasacciones min 01:05)
    public class RepositorioTransacciones : IRepositorioTransacciones //hereda de la Interfaz
    {
        //campo para asignarle la cadena de conexión
        private readonly string connectionString;

        //Constructor para extraer y asicnar la CdeC
        public RepositorioTransacciones(IConfiguration configuration) //No de AutoMapper
        {
            //Obtiene la cadena de conexion y la almacena aquí
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //V#138 Creando Transacciones (Método para Crear una transacción min 03.30)
        public async Task Crear(TransaccionesViewModel transaccionesViewModel)
        {
            //Obtenemos la conexión
            using var connection = new SqlConnection(connectionString);

            //Obtenemos el id de la Consulta
            //Query que ejecuta el SP al que mandaremos todos los parametros
            var id = await connection.QuerySingleAsync<int> //<int> tipo de dato que devuelve 
                (@"Transacciones_Insertar",
                new
                {
                    //V#138 Creando Transacciones (Modificando el SP 05.30)
                    transaccionesViewModel.UsuarioId,
                    transaccionesViewModel.FechaTransaccion,
                    transaccionesViewModel.Monto,
                    transaccionesViewModel.CategoriaId,
                    transaccionesViewModel.CuentaId,
                    transaccionesViewModel.Observaciones
                },
                commandType: System.Data.CommandType.StoredProcedure); //Especifica que sera un SP

            transaccionesViewModel.Id = id; //Asignamos el id
        }
    }
}
