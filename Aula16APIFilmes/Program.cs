using Aula16APIFilmes.Database;
using Aula16APIFilmes.Endpoints;
using Aula16APIFilmes.Models;
using Aula16APIFilmes.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Aula16APIFilmes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria��o da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configura��o do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Filmes API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Configura��o do Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirTodasOrigens",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            // Configura��o de autentica��o e autoriza��o
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SECRET_KEY"]!))
                    };
                });
            // builder.Services.AddAuthorization();
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("admin", 
                    policy => policy.RequireRole(PerfilUsuarioEnum.Administrador.ToString())
                );

            // Configura��o do Banco de Dados
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MeusFilmesDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            // Registrar o TokenService
            builder.Services.AddSingleton<ITokenService, TokenService>();

            // Constru��o da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicializa��o do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Registro dos endpoints de filme
            app.RegistrarEndpointsFilme();

            // Registro dos endpoints de usu�rio
            app.RegistrarEndpointsUsuario();

            // Registro dos endpoints de autentica��o
            app.RegistrarEndpointsAutenticacao();

            app.UseCors("PermitirTodasOrigens");

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
