//using AdamTibi.OpenWeather;
using OpenWeather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OpenWeather.OpenWeatherClient>(_ =>
{
    string apiKey = builder.Configuration["OpenWeather:Key"];
    bool isLoad = bool.Parse(builder.Configuration["LoadTest:IsActive"]);
    if (isLoad)
    {
        return new ClientStub(apiKey, Unit.Metric);
    }
    
    HttpClient httpClient = new HttpClient();
    return new OpenWeather.OpenWeatherClient(apiKey, Unit.Metric);
});

// una nueva instancia es proveida cada vez que es requerida
builder.Services.AddTransient<IRandomWrapper>(_ => new RandomWrapper());

builder.Services.AddSingleton<INowWrapper>(_ => new NowWrapper());

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
