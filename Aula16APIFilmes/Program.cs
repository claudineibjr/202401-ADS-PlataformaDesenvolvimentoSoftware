namespace Aula16APIFilmes
{
    public class Filme
    {
        public int Id { get; set; }
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

        public static List<Filme> filmes = new List<Filme>();

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
            app.MapGet("/filmes", () =>
            {
                // Retorna todos os filmes
                return Results.Ok(filmes);
            });

            // GET      /filmes/{Id}
            app.MapGet("/filmes/{Id}", (int Id) =>
            {
                // Procura pelo filme com o Id recebido
                Filme? filme = filmes.Find(u => u.Id == Id);
                if (filme is null)
                {
                    // Indica que o filme n�o foi encontrado
                    return Results.NotFound();
                }

                // Devolve o filme encontrado
                return Results.Ok(filme);
            });

            // POST     /filmes
            app.MapPost("/filmes", (Filme filme) =>
            {
                if (filmes.Count() == 0)
                {
                    // Atribui o Id 1 ao novo filme j� que n�o tem nenhum filme cadastrado
                    filme.Id = 1;
                }
                else
                {
                    // Atribui o Id ao novo filme como o Id m�ximo + 1
                    filme.Id = 1 + filmes.Max(u => u.Id);
                }
                filmes.Add(filme);

                return Results.Created($"/filmes/{filme.Id}", filme);
            });

            // PUT      /filmes/{Id}
            app.MapPut("/filmes/{Id}", (int Id, Filme filme) =>
            {
                // Encontra o filme especificado buscando pelo Id enviado
                int indiceFilme = filmes.FindIndex(u => u.Id == Id);
                if (indiceFilme == -1)
                {
                    // Indica que o filme n�o foi encontrado
                    return Results.NotFound();
                }

                // Mant�m o Id do filme como o Id existente
                filme.Id = Id;

                // Atribui o filme enviado na lista de filmes
                filmes[indiceFilme] = filme;

                return Results.NoContent();
            });

            // DELETE   /filmes/{Id}
            app.MapDelete("/filmes/{Id}", (int Id) =>
            {
                // Encontra o filme especificado buscando pelo Id enviado
                int indiceFilme = filmes.FindIndex(u => u.Id == Id);
                if (indiceFilme == -1)
                {
                    // Indica que o filme n�o foi encontrado
                    return Results.NotFound();
                }

                // Remove o filme encontrado da lista de filmes
                filmes.RemoveAt(indiceFilme);

                return Results.NoContent();
            });
            #endregion

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
