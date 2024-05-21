using Aula16APIFilmes.Database;
using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static void RegistrarEndpointsUsuario(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaUsuarios = rotas.MapGroup("/usuarios");

            // GET      /usuairos
            rotaUsuarios.MapGet("/", (MeusFilmesDbContext dbContext) =>
            {
                IEnumerable<Usuario> usuariosFiltrados = dbContext.Usuarios;

                // Retorna os usuarios filtrados
                return TypedResults.Ok(usuariosFiltrados);
            });

            // GET      /usuarios/{Id}
            rotaUsuarios.MapGet("/{Id}", (MeusFilmesDbContext dbContext, int Id) =>
            {
                // Procura pelo usuário com o Id recebido
                Usuario? usuario = dbContext.Usuarios.Find(Id);
                if (usuario is null)
                {
                    // Indica que o usuário não foi encontrado
                    return Results.NotFound();
                }

                // Devolve o usuário encontrado
                return TypedResults.Ok(usuario);
            }).Produces<Usuario>();

            // POST     /usuarios
            rotaUsuarios.MapPost("/", (MeusFilmesDbContext dbContext, Usuario usuario) =>
            {
                var novoUsuario = dbContext.Usuarios.Add(usuario);
                dbContext.SaveChanges();

                return TypedResults.Created($"/usuarios/{usuario.Id}", usuario);
            });

            // PUT      /usuarios/{Id}
            rotaUsuarios.MapPut("/{Id}", (MeusFilmesDbContext dbContext, int Id, Usuario usuario) =>
            {
                // Encontra o usuário especificado buscando pelo Id enviado
                Usuario? usuarioEncontrado = dbContext.Usuarios.Find(Id);
                if (usuarioEncontrado is null)
                {
                    // Indica que o usuário não foi encontrado
                    return Results.NotFound();
                }

                // Mantém o Id do usuario como o Id existente
                usuario.Id = Id;

                // Atualiza a lista de usuarios
                dbContext.Entry(usuarioEncontrado).CurrentValues.SetValues(usuario);

                // Salva as alterações no banco de dados
                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });

            // DELETE   /usuarios/{Id}
            rotaUsuarios.MapDelete("/{Id}", (MeusFilmesDbContext dbContext, int Id) =>
            {
                // Encontra o usuário especificado buscando pelo Id enviado
                Usuario? usuarioEncontrado = dbContext.Usuarios.Find(Id);
                if (usuarioEncontrado is null)
                {
                    // Indica que o usuário não foi encontrado
                    return Results.NotFound();
                }

                // Remove o usuário encontrado da lista de usuarios
                dbContext.Usuarios.Remove(usuarioEncontrado);

                dbContext.SaveChanges();

                return TypedResults.NoContent();
            });
        }
    }
}
