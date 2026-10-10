public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Segmento> Segmentos { get; set; } = new List<Segmento>();
}
