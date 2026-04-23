using DoQCI.Configuration;
using DoQCI.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection("Storage"));

builder.Services.Configure<PythonServiceOptions>(
    builder.Configuration.GetSection("PythonService"));

builder.Services.AddHttpClient();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var storagePath = builder.Configuration["Storage:RootPath"];
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storagePath!),
    RequestPath = "/storage"
});
if (string.IsNullOrWhiteSpace(storagePath))
    throw new InvalidOperationException("Storage:RootPath is not configured.");

Directory.CreateDirectory(Path.Combine(storagePath, "temp", "jobs"));
Directory.CreateDirectory(Path.Combine(storagePath, "temp", "downloads"));

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();