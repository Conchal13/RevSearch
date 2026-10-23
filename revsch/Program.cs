using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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
        var categorias = await context.Categorias.Include(c => c.Segmentos).ToListAsync();
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
        var categoria = await context.Categorias.Include(c => c.Segmentos).FirstOrDefaultAsync(c => c.Id == id);
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

//Endpoints de Segmento
app.MapGet("/segmentos", async ([FromServices] AppDbContext context) =>
{
    try
    {
        var segmentos = await context.Segmentos.Include(s => s.Categoria).ToListAsync();
        if (segmentos.Count == 0)
        {
            return Results.Ok(
                new Response<List<Segmento>>
                {
                    Mensagem = "Nenhum segmento cadastrado",
                    Sucesso = true
                }
            );
        }

        return Results.Ok(
            new Response<List<Segmento>>
            {
                Mensagem = "Segmentos listados com sucesso",
                Sucesso = true,
                Dados = segmentos
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Segmento>>
            {
                Mensagem = "Houve um erro ao listar os segmentos",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapGet("/segmento/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var segmento = await context.Segmentos.Include(s => s.Categoria).FirstOrDefaultAsync(s => s.Id == id);
        if (segmento == null)
        {
            return Results.NotFound(
                new Response<Segmento>
                {
                    Mensagem = "Segmento não encontrado",
                    Sucesso = false
                }
            );
        }

        return Results.Ok(
            new Response<Segmento>
            {
                Mensagem = "Segmento encontrado com sucesso",
                Sucesso = true,
                Dados = segmento
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Segmento>
            {
                Mensagem = "Houve um erro ao buscar o segmento",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapPost("/segmento", async ([FromServices] AppDbContext context, [FromBody] Segmento request) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
                new Response<Segmento>
                {
                    Mensagem = "Nome do segmento é obrigatório",
                    Sucesso = false
                }
            );
        }

        var categoria = await context.Categorias.FindAsync(request.CategoriaId);
        if (categoria == null)
        {
            return Results.BadRequest(
                new Response<Segmento>
                {
                    Mensagem = "Categoria não encontrada para o id: " + request.CategoriaId,
                    Sucesso = false
                }
            );
        }

        var segmento = new Segmento
        {
            Nome = request.Nome,
            CategoriaId = request.CategoriaId
        };
        var novoSegmento = await context.Segmentos.AddAsync(segmento);
        await context.SaveChangesAsync();

        return Results.Created(
            "",
            new Response<Segmento>
            {
                Mensagem = "Segmento criado com sucesso",
                Sucesso = true,
                Dados = novoSegmento.Entity
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Segmento>
            {
                Mensagem = "Houve um erro ao cadastrar o segmento",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapPut("/segmento/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] Segmento request) =>
{
    try
    {
        var segmento = await context.Segmentos.FindAsync(id);
        if (segmento == null)
        {
            return Results.NotFound(
                new Response<Segmento>
                {
                    Mensagem = "Segmento não encontrado",
                    Sucesso = false
                }
            );
        }

        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
                new Response<Segmento>
                {
                    Mensagem = "Nome do segmento é obrigatório",
                    Sucesso = false
                }
            );
        }

        var categoria = await context.Categorias.FindAsync(request.CategoriaId);
        if (categoria == null)
        {
            return Results.BadRequest(
                new Response<Segmento>
                {
                    Mensagem = "Categoria não encontrada para o id: " + request.CategoriaId,
                    Sucesso = false
                }
            );
        }

        segmento.Nome = request.Nome;
        segmento.CategoriaId = request.CategoriaId;
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Segmento>
            {
                Mensagem = "Segmento atualizado com sucesso",
                Sucesso = true,
                Dados = segmento
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Segmento>
            {
                Mensagem = "Houve um erro ao atualizar o segmento",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});
app.MapDelete("/segmento/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var segmento = await context.Segmentos.FindAsync(id);
        if (segmento == null)
        {
            return Results.NotFound(
                new Response<Segmento>
                {
                    Mensagem = "Segmento não encontrado",
                    Sucesso = false
                }
            );
        }

        context.Segmentos.Remove(segmento);
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Segmento>
            {
                Mensagem = "Segmento removido com sucesso",
                Sucesso = true,
                Dados = segmento
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Segmento>
            {
                Mensagem = "Houve um erro ao remover o segmento",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});

//Endpoints de Produto
app.MapGet("/produtos", async ([FromServices] AppDbContext context) =>
{
    try
    {
        var produtos = await context.Produtos.Include(p => p.Segmento).Include(p => p.Loja).ToListAsync();
        if (produtos.Count == 0)
        {
            return Results.Ok(new Response<List<Produto>>
            {
                Mensagem = "Nenhum produto cadastrado",
                Sucesso = true
            });
        }

        return Results.Ok(new Response<List<Produto>>
        {
            Dados = produtos,
            Mensagem = "Produtos listados com sucesso",
            Sucesso = true
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Produto>
        {
            Mensagem = "Houve um erro ao listar os produtos",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapGet("/produtos/loja/{idLoja}", async ([FromServices] AppDbContext context, [FromRoute] int idLoja) =>
{
    try
    {
        var produtos = await context.Produtos.Where(p => p.LojaId == idLoja).Include(p => p.Segmento).Include(p => p.Loja).ToListAsync();
        if (produtos.Count == 0)
        {
            return Results.Ok(new Response<List<Produto>>
            {
                Mensagem = "Nenhum produto encontrado",
                Sucesso = true
            });
        }

        return Results.Ok(new Response<List<Produto>>
        {
            Dados = produtos,
            Mensagem = "Produtos listados com sucesso",
            Sucesso = true
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Produto>
        {
            Mensagem = "Houve um erro ao listar os produtos",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapGet("/produtos/segmento/{idSegmento}", async ([FromServices] AppDbContext context, [FromRoute] int idSegmento) =>
{
    try
    {
        var produtos = await context.Produtos.Where(p => p.SegmentoId == idSegmento).Include(p => p.Segmento).Include(p => p.Loja).ToListAsync();
        if (produtos.Count == 0)
        {
            return Results.Ok(new Response<List<Produto>>
            {
                Mensagem = "Nenhum produto encontrado",
                Sucesso = true
            });
        }

        return Results.Ok(new Response<List<Produto>>
        {
            Dados = produtos,
            Mensagem = "Produtos listados com sucesso",
            Sucesso = true
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Produto>
        {
            Mensagem = "Houve um erro ao listar os produtos",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapGet("/produto/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var produto = await context.Produtos.Include(p => p.Segmento).Include(p => p.Loja).FirstOrDefaultAsync(p => p.Id == id);
        if (produto == null)
        {
            return Results.NotFound(
                new Response<Produto>
                {
                    Mensagem = "Produto não encontrado",
                    Sucesso = false
                }
            );
        }

        return Results.Ok(
            new Response<Produto>
            {
                Mensagem = "Produto encontrado com sucesso",
                Sucesso = true,
                Dados = produto
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Produto>
            {
                Mensagem = "Houve um erro ao buscar o Produto",
                Sucesso = false
            },
            statusCode: 500
        );
    }

});
app.MapGet("/produtos/categoria/{idCategoria}", async ([FromServices] AppDbContext context, [FromRoute] int idCategoria) =>
{
     try
    {
        var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == idCategoria);
        if (categoria == null)
        {
             return Results.BadRequest(
                new Response<List<Produto>>
                {
                    Mensagem = "Categoria não encontrada para o id: " + idCategoria,
                    Sucesso = false
                }
                );
        }

        var produtos = await context.Produtos.Include(p => p.Segmento).Where(p => p.Segmento!.CategoriaId == idCategoria).ToListAsync();

         if (produtos.Count == 0)
        {
            return Results.Ok(new Response<List<Produto>>
            {
                Mensagem = "Nenhum produto encontrado",
                Sucesso = true
            });
        }
        
        return Results.Ok(
            new Response<List<Produto>>
            {
                Mensagem = "Produtos listados com sucesso",
                Sucesso = true,
                Dados = produtos
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Produto>>
            {
                Mensagem = "Houve um erro ao listar os Produtos",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});

app.MapGet("/produtos/filtrar", async ([FromServices] AppDbContext context, [FromQuery]int? idCategoria, [FromQuery]int? idLoja, [FromQuery]int? idSegmento, [FromQuery] string? nomeProduto) =>
{
     try
    {

        
        var produtos = await context.Produtos
        .Include(p => p.Avaliacoes)
        .Where(p => 
            (idCategoria == null || p.Segmento!.CategoriaId == idCategoria) &&
            (idSegmento == null || p.SegmentoId == idSegmento) &&
            (idLoja == null || p.LojaId == idLoja) &&
            (nomeProduto == null || p.Nome.Contains(nomeProduto))
        )
        .ToListAsync();

         if (produtos.Count == 0)
        {
            return Results.Ok(new Response<List<Produto>>
            {
                Mensagem = "Nenhum produto encontrado",
                Sucesso = true
            });
        }
        
        return Results.Ok(
            new Response<List<Produto>>
            {
                Mensagem = "Produtos listados com sucesso",
                Sucesso = true,
                Dados = produtos
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Produto>>
            {
                Mensagem = "Houve um erro ao listar os Produtos",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});


app.MapPost("/produto", async ([FromServices] AppDbContext context, [FromBody] Produto request) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
               new Response<Produto>
               {
                   Mensagem = "Nome do produto é Obrigatório",
                   Sucesso = false
               }
            );

        }

        if (string.IsNullOrEmpty(request.UrlProduto))
        {
            return Results.BadRequest(
               new Response<Produto>
               {
                   Mensagem = "URL do produto é Obrigatório",
                   Sucesso = false
               }
            );

        }

        var loja = await context.Lojas.FindAsync(request.LojaId);
        if (loja == null)
        {
            return Results.BadRequest(
                new Response<Produto>
                {
                    Mensagem = "Loja não encontrada para o id: " + request.LojaId,
                    Sucesso = false
                }
                );
        }

        var segmento = await context.Segmentos.FindAsync(request.SegmentoId);
        if (segmento == null)
        {
            return Results.BadRequest(
                new Response<Produto>
                {
                    Mensagem = "Segmento não encontrada para o id: " + request.SegmentoId,
                    Sucesso = false
                }
                );
        }

        var produtos = await context.Produtos.AddAsync(request);
        await context.SaveChangesAsync();

        return Results.Created(
            "",
            new Response<Produto>
            {
                Mensagem = "Produto criado com sucesso",
                Sucesso = true,
                Dados = produtos.Entity
            }
           );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Produto>>
            {
                Mensagem = "Houve um erro ao cadastrar o produto",
                Sucesso = false,
            },
            statusCode: 500
        );

    }
});
app.MapPut("/produto/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] Produto request) =>
{
    try
    {
        var produto = await context.Produtos.FindAsync(id);

        if (produto == null)
        {
            return Results.NotFound(
                new Response<Produto>
                {
                    Mensagem = "Produto não encontrado",
                    Sucesso = false
                }
            );
        }

        if (string.IsNullOrEmpty(request.Nome))
        {
            return Results.BadRequest(
               new Response<Produto>
               {
                   Mensagem = "Nome do produto é Obrigatório",
                   Sucesso = false
               }
            );

        }

        if (string.IsNullOrEmpty(request.UrlProduto))
        {
            return Results.BadRequest(
               new Response<Produto>
               {
                   Mensagem = "URL do produto é Obrigatório",
                   Sucesso = false
               }
            );

        }

        var loja = await context.Lojas.FindAsync(request.LojaId);
        if (loja == null)
        {
            return Results.BadRequest(
                new Response<Produto>
                {
                    Mensagem = "Loja não encontrada para o id: " + request.LojaId,
                    Sucesso = false
                }
            );
        }

        var segmento = await context.Segmentos.FindAsync(request.SegmentoId);
        if (segmento == null)
        {
            return Results.BadRequest(
                new Response<Produto>
                {
                    Mensagem = "Segmento não encontrada para o id: " + request.SegmentoId,
                    Sucesso = false
                }
            );
        }

        produto.Nome = request.Nome;
        produto.UrlProduto = request.UrlProduto;
        produto.LojaId = request.LojaId;
        produto.SegmentoId = request.SegmentoId;
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Produto>
            {
                Mensagem = "Produto editado com sucesso",
                Sucesso = true,
                Dados = produto
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<List<Produto>>
            {
                Mensagem = "Houve um erro ao editar o produto",
                Sucesso = false,
            },
            statusCode: 500
        );

    }
});
app.MapDelete("/produto/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var produto = await context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return Results.NotFound(
                new Response<Produto>
                {
                    Mensagem = "Produto não encontrado",
                    Sucesso = false
                }
            );
        }

        context.Produtos.Remove(produto);
        await context.SaveChangesAsync();

        return Results.Ok(
            new Response<Produto>
            {
                Mensagem = "Produto removido com sucesso",
                Sucesso = true,
                Dados = produto
            }
        );
    }
    catch (Exception)
    {
        return Results.Json(
            new Response<Produto>
            {
                Mensagem = "Houve um erro ao buscar o Produto",
                Sucesso = false
            },
            statusCode: 500
        );
    }
});

//Endpoints de Avaliacao
app.MapGet("/avaliacoes", async ([FromServices] AppDbContext context) =>
{
    try
    {
        var avaliacoes = await context.Avaliacoes.ToListAsync();
        if (avaliacoes.Count == 0)
        {
            return Results.Ok(new Response<List<Avaliacao>>
            {
                Mensagem = "Nenhuma avaliação cadastrada",
                Sucesso = true
            });
        }

        return Results.Ok(new Response<List<Avaliacao>>
        {
            Dados = avaliacoes,
            Mensagem = "Avaliações listadas com sucesso",
            Sucesso = true
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<List<Avaliacao>>
        {
            Mensagem = "Houve um erro ao listar as avaliações",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapGet("/avaliacoes/produto/{idProduto}", async ([FromServices] AppDbContext context, [FromRoute] int idProduto) =>
{
    try
    {
        var avaliacoes = await context.Avaliacoes
            .Where(a => a.ProdutoId == idProduto)
            .ToListAsync();

        if (avaliacoes.Count == 0)
        {
            return Results.Ok(new Response<List<Avaliacao>>
            {
                Mensagem = "Nenhuma avaliação encontrada para o produto",
                Sucesso = true
            });
        }

        return Results.Ok(new Response<List<Avaliacao>>
        {
            Dados = avaliacoes,
            Mensagem = "Avaliações do produto listadas com sucesso",
            Sucesso = true
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<List<Avaliacao>>
        {
            Mensagem = "Houve um erro ao listar as avaliações do produto",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapGet("/avaliacao/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var avaliacao = await context.Avaliacoes.FindAsync(id);
        if (avaliacao == null)
        {
            return Results.NotFound(new Response<Avaliacao>
            {
                Mensagem = "Avaliação não encontrada",
                Sucesso = false
            });
        }

        return Results.Ok(new Response<Avaliacao>
        {
            Mensagem = "Avaliação encontrada com sucesso",
            Sucesso = true,
            Dados = avaliacao
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Avaliacao>
        {
            Mensagem = "Houve um erro ao buscar a avaliação",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapPost("/avaliacao", async ([FromServices] AppDbContext context, [FromBody] Avaliacao request) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.NomeUsuario))
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Nome do usuário é obrigatório",
                Sucesso = false
            });
        }

        if (string.IsNullOrEmpty(request.Comentario))
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Comentário é obrigatório",
                Sucesso = false
            });
        }

        var produto = await context.Produtos.FindAsync(request.ProdutoId);
        if (produto == null)
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Produto não encontrado para o id: " + request.ProdutoId,
                Sucesso = false
            });
        }

        if (request.Nota < 0 || request.Nota > 5)
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "A nota do produto deve estar entre 0 e 5 ",
                Sucesso = false
            });
        }

        var avaliacao = new Avaliacao
        {
            NomeUsuario = request.NomeUsuario,
            Nota = request.Nota,
            UrlImagem = request.UrlImagem,
            Comentario = request.Comentario,
            DataAvaliacao = request.DataAvaliacao,
            ProdutoId = request.ProdutoId
        };

        var novaAvaliacao = await context.Avaliacoes.AddAsync(avaliacao);
        await context.SaveChangesAsync();

        return Results.Created("", new Response<Avaliacao>
        {
            Mensagem = "Avaliação criada com sucesso",
            Sucesso = true,
            Dados = novaAvaliacao.Entity
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Avaliacao>
        {
            Mensagem = "Houve um erro ao cadastrar a avaliação",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapPut("/avaliacao/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id, [FromBody] Avaliacao request) =>
{
    try
    {
        var avaliacao = await context.Avaliacoes.FindAsync(id);
        if (avaliacao == null)
        {
            return Results.NotFound(new Response<Avaliacao>
            {
                Mensagem = "Avaliação não encontrada",
                Sucesso = false
            });
        }

        if (string.IsNullOrEmpty(request.NomeUsuario))
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Nome do usuário é obrigatório",
                Sucesso = false
            });
        }

        if (string.IsNullOrEmpty(request.Comentario))
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Comentário é obrigatório",
                Sucesso = false
            });
        }

        var produto = await context.Produtos.FindAsync(request.ProdutoId);
        if (produto == null)
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "Produto não encontrado para o id: " + request.ProdutoId,
                Sucesso = false
            });
        }

        if (request.Nota < 0 || request.Nota > 5)
        {
            return Results.BadRequest(new Response<Avaliacao>
            {
                Mensagem = "A nota do produto deve estar entre 0 e 5 ",
                Sucesso = false
            });
        }

        avaliacao.NomeUsuario = request.NomeUsuario;
        avaliacao.Nota = request.Nota;
        avaliacao.UrlImagem = request.UrlImagem;
        avaliacao.Comentario = request.Comentario;
        avaliacao.DataAvaliacao = request.DataAvaliacao;
        avaliacao.ProdutoId = request.ProdutoId;
        await context.SaveChangesAsync();

        return Results.Ok(new Response<Avaliacao>
        {
            Mensagem = "Avaliação atualizada com sucesso",
            Sucesso = true,
            Dados = avaliacao
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Avaliacao>
        {
            Mensagem = "Houve um erro ao atualizar a avaliação",
            Sucesso = false
        }, statusCode: 500);
    }
});
app.MapDelete("/avaliacao/{id}", async ([FromServices] AppDbContext context, [FromRoute] int id) =>
{
    try
    {
        var avaliacao = await context.Avaliacoes.FindAsync(id);
        if (avaliacao == null)
        {
            return Results.NotFound(new Response<Avaliacao>
            {
                Mensagem = "Avaliação não encontrada",
                Sucesso = false
            });
        }

        context.Avaliacoes.Remove(avaliacao);
        await context.SaveChangesAsync();

        return Results.Ok(new Response<Avaliacao>
        {
            Mensagem = "Avaliação removida com sucesso",
            Sucesso = true,
            Dados = avaliacao
        });
    }
    catch (Exception)
    {
        return Results.Json(new Response<Avaliacao>
        {
            Mensagem = "Houve um erro ao remover a avaliação",
            Sucesso = false
        }, statusCode: 500);
    }


});



app.Run();
