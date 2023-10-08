# Projeto Backend em C#

Este é um projeto backend em C# que utiliza o framework ASP.NET Core. Ele fornece uma estrutura básica para criar um serviço web API.

## Pré-requisitos

Certifique-se de ter o seguinte software instalado em sua máquina antes de prosseguir:

- [ASP.NET Core SDK](https://dotnet.microsoft.com/download) (versão 6.0 ou superior)
- EntityFramework

## Configuração do Projeto

### Atualizar o Banco de Dados

Sempre que você fizer alterações nas classes relacionadas ao banco de dados, escolha um novo nome para a migração e execute os seguintes comandos:

```bash
dotnet ef migrations add <NomeDaNovaMigracao>
dotnet ef database update
```

## Executando o Projeto

Para executar o projeto, utilize o seguinte comando:

```bash
dotnet run
```

O serviço web API estará disponível em `http://localhost:3000` por padrão.
