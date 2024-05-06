namespace Aula16APIFilmes
{
    class Filme
    {
        public string Titulo { get; set; }
        public int AnoLancamento { get; set; }
        public double NotaIMDB { get; set; }

        public Filme(string titulo, int anoLancamento, double notaIMDB)
        {
            Titulo = titulo;
            AnoLancamento = anoLancamento;
            NotaIMDB = notaIMDB;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Cria��o da WebApplication
            var builder = WebApplication.CreateBuilder(args);

            // Configura��o do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Constru��o da WebApplication
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Inicializa��o do Swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Endpoints
            #endregion

            // Execu��o da aplica��o
            app.Run();
        }
    }
}
