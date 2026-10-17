using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Bem vindo ao RevSch API!");

//Endpoints de Loja
app.MapGet("/lojas", async ([FromServices] AppDbContext context) =>
{
    try
    {
        var lojas = await context.Lojas.ToListAsync();
        if (lojas.Count == 0)
        {
            return Results.Ok(
             new Response<List<Loja>>
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
app.MapGet("/loja/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
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
                Mensagem = "Houve um erro ao cadastrar a loja",
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
app.MapDelete("/loja/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
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

//Endpoints de Categoria
app.MapGet("/categorias", async ([FromServices] AppDbContext context) =>
{
    try
    {
        var categorias = await context.Categorias.ToListAsync();
        if (categorias.Count == 0)
        {
            return Results.Ok(
                new Response<List<Categoria>>
                {
                    Mensagem = "Nenhuma categoria cadastrada",
                    Sucesso = true
                }
            );
        }

        return Results.Ok(
            new Response<List<Categoria>>
            {
                Mensagem = "Categorias listadas com sucesso",
                Sucesso = true,
                Dados = categorias
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Categoria>>
            {
                Mensagem = "Houve um erro ao listar as categorias",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapGet("/categoria/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var categoria = await context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return Results.NotFound(
                new Response<Categoria>
                {
                    Mensagem = "Categoria não encontrada",
                    Sucesso = false
                }
            );
        }

        return Results.Ok(
            new Response<Categoria>
            {
                Mensagem = "Categoria encontrada com sucesso",
                Sucesso = true,
                Dados = categoria
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Categoria>
            {
                Mensagem = "Houve um erro ao buscar a categoria",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapPost("/categoria", async ([FromServices] AppDbContext context, [FromBody] Categoria request) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
                new Response<Categoria>
                {
                    Mensagem = "Nome da categoria é obrigatório",
                    Sucesso = false
                }
            );
        }

        var categoria = await context.Categorias.AddAsync(request);
        await context.SaveChangesAsync();

        return Results.Created(
            "",
            new Response<Categoria>
            {
                Mensagem = "Categoria criada com sucesso",
                Sucesso = true,
                Dados = categoria.Entity
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Categoria>
            {
                Mensagem = "Houve um erro ao cadastrar a categoria",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapPut("/categoria/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] Categoria request) =>
{
    try
    {
        var categoria = await context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return Results.NotFound(
                new Response<Categoria>
                {
                    Mensagem = "Categoria não encontrada",
                    Sucesso = false
                }
            );
        }

        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
                new Response<Categoria>
                {
                    Mensagem = "Nome da categoria é obrigatório",
                    Sucesso = false
                }
            );
        }

        categoria.Nome = request.Nome;
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Categoria>
            {
                Mensagem = "Categoria atualizada com sucesso",
                Sucesso = true,
                Dados = categoria
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Categoria>
            {
                Mensagem = "Houve um erro ao atualizar a categoria",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapDelete("/categoria/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var categoria = await context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return Results.NotFound(
                new Response<Categoria>
                {
                    Mensagem = "Categoria não encontrada",
                    Sucesso = false
                }
            );
        }

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Categoria>
            {
                Mensagem = "Categoria removida com sucesso",
                Sucesso = true,
                Dados = categoria
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Categoria>
            {
                Mensagem = "Houve um erro ao remover a categoria",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});



app.Run();
