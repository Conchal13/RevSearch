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
    

}

