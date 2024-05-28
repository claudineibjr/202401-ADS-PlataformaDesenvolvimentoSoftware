using Aula16APIFilmes.Database;
using Aula16APIFilmes.Endpoints;
using Aula16APIFilmes.Models;
using Aula16APIFilmes.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Aula16APIFilmes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Criação da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuração do Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirTodasOrigens",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            // Configuração de autenticação e autorização
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

            // Configuração do Banco de Dados
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MeusFilmesDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            // Registrar o TokenService
            builder.Services.AddSingleton<ITokenService, TokenService>();

            // Construção da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicialização do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Registro dos endpoints de filme
            app.RegistrarEndpointsFilme();

            // Registro dos endpoints de usuário
            app.RegistrarEndpointsUsuario();

            // Registro dos endpoints de autenticação
            app.RegistrarEndpointsAutenticacao();

            app.UseCors("PermitirTodasOrigens");

            // Execução da aplicação
            app.Run();
        }
    }
}
