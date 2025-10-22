using Microsoft.EntityFrameworkCore;
using ProjectIntern.Data;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {

policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "https://konusarakogrenstaj.vercel.app")
      .AllowAnyHeader()
      .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
 //   var services = scope.ServiceProvider;
 //   try
 //   {
 //       var context = services.GetRequiredService<ChatDbContext>();
 //       context.Database.Migrate();
 //   }
 //   catch (Exception ex)
 //   {
 //       var logger = services.GetRequiredService<ILogger<Program>>();
 //       logger.LogError(ex, "Veritabaný migrate edilirken bir hata oluþtu.");
  //  }
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

