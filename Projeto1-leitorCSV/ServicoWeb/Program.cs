using Application.Services;
using Application.Services.Interfaces;
using Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddTransient<IServicoArquivosCSV, ServicoArquivosCSV>();
builder.Services.AddTransient<IServicoFuncionario, ServicoFuncionario>();
builder.Services.AddTransient<RepositoryDepartamentoCSV, RepositoryDepartamentoCSV>();
builder.Services.AddTransient<IServicoDepartamento, ServicoDepartamento>();



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
