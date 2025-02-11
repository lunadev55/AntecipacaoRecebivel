# Projeto de API - Antecipação de Recebíveis

Este projeto é uma API desenvolvida com .NET 8.0.201, utilizando a arquitetura limpa (Clean Architecture) e outras boas práticas de desenvolvimento. A API oferece funcionalidades de registro e login de usuários, bem como a criação de empresas, notas fiscais e antecipação de recebíveis.

## Tecnologias Utilizadas

- **.NET 8.0.201**
- **Entity Framework Core** (Code First)
- **SQL Server** como banco de dados
- **AutoMapper** para mapeamento entre entidades e DTOs
- **FluentValidation** para validação de dados
- **Serilog** para logging
- **JWT Token** para autenticação e autorização com Identity
- **Dependency Injection**
- **Middleware simples**

## Funcionalidades

1. **Autenticação e Autorização**
   - Registro de usuários.
   - Login de usuários com e geração de JWT Token.
   - Autorização de acesso com base no JWT.

2. **Cadastro de Empresas**
   - Criação de empresas no sistema, incluindo validação de dados.
   - Uso do AutoMapper para conversão de objetos DTO para entidade e vice-versa.

3. **Antecipação de Recebíveis**
   - Cálculo de antecipação de recebíveis com base nos dados de uma empresa.

4. **Validação de Requisições**
   - Uso do FluentValidation para validar modelos vindos na requisição.

5. **Logging**
   - Implementação de logging com Serilog para registrar as ações e erros do sistema.

6. **Middleware**
   - Middleware simples para log the requests e responses do sistema.

## Estrutura do Projeto

O projeto segue a arquitetura limpa, organizada da seguinte forma:

- **Application**: Contém os casos de uso, serviços e validações.
- **Domain**: Contém as entidades de domínio.
- **Data**: Contem a implementação de acesso ao banco de dados, repositórios e contextos de banco.
- **Infrastructure**
- **WebApi**: Contém os controladores e configurações de API.

## Como Rodar o Projeto

### 1. Clonar o Repositório

```bash
git clone https://github.com/lunadev55/AntecipacaoRecebivel.git
cd AntecipacaoRecebivel
```

### 2. Configurar o Banco de Dados 

Altere a string de conexão no arquivo `appsettings.json` (AntecipacaoRecebivel.WebApi) para a sua configuração do SQL Server:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=SEU_BANCO_DE_DADOS;User Id=SEU_USUARIO;Password=SUA_SENHA;"
}
```

### 3. Criar e Executar a Migração

Para criar as Migrations é preciso rodar os comandos nos 2 contextos (tendo como target o projeto AntecipacaoRecebivel.Data):

```bash
dotnet ef migrations add InitialUser --context UserDbContext,
dotnet ef migrations add InitialApplication --context ApplicationDbContext
```

Para criar o banco de dados e as tabelas, rode o seguinte comando no terminal:

```bash
'dotnet ef database update --context UserDbContext`,
'dotnet ef database update --context ApplicationDbContext'

```

### 4. Rodar o Projeto

Execute o comando abaixo para rodar a aplicação:

`dotnet run`

A API estará disponível em `https://localhost:5001`.

## **Testes Unitários**

O projeto inclui testes unitários que validam o funcionamento das funcionalidades principais. Para rodar os testes, use o seguinte comando:

`dotnet test`

## **Endpoints**

### **POST /api/auth/registrar**

Cria um novo usuário.

**Request:** 
`{`  
  `"username": "novoUsuario",`  
  `"password": "senhaSegura"`  
`}`

**Response:**

* Status 200 OK: "Usuário registrado com sucesso."  
* Status 400 Bad Request: Erro de validação.

### **POST /api/auth/login**

Realiza o login de um usuário e retorna um JWT Token.

**Request:** 
`{`  
  `"username": "usuarioExistente",`  
  `"password": "senhaCorrecta"`  
`}`

**Response:**

* Status 200 OK: `{ "Token": "JWT_TOKEN" }`  
* Status 401 Unauthorized: Credenciais inválidas.
