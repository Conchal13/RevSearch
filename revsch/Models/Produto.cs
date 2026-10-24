using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string UrlProduto { get; set; } = string.Empty;

    public int LojaId { get; set; }
    public Loja? Loja { get; set; }

    public int SegmentoId { get; set; }
    public Segmento? Segmento { get; set; }

    public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
    
    [NotMapped]
    public float MediaNotas => Avaliacoes.Count == 0 ? 0F : float.Round((float)Avaliacoes.Average(a => a.Nota), 2);

}

