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
            // Cria��o da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configura��o do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Constru��o da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicializa��o do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Endpoints
            // GET      /filmes
            app.MapGet("/filmes", () => { /* IMPLEMENTA��O */ });

            // GET      /filmes/{Id}
            app.MapGet("/filmes/{Id}", (int Id) => { /* IMPLEMENTA��O */ });

            // POST     /filmes
            app.MapPost("/filmes", (Filme filme) => { /* IMPLEMENTA��O */ });

            // PUT      /filmes/{Id}
            app.MapPut("/filmes/{Id}", (int id, Filme filme) => { /* IMPLEMENTA��O */ });

            // DELETE   /filmes/{Id}
            app.MapDelete("/filmes/{Id}", (int id) => { /* IMPLEMENTA��O */ });
            #endregion

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
