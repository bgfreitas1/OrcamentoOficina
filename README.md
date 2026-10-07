# OrcamentoOficina

API REST para gerenciamento de orçamentos de uma oficina automotiva, desenvolvida em **C# / .NET 10**, aplicando princípios de **Clean Architecture**, modelagem de domínio e separação de responsabilidades.

O projeto foi desenvolvido como solução para um desafio técnico de Back-end C# Sênior.

---

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- Swagger / OpenAPI
- xUnit
- Docker / Docker Compose
- Health Checks
- RFC 9457 Problem Details

---

## Arquitetura

A solução foi organizada seguindo os princípios de **Clean Architecture**, mantendo o domínio independente de detalhes de infraestrutura.

```text
OrcamentoOficina
│
├── OrcamentoOficina.API
├── OrcamentoOficina.Application
├── OrcamentoOficina.Domain
├── OrcamentoOficina.Infrastructure
├── OrcamentoOficina.UnitTests
└── OrcamentoOficina.IntegrationTests


Bash 
dotnet --version
dotnet restore
dotnet build

SQL 
dotnet ef database update --project OrcamentoOficina.Infrastructure --startup-project OrcamentoOficina.API

Executar 
dotnet run --project OrcamentoOficina.API


docker compose up -d sqlserver


Server=localhost,14330;Database=OrcamentoOficina;User Id=sa;Password=SuaSenhaForte!123;TrustServerCertificate=True;



Funcionalidades
A API implementa o fluxo completo de orçamento da oficina:
- criação de orçamento em rascunho;
- inclusão de peças e serviços;
- alteração e remoção de itens;
- cálculo dos valores do orçamento;
- desconto fixo ou percentual;
- autorização para descontos superiores a 15%;
- envio do orçamento;
- aprovação total;
- aprovação parcial;
- reprovação com motivo;
- controle de validade;
- criação de revisão do orçamento;
- conversão do orçamento aprovado em ordem de serviço;
- consulta por identificador;
- consulta paginada com filtros;
- histórico das alterações de estado;
- consulta simulada de disponibilidade de peças em estoque.