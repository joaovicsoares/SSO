# SSO – Sistema de Single Sign-On

##  Visão Geral

Este projeto é um **Sistema de Single Sign-On (SSO)** desenvolvido em **.NET**, com foco em **segurança, escalabilidade e boas práticas de arquitetura**.

O SSO será responsável por:

* Autenticação centralizada
* Emissão e validação de tokens (JWT)
* Gerenciamento de usuários, roles e permissões
* Integração com múltiplas aplicações clientes

O projeto utiliza **Clean Architecture**, **Docker** e está preparado para **CI/CD**, com ambientes separados de **development**, **staging** e **production**.

---

## 🏗️ Arquitetura

Estrutura baseada em **Clean Architecture**:

```
📦 SSO/
├── 📁 Sso.Api/              → Presentation Layer (Controllers, Middleware, Startup)
├── 📁 Sso.Application/      → Application Layer (Use Cases, DTOs, Interfaces)
├── 📁 Sso.Domain/           → Domain Layer (Entities, Value Objects, Business Rules)
└── 📁 Sso.Infrastructure/   → Infrastructure Layer (Data Access, External Services)
```

### Responsabilidades das Camadas:

* **Sso.Api** → Camada de apresentação (Controllers, Middlewares, Configurações)
* **Sso.Application** → Casos de uso, DTOs, validações e interfaces de serviços
* **Sso.Domain** → Entidades de domínio, regras de negócio e interfaces de repositórios (núcleo sem dependências)
* **Sso.Infrastructure** → Implementação de repositórios, EF Core, serviços externos

### Fluxo de Dependências:

```
Sso.Api → Sso.Application → Sso.Domain ← Sso.Infrastructure
```

A separação garante baixo acoplamento, alta testabilidade e facilidade de evolução.

---

## 🌱 Ambientes

O projeto trabalha com três ambientes principais:

| Ambiente    | Descrição                      |
| ----------- | ------------------------------ |
| Development | Desenvolvimento local          |
| Staging     | Ambiente de homologação/testes |
| Production  | Ambiente produtivo             |

O ambiente é controlado pela variável:

```bash
ASPNETCORE_ENVIRONMENT
```

---

## 🐳 Setup de Desenvolvimento (Docker)

### Pré-requisitos

* Git
* Docker e Docker Compose
* .NET 10 SDK (opcional, para desenvolvimento local sem Docker)

---

### 1️⃣ Clonar o repositório

```bash
git clone https://github.com/joaovicosoares/SSO.git
cd SSO
```

---

### 2️⃣ Subir o ambiente local

```bash
docker-compose up
```

Isso irá iniciar:
- **API SSO** em `http://localhost:5000`
- **PostgreSQL** em `localhost:5432`

---

### 3️⃣ Desenvolvimento local (sem Docker)

Se preferir rodar diretamente com .NET:

```bash
# Restaurar dependências
dotnet restore

# Executar a API
dotnet run --project Sso.Api
```

**Nota:** Configure a connection string do PostgreSQL em `appsettings.Development.json`

---

### 4️⃣ Verificar se está funcionando

```bash
curl http://localhost:5000/weatherforecast
```

---

## 🔄 CI/CD

O projeto utiliza **GitHub Actions** para integração e entrega contínua:

### Pipeline Atual:

* ✅ Build da aplicação (.NET 10)
* ✅ Testes unitários e de integração
* ✅ Cobertura de código
* ✅ PostgreSQL para testes

### Estratégia de Branches:

* **feature/*** → Build e validações automáticas
* **develop** → Deploy automático em **staging** (futuro)
* **main** → Deploy em **production** (futuro)

Veja mais detalhes em [CONTRIBUTING.md](CONTRIBUTING.md)

---


## 🧠 Observações

Este projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**, com foco em:

* Separação clara de responsabilidades
* Baixo acoplamento entre camadas
* Alta testabilidade
* Facilidade de manutenção e evolução

O projeto tem caráter **evolutivo**, servindo tanto para uso real quanto como base de estudo e referência arquitetural.

---

## 📚 Documentação Adicional

* [CONTRIBUTING.md](CONTRIBUTING.md) - Guia de contribuição e branching strategy
* [Product Backlog](product-backlog.md) - PBIs e roadmap do projeto

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor, leia o [CONTRIBUTING.md](CONTRIBUTING.md) antes de enviar um Pull Request.
