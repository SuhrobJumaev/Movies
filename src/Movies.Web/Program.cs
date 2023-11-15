using Movies.Web;
using Movies.BusinessLogic;
using Movies.DataAccess;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(Utils.ConnectionStringsSectionName));

builder.Services.ConfigureMoviesServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
