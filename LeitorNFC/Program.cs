var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<DapperDbContext>();
builder.Services.AddScoped<IRepository<NFC>, CompraNFCRepository>();
builder.Services.AddScoped<IRepository<ItemNFC>, ItemNFCRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
