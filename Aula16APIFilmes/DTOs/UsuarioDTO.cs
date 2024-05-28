using Aula16APIFilmes.Models;
using Microsoft.OpenApi.Extensions;

namespace Aula16APIFilmes.DTOs
{
    public class UsuarioDtoInput
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }

        public Usuario ToUsuario()
        {
            PerfilUsuarioEnum perfil;
            Enum.TryParse(Perfil, out perfil);
            return new Usuario(Nome, Email, Senha, perfil);
        }
    }

    public class UsuarioDtoOutput
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }

        public UsuarioDtoOutput(int id, string nome, string email, PerfilUsuarioEnum perfil)
        {
            this.Id = id;
            this.Nome = nome;
            this.Email = email;
            this.Perfil = perfil.ToString();
        }
    }
}
