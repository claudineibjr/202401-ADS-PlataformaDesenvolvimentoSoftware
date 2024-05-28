namespace Aula16APIFilmes.Utils
{
    public static class UsuarioUtils
    {
        public static int CalcularIdade(DateTime dataNascimento)
        {
            int idade = DateTime.Now.Year - dataNascimento.Year;
            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
            {
                idade--;
            }

            return idade;
        }
    }
}
