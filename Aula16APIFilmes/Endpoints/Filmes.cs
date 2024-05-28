using Aula16APIFilmes.Database;
using Aula16APIFilmes.Models;
using Aula16APIFilmes.Service;
using Aula16APIFilmes.Utils;
using System.Security.Claims;

namespace Aula16APIFilmes.Endpoints
{
    public static class Filmes
    {
        public static void RegistrarEndpointsFilme(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaFilmes = rotas.MapGroup("/filmes");

            // GET      /filmes
            rotaFilmes.MapGet("/", (MeusFilmesDbContext dbContext, ClaimsPrincipal _usuarioLogado, string? tituloFilme, double? notaMinimaIMDB, int pagina = 1, int tamanhoPagina = 10) =>
            {
                IEnumerable<Filme> filmesFiltrados = dbContext.Filmes.AsQueryable();

                // Obtém o usuário logado para obter os filmes de acordo com a classificação indicativa
                Usuario? usuarioLogado = UserService.GetUsuarioPorUsuarioLogado(dbContext, _usuarioLogado);
                if (usuarioLogado == null)
                {
                    return Results.NotFound();
                }
                int idadeUsuarioLogado = UsuarioUtils.CalcularIdade(usuarioLogado.DataNascimento);

                // Filtra os filmes pela classificação indicativa
                filmesFiltrados = filmesFiltrados
                    .Where(f => FilmeUtils.UsuarioPodeAssistirFilme(f, idadeUsuarioLogado));

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
            rotaFilmes.MapGet("/{Id}", (MeusFilmesDbContext dbContext, ClaimsPrincipal _usuarioLogado, int Id) =>
            {
                // Procura pelo filme com o Id recebido
                Filme? filme = dbContext.Filmes.Find(Id);
                if (filme is null)
                {
                    // Indica que o filme não foi encontrado
                    return Results.NotFound();
                }

                // Obtém o usuário logado para verificar se o usuário pode assistir o filme de acordo com a classificação indicativa
                Usuario? usuarioLogado = UserService.GetUsuarioPorUsuarioLogado(dbContext, _usuarioLogado);
                if (usuarioLogado == null)
                {
                    return Results.NotFound();
                }
                int idadeUsuarioLogado = UsuarioUtils.CalcularIdade(usuarioLogado.DataNascimento);

                if (!FilmeUtils.UsuarioPodeAssistirFilme(filme, idadeUsuarioLogado))
                {
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
            }).RequireAuthorization("admin");

            // POST     /filmes/seed
            rotaFilmes.MapPost("/seed", (MeusFilmesDbContext dbContext, bool excluirFilmesExistentes = false) =>
            {
                // Cria uma lista de filmes "mockados"
                Filme entrevistaComVampiro = new Filme("Entrevista com o Vampiro", 1994, 7.6, "18");
                Filme srESraSmith = new Filme("Sr. e Sra. Smith", 2005, 6.5, "14");
                Filme missaoImpossivel = new Filme("Missão Impossível: Protocolo Fantasma", 2011, 7.4, "12");
                Filme topGun = new Filme("Top Gun", 1986, 6.9, "12");
                Filme osVingadores = new Filme("Os Vingadores", 2012, 8.0, "12");
                Filme sherlockHolmes = new Filme("Sherlock Holmes", 2009, 7.6, "14");
                Filme loboWallStreet = new Filme("O Lobo de Wall Street", 2013, 8.2, "18");
                Filme fugaDasGalinhas = new Filme("A Fuga das Galinhas", 2000, 7.1, "L");


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
                    loboWallStreet,
                    fugaDasGalinhas,
                ]);

                dbContext.SaveChanges();

                return TypedResults.Created();
            }).RequireAuthorization("admin");

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
            }).RequireAuthorization("admin");

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
            }).RequireAuthorization("admin");
        }
    }
}
