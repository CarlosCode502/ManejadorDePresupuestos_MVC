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
                    WHERE UsuarioId = @UsuarioId", new { usuarioId });
        }


        //V#136 Editar Categorías (Método ObtenerPorIdCategoria min 00.50)
        public async Task<CategoriaViewModel> ObtenerPorIdCategoria(int id, int usuarioId)
        {
            //Abre o obtiene la conexión
            using var connection = new SqlConnection(connectionString);

            //QueryFirstOrDefaultAsync Obten lo primero que encuentres o un valor por defecto si es 0
            //Obtiene las categorias que corespondan al Id y UsuarioId
            return await connection.QueryFirstOrDefaultAsync<CategoriaViewModel>
                (@"SELECT * FROM Tbl_Categorias_Sys 
                    WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId } ); 
            //Cuando no le paso los paramentros sale un
            //Error de tipo "An unhandled exception occurred while processing the request."
        }


        //V#136 Editar Categorías (Método Actualizar min 01.50)
        public async Task Actualizar(CategoriaViewModel categoriaViewModel)
        {
            //Conexión
            using var connection = new SqlConnection(connectionString);

            //Realiza una consulta a la Bd de tipo Update a la tabla Categorias cuando el Id sea = @Id
            await connection.ExecuteAsync
                (@"UPDATE Tbl_Categorias_Sys
                    SET NombreCategoria = @NombreCategoria, TipoOperacionId = @TipoOperacionId
                    WHERE Id = @Id", categoriaViewModel);
        }

        //V#137 Borrar Categorias (Creando método borrar categoria min 0.40)
        public async Task Borrar(int id)
        {
            //Conexión
            using var connection = new SqlConnection(connectionString);

            //Query para eliminar un registro de Categoria Según id
            await connection.ExecuteAsync
                (@"DELETE Tbl_Categorias_Sys
                    WHERE Id = @Id", new { id });
        }


        //V#141 DropDown Cascada (Método que permita obtener las categorias según usuarioId y tipoOperacion min 04.50)
        public async Task<IEnumerable<CategoriaViewModel>> ObtenerPorUsuarioIdyTipoOperacion(int usuarioId, TipoOperacionEnum tipoOperacionId)
        {
            //Obtiene la conexion
            using var connection = new SqlConnection(connectionString);

            //Retorna los registros de la tabla categorias que correspondan a usuario id y tipoOperaciónId
            return await connection.QueryAsync<CategoriaViewModel>
                (@"SELECT * FROM Tbl_Categorias_Sys 
                    WHERE UsuarioId = @UsuarioId
                    AND TipoOperacionId = @TipoOperacionId",
                    new { usuarioId, tipoOperacionId });
        }
    }
}