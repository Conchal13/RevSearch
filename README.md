# 🔍 RevSearch - API de Avaliação e Busca de Produtos

> Uma API Web desenvolvida em **C# com Minimal APIs**, projetada para centralizar avaliações de produtos por loja e facilitar a busca pelos itens mais recomendados pelos usuários.

---

## 📌 1. Identificação do Projeto

* **Título do Projeto:** RevSearch
* **Alunos:**
  * Pedro Henrique Volpatto Costa
  * Lucas Aliel Conchal Torres
* **Curso:** Ciência da Computação
* **Turma:** Noturno

---

## 📖 2. Resumo do Projeto

O **RevSearch** é uma aplicação backend estruturada no padrão Minimal APIs com foco em performance e simplicidade. O sistema gerencia avaliações de produtos disponibilizados por diferentes lojas.

A proposta principal é consolidar uma base de dados rica em feedbacks de clientes para viabilizar consultas avançadas de ranqueamento. Com isso, os consumidores podem encontrar com precisão os melhores produtos do mercado com base na média e quantidade de avaliações, aplicando filtros contextuais por loja, segmento e categoria.

### 🏛️ Modelo de Domínio e Relacionamentos

O sistema é sustentado por 5 entidades principais:

* **Loja:** Estabelecimento que cadastra e oferta produtos.
* **Categoria:** Classificação macro (ex.: *Eletrônicos*, *Vestuário*).
* **Segmento:** Subdivisão vinculada a uma categoria (ex.: *Smartphones*, *Calçados Esportivos*).
* **Produto:** Item comercializado por uma loja, pertencente a um segmento e que recebe avaliações.
* **Avaliação:** Nota (score) e comentários atribuídos por um usuário a determinado produto.

**Hierarquia:**
```text
Categoria
   └── Segmento
          └── Produto (pertencente a uma Loja)
                 └── Avaliações
```

## ⚙️ 3. Funcionalidades
### 🔹 CRUD Completo (Create, Read, Update, Delete)

Operações completas de gerenciamento para todos os modelos do sistema:

- Lojas (cadastro, consulta, atualização e remoção)
- Produtos (cadastro, consulta, atualização e remoção)
- Categorias (cadastro, consulta, atualização e remoção)
- Segmentos (cadastro, consulta, atualização e remoção)
- Avaliações (envio de feedback, leitura, edição e exclusão)

### 🔹 Consultas e Buscas Específicas

- Avaliações por Produto: Listagem de todas as notas e comentários recebidos por um- produto específico.
- Produtos por Loja: Consulta de todo o catálogo oferecido por um estabelecimento.
- Produtos por Segmento: Filtro de itens com base em seu nicho/segmento direto.
- Produtos por Categoria: Filtro de itens de forma ampla através da categoria pai.

### ⭐ Ranqueamento dos Melhores Produtos

- Busca por Melhores Avaliados: Endpoint otimizado para retornar os produtos com as notas médias mais altas, permitindo combinar filtros cumulativos:
  - Filtrar por Nome do Produto
  - Filtrar por Loja
  - Filtrar por Categoria
  - Filtrar por Segmento

---

## 🧩 4. Funcionalidades Implementadas

O sistema já oferece um conjunto de funcionalidades voltadas para o gerenciamento de lojas, categorias, segmentos, produtos e avaliações, permitindo que a API funcione como uma base para busca e recomendação de itens com base no feedback dos usuários.

### 🏪 Gestão de lojas

- Cadastro de novas lojas
- Consulta de todas as lojas cadastradas
- Busca de uma loja específica por identificador
- Atualização dos dados da loja
- Exclusão de lojas do sistema

Essas operações permitem registrar os estabelecimentos que comercializam os produtos avaliados pela plataforma.

### 🗂️ Organização por categoria e segmento

- Cadastro e consulta de categorias
- Cadastro e consulta de segmentos
- Associação de cada segmento a uma categoria
- Atualização e remoção das informações

Com isso, o sistema organiza o catálogo em níveis hierárquicos, facilitando a classificação dos produtos por nicho e área de mercado.

### 📦 Cadastro e consulta de produtos

- Registro de produtos com nome, URL, loja e segmento
- Listagem completa de todos os produtos
- Busca de produtos por loja
- Busca de produtos por segmento
- Consulta individual de um produto específico
- Edição e remoção de produtos

Essa funcionalidade permite montar um catálogo completo e manter os itens sempre relacionados ao seu segmento e ao estabelecimento responsável.

### ⭐ Gestão de avaliações

- Cadastro de avaliações de produtos
- Registro de nome do usuário, nota, comentário e data da avaliação
- Consulta de todas as avaliações
- Busca das avaliações de um produto específico
- Edição e exclusão de avaliações

A plataforma também valida que a nota esteja dentro do intervalo aceitável e que o produto informado exista antes da avaliação ser registrada.

### 🔎 Funcionalidade de busca e recomendação

Além do CRUD, a API já suporta consultas que ajudam na experiência de compra e recomendação:

- listar produtos por loja
- listar produtos por segmento
- visualizar avaliações de um produto
- identificar os itens melhor avaliados com base na média de notas



## 🤖 5. Uso de IA

### 🔧 Ferramentas Utilizadas
- Google AI Studio (Gemini 3.8 Flash)
- GitHub Copilot

### 🧑‍💻 Forma de Uso
#### ⌨️ Prompts (Em ordem e ferramenta)
**Primeiro prompt, Google AI Studio**
```
Preciso de um README para meu projeto, é uma aplicação web utilizando minimal api com C#

Primeiro tópico: Identificação do projeto
Titulo do Projeto: RevSearch
Alunos: Pedro Henrique Volpatto Costa, Lucas Aliel Conchal Torres
Curso e Turma: Ciência da Computação - Noite

Segundo Tópico: Resumo

O sistema consiste em uma api de avaliação de produtos, por loja
A ideia é, a partir de quando houver uma boa base de dados, exista metodos de busca para encontrar os melhores produtos, com base na avaliação dos usuários
O sistema tem 5 principais classes, Loja, Produto, Categoria, Segmento e Avaliação
Loja tem Produtos, Produtos tem avalições e segmento, segmento tem categoria

Terceiro Tópico: Funcionalidades:

CRUD de todos os modelos
Busca de Avaliações por produto
Busca de Produtos por Loja
Busca de Produtos por Segmento
Busca de Produtos por Categoria
Busca de Produtos melhores avaliados, podendo ser filtrado por, categoria, loja, segmento
```

**Segundo Prompt, GitHub Copilot**
```
Analise as funcionalidades ja implementadas em Program.cs e faça um quarto topico no README.md com uma descrição das funcionalidades de cada endpoint, sem entrar em méritos técnicos
```

### 🔎 Revisões
- O segundo prompt tinha gerado um descritivo muito técnico, falando sobre arquitetura, estrutura de Minimal APIs e descrição de entidades, então corrigi o prompt para solicitar uma explicação menos técnica, como se fosse direcionado pro cliente