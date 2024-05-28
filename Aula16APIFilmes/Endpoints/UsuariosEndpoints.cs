using Aula16APIFilmes.Database;
using Aula16APIFilmes.DTOs;
using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static void RegistrarEndpointsUsuario(this IEndpointRouteBuilder rotas)
        {
            // Grupamento de rotas
            RouteGroupBuilder rotaUsuarios = rotas.MapGroup("/usuarios").RequireAuthorization("admin");

            // GET      /usuairos
            rotaUsuarios.MapGet("/", (MeusFilmesDbContext dbContext) =>
            {
                IEnumerable<Usuario> usuariosFiltrados = dbContext.Usuarios;

                // Retorna os usuarios filtrados
                return Results.Ok(usuariosFiltrados.Select(u => u.GetUsuarioDtoOutput()).ToList());
            }).Produces<List<UsuarioDtoOutput>>();

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
                return TypedResults.Ok<UsuarioDtoOutput>(usuario.GetUsuarioDtoOutput());
            }).Produces<UsuarioDtoOutput>();

            // POST     /usuarios
            rotaUsuarios.MapPost("/", (MeusFilmesDbContext dbContext, UsuarioDtoInput usuario) =>
            {
                Usuario _novoUsuario = usuario.ToUsuario();
                var novoUsuario = dbContext.Usuarios.Add(_novoUsuario);
                dbContext.SaveChanges();

                return TypedResults.Created<UsuarioDtoOutput>($"/usuarios/{novoUsuario.Entity.Id}", novoUsuario.Entity.GetUsuarioDtoOutput());
            }).Produces<UsuarioDtoOutput>();

            // PUT      /usuarios/{Id}
            rotaUsuarios.MapPut("/{Id}", (MeusFilmesDbContext dbContext, int Id, UsuarioDtoInput usuario) =>
            {
                // Encontra o usuário especificado buscando pelo Id enviado
                Usuario? usuarioEncontrado = dbContext.Usuarios.Find(Id);
                if (usuarioEncontrado is null)
                {
                    // Indica que o usuário não foi encontrado
                    return Results.NotFound();
                }

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
