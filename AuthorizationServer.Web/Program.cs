using AuthorizationServer.BusinessLogic;
using AuthorizationServer.DataAccess;
using AuthorizationServer.DataAccess.Context;
using AuthorizationServer.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AppDbContext>();
    })
    .AddServer(o =>
    {
        // Enable the token endpoint.
        o.SetTokenEndpointUris("connect/token");

        // Enable the client credentials flow.
        o.AllowClientCredentialsFlow();

        // Register the signing and encryption credentials.
        o.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core options.
        o.UseAspNetCore().EnableTokenEndpointPassthrough();
        
        o.DisableAccessTokenEncryption();
    });

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();