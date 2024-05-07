using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.Endpoints
{
    public static class Filmes
    {

        private static List<Filme> filmes = new List<Filme>();

        public static void RegistrarEndpointsFilme(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaFilmes = rotas.MapGroup("/filmes");

            // GET      /filmes
            rotaFilmes.MapGet("/", (string? tituloFilme, double? notaMinimaIMDB) =>
            {
                IEnumerable<Filme> filmesFiltrados = filmes;

                // Verifica se foi passado a nota mínima IMDB do filme como parâmetro de busca
                if (notaMinimaIMDB is not null)
                {
                    // Filtra os filmes por Nota mínima IMDB
                    filmesFiltrados = filmesFiltrados
                        .Where(u => u.NotaIMDB >= notaMinimaIMDB);
                }

                // Verifica se foi passado o título do filme como parâmetro de busca
                if (!string.IsNullOrEmpty(tituloFilme))
                {
                    // Filtra os filmes por Título
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
                    // Indica que o filme não foi encontrado
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
                    // Atribui o Id 1 ao novo filme já que não tem nenhum filme cadastrado
                    filme.Id = 1;
                }
                else
                {
                    // Atribui o Id ao novo filme como o Id máximo + 1
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
                Filme missaoImpossivel = new Filme("Missão Impossível: Protocolo Fantasma", 2011, 7.4) { Id = 3 };
                Filme topGun = new Filme("Top Gun", 1986, 6.9) { Id = 4 };
                Filme osVingadores = new Filme("Os Vingadores", 2012, 8.0) { Id = 5 };
                Filme sherlockHolmes = new Filme("Sherlock Holmes", 2009, 7.6) { Id = 6 };

                // Limpa a lista de filmes
                filmes.Clear();

                // Adiciona os filmes mockados à lista
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
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Mantém o Id do filme como o Id existente
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
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Remove o filme encontrado da lista de filmes
                filmes.RemoveAt(indiceFilme);

                return Results.NoContent();
            });
        }
    }
}
