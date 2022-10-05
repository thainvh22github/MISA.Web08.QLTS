using MISA.Web08.QLTS.BL;
using MISA.Web08.QLTS.Common.Entities;
using MISA.Web08.QLTS.DL;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAssetBL, AssetBL>();
builder.Services.AddScoped<IAssetDL, AssetDL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IAssetCategoryBL, AssetCategoryBL>();
builder.Services.AddScoped<IAssetCategoryDL, AssetCategoryDL>();

// Lấy dữ liệu connection string từ file appsetting
DataContext.MySqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

// validate sử dụng ModelState
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
});

builder.Services.AddControllers();
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
