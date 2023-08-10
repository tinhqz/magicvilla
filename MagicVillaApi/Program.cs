using MagicVillaApi;
using MagicVillaApi.Datos;
using MagicVillaApi.Repositorio;
using MagicVillaApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// !! servicios personalizados para poder ser utilizado como inyeccion de dependencias

// para serializar los modelos entre controladores
builder.Services.AddControllers().AddNewtonsoftJson();
// agregando cadena de coneccion
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// configurando el automapper, indicamos cual es la clase que tiene el mapeo de objetos
builder.Services.AddAutoMapper(typeof(MappingConfig));
// agregando repositorios
builder.Services.AddScoped<IVillaRepositorio, VillaRepositorio>();
builder.Services.AddScoped<INumeroVillaRepositorio, NumeroVillaRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
