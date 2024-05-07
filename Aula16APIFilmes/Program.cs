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

            // Rotas
            RouteGroupBuilder rotaFilmes = app.MapGroup("/filmes");

            #region Endpoints
            // GET      /filmes
            rotaFilmes.MapGet("/", (string? tituloFilme, double? notaMinimaIMDB) =>
            {
                IEnumerable<Filme> filmesFiltrados = filmes;

                // Verifica se foi passado a nota m�nima IMDB do filme como par�metro de busca
                if (notaMinimaIMDB is not null)
                {
                    // Filtra os filmes por Nota m�nima IMDB
                    filmesFiltrados = filmesFiltrados
                        .Where(u => u.NotaIMDB >= notaMinimaIMDB);
                }

                // Verifica se foi passado o t�tulo do filme como par�metro de busca
                if (!string.IsNullOrEmpty(tituloFilme))
                {
                    // Filtra os filmes por T�tulo
                    filmesFiltrados = filmesFiltrados
                        .Where(u => u.Titulo.Contains(tituloFilme, StringComparison.OrdinalIgnoreCase));
                }

                // Retorna os filmes filtrados
                return Results.Ok(filmesFiltrados);
            });

            // GET      /filmes/{Id}
            rotaFilmes.MapGet("/{Id}", (int Id) =>
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
            rotaFilmes.MapPost("/", (Filme filme) =>
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

            // POST     /filmes/seed
            rotaFilmes.MapPost("/seed", () =>
            {
                // Cria uma lista de filmes "mockados"
                Filme entrevistaComVampiro = new Filme("Entrevista com o Vampiro", 1994, 7.6) { Id = 1 };
                Filme srESraSmith = new Filme("Sr. e Sra. Smith", 2005, 6.5) { Id = 2 };
                Filme missaoImpossivel = new Filme("Miss�o Imposs�vel: Protocolo Fantasma", 2011, 7.4) { Id = 3 };
                Filme topGun = new Filme("Top Gun", 1986, 6.9) { Id = 4 };
                Filme osVingadores = new Filme("Os Vingadores", 2012, 8.0) { Id = 5 };
                Filme sherlockHolmes = new Filme("Sherlock Holmes", 2009, 7.6) { Id = 6 };

                // Limpa a lista de filmes
                filmes.Clear();

                // Adiciona os filmes mockados � lista
                filmes.AddRange([
                    entrevistaComVampiro,
                    srESraSmith,
                    missaoImpossivel,
                    topGun,
                    osVingadores,
                    sherlockHolmes,
                ]);

                return Results.Created();
            });

            // PUT      /filmes/{Id}
            rotaFilmes.MapPut("/{Id}", (int Id, Filme filme) =>
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
            rotaFilmes.MapDelete("/{Id}", (int Id) =>
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
