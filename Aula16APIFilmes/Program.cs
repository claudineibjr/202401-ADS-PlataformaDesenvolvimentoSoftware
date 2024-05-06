namespace Aula16APIFilmes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Criação da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Construção da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicialização do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Endpoints
            
            // GET /
            app.MapGet("/", () => "Hello World!");
            #endregion

            // Execução da aplicação
            app.Run();
        }
    }
}
