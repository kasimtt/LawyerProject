using FluentValidation.AspNetCore;
using LawyerProject.API.Extensions;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using LawyerProject.AspNetCore.Infrastracture;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using LawyerProject.Persistence.Context;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using LawyerProject.Application;
using LawyerProject.Persistence;
using LawyerProject.Application.Mappers;
using LawyerProject.Persistence.Filters;
using LawyerProject.Infrastructure;
using LawyerProject.Infrastructure.Services.Storage.Local;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var connectionString = ConnectionString.DefaultConnection; //connection string icin bir static s�n�f yaz�labilir.

// Add services to the container.
#region Controller - Fluent Validation - Json
// IServiceCollection arabirimine Controllers hizmetini ekler. Bu, Web API projesinde denetleyicilerin (Controller'lar�n) kullan�lmas�n� sa�lar.
builder.Services.AddControllers(opt =>
{
    //opt.Filters.Add(typeof(UserActivity_));
    opt.Filters.Add<ValidationFilter>();
})
    // FluentValidation k�t�phanesinin yap�land�rmas�n� ekler. 
    .AddFluentValidation(configuration =>
    {
        // Veri anotasyonlar� tabanl� do�rulama �zelli�ini devre d��� b�rak�r. Bu, veri modellerinin �zerindeki DataAnnotations do�rulama �zelliklerini kullanmadan sadece FluentValidation kurallar�n� kullanmay� sa�lar.
        configuration.DisableDataAnnotationsValidation = true;

        // YoneticiValidator s�n�f�n�n bulundu�u derlemedeki do�rulay�c�lar� FluentValidation yap�land�rmas�na kaydeder. Bu �ekilde, belirtilen derlemedeki t�m do�rulay�c�lar otomatik olarak kullan�labilir hale gelir.
        //configuration.RegisterValidatorsFromAssemblyContaining<YoneticiValidator>(); -->validatorleri yazd�ktan sonra eklicez

        //otomatik do�rulama �zelli�ini etkinle�tirir. Bu, bir HTTP iste�i al�nd���nda, do�rulama kurallar�n�n otomatik olarak uygulanmas�n� sa�lar.
        configuration.AutomaticValidationEnabled = true;
    })

    //JSON serile�tirme yap�land�rmas�n� ekler.
    .AddJsonOptions(configurations =>
    {
        // Serile�tirmesinde d�ng�sel referanslar� i�lemek i�in referans i�leyiciyi ayarlar. IgnoreCycles, d�ng�sel referanslar� g�rmezden gelerek olas� bir sonsuz d�ng�y� �nler.
        configurations.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
#endregion

#region Serilog
builder.Services.AddLogging();

// Veritaban�na aktar�m yap�l�rken kullan�lacak konfig�rasyonlar "sinkOptions"da belirlenir.

// Veritaban� tablosunun �zellikleri "columnOptions"da belirlenir.
var LogEventsTableColumnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn {ColumnName = "UserName", PropertyName = "UserName", DataType = SqlDbType.VarChar, DataLength = 64},
        new SqlColumn {ColumnName = "ApiPath", PropertyName = "ApiPath", DataType = SqlDbType.VarChar, NonClusteredIndex = true},
        new SqlColumn {ColumnName = "IpAdres", PropertyName = "IpAdres", DataType = SqlDbType.VarChar, DataLength = 20}
    }
};
LogEventsTableColumnOptions.Store.Remove(StandardColumn.Properties);
LogEventsTableColumnOptions.Store.Remove(StandardColumn.MessageTemplate);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "LogEvents",
            AutoCreateSqlDatabase = true,
            AutoCreateSqlTable = true
        },
        columnOptions: LogEventsTableColumnOptions,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

Log.CloseAndFlush();
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

#endregion

#region Api Versioning & Api Explorer
// IServiceCollection arabirimine ApiVersioning hizmetini ekler. 
builder.Services.AddApiVersioning(options =>
{
    // Varsay�lan API s�r�m�n� ayarlar. Burada 1.0 s�r�m�, projenin varsay�lan API s�r�m� olarak belirlenir.
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Belirtilmeyen durumlarda varsay�lan API s�r�m�n�n kullan�lmas�n� sa�lar. Yani istemci bir API s�r�m� belirtmezse, options.DefaultApiVersion ile belirtilen varsay�lan s�r�m kullan�l�r.
    options.AssumeDefaultVersionWhenUnspecified = true;

    // API s�r�mlerini yan�tta raporlama ayar�n� etkinle�tirir. Bu, API yan�tlar�nda kullan�lan s�r�m bilgisini g�nderir.
    options.ReportApiVersions = true;
});

// IServiceCollection arabirimine VersionedApiExplorer hizmetini ekler. 
builder.Services.AddVersionedApiExplorer(options =>
{
    // API grup ad� bi�imini belirler. 'v'VVV format� kullan�larak grup adlar� olu�turulur. VVV, API s�r�m�n� temsil eden yer tutucudur.
    options.GroupNameFormat = "'v'VVV";

    // API s�r�m�n� URL i�inde yerine koyma ayar�n� etkinle�tirir. B�ylece, API isteklerinde URL i�indeki s�r�m belirtilmi� olur.
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region Swagger
// Swagger yap�land�rma se�eneklerini SwaggerGenOptions i�in yap�land�ran ConfigureSwaggerOptions s�n�f�n� IConfigureOptions arabirimine ekler. Bu, Swagger belgelendirmesinin yap�land�r�lmas�n� sa�lar.
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// IServiceCollection arabirimine SwaggerGen hizmetini ekler.
builder.Services.AddSwaggerGen(options =>
{
    // Mevcut projenin XML belgelendirme dosyas�n�n ad�n� olu�turur. Bu dosya, projenin i�inde API hakk�nda detayl� bilgileri i�erir.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    // XML belgelendirme dosyas�n�n tam yolunu olu�turur. AppContext.BaseDirectory, uygulaman�n �al��t��� dizini temsil eder.
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // Swagger belgelendirmesine XML belgelendirme dosyas�n� dahil etme ayar�n� yapar. Bu sayede, API Controller'lar�ndaki �rnekler, parametreler ve d�n�� de�erleri gibi detayl� a��klamalar� Swagger belgelerine ekler.
});
#endregion
#region Authentication - JWT
// IServiceCollection arabirimine kimlik do�rulama hizmetini ekler. JwtBearerDefaults.AuthenticationScheme, JWT Bearer kimlik do�rulamas�n�n kullan�lmas�n� belirtir.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // JWT Bearer kimlik do�rulama se�eneklerini yap�land�rmak i�in kullan�l�r. 
    .AddJwtBearer(options =>
    {
        // Yap�land�rmadaki JWT anahtar�n� key de�i�kenine atar. JWT anahtar�, token olu�turma ve do�rulama i�lemlerinde kullan�l�r.
        var key = builder.Configuration["Auth:Jwt:Key"];

        // Yap�land�rmadaki JWT yay�nc�s�n� (Issuer) issuer de�i�kenine atar. JWT yay�nc�s�, tokenin hangi kaynaktan geldi�ini belirtir.
        var issuer = builder.Configuration["Auth:Jwt:Issuer"];

        // JWT anahtar�n� simetrik bir g�venlik anahtara (SymmetricSecurityKey) d�n��t�r�r. Anahtar, UTF-8 kodlamas�n� kullanarak byte dizisinden olu�turulur.
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // Token do�rulama parametrelerini yap�land�r�r. TokenValidationParameters nesnesi, tokenin do�rulama s�recinde kullan�lan parametreleri i�erir.
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            // Tokenin hedef kitle (audience) do�rulamas�n� devre d��� b�rak�r.
            ValidateAudience = false,

            // Tokenin yay�nc� (issuer) do�rulamas�n� etkinle�tirir.
            ValidateIssuer = true,

            // Tokenin s�resi dolmu� (expired) olup olmad���n� do�rular.
            ValidateLifetime = true,

            // Yay�nc� taraf�ndan kullan�lan imza anahtar�n�n do�rulanmas�n� etkinle�tirir.
            ValidateIssuerSigningKey = true,

            // Do�rulama i�in kabul edilen yay�nc�y� belirtir.
            ValidIssuer = issuer,

            // �mza anahtar�n� belirtir. Yay�nc� taraf�ndan kullan�lan anahtar, do�rulama i�lemi s�ras�nda kullan�l�r.
            IssuerSigningKey = symmetricKey
        };
    });
#endregion
#region Authorization - Policy
// IServiceCollection arabirimine yetkilendirme (authorization) hizmetini ekler. 
builder.Services.AddAuthorization(options =>
{
    // HasSpecialRules ad�nda bir yetkilendirme politikas� ekler. Yetkilendirme politikas�, belirli bir kural k�mesine dayanarak kullan�c�n�n eri�imini kontrol etmek i�in kullan�l�r. 
    options.AddPolicy("HasSpecialRules", builder =>
    {
        // Kullan�c�n�n "Admin" rol�ne sahip olmas�n� gerektiren bir kural ekler. Bu, sadece "Admin" rol�ne sahip kullan�c�lar�n bu yetkilendirme politikas�n� ge�ebilece�i anlam�na gelir.
        builder.RequireRole("Admin");

        // Kullan�c�n�n "NameIdentifier" ad�nda bir talepte bulunmas�n� gerektiren bir kural ekler. ClaimTypes.NameIdentifier, kullan�c�n�n benzersiz kimlik bilgisini temsil eden bir talep tipini ifade eder. Kullan�c�n�n bu talebi sunmas� gerekmektedir.
        builder.RequireClaim(ClaimTypes.NameIdentifier);

        // Kullan�c�n�n kullan�c� ad�n�n "kasimislamtatli" olmas�n� gerektiren bir kural ekler. Sadece kullan�c� ad� "kasimislamtatli" olan kullan�c�lar bu yetkilendirme politikas�n� ge�ebilir.
        builder.RequireUserName("kasimislamtatli");
    });
});
#endregion

//->Automapperlari eklemeyi unutma kanka

#region Cors
// IServiceCollection arabirimine CORS (Cross-Origin Resource Sharing) hizmetini ekler. CORS, web uygulamalar�n�n farkl� kaynaklardan gelen isteklere izin vermesini sa�layan bir mekanizmad�r.
// CORS hizmetini eklemek, Web API'nin farkl� etki alanlar�ndan gelen istekleri kabul etmesini ve gerekirse yan�tlara uygun CORS ba�l�klar�n� eklemesini sa�lar. Bu �ekilde, Web API'ye d�� kaynaklardan eri�im sa�lanabilir.
builder.Services.AddCors();
#endregion
#region DbContext
// IServiceCollection arabirimine XXXContext tipinde bir veritaban� ba�lam�n� (DbContext) ekler.
builder.Services.AddDbContext<LawyerProjectContext>(options =>
{
    // SQL Server veritaban� sa�lay�c�s�n� kullanarak veritaban� ba�lam�n�n yap�land�r�lmas�n� yapar. connectionString parametresi, SQL Server ba�lant� dizesini temsil eder. 
    // b => b.MigrationsAssembly("SalihPoc.Api") ifadesi, veritaban� migrasyonlar�n� uygulamak i�in kullan�lacak olan migrasyon derlemesinin ad�n� belirtir. Bu ad, "SalihPoc.Api" olarak belirtilmi�tir.

    options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("LawyerProject.Persistence"));
});
#endregion   

builder.Services.AddContainerWithDependenciesApplication();
builder.Services.AddContainerWithDependenciesPersistence();
builder.Services.AddContainerWithDependenciesInfrastucture();
builder.Services.AddStorage<LocalStorage>();  // istedi�imiz storage burada aktif edebiliriz 

builder.Services.AddAutoMapper(typeof(CasesProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"{description.GroupName}/swagger.json",
                $"LawyerProject API {description.GroupName.ToUpperInvariant()}");
        }
    });

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
