namespace Aula16APIFilmes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria��o da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Constru��o da WebApplication
            var app = builder.Build();

            #region Endpoints
            
            // GET /
            app.MapGet("/", () => "Hello World!");
            #endregion

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
