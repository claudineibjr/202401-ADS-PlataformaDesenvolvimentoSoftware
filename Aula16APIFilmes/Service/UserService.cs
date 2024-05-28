using Aula16APIFilmes.Database;
using Aula16APIFilmes.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Aula16APIFilmes.Service
{
    public static class UserService
    {
        public static Usuario? GetUsuarioPorUsuarioLogado(MeusFilmesDbContext dbContext, ClaimsPrincipal usuarioLogado)
        {
            var usuarioLogadoId = usuarioLogado.Claims.FirstOrDefault(c => c.Type == "id");
            if (usuarioLogadoId is null)
            {
                return null;
            }

            Usuario? usuarioEncontrado = dbContext.Usuarios.FirstOrDefault(u => u.Id.ToString() == usuarioLogadoId.Value.ToString());
            return usuarioEncontrado;
        }
    }
}
