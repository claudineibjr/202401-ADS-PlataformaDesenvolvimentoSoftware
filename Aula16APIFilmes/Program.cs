namespace Aula16APIFilmes
{
    class Filme
    {
        public string Titulo { get; set; }
        public int AnoLancamento { get; set; }
        public double NotaIMDB { get; set; }

        public Filme(string titulo, int anoLancamento, double notaIMDB)
        {
            Titulo = titulo;
            AnoLancamento = anoLancamento;
            NotaIMDB = notaIMDB;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Criação da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Construção da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicialização do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Endpoints
            // GET      /filmes
            app.MapGet("/filmes", () => { /* IMPLEMENTAÇÃO */ });

            // GET      /filmes/{Id}
            app.MapGet("/filmes/{Id}", (int Id) => { /* IMPLEMENTAÇÃO */ });

            // POST     /filmes
            app.MapPost("/filmes", (Filme filme) => { /* IMPLEMENTAÇÃO */ });

            // PUT      /filmes/{Id}
            app.MapPut("/filmes/{Id}", (int id, Filme filme) => { /* IMPLEMENTAÇÃO */ });

            // DELETE   /filmes/{Id}
            app.MapDelete("/filmes/{Id}", (int id) => { /* IMPLEMENTAÇÃO */ });
            #endregion

            // Execução da aplicação
            app.Run();
        }
    }
}
