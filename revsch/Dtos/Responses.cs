using Microsoft.AspNetCore.SignalR;

public interface IResponse
{
    public string Mensagem {get; set;} 
    
    public bool Sucesso {get; set;} 
    
    public DateTime Timestamp {get; set;}

}

public class Response<T> : IResponse
{
    public string Mensagem {get; set;} = string.Empty;
    public bool Sucesso {get; set;} 
    public DateTime Timestamp {get; set;} = DateTime.Now; 

    public T? Dados {get; set;}
}