public class Avaliacao
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string? UrlImagem { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime DataAvaliacao { get; set; } = DateTime.Now;
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }
}
