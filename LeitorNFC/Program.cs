var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();
builder.Services.AddScoped<DapperDbContext>();
builder.Services.AddScoped<IRepository<NFC>, CompraNFCRepository>();
builder.Services.AddScoped<IRepository<ItemNFC>, ItemNFCRepository>();

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
