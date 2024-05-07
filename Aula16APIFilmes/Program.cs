using Aula16APIFilmes.Database;
using Aula16APIFilmes.Endpoints;

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

            // Configuração do Banco de Dados
            builder.Services.AddDbContext<MeusFilmesDbContext>();

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

            // Execução da aplicação
            app.Run();
        }
    }
}
