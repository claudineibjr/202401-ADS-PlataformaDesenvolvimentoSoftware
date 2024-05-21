using Aula16APIFilmes.DTOs;

namespace Aula16APIFilmes.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        public UsuarioDtoOutput GetUsuarioDtoOutput()
        {
            return new UsuarioDtoOutput(Id, Nome, Email);
        }
    }
}
