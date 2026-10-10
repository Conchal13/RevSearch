using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Bem vindo ao RevSch API!");
app.MapGet("/lojas", async ([FromServices] AppDbContext context) =>
{
    var lojas = await context.Lojas.ToListAsync();
    if (lojas.Count == 0)
    {
        return Results.Ok(
         new Response<Loja>
         {
             Mensagem = "Nenhuma loja cadastrada",
             Sucesso = true
         }
        );
    }
    return Results.Ok(
        new Response<List<Loja>>
        {
            Mensagem = "Lojas listadas com sucesso",
            Sucesso = true,
            Dados = lojas
        }
        
       );

});

app.Run();
