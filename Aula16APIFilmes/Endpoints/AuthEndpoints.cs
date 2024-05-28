using Aula16APIFilmes.Database;
using Aula16APIFilmes.Models;
using Aula16APIFilmes.Utils;

namespace Aula16APIFilmes.Endpoints
{
    public static class AuthEndpoints
    {
        class SignInParameters
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }

        public static void RegistrarEndpointsAutenticacao(this IEndpointRouteBuilder rotas)
        {
            RouteGroupBuilder rotasAuth = rotas.MapGroup("/auth");

            rotasAuth.MapPost("/signIn", (MeusFilmesDbContext dbContext, ITokenService tokenService, SignInParameters usuario) => {
                Usuario? usuarioEncontrado = dbContext.Usuarios.FirstOrDefault(u => u.Email == usuario.Email && u.Senha == usuario.Senha);

                if (usuarioEncontrado == null)
                {
                    return Results.NotFound();
                }

                string token = tokenService.CreateToken(usuarioEncontrado);

                return Results.Ok(new {token});
            });
        }
    }
}
