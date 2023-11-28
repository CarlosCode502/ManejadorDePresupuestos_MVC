using Dapper;
using ManejadorDePresupuestos_MVC.Models;
using Microsoft.Data.SqlClient;

namespace ManejadorDePresupuestos_MVC.Services
{
    //V#133 Creando Categorias (Creando el RepositorioCategorias)
    public class RepositorioCategorias : IRepositorioCategorias //Va heredar de la IRepoCategorias
    {
        //Campo para sacar o obtener la cadena de conexion de AppSettings.json
        private readonly string connectionString;

        //Constructor para inicializar IConfiguration
        public RepositorioCategorias(IConfiguration configuration)
        {
            //Obtiene la cadena de conexion y la almacena aquí
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //V#133 Creando Categorias (Método Crear Categoria)
        public async Task Crear(CategoriaViewModel categoriaViewModel)
        {
            //Abrimos la conexión
            using var connection = new SqlConnection(connectionString);

            //QuerySingleAsync crea una sola consulta hacia la cadena de conexión
            //Se va  a asignar un id a esta consulta donde se reciben los datos de httpget y se guardan en post
            var id = await connection.QuerySingleAsync<int>
                (@"INSERT INTO Tbl_Categorias_Sys (NombreCategoria, TipoOperacionId, UsuarioId)
                    VALUES (@NombreCategoria, @TipoOperacionId, @UsuarioId);
                    SELECT SCOPE_IDENTITY();", categoriaViewModel); //Para obtener el id
        }

        //V#135 Indice de Categorias (Creando método Mostrar 00.35)
        //Para obtener las categorias de un usuario en especifico
        public async Task<IEnumerable<CategoriaViewModel>> Obtener(int usuarioId)
        {
            //Abre la conexión
            using var connection = new SqlConnection(connectionString);

            //Realiza una consulta para obtener todos los registros de la tabla Categorias
            return await connection.QueryAsync<CategoriaViewModel>
                (@"SELECT * FROM Tbl_Categorias_Sys 
                    WHERE UsuarioId = @UsuarioId;", new { usuarioId });
        }
    }
}
