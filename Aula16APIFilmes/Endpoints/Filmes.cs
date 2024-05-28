using Aula16APIFilmes.Database;
using Aula16APIFilmes.Models;
using Aula16APIFilmes.Utils;

namespace Aula16APIFilmes.Endpoints
{
    public static class Filmes
    {
        public static void RegistrarEndpointsFilme(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaFilmes = rotas.MapGroup("/filmes");

            // GET      /filmes
            rotaFilmes.MapGet("/", (MeusFilmesDbContext dbContext, string? tituloFilme, double? notaMinimaIMDB, int pagina = 1, int tamanhoPagina = 10) =>
            {
                IEnumerable<Filme> filmesFiltrados = dbContext.Filmes.AsQueryable();

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

                int totalItens = filmesFiltrados.Count();
                List<Filme> filmes = filmesFiltrados.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();

                // Retorna os filmes filtrados
                ListaPaginada<Filme> listaFilmes = new ListaPaginada<Filme>(filmes, pagina, tamanhoPagina, totalItens);
                return TypedResults.Ok(listaFilmes);
            }).Produces<ListaPaginada<Filme>>().RequireAuthorization();

            // GET      /filmes/{Id}
            rotaFilmes.MapGet("/{Id}", (MeusFilmesDbContext dbContext, int Id) =>
            {
                // Procura pelo filme com o Id recebido
                Filme? filme = dbContext.Filmes.Find(Id);
                if (filme is null)
                {
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Devolve o filme encontrado
                return TypedResults.Ok(filme);
            }).Produces<Filme>().RequireAuthorization();

            // POST     /filmes
            rotaFilmes.MapPost("/", (MeusFilmesDbContext dbContext, Filme filme) =>
            {
                var novoFilme = dbContext.Filmes.Add(filme);
                dbContext.SaveChanges();
                
                return TypedResults.Created($"/filmes/{filme.Id}", filme);
            }).RequireAuthorization();

            // POST     /filmes/seed
            rotaFilmes.MapPost("/seed", (MeusFilmesDbContext dbContext, bool excluirFilmesExistentes = false) =>
            {
                // Cria uma lista de filmes "mockados"
                Filme entrevistaComVampiro = new Filme("Entrevista com o Vampiro", 1994, 7.6);
                Filme srESraSmith = new Filme("Sr. e Sra. Smith", 2005, 6.5);
                Filme missaoImpossivel = new Filme("Missão Impossível: Protocolo Fantasma", 2011, 7.4);
                Filme topGun = new Filme("Top Gun", 1986, 6.9);
                Filme osVingadores = new Filme("Os Vingadores", 2012, 8.0);
                Filme sherlockHolmes = new Filme("Sherlock Holmes", 2009, 7.6);

                // Excluir todos os atuais filmes
                if (excluirFilmesExistentes)
                {
                    dbContext.Filmes.RemoveRange(dbContext.Filmes);
                }

                // Adiciona os filmes mockados à lista
                dbContext.Filmes.AddRange([
                    entrevistaComVampiro,
                    srESraSmith,
                    missaoImpossivel,
                    topGun,
                    osVingadores,
                    sherlockHolmes,
                ]);

                dbContext.SaveChanges();

                return TypedResults.Created();
            }).RequireAuthorization();

            // PUT      /filmes/{Id}
            rotaFilmes.MapPut("/{Id}", (MeusFilmesDbContext dbContext, int Id, Filme filme) =>
            {
                // Encontra o filme especificado buscando pelo Id enviado
                Filme? filmeEncontrado = dbContext.Filmes.Find(Id);
                if (filmeEncontrado is null)
                {
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Mantém o Id do filme como o Id existente
                filme.Id = Id;

                // Atualiza a lista de filmes
                dbContext.Entry(filmeEncontrado).CurrentValues.SetValues(filme);

                // Salva as alterações no banco de dados
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            }).RequireAuthorization();

            // DELETE   /filmes/{Id}
            rotaFilmes.MapDelete("/{Id}", (MeusFilmesDbContext dbContext, int Id) =>
            {
                // Encontra o filme especificado buscando pelo Id enviado
                Filme? filmeEncontrado = dbContext.Filmes.Find(Id);
                if (filmeEncontrado is null)
                {
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Remove o filme encontrado da lista de filmes
                dbContext.Filmes.Remove(filmeEncontrado);

                dbContext.SaveChanges();

                return TypedResults.NoContent();
            }).RequireAuthorization();
        }
    }
}
