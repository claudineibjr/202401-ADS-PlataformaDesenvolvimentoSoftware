using Aula16APIFilmes.Models;

namespace Aula16APIFilmes.Utils
{
    public static class FilmeUtils
    {
        public static bool UsuarioPodeAssistirFilme(Filme filme, int idadeUsuario)
        {
            if (filme.ClassificacaoIndicativa == "L")
                return true;

            int classificacaoIndicativa;

            // Se não tiver classificação cadastrada ou não for possível converter para número, não exibe o filme
            if (String.IsNullOrEmpty(filme.ClassificacaoIndicativa) || !Int32.TryParse(filme.ClassificacaoIndicativa, out classificacaoIndicativa))
                return false;

            return idadeUsuario >= classificacaoIndicativa;
        }
    }
}
