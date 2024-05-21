using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.DTOs
{
    public class UsuarioDtoInput
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public Usuario ToUsuario()
        {
            return new Usuario(Nome, Email, Senha);
        }
    }

    public class UsuarioDtoOutput
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public UsuarioDtoOutput(int id, string nome, string email)
        {
            this.Id = id;
            this.Nome = nome;
            this.Email = email;
        }
    }
}
