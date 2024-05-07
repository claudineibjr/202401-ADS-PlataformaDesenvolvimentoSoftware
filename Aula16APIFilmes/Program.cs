using Aula16APIFilmes.Database;
using Aula16APIFilmes.Endpoints;

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
            builder.Services.AddSwaggerGen();

            // Configura��o do Banco de Dados
            builder.Services.AddDbContext<MeusFilmesDbContext>();

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

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
