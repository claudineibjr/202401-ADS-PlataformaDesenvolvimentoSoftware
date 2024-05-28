using Aula16APIFilmes.DTOs;

namespace Aula16APIFilmes.Models
{
    public enum PerfilUsuarioEnum
    {
        Usuario,
        Administrador
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public PerfilUsuarioEnum Perfil { get; set; }

        public Usuario(string nome, string email, string senha, PerfilUsuarioEnum perfil)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Perfil = perfil;
        }

        public UsuarioDtoOutput GetUsuarioDtoOutput()
        {
            return new UsuarioDtoOutput(Id, Nome, Email, Perfil);
        }
    }
}
