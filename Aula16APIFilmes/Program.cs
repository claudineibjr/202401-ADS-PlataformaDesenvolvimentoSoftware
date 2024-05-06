namespace Aula16APIFilmes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Criação da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Construção da WebApplication
            var app = builder.Build();

            #region Endpoints
            
            // GET /
            app.MapGet("/", () => "Hello World!");
            #endregion

            // Execução da aplicação
            app.Run();
        }
    }
}
