-- Dados de exemplo para o schema criado pelas migrations do EF Core.
-- O script pode ser executado mais de uma vez sem duplicar estes registros.
PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

WITH dados(Nome) AS (
    VALUES
        ('Categoria Eletronicos'),
        ('Categoria Casa'),
        ('Categoria Esportes'),
        ('Categoria Livros'),
        ('Categoria Beleza')
)
INSERT INTO Categorias (Nome)
SELECT dados.Nome
FROM dados
WHERE NOT EXISTS (
    SELECT 1 FROM Categorias existente WHERE existente.Nome = dados.Nome
);

WITH dados(Nome, URL) AS (
    VALUES
        ('Loja TechMundo', 'https://exemplo.local/techmundo'),
        ('Loja CasaPratica', 'https://exemplo.local/casapratica'),
        ('Loja Ativa', 'https://exemplo.local/ativa'),
        ('Loja Livraria Horizonte', 'https://exemplo.local/horizonte'),
        ('Loja BelaEssencia', 'https://exemplo.local/belaessencia')
)
INSERT INTO Lojas (Nome, URL)
SELECT dados.Nome, dados.URL
FROM dados
WHERE NOT EXISTS (
    SELECT 1 FROM Lojas existente WHERE existente.Nome = dados.Nome
);

WITH dados(Nome, CategoriaNome) AS (
    VALUES
        ('Celulares', 'Categoria Eletronicos'),
        ('Computadores', 'Categoria Eletronicos'),
        ('Audio', 'Categoria Eletronicos'),
        ('Cozinha', 'Categoria Casa'),
        ('Decoracao', 'Categoria Casa'),
        ('Organizacao', 'Categoria Casa'),
        ('Corrida', 'Categoria Esportes'),
        ('Fitness', 'Categoria Esportes'),
        ('Ciclismo', 'Categoria Esportes'),
        ('Ficcao', 'Categoria Livros'),
        ('Desenvolvimento', 'Categoria Livros'),
        ('Infantil', 'Categoria Livros'),
        ('Cuidados com a pele', 'Categoria Beleza'),
        ('Maquiagem', 'Categoria Beleza'),
        ('Cabelos', 'Categoria Beleza')
)
INSERT INTO Segmentos (Nome, CategoriaId)
SELECT dados.Nome, categoria.Id
FROM dados
JOIN Categorias categoria ON categoria.Nome = dados.CategoriaNome
WHERE NOT EXISTS (
    SELECT 1
    FROM Segmentos existente
    WHERE existente.Nome = dados.Nome
      AND existente.CategoriaId = categoria.Id
);

WITH dados(Nome, UrlProduto, LojaNome, SegmentoNome) AS (
    VALUES
        ('Smartphone Aurora', 'https://exemplo.local/produtos/smartphone-aurora', 'Loja TechMundo', 'Celulares'),
        ('Smartphone Boreal', 'https://exemplo.local/produtos/smartphone-boreal', 'Loja TechMundo', 'Celulares'),
        ('Notebook Atlas', 'https://exemplo.local/produtos/notebook-atlas', 'Loja TechMundo', 'Computadores'),
        ('Teclado Nebula', 'https://exemplo.local/produtos/teclado-nebula', 'Loja TechMundo', 'Computadores'),
        ('Fone Solaris', 'https://exemplo.local/produtos/fone-solaris', 'Loja TechMundo', 'Audio'),
        ('Caixa Sonora', 'https://exemplo.local/produtos/caixa-sonora', 'Loja TechMundo', 'Audio'),

        ('Cafeteira Brisa', 'https://exemplo.local/produtos/cafeteira-brisa', 'Loja CasaPratica', 'Cozinha'),
        ('Panela Horizonte', 'https://exemplo.local/produtos/panela-horizonte', 'Loja CasaPratica', 'Cozinha'),
        ('Luminaria Serena', 'https://exemplo.local/produtos/luminaria-serena', 'Loja CasaPratica', 'Decoracao'),
        ('Almofada Nuvem', 'https://exemplo.local/produtos/almofada-nuvem', 'Loja CasaPratica', 'Decoracao'),
        ('Organizador Prisma', 'https://exemplo.local/produtos/organizador-prisma', 'Loja CasaPratica', 'Organizacao'),
        ('Caixa Modular', 'https://exemplo.local/produtos/caixa-modular', 'Loja CasaPratica', 'Organizacao'),

        ('Tenis Veloz', 'https://exemplo.local/produtos/tenis-veloz', 'Loja Ativa', 'Corrida'),
        ('Meia Impulso', 'https://exemplo.local/produtos/meia-impulso', 'Loja Ativa', 'Corrida'),
        ('Halteres Forca', 'https://exemplo.local/produtos/halteres-forca', 'Loja Ativa', 'Fitness'),
        ('Colchonete Equilibrio', 'https://exemplo.local/produtos/colchonete-equilibrio', 'Loja Ativa', 'Fitness'),
        ('Capacete Ventus', 'https://exemplo.local/produtos/capacete-ventus', 'Loja Ativa', 'Ciclismo'),
        ('Luz Traseira Pulsar', 'https://exemplo.local/produtos/luz-traseira-pulsar', 'Loja Ativa', 'Ciclismo'),

        ('Livro Mar de Estrelas', 'https://exemplo.local/produtos/mar-de-estrelas', 'Loja Livraria Horizonte', 'Ficcao'),
        ('Livro Cidade de Areia', 'https://exemplo.local/produtos/cidade-de-areia', 'Loja Livraria Horizonte', 'Ficcao'),
        ('Guia Codigo Claro', 'https://exemplo.local/produtos/codigo-claro', 'Loja Livraria Horizonte', 'Desenvolvimento'),
        ('Guia Dados em Foco', 'https://exemplo.local/produtos/dados-em-foco', 'Loja Livraria Horizonte', 'Desenvolvimento'),
        ('Livro Pequeno Planeta', 'https://exemplo.local/produtos/pequeno-planeta', 'Loja Livraria Horizonte', 'Infantil'),
        ('Livro A Ilha Colorida', 'https://exemplo.local/produtos/ilha-colorida', 'Loja Livraria Horizonte', 'Infantil'),

        ('Hidratante Orvalho', 'https://exemplo.local/produtos/hidratante-orvalho', 'Loja BelaEssencia', 'Cuidados com a pele'),
        ('Protetor Solar Horizonte', 'https://exemplo.local/produtos/protetor-solar-horizonte', 'Loja BelaEssencia', 'Cuidados com a pele'),
        ('Batom Rubi', 'https://exemplo.local/produtos/batom-rubi', 'Loja BelaEssencia', 'Maquiagem'),
        ('Mascara de Cilios Prisma', 'https://exemplo.local/produtos/mascara-cilios-prisma', 'Loja BelaEssencia', 'Maquiagem'),
        ('Shampoo Raiz', 'https://exemplo.local/produtos/shampoo-raiz', 'Loja BelaEssencia', 'Cabelos'),
        ('Condicionador Seda', 'https://exemplo.local/produtos/condicionador-seda', 'Loja BelaEssencia', 'Cabelos')
)
INSERT INTO Produtos (Nome, UrlProduto, LojaId, SegmentoId)
SELECT dados.Nome, dados.UrlProduto, loja.Id, segmento.Id
FROM dados
JOIN Lojas loja ON loja.Nome = dados.LojaNome
JOIN Segmentos segmento ON segmento.Nome = dados.SegmentoNome
WHERE NOT EXISTS (
    SELECT 1
    FROM Produtos existente
    WHERE existente.Nome = dados.Nome
      AND existente.LojaId = loja.Id
);

WITH dados(ProdutoNome, NomeUsuario, Nota, UrlImagem, Comentario, DiasAtras) AS (
    VALUES
        ('Smartphone Aurora', 'Ana Lima', 5, NULL, 'Produto excelente, recomendo.', 1),
        ('Smartphone Boreal', 'Bruno Reis', 4, NULL, 'Boa qualidade e entrega rapida.', 3),
        ('Smartphone Boreal', 'Carla Souza', 5, 'https://exemplo.local/imagens/avaliacao-01.jpg', 'Atendeu minhas expectativas.', 2),
        ('Notebook Atlas', 'Diego Alves', 5, NULL, 'Muito rapido para o trabalho.', 5),
        ('Notebook Atlas', 'Eva Martins', 4, NULL, 'Bom custo-beneficio.', 4),
        ('Notebook Atlas', 'Felipe Rocha', 5, NULL, 'Recomendo a compra.', 2),
        ('Teclado Nebula', 'Gabi Costa', 4, NULL, 'Confortavel para digitar.', 7),
        ('Teclado Nebula', 'Hugo Nunes', 3, NULL, 'Bom, mas poderia ser mais silencioso.', 6),
        ('Teclado Nebula', 'Iris Melo', 5, NULL, 'Gostei bastante.', 3),
        ('Teclado Nebula', 'Joao Prado', 4, NULL, 'Funciona muito bem.', 2),
        ('Fone Solaris', 'Karen Dias', 5, NULL, 'Som limpo e confortavel.', 8),
        ('Fone Solaris', 'Leo Freitas', 4, NULL, 'Boa bateria.', 6),
        ('Fone Solaris', 'Mia Barros', 4, NULL, 'Cumpre o que promete.', 4),
        ('Fone Solaris', 'Nina Lopes', 5, NULL, 'Excelente pelo preco.', 3),
        ('Fone Solaris', 'Otavio Melo', 4, NULL, 'Gostei da qualidade.', 1),
        ('Caixa Sonora', 'Paula Luz', 5, NULL, 'Som potente para o tamanho.', 9),
        ('Caixa Sonora', 'Rui Tavares', 4, NULL, 'Pratica e facil de usar.', 7),
        ('Caixa Sonora', 'Sara Pires', 5, NULL, 'Muito boa.', 5),
        ('Caixa Sonora', 'Theo Campos', 4, NULL, 'Recomendo.', 4),
        ('Caixa Sonora', 'Uli Moreira', 3, NULL, 'Boa, mas esperava mais graves.', 2),

        ('Cafeteira Brisa', 'Vera Azevedo', 5, NULL, 'Cafe saboroso e preparo simples.', 2),
        ('Cafeteira Brisa', 'Will Cardoso', 4, NULL, 'Muito pratica.', 1),
        ('Panela Horizonte', 'Yasmin Teixeira', 5, NULL, 'Material resistente.', 3),
        ('Panela Horizonte', 'Zeca Ribeiro', 4, NULL, 'Boa distribuicao de calor.', 2),
        ('Panela Horizonte', 'Alice Ramos', 5, NULL, 'Facil de limpar.', 1),
        ('Luminaria Serena', 'Beto Farias', 4, NULL, 'Luz agradavel para leitura.', 5),
        ('Luminaria Serena', 'Ceci Duarte', 5, NULL, 'Ficou perfeita no quarto.', 4),
        ('Luminaria Serena', 'Duda Paiva', 4, NULL, 'Bom acabamento.', 3),
        ('Almofada Nuvem', 'Eli Monteiro', 5, NULL, 'Muito macia.', 6),
        ('Almofada Nuvem', 'Fabi Leal', 4, NULL, 'Boa qualidade.', 5),
        ('Almofada Nuvem', 'Gui Pacheco', 5, NULL, 'As cores sao bonitas.', 4),
        ('Almofada Nuvem', 'Helena Melo', 4, NULL, 'Gostei da compra.', 3),
        ('Organizador Prisma', 'Igor Neves', 5, NULL, 'Ajudou a organizar tudo.', 8),
        ('Organizador Prisma', 'Jade Brito', 4, NULL, 'Bom tamanho.', 6),
        ('Organizador Prisma', 'Kiko Maia', 5, NULL, 'Resistente e util.', 4),
        ('Organizador Prisma', 'Lara Couto', 4, NULL, 'Recomendo.', 3),
        ('Organizador Prisma', 'Marcos Gil', 5, NULL, 'Muito pratico.', 2),

        ('Tenis Veloz', 'Nara Reis', 5, NULL, 'Leve e confortavel para correr.', 2),
        ('Tenis Veloz', 'Omar Lima', 4, NULL, 'Bom amortecimento.', 1),
        ('Meia Impulso', 'Pati Melo', 4, NULL, 'Confortavel durante o treino.', 3),
        ('Meia Impulso', 'Quim Rocha', 5, NULL, 'Tecido muito bom.', 2),
        ('Meia Impulso', 'Rita Alves', 4, NULL, 'Boa compra.', 1),
        ('Halteres Forca', 'Sami Costa', 5, NULL, 'Otimo para treinar em casa.', 5),
        ('Halteres Forca', 'Tati Nunes', 4, NULL, 'Acabamento de qualidade.', 4),
        ('Halteres Forca', 'Ugo Prado', 5, NULL, 'Exatamente como anunciado.', 3),
        ('Colchonete Equilibrio', 'Vivi Dias', 4, NULL, 'Confortavel e facil de guardar.', 7),
        ('Colchonete Equilibrio', 'Wes Freitas', 5, NULL, 'Boa espessura.', 6),
        ('Colchonete Equilibrio', 'Xuxa Barros', 4, NULL, 'Atendeu bem.', 4),
        ('Colchonete Equilibrio', 'Yuri Lopes', 5, NULL, 'Recomendo.', 3),
        ('Capacete Ventus', 'Zoe Prado', 5, NULL, 'Leve e bem ajustavel.', 8),
        ('Capacete Ventus', 'Ari Tavares', 4, NULL, 'Boa ventilacao.', 6),
        ('Capacete Ventus', 'Bia Pires', 5, NULL, 'Me senti seguro.', 4),
        ('Capacete Ventus', 'Caio Luz', 4, NULL, 'Bom produto.', 3),
        ('Capacete Ventus', 'Dani Azevedo', 5, NULL, 'Otimo acabamento.', 2),

        ('Livro Mar de Estrelas', 'Enzo Cardoso', 5, NULL, 'Historia envolvente.', 2),
        ('Livro Cidade de Areia', 'Fefa Teixeira', 4, NULL, 'Leitura agradavel.', 3),
        ('Livro Cidade de Areia', 'Guto Ribeiro', 5, NULL, 'Gostei muito do final.', 2),
        ('Guia Codigo Claro', 'Hana Ramos', 5, NULL, 'Explicacoes bem claras.', 5),
        ('Guia Codigo Claro', 'Ivo Farias', 4, NULL, 'Otima referencia.', 4),
        ('Guia Codigo Claro', 'Juca Duarte', 5, NULL, 'Aprendi bastante.', 3),
        ('Guia Dados em Foco', 'Kika Paiva', 4, NULL, 'Conteudo util.', 7),
        ('Guia Dados em Foco', 'Luca Monteiro', 5, NULL, 'Bem organizado.', 6),
        ('Guia Dados em Foco', 'Malu Leal', 4, NULL, 'Recomendo para iniciantes.', 4),
        ('Guia Dados em Foco', 'Nico Pacheco', 5, NULL, 'Muito completo.', 3),
        ('Livro Pequeno Planeta', 'Olga Neves', 5, NULL, 'Meu filho adorou.', 6),
        ('Livro Pequeno Planeta', 'Paco Brito', 4, NULL, 'Ilustracoes bonitas.', 5),
        ('Livro Pequeno Planeta', 'Rafa Maia', 5, NULL, 'Boa historia para criancas.', 4),
        ('Livro Pequeno Planeta', 'Sueli Couto', 4, NULL, 'Leitura divertida.', 3),
        ('Livro Pequeno Planeta', 'Tito Gil', 5, NULL, 'Recomendo.', 2),

        ('Hidratante Orvalho', 'Uma Reis', 5, NULL, 'Deixa a pele macia.', 2),
        ('Hidratante Orvalho', 'Vini Lima', 4, NULL, 'Textura leve.', 1),
        ('Protetor Solar Horizonte', 'Wanda Melo', 5, NULL, 'Espalha facilmente.', 3),
        ('Protetor Solar Horizonte', 'Xande Rocha', 4, NULL, 'Boa protecao no dia a dia.', 2),
        ('Protetor Solar Horizonte', 'Yara Costa', 5, NULL, 'Nao deixa a pele oleosa.', 1),
        ('Batom Rubi', 'Zilda Nunes', 5, NULL, 'Cor bonita e duradoura.', 5),
        ('Batom Rubi', 'Andre Prado', 4, NULL, 'Gostei da cor.', 4),
        ('Batom Rubi', 'Bela Dias', 5, NULL, 'Acabamento excelente.', 3),
        ('Mascara de Cilios Prisma', 'Ciro Freitas', 4, NULL, 'Aplicacao facil.', 7),
        ('Mascara de Cilios Prisma', 'Dora Barros', 5, NULL, 'Otimo resultado.', 6),
        ('Mascara de Cilios Prisma', 'Eder Lopes', 4, NULL, 'Boa duracao.', 4),
        ('Mascara de Cilios Prisma', 'Fia Tavares', 5, NULL, 'Recomendo.', 3),
        ('Shampoo Raiz', 'Gabi Pires', 5, NULL, 'Deixou o cabelo leve.', 8),
        ('Shampoo Raiz', 'Hugo Luz', 4, NULL, 'Cheiro agradavel.', 6),
        ('Shampoo Raiz', 'Iara Azevedo', 5, NULL, 'Gostei bastante.', 4),
        ('Shampoo Raiz', 'Jair Cardoso', 4, NULL, 'Boa qualidade.', 3),
        ('Shampoo Raiz', 'Keli Teixeira', 5, NULL, 'Compraria novamente.', 2)
)
INSERT INTO Avaliacoes (NomeUsuario, Nota, UrlImagem, Comentario, DataAvaliacao, ProdutoId)
SELECT
    dados.NomeUsuario,
    dados.Nota,
    dados.UrlImagem,
    dados.Comentario,
    datetime('now', '-' || dados.DiasAtras || ' days'),
    produto.Id
FROM dados
JOIN Produtos produto ON produto.Nome = dados.ProdutoNome
WHERE NOT EXISTS (
    SELECT 1
    FROM Avaliacoes existente
    WHERE existente.ProdutoId = produto.Id
      AND existente.NomeUsuario = dados.NomeUsuario
      AND existente.Comentario = dados.Comentario
);

COMMIT;
