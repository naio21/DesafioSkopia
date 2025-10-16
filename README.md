# TaskManager

# Documentação da Solução

## Configuração do Banco de Dados

Essa aplicação utiliza como SGDB o **SQL Server LocalDb**.
A string de conexão está definida no arquivo `appsettings.json`: "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=TaskManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true" }

**Geração do Database:**  
As tabelas e dados iniciais podem ser gerados através do script presente em: `Database/Scripts/01_InitialSchema.sql`

Para configurar o BD:
1. Abra o SQL Server Management Studio ou utilize o comando `sqlcmd`.
2. Execute o script na ordem em que está, para criar as tabelas, metadados e inserir usuários padrão.

---

## Bibliotecas

Esta solução utiliza os seguintes pacotes Nuget essenciais (confira o `.csproj` para mais detalhes):

- **Dapper**: ORM para acesso a dados
- **Microsoft.Data.SqlClient**: provider do SQL Server
- **FluentValidation.AspNetCore**: Validação dos Modelos
- **Swashbuckle.AspNetCore**: Documentação e geração de interface Swagger/OpenAPI
- **xUnit**: Framework para testes de unidade
- **Moq**: Biblioteca para simular objetos durante os testes
- **Microsoft.Data.Sqlite**: BD In-memory para os testes
- **coverlet.collector**: Para calcular a cobertura de código pelos testes

---

## Rodando a Aplicação no Docker

### 1. Efetuando Build da Docker Image

Abra um terminal na raíz da Solution e execute: **docker build -t taskmanager-api ./TaskManager**

### 2. Executar o Docker Container

**docker run -p 5243:80 taskmanager-api**

- A API será acessível a partir de `http://localhost:5243`
- A Swagger UI poderá ser acessada a partir de: `http://localhost:5243/swagger`

---

## Notas Adicionais

- Certifique-se de que o DB foi criado e está acessível antes de executar a API.
- Passe o user ID em todos os requests no header `X-User-Id`.
- Para efetuar os testes, execute o comando: **dotnet test TaskManager.Tests/TaskManager.Tests.csproj**

## Próximos Passos

- Implementar autenticação (JWT token) e autorização (para definir se o usuário tem o role de Gerente)
- Adotar um SGBD mais robusto, como o SQL Server "full", Oracle ou MySQL
- Implementar o front-end da aplicação
