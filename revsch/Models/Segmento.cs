using System.Text.Json.Serialization;

public class Segmento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    [JsonIgnore]
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    
}
