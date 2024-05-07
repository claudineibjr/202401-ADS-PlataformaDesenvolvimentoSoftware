using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aula16APIFilmes.Models
{
    public class Filme
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AnoLancamento { get; set; }
        public double NotaIMDB { get; set; }

        private Filme() { }

        public Filme(string titulo, int anoLancamento, double notaIMDB)
        {
            Titulo = titulo;
            AnoLancamento = anoLancamento;
            NotaIMDB = notaIMDB;
        }
    }
}
