using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Bem vindo ao RevSch API!");
app.MapGet("/lojas", async ([FromServices] AppDbContext context) =>
{
    try
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
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Loja>>
            {
                Mensagem = "Houve um erro ao listar as lojas",
                Sucesso = false,
            },
            statusCode: 500
        );

    }

});

app.MapGet("/loja/{id}", async ([FromServices] AppDbContext context, int id) =>
{
    try
    {
        var loja = await context.Lojas.FindAsync(id);
        if (loja == null)
        {
            return Results.NotFound(
                new Response<Loja>
                {
                    Mensagem = "Loja não encontrada",
                    Sucesso = false
                }
            );
        }

        return Results.Ok(
            new Response<Loja>
            {
                Mensagem = "Loja encontrada com sucesso",
                Sucesso = true,
                Dados = loja
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Loja>
            {
                Mensagem = "Houve um erro ao buscar a loja",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});

app.MapPost("/loja", async ([FromServices] AppDbContext context, [FromBody] Loja request) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.Nome))
        {
        return Results.BadRequest(
           new Response<Loja>
           {
               Mensagem = "Nome da loja é Obrigatório",
               Sucesso = false
           }
          );

        }

        if (string.IsNullOrEmpty(request.URL))
        {
        return Results.BadRequest(
           new Response<Loja>
           {
               Mensagem = "URL da loja é Obrigatório",
               Sucesso = false
           }
          );

        }


        var loja = await context.Lojas.AddAsync(request);
        await context.SaveChangesAsync();
       
        return Results.Created(
            "",
            new Response<Loja>
            {
                Mensagem = "Loja criada com sucesso",
                Sucesso = true,
                Dados = loja.Entity
            }
           );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Loja>>
            {
                Mensagem = "Houve um erro ao listar as lojas",
                Sucesso = false,
            },
            statusCode: 500
        );

    }

});

app.MapPut("/loja/{id}", async ([FromServices] AppDbContext context, int id, [FromBody] Loja request) =>
{
    try
    {
        var loja = await context.Lojas.FindAsync(id);
        if (loja == null)
        {
            return Results.NotFound(
                new Response<Loja>
                {
                    Mensagem = "Loja não encontrada",
                    Sucesso = false
                }
            );
        }

        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
                new Response<Loja>
                {
                    Mensagem = "Nome da loja é obrigatório",
                    Sucesso = false
                }
            );
        }

        if (string.IsNullOrEmpty(request.URL))
        {
            return Results.BadRequest(
                new Response<Loja>
                {
                    Mensagem = "URL da loja é obrigatória",
                    Sucesso = false
                }
            );
        }

        loja.Nome = request.Nome;
        loja.URL = request.URL;
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Loja>
            {
                Mensagem = "Loja atualizada com sucesso",
                Sucesso = true,
                Dados = loja
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Loja>
            {
                Mensagem = "Houve um erro ao atualizar a loja",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});

app.MapDelete("/loja/{id}", async ([FromServices] AppDbContext context, int id) =>
{
    try
    {
        var loja = await context.Lojas.FindAsync(id);
        if (loja == null)
        {
            return Results.NotFound(
                new Response<Loja>
                {
                    Mensagem = "Loja não encontrada",
                    Sucesso = false
                }
            );
        }

        context.Lojas.Remove(loja);
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Loja>
            {
                Mensagem = "Loja removida com sucesso",
                Sucesso = true,
                Dados = loja
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Loja>
            {
                Mensagem = "Houve um erro ao remover a loja",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});



app.Run();
