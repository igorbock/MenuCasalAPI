var builder = WebApplication.CreateBuilder(args);

// Definição para Dapper
DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new NpgsqlConnection(connectionString);
});
builder.Services.AddScoped<IRepository<NFC>, RepositoryGenerico<NFC>>();
builder.Services.AddScoped<IRepository<ItemNFC>, RepositoryGenerico<ItemNFC>>();
builder.Services.AddScoped<INFCService, NFCService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "LeitorNFC API Reference";
        opt.Theme = ScalarTheme.Default;
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
