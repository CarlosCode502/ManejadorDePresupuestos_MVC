using ManejadorDePresupuestos_MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//V#110 Insertando un Tipo de Cuenta en la Base de Datos (Agregando el servicio repo)
builder.Services.AddTransient<IRepositorioTiposCuentas, RepositorioTiposCuentas>(); //Es AddTrasient ya que no se comparte

//V#116 Evitando repetir código (Creando el servicio)
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>(); //No comparte instancias entre el mismo servicio

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
