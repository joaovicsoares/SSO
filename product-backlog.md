\# Product Backlog – Sistema de Single Sign-On (SSO)



\## Informações do Documento



\*\*Versão:\*\* 1.0  

\*\*Data:\*\* 03/02/2026  

\*\*Projeto:\*\* Sistema de Single Sign-On (SSO)  

\*\*Metodologia:\*\* Desenvolvimento Incremental baseado em PBIs



---



\## Índice



1\. \[Visão Geral do Backlog](#1-visão-geral-do-backlog)

2\. \[Estrutura das PBIs](#2-estrutura-das-pbis)

3\. \[Roadmap de Desenvolvimento](#3-roadmap-de-desenvolvimento)

4\. \[Product Backlog Items](#4-product-backlog-items)



---



\## 1. Visão Geral do Backlog



Este documento organiza o desenvolvimento do Sistema SSO em \*\*20 PBIs\*\* (Product Backlog Items) priorizadas e sequenciadas para entrega incremental de valor.



\### 1.1 Princípios de Organização



\- \*\*Dependências técnicas\*\* foram consideradas na sequência

\- \*\*Valor de negócio\*\* foi balanceado com complexidade técnica

\- \*\*Entregas incrementais\*\* permitem validação contínua

\- \*\*Critérios de aceitação\*\* são testáveis e mensuráveis



\### 1.2 Estimativas



As estimativas utilizam \*\*Story Points\*\* (escala Fibonacci: 1, 2, 3, 5, 8, 13, 21) baseadas em:

\- Complexidade técnica

\- Esforço de desenvolvimento

\- Incertezas e riscos



---



\## 2. Estrutura das PBIs



Cada PBI contém:



\- \*\*ID e Título\*\*

\- \*\*Descrição\*\*: Contexto e objetivo

\- \*\*Prioridade\*\*: Alta, Média ou Baixa

\- \*\*Story Points\*\*: Estimativa de esforço

\- \*\*Dependências\*\*: PBIs predecessoras

\- \*\*Análise Técnica\*\*: Detalhamento da implementação

\- \*\*Ferramentas e Tecnologias\*\*: Stack sugerido

\- \*\*Critérios de Aceitação\*\*: Condições para conclusão

\- \*\*Testes\*\*: Estratégia de validação

\- \*\*Riscos\*\*: Pontos de atenção



---



\## 3. Roadmap de Desenvolvimento



\### Sprint 1 - Fundação (PBIs 1-4)

Infraestrutura básica, banco de dados e autenticação simples



\### Sprint 2 - Core OAuth 2.0 (PBIs 5-8)

Implementação do Authorization Code Flow completo



\### Sprint 3 - OpenID Connect (PBIs 9-11)

ID Tokens, UserInfo e descoberta OIDC



\### Sprint 4 - Autorização (PBIs 12-14)

RBAC, permissões e consentimento



\### Sprint 5 - Interface de Usuário (PBIs 15-17)

UIs de login, cadastro e administração



\### Sprint 6 - Segurança e Produção (PBIs 18-20)

Auditoria, segurança avançada e otimizações



---



\## 4. Product Backlog Items



---



\### PBI-001: Configuração da Infraestrutura Inicial do Projeto



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 3  

\*\*Dependências:\*\* Nenhuma



\#### Descrição



Configurar a estrutura base do projeto, incluindo repositório, arquitetura de pastas, CI/CD básico e ambientes de desenvolvimento.



\#### Análise Técnica



\*\*Estrutura do Projeto:\*\*

\- Arquitetura em camadas (Presentation, Application, Domain, Infrastructure)

\- Separação entre SSO Server e Admin UI

\- Configuração de ambientes (Development, Staging, Production)



\*\*Componentes:\*\*

1\. Repositório Git com branching strategy (GitFlow)

2\. Estrutura de pastas seguindo Clean Architecture

3\. Configuração de ambiente Docker para desenvolvimento

4\. Pipeline CI/CD básico (build e testes unitários)

5\. Documentação inicial (README, CONTRIBUTING)



\#### Ferramentas e Tecnologias



\*\*Backend:\*\*

\- .NET 8 / ASP.NET Core

\- Docker e Docker Compose

\- Git / GitHub ou GitLab



\*\*CI/CD:\*\*

\- GitHub Actions ou GitLab CI

\- SonarQube (análise de código)



\*\*Documentação:\*\*

\- Markdown para docs

\- Swagger/OpenAPI para documentação de APIs



\#### Critérios de Aceitação



\- \[ ] Repositório criado com estrutura de pastas definida

\- \[ ] Docker Compose configurado para desenvolvimento local

\- \[ ] Pipeline CI executando build com sucesso

\- \[ ] README com instruções de setup

\- \[ ] Branching strategy documentada

\- \[ ] Ambientes (dev, staging, prod) configurados



\#### Testes



\- Pipeline CI executando com sucesso

\- Build local funcionando via Docker

\- Validação de estrutura de pastas



\#### Riscos



\- \*\*Baixo:\*\* Tecnologias maduras e bem documentadas

\- Definição de padrões arquiteturais pode gerar debates



---



\### PBI-002: Modelagem e Criação do Banco de Dados



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-001



\#### Descrição



Implementar o modelo de dados completo conforme especificação, incluindo todas as tabelas, relacionamentos, índices e migrations.



\#### Análise Técnica



\*\*Modelo de Dados:\*\*



Implementação de 12 tabelas:

1\. Users

2\. Roles

3\. Permissions

4\. RolePermissions

5\. UserRoles

6\. UserPermissions

7\. Clients

8\. ClientPermissions

9\. Scopes

10\. ClientScopes

11\. UserConsents

12\. AuthorizationCodes

13\. RefreshTokens

14\. AuditLogs



\*\*Decisões Técnicas:\*\*

\- Primary Keys: GUIDs para distribuição e segurança

\- Timestamps: UTC em todas as tabelas

\- Soft Delete: campo IsActive/IsDeleted onde aplicável

\- Índices: em chaves estrangeiras e campos de busca frequente



\#### Ferramentas e Tecnologias



\*\*ORM:\*\*

\- Entity Framework Core 8



\*\*Banco de Dados:\*\*

\- PostgreSQL 15+ (recomendado) ou SQL Server 2019+



\*\*Migrations:\*\*

\- EF Core Migrations

\- Scripts SQL versionados



\*\*Ferramentas:\*\*

\- DbUp ou FluentMigrator (alternativa)

\- pgAdmin ou DBeaver (administração)



\#### Critérios de Aceitação



\- \[ ] Todas as 13 tabelas criadas via migrations

\- \[ ] Relacionamentos e constraints implementados

\- \[ ] Índices criados em campos críticos

\- \[ ] Seeds de dados iniciais (scopes padrão: openid, profile, email)

\- \[ ] Documentação do modelo de dados (diagrama ER)

\- \[ ] Migrations executando em todos os ambientes

\- \[ ] Rollback de migrations funcional



\#### Testes



\- Migrations aplicadas com sucesso

\- Constraints validadas (FK, unique, not null)

\- Seeds executados corretamente

\- Rollback testado



\#### Riscos



\- \*\*Médio:\*\* Mudanças futuras no modelo podem requerer migrações complexas

\- Escolha do banco de dados impacta performance (PostgreSQL recomendado)



---



\### PBI-003: Implementação de Entidades de Domínio e Repositories



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-002



\#### Descrição



Criar as entidades de domínio (Domain Models) e implementar o padrão Repository para acesso a dados, garantindo separação de responsabilidades.



\#### Análise Técnica



\*\*Domain Layer:\*\*

\- Entidades ricas (não anêmicas) com comportamentos

\- Value Objects para conceitos como Email, Password

\- Domain Events para ações críticas



\*\*Repository Pattern:\*\*

\- Interfaces na camada de domínio

\- Implementações na camada de infraestrutura

\- Unit of Work para transações



\*\*Entidades Principais:\*\*

```csharp

\- User (com métodos SetPassword, Activate, Deactivate)

\- Role (com métodos AddPermission, RemovePermission)

\- Permission

\- Client (com métodos ValidateRedirectUri, AddScope)

\- RefreshToken (com métodos Rotate, Revoke)

\- AuthorizationCode (com método MarkAsUsed)

```



\#### Ferramentas e Tecnologias



\*\*Padrões:\*\*

\- Domain-Driven Design (DDD)

\- Repository Pattern

\- Unit of Work Pattern



\*\*Bibliotecas:\*\*

\- FluentValidation (validações de domínio)

\- MediatR (domain events - opcional)



\#### Critérios de Aceitação



\- \[ ] Todas as entidades de domínio criadas

\- \[ ] Value Objects implementados (Email, Password)

\- \[ ] Interfaces de Repository definidas

\- \[ ] Implementações de Repository com EF Core

\- \[ ] Unit of Work implementado

\- \[ ] Validações de domínio funcionando

\- \[ ] Testes unitários das entidades (>80% cobertura)



\#### Testes



\*\*Testes Unitários:\*\*

\- Comportamentos das entidades

\- Validações de domínio

\- Value Objects



\*\*Testes de Integração:\*\*

\- Repositories com banco de dados

\- Operações CRUD



\#### Riscos



\- \*\*Baixo:\*\* Padrões bem estabelecidos

\- Over-engineering: manter simplicidade quando possível



---



\### PBI-004: Sistema de Autenticação Básico (Login com Email/Senha)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* PBI-003



\#### Descrição



Implementar autenticação básica de usuários com email e senha, incluindo hashing seguro, sessão e proteções contra ataques.



\#### Análise Técnica



\*\*Componentes:\*\*



1\. \*\*Password Hashing:\*\*

&nbsp;  - Algoritmo: Argon2id (OWASP recomendado)

&nbsp;  - Fallback: BCrypt (mínimo aceitável)

&nbsp;  - Salt automático por usuário



2\. \*\*Sessão:\*\*

&nbsp;  - Cookie-based authentication

&nbsp;  - Flags: HttpOnly, Secure, SameSite=Strict

&nbsp;  - Duração configurável (padrão: 8 horas)



3\. \*\*Proteções:\*\*

&nbsp;  - Rate limiting por IP e usuário

&nbsp;  - Account lockout após N tentativas falhas

&nbsp;  - Mensagens genéricas (evitar enumeração)

&nbsp;  - Logging de tentativas



\*\*Endpoints:\*\*

```

POST /auth/login

POST /auth/logout

GET  /auth/session (validação de sessão)

```



\#### Ferramentas e Tecnologias



\*\*Hashing:\*\*

\- Konscious.Security.Cryptography.Argon2 (Argon2id)

\- BCrypt.Net-Next (alternativa)



\*\*Rate Limiting:\*\*

\- AspNetCoreRateLimit

\- Redis (storage distribuído)



\*\*Session:\*\*

\- ASP.NET Core Cookie Authentication

\- Data Protection API



\#### Critérios de Aceitação



\- \[ ] Endpoint de login funcional

\- \[ ] Senhas hasheadas com Argon2id ou BCrypt

\- \[ ] Cookie de sessão seguro (HttpOnly, Secure, SameSite)

\- \[ ] Rate limiting ativo (5 tentativas/min por IP)

\- \[ ] Account lockout após 5 tentativas falhas (15 min)

\- \[ ] Logout invalidando sessão

\- \[ ] Mensagens de erro genéricas

\- \[ ] Logs de autenticação registrados



\#### Testes



\*\*Testes Unitários:\*\*

\- Password hashing e verificação

\- Validações de email



\*\*Testes de Integração:\*\*

\- Fluxo completo de login

\- Lockout funcionando

\- Rate limiting



\*\*Testes de Segurança:\*\*

\- Brute force bloqueado

\- Timing attacks mitigados

\- Cookie flags corretos



\#### Riscos



\- \*\*Médio:\*\* Configuração incorreta de rate limiting pode bloquear usuários legítimos

\- \*\*Alto:\*\* Vulnerabilidades de segurança se implementação incorreta



---



\### PBI-005: Implementação do Endpoint /authorize (OAuth 2.0)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* PBI-004



\#### Descrição



Implementar o endpoint /authorize do Authorization Code Flow, incluindo validações de client, redirect\_uri, scopes e geração de authorization code.



\#### Análise Técnica



\*\*Fluxo do Endpoint:\*\*



1\. Validar parâmetros da requisição:

&nbsp;  - client\_id (existente e ativo)

&nbsp;  - redirect\_uri (whitelist do client)

&nbsp;  - response\_type=code

&nbsp;  - scope (permitido para o client)

&nbsp;  - state (CSRF protection)



2\. Verificar autenticação do usuário:

&nbsp;  - Se não autenticado → redirecionar para /login

&nbsp;  - Se autenticado → verificar consentimento



3\. Verificar consentimento:

&nbsp;  - Se já consentiu → gerar code

&nbsp;  - Se não consentiu → exibir tela de consentimento



4\. Gerar Authorization Code:

&nbsp;  - Código único, curta duração (10 minutos)

&nbsp;  - Vinculado a: user, client, redirect\_uri, scopes

&nbsp;  - Armazenado no banco (AuthorizationCodes)



5\. Redirecionar com code:

&nbsp;  - `{redirect\_uri}?code={code}\&state={state}`



\*\*Validações de Segurança:\*\*

\- Redirect URI exact match (sem wildcards)

\- State parameter obrigatório

\- Code com entropia suficiente (32+ bytes)



\#### Ferramentas e Tecnologias



\*\*Geração de Códigos:\*\*

\- System.Security.Cryptography.RandomNumberGenerator

\- Base64Url encoding



\*\*Validações:\*\*

\- FluentValidation

\- Custom middleware para OAuth validations



\#### Critérios de Aceitação



\- \[ ] Endpoint GET /authorize implementado

\- \[ ] Validação de client\_id e ativação

\- \[ ] Validação estrita de redirect\_uri (exact match)

\- \[ ] Validação de scopes permitidos

\- \[ ] Redirecionamento para login se não autenticado

\- \[ ] Authorization code gerado e armazenado

\- \[ ] Code com expiração de 10 minutos

\- \[ ] Redirecionamento com code e state

\- \[ ] Erros retornados conforme RFC 6749

\- \[ ] Logs de eventos /authorize



\#### Testes



\*\*Testes Unitários:\*\*

\- Validações de parâmetros

\- Geração de códigos



\*\*Testes de Integração:\*\*

\- Fluxo completo com usuário autenticado

\- Fluxo com redirecionamento para login

\- Validação de redirect\_uri inválido

\- Client inativo bloqueado



\*\*Testes de Segurança:\*\*

\- Code com entropia adequada

\- Expiração de codes

\- Redirect URI manipulation



\#### Riscos



\- \*\*Alto:\*\* Vulnerabilidades de open redirect se validação incorreta

\- \*\*Médio:\*\* Timing de expiração de codes pode gerar problemas em produção



---



\### PBI-006: Implementação do Endpoint /token (OAuth 2.0)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 13  

\*\*Dependências:\*\* PBI-005



\#### Descrição



Implementar o endpoint /token para troca de authorization code por tokens (Access Token, ID Token, Refresh Token), incluindo validações e geração de JWTs.



\#### Análise Técnica



\*\*Grant Types Suportados:\*\*



1\. \*\*authorization\_code:\*\*

&nbsp;  - Valida code, client\_id, client\_secret, redirect\_uri

&nbsp;  - Marca code como usado (uso único)

&nbsp;  - Gera Access Token, ID Token, Refresh Token



2\. \*\*refresh\_token (PBI futura):\*\*

&nbsp;  - Valida refresh token

&nbsp;  - Rotaciona refresh token

&nbsp;  - Gera novos Access Token e ID Token



\*\*Estrutura dos Tokens:\*\*



\*\*Access Token (JWT):\*\*

```json

{

&nbsp; "iss": "https://sso.example.com",

&nbsp; "sub": "user-guid",

&nbsp; "aud": "client-id",

&nbsp; "exp": 1234567890,

&nbsp; "iat": 1234567000,

&nbsp; "roles": \["admin", "user"],

&nbsp; "permissions": \["users.read", "users.write"]

}

```

\*\*OBS As permissões emitidas no token devem ser calculadas como:\*\*



```permissoes\_finais = permissoes\_do\_usuario ∩ permissoes\_do\_client```



Somente permissoes\_finais entram na claim permissions.



\*\*ID Token (JWT - OpenID Connect):\*\*

```json

{

&nbsp; "iss": "https://sso.example.com",

&nbsp; "sub": "user-guid",

&nbsp; "aud": "client-id",

&nbsp; "exp": 1234567890,

&nbsp; "iat": 1234567000,

&nbsp; "email": "user@example.com",

&nbsp; "name": "John Doe",

&nbsp; "email\_verified": true

}

```



\*\*Refresh Token:\*\*

\- String opaca (não JWT)

\- Armazenado no banco

\- Vinculado a user e client

\- Duração longa (30 dias)



\*\*Assinatura JWT:\*\*

\- Algoritmo: RS256 ou ES256

\- Chaves assimétricas (privada para assinar, pública via JWKS)

\- Rotação de chaves suportada



\#### Ferramentas e Tecnologias



\*\*JWT:\*\*

\- Microsoft.IdentityModel.Tokens

\- System.IdentityModel.Tokens.Jwt



\*\*Criptografia:\*\*

\- RSA ou ECDSA (geração de chaves)

\- System.Security.Cryptography



\*\*Validação:\*\*

\- Client authentication (Basic Auth ou client\_secret no body)



\#### Critérios de Aceitação



\- \[ ] Endpoint POST /token implementado

\- \[ ] Grant type authorization\_code funcional

\- \[ ] Validação de code (existência, expiração, uso único)

\- \[ ] Validação de client\_secret

\- \[ ] Validação de redirect\_uri match

\- \[ ] Access Token JWT gerado com roles e permissions

\- \[ ] ID Token JWT gerado com claims de identidade

\- \[ ] Refresh Token gerado e armazenado

\- \[ ] Tokens com durações corretas (Access: 15min, Refresh: 30d)

\- \[ ] Authorization code marcado como usado

\- \[ ] Erros conforme RFC 6749

\- \[ ] Logs de emissão de tokens



\#### Testes



\*\*Testes Unitários:\*\*

\- Geração e assinatura de JWTs

\- Validações de grant type

\- Construção de claims



\*\*Testes de Integração:\*\*

\- Fluxo completo authorization\_code

\- Code reutilizado retorna erro

\- Code expirado retorna erro

\- Client\_secret inválido retorna erro

\- Redirect\_uri mismatch retorna erro



\*\*Testes de Segurança:\*\*

\- Validação de assinatura JWT

\- Tokens não aceitam algoritmo "none"

\- Client authentication necessária



\#### Riscos



\- \*\*Alto:\*\* Implementação incorreta de JWT pode gerar vulnerabilidades críticas

\- \*\*Médio:\*\* Gerenciamento de chaves privadas requer cuidado especial

\- \*\*Médio:\*\* Performance de geração de tokens pode ser gargalo



---



\### PBI-007: Publicação de JWKS (JSON Web Key Set)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 3  

\*\*Dependências:\*\* PBI-006



\#### Descrição



Implementar o endpoint /.well-known/jwks.json para publicação das chaves públicas utilizadas na assinatura dos JWTs.



\#### Análise Técnica



\*\*Funcionalidade:\*\*



1\. Expor chaves públicas em formato JWK (JSON Web Key)

2\. Incluir metadata: kid (key ID), alg (algoritmo), use (sig)

3\. Suportar rotação de chaves (múltiplas chaves ativas)



\*\*Formato JWKS:\*\*

```json

{

&nbsp; "keys": \[

&nbsp;   {

&nbsp;     "kty": "RSA",

&nbsp;     "use": "sig",

&nbsp;     "kid": "key-2024-01",

&nbsp;     "alg": "RS256",

&nbsp;     "n": "...",

&nbsp;     "e": "AQAB"

&nbsp;   }

&nbsp; ]

}

```



\*\*Rotação de Chaves:\*\*

\- Novas chaves adicionadas antes da antiga expirar

\- Múltiplas chaves ativas durante período de transição

\- Chaves antigas removidas após todos os tokens assinados expirarem



\#### Ferramentas e Tecnologias



\*\*Bibliotecas:\*\*

\- Microsoft.IdentityModel.Tokens (JWK generation)

\- System.Security.Cryptography



\*\*Cache:\*\*

\- In-memory cache para chaves públicas

\- Refresh automático



\#### Critérios de Aceitação



\- \[ ] Endpoint GET /.well-known/jwks.json implementado

\- \[ ] Chaves públicas no formato JWK correto

\- \[ ] Metadata completo (kid, alg, use)

\- \[ ] Cache de chaves implementado

\- \[ ] CORS habilitado para acesso público

\- \[ ] Documentação de rotação de chaves



\#### Testes



\*\*Testes de Integração:\*\*

\- JWKS retornando chaves válidas

\- Formato JSON correto

\- Validação de JWT usando JWKS



\*\*Testes de Compatibilidade:\*\*

\- Validação com bibliotecas JWT de diferentes linguagens



\#### Riscos



\- \*\*Baixo:\*\* Implementação padrão e bem documentada



---



\### PBI-008: Implementação de PKCE (Proof Key for Code Exchange)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-006



\#### Descrição



Adicionar suporte obrigatório a PKCE para proteção contra ataques de interceptação de authorization code, especialmente importante para aplicações públicas.



\#### Análise Técnica



\*\*PKCE Flow:\*\*



1\. \*\*Client gera code\_verifier:\*\*

&nbsp;  - String aleatória (43-128 caracteres)

&nbsp;  - Base64url encoded



2\. \*\*Client gera code\_challenge:\*\*

&nbsp;  - Métodos: plain ou S256 (SHA256)

&nbsp;  - `code\_challenge = BASE64URL(SHA256(code\_verifier))`



3\. \*\*Request /authorize:\*\*

&nbsp;  - Adiciona: `code\_challenge`, `code\_challenge\_method`

&nbsp;  - SSO armazena com authorization code



4\. \*\*Request /token:\*\*

&nbsp;  - Adiciona: `code\_verifier`

&nbsp;  - SSO valida: `code\_challenge == SHA256(code\_verifier)`



\*\*Implementação no SSO:\*\*

\- Armazenar code\_challenge e method no AuthorizationCode

\- Validar code\_verifier no endpoint /token

\- Tornar PKCE obrigatório (não opcional)



\#### Ferramentas e Tecnologias



\*\*Hashing:\*\*

\- System.Security.Cryptography.SHA256

\- Base64Url encoding



\*\*Validação:\*\*

\- Custom PKCE validator



\#### Critérios de Aceitação



\- \[ ] PKCE suportado no /authorize (armazena code\_challenge)

\- \[ ] PKCE validado no /token

\- \[ ] Métodos S256 e plain suportados

\- \[ ] PKCE obrigatório para todos os clients

\- \[ ] Erro retornado se code\_verifier inválido

\- \[ ] Documentação de PKCE para clients

\- \[ ] Exemplos de implementação de PKCE



\#### Testes



\*\*Testes Unitários:\*\*

\- Geração de code\_challenge

\- Validação de code\_verifier



\*\*Testes de Integração:\*\*

\- Fluxo completo com PKCE S256

\- Fluxo completo com PKCE plain

\- Code\_verifier incorreto retorna erro

\- Ausência de PKCE retorna erro



\*\*Testes de Segurança:\*\*

\- Proteção contra code interception



\#### Riscos



\- \*\*Médio:\*\* Clients existentes precisarão adaptar implementação

\- \*\*Baixo:\*\* Padrão bem documentado (RFC 7636)



---



\### PBI-009: Implementação de ID Token (OpenID Connect)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-006



\#### Descrição



Implementar a geração de ID Token conforme especificação OpenID Connect, contendo claims de identidade do usuário.



\#### Análise Técnica



\*\*Claims do ID Token:\*\*



\*\*Obrigatórias:\*\*

\- iss (issuer): URL do SSO

\- sub (subject): User ID

\- aud (audience): Client ID

\- exp (expiration): Timestamp

\- iat (issued at): Timestamp



\*\*Padrão OpenID:\*\*

\- email

\- email\_verified

\- name

\- given\_name (opcional)

\- family\_name (opcional)

\- picture (opcional)



\*\*Claims Adicionais (baseadas em scopes):\*\*

\- Scope "profile": name, given\_name, family\_name, picture

\- Scope "email": email, email\_verified



\*\*Assinatura:\*\*

\- Mesmo par de chaves do Access Token

\- Algoritmo RS256 ou ES256



\#### Ferramentas e Tecnologias



\*\*JWT:\*\*

\- System.IdentityModel.Tokens.Jwt

\- Claims mapping baseado em scopes



\#### Critérios de Aceitação



\- \[ ] ID Token gerado no endpoint /token

\- \[ ] Claims obrigatórias presentes

\- \[ ] Claims baseadas em scopes solicitados

\- \[ ] Assinatura válida (mesmas chaves do Access Token)

\- \[ ] Duração apropriada (mesma do Access Token)

\- \[ ] Nonce suportado (se enviado no /authorize)

\- \[ ] at\_hash incluído (hash do Access Token)



\#### Testes



\*\*Testes Unitários:\*\*

\- Geração de ID Token

\- Claims mapping



\*\*Testes de Integração:\*\*

\- ID Token retornado com authorization\_code grant

\- Claims corretas baseadas em scopes

\- Validação de assinatura



\*\*Testes de Conformidade:\*\*

\- Validação com OpenID Connect Validator



\#### Riscos



\- \*\*Baixo:\*\* Especificação clara e bem documentada



---



\### PBI-010: Endpoint /userinfo (OpenID Connect)



\*\*Prioridade:\*\* Média  

\*\*Story Points:\*\* 3  

\*\*Dependências:\*\* PBI-009



\#### Descrição



Implementar o endpoint /userinfo para retornar informações do usuário autenticado, protegido por Access Token.



\#### Análise Técnica



\*\*Endpoint:\*\*

```

GET /userinfo

Authorization: Bearer {access\_token}

```



\*\*Response:\*\*

```json

{

&nbsp; "sub": "user-guid",

&nbsp; "email": "user@example.com",

&nbsp; "email\_verified": true,

&nbsp; "name": "John Doe",

&nbsp; "given\_name": "John",

&nbsp; "family\_name": "Doe"

}

```



\*\*Funcionalidade:\*\*

1\. Validar Access Token (assinatura, expiração, aud, iss)

2\. Extrair sub (user ID) do token

3\. Buscar informações do usuário no banco

4\. Filtrar claims baseadas em scopes do token

5\. Retornar JSON com claims



\*\*Segurança:\*\*

\- Validação completa do JWT

\- Rate limiting

\- Sem informações sensíveis (senha, tokens)



\#### Ferramentas e Tecnologias



\*\*JWT Validation:\*\*

\- Microsoft.AspNetCore.Authentication.JwtBearer

\- Configuração de validação automática



\*\*Rate Limiting:\*\*

\- AspNetCoreRateLimit (100 req/min por usuário)



\#### Critérios de Aceitação



\- \[ ] Endpoint GET /userinfo implementado

\- \[ ] Autenticação via Bearer token obrigatória

\- \[ ] Validação completa de JWT (assinatura, exp, iss, aud)

\- \[ ] Claims filtradas por scopes do token

\- \[ ] Erro 401 se token inválido ou ausente

\- \[ ] Erro 403 se token válido mas sem scopes necessários

\- \[ ] Rate limiting ativo

\- \[ ] CORS configurado para clients autorizados



\#### Testes



\*\*Testes de Integração:\*\*

\- Request com token válido retorna claims

\- Token inválido retorna 401

\- Token sem scope openid retorna erro

\- Claims filtradas corretamente



\*\*Testes de Segurança:\*\*

\- Token expirado rejeitado

\- Token adulterado rejeitado

\- Assinatura inválida rejeitada



\#### Riscos



\- \*\*Baixo:\*\* Endpoint padrão do OpenID Connect



---



\### PBI-011: Discovery Endpoint (/.well-known/openid-configuration)



\*\*Prioridade:\*\* Média  

\*\*Story Points:\*\* 2  

\*\*Dependências:\*\* PBI-010



\#### Descrição



Implementar o endpoint de descoberta OpenID Connect para permitir que clients descubram automaticamente a configuração do SSO.



\#### Análise Técnica



\*\*Endpoint:\*\*

```

GET /.well-known/openid-configuration

```



\*\*Response:\*\*

```json

{

&nbsp; "issuer": "https://sso.example.com",

&nbsp; "authorization\_endpoint": "https://sso.example.com/authorize",

&nbsp; "token\_endpoint": "https://sso.example.com/token",

&nbsp; "userinfo\_endpoint": "https://sso.example.com/userinfo",

&nbsp; "jwks\_uri": "https://sso.example.com/.well-known/jwks.json",

&nbsp; "response\_types\_supported": \["code"],

&nbsp; "subject\_types\_supported": \["public"],

&nbsp; "id\_token\_signing\_alg\_values\_supported": \["RS256"],

&nbsp; "scopes\_supported": \["openid", "profile", "email"],

&nbsp; "token\_endpoint\_auth\_methods\_supported": \["client\_secret\_basic", "client\_secret\_post"],

&nbsp; "claims\_supported": \["sub", "email", "email\_verified", "name"]

}

```



\*\*Configuração:\*\*

\- Valores dinâmicos baseados na configuração do SSO

\- Cache para performance

\- CORS habilitado



\#### Ferramentas e Tecnologias



\*\*Configuração:\*\*

\- ASP.NET Core Configuration

\- IOptions pattern



\#### Critérios de Aceitação



\- \[ ] Endpoint /.well-known/openid-configuration implementado

\- \[ ] Todos os campos obrigatórios presentes

\- \[ ] URLs corretas e acessíveis

\- \[ ] Cache configurado

\- \[ ] CORS habilitado

\- \[ ] Documentação de uso



\#### Testes



\*\*Testes de Integração:\*\*

\- Endpoint retorna JSON válido

\- Todas as URLs funcionais

\- Conformidade com spec OpenID



\*\*Testes de Validação:\*\*

\- Validação com ferramentas OpenID Connect



\#### Riscos



\- \*\*Baixo:\*\* Implementação simples e padronizada



---



\### PBI-012: Sistema de Roles (RBAC)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-003



\#### Descrição



Implementar o sistema de Roles (RBAC - Role-Based Access Control) permitindo criação, edição e atribuição de roles a usuários.



\#### Análise Técnica



\*\*Funcionalidades:\*\*



1\. \*\*Gerenciamento de Roles:\*\*

&nbsp;  - Criar role (nome, descrição)

&nbsp;  - Editar role

&nbsp;  - Listar roles

&nbsp;  - Deletar role (se não estiver em uso)



2\. \*\*Associação Permissions → Roles:\*\*

&nbsp;  - Adicionar permission a role

&nbsp;  - Remover permission de role

&nbsp;  - Listar permissions de uma role



3\. \*\*Atribuição Roles → Users:\*\*

&nbsp;  - Atribuir role a usuário

&nbsp;  - Remover role de usuário

&nbsp;  - Listar roles de um usuário



\*\*Regras de Negócio:\*\*

\- Role pode ter múltiplas permissions

\- Usuário pode ter múltiplas roles

\- Permissions são herdadas das roles

\- Roles não podem ser deletadas se em uso



\*\*API Endpoints:\*\*

```

POST   /api/roles

GET    /api/roles

GET    /api/roles/{id}

PUT    /api/roles/{id}

DELETE /api/roles/{id}

POST   /api/roles/{id}/permissions

DELETE /api/roles/{id}/permissions/{permissionId}

```



\#### Ferramentas e Tecnologias



\*\*Validação:\*\*

\- FluentValidation



\*\*Autorização:\*\*

\- Custom Authorization Policies



\#### Critérios de Aceitação



\- \[ ] CRUD de Roles implementado

\- \[ ] Associação Permission-Role funcional

\- \[ ] Atribuição User-Role funcional

\- \[ ] Validações de negócio (role em uso não pode ser deletada)

\- \[ ] Endpoints de API criados

\- \[ ] Documentação de API (Swagger)

\- \[ ] Testes unitários (>80% cobertura)



\#### Testes



\*\*Testes Unitários:\*\*

\- Validações de domínio

\- Regras de negócio



\*\*Testes de Integração:\*\*

\- CRUD completo de roles

\- Associações funcionando

\- Restrições de deleção



\#### Riscos



\- \*\*Baixo:\*\* Padrão bem estabelecido

\- Complexidade em hierarquias de roles (não está no escopo)



---



\### PBI-013: Sistema de Permissions



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-012



\#### Descrição



Implementar o sistema de Permissions granulares, permitindo controle fino de acesso e atribuição direta a usuários além das roles.



\#### Análise Técnica



\*\*Funcionalidades:\*\*



1\. \*\*Gerenciamento de Permissions:\*\*

&nbsp;  - Criar permission (nome, descrição)

&nbsp;  - Editar permission

&nbsp;  - Listar permissions

&nbsp;  - Deletar permission (se não estiver em uso)



2\. \*\*Atribuição Direta a Usuários:\*\*

&nbsp;  - Atribuir permission a usuário (além das roles)

&nbsp;  - Remover permission de usuário

&nbsp;  - Listar permissions diretas de um usuário



3\. \*\*Cálculo de Permissions Efetivas:\*\*

&nbsp;  - União de permissions das roles

&nbsp;  - União de permissions diretas

&nbsp;  - Sem duplicatas



4\. \*\*Além do gerenciamento global de permissions, o sistema deve permitir:\*\*

&nbsp;  - Associar permissions a clients

&nbsp;  - Listar permissions habilitadas por client



\*\*Convenção de Nomenclatura:\*\*

```

{recurso}.{ação}



Exemplos:

\- users.read

\- users.create

\- users.update

\- users.delete

\- orders.read

\- orders.approve

\- reports.generate

```



\*\*Inclusão no Access Token:\*\*

\- Permissions consolidadas (roles + diretas)

\- Claim "permissions" como array



\*\*API Endpoints:\*\*

```

POST   /api/permissions

GET    /api/permissions

GET    /api/permissions/{id}

PUT    /api/permissions/{id}

DELETE /api/permissions/{id}

POST   /api/users/{userId}/permissions

DELETE /api/users/{userId}/permissions/{permissionId}

GET    /api/users/{userId}/effective-permissions

POST /api/clients/{clientId}/permissions

DELETE /api/clients/{clientId}/permissions/{permissionId}

GET /api/clients/{clientId}/permissions

```



\#### Ferramentas e Tecnologias



\*\*Validação:\*\*

\- FluentValidation (validar formato nome)



\*\*Cache:\*\*

\- In-memory cache para permissions efetivas



\#### Critérios de Aceitação



\- \[ ] CRUD de Permissions implementado

\- \[ ] Atribuição direta User-Permission funcional

\- \[ ] Cálculo de permissions efetivas implementado

\- \[ ] Permissions incluídas no Access Token

\- \[ ] Convenção de nomenclatura validada

\- \[ ] Cache de permissions efetivas

\- \[ ] API endpoints criados

\- \[ ] Documentação Swagger



\#### Testes



\*\*Testes Unitários:\*\*

\- Cálculo de permissions efetivas

\- Validações de formato



\*\*Testes de Integração:\*\*

\- CRUD completo

\- Permissions no Access Token corretas

\- Cache funcionando



\#### Riscos



\- \*\*Médio:\*\* Performance do cálculo de permissions pode ser gargalo

\- Cache invalidation precisa ser bem implementado



---



\### PBI-014: Sistema de Consentimento de Identidade



\*\*Prioridade:\*\* Média  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-005



\#### Descrição



Implementar o sistema de consentimento do usuário para compartilhamento de identidade com sistemas clientes, conforme padrão OpenID Connect.



\#### Análise Técnica



\*\*Fluxo de Consentimento:\*\*



1\. Usuário autenticado no /authorize

2\. SSO verifica se já existe consentimento (UserConsents)

3\. Se não existe:

&nbsp;  - Exibir tela de consentimento

&nbsp;  - Usuário aprova ou rejeita

&nbsp;  - Registrar consentimento no banco

4\. Se existe:

&nbsp;  - Prosseguir com geração do authorization code



\*\*Tela de Consentimento:\*\*



Informações exibidas:

\- Nome do sistema cliente

\- Scopes solicitados (traduzidos para linguagem amigável)

\- Informações que serão compartilhadas

\- Botões: "Permitir" e "Cancelar"



Exemplo de tradução de scopes:

\- `openid` → "Autenticação"

\- `profile` → "Nome e informações básicas"

\- `email` → "Endereço de e-mail"



\*\*Persistência:\*\*

\- Consentimento salvo na tabela UserConsents

\- Válido indefinidamente (ou até revogação manual)

\- Vinculado a User e Client



\*\*Revogação (implementação futura):\*\*

\- Usuário pode revogar consentimento

\- Sistemas clientes devem solicitar novo consentimento



\#### Ferramentas e Tecnologias



\*\*UI:\*\*

\- Razor Pages ou MVC Views

\- Tailwind CSS ou Bootstrap



\*\*Validação:\*\*

\- CSRF protection (anti-forgery tokens)



\#### Critérios de Aceitação



\- \[ ] Tela de consentimento implementada

\- \[ ] Fluxo integrado ao /authorize

\- \[ ] Consentimento salvo no banco (UserConsents)

\- \[ ] Consentimento verificado antes de gerar code

\- \[ ] Scopes traduzidos para linguagem amigável

\- \[ ] Botões "Permitir" e "Cancelar" funcionais

\- \[ ] Cancelamento retorna erro ao client

\- \[ ] CSRF protection implementado

\- \[ ] Design responsivo



\#### Testes



\*\*Testes de Integração:\*\*

\- Primeiro acesso exibe consentimento

\- Acesso subsequente não exige novo consentimento

\- Cancelamento retorna erro correto

\- Consentimento salvo no banco



\*\*Testes de UX:\*\*

\- Tela de consentimento clara e compreensível

\- Responsividade em mobile



\#### Riscos



\- \*\*Baixo:\*\* Funcionalidade padrão do OpenID Connect

\- UX ruim pode confundir usuários



---



\### PBI-015: UI de Login e Autenticação



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* PBI-004, PBI-014



\#### Descrição



Desenvolver a interface de usuário completa para login, incluindo tela de login, recuperação de senha e integração com o fluxo de autenticação.



\#### Análise Técnica



\*\*Telas:\*\*



1\. \*\*Login:\*\*

&nbsp;  - Campos: Email, Senha

&nbsp;  - Checkbox: "Lembrar-me"

&nbsp;  - Link: "Esqueci minha senha"

&nbsp;  - Botão: "Entrar"

&nbsp;  - Mensagens de erro inline

&nbsp;  - Indicador de loading



2\. \*\*Recuperação de Senha:\*\*

&nbsp;  - Campo: Email

&nbsp;  - Botão: "Enviar link de recuperação"

&nbsp;  - Mensagem de confirmação (genérica)



3\. \*\*Redefinição de Senha:\*\*

&nbsp;  - Campos: Nova senha, Confirmar senha

&nbsp;  - Validação de força de senha

&nbsp;  - Botão: "Redefinir senha"



4\. \*\*Erro de Autenticação:\*\*

&nbsp;  - Mensagem genérica de erro

&nbsp;  - Link para tentar novamente

&nbsp;  - Link para recuperação de senha



\*\*Funcionalidades:\*\*



\- Validação client-side (HTML5 + JavaScript)

\- Validação server-side

\- Rate limiting visual (mostrar bloqueio temporário)

\- Redirect após login para URL original (/authorize)

\- Acessibilidade (WCAG 2.1 AA)



\*\*Design:\*\*

\- Responsivo (mobile-first)

\- Marca do SSO (logo, cores)

\- UX limpa e profissional



\#### Ferramentas e Tecnologias



\*\*Frontend:\*\*

\- Razor Pages ou Blazor Server

\- Tailwind CSS ou Bootstrap 5

\- JavaScript vanilla ou Alpine.js (leve)



\*\*Validação:\*\*

\- FluentValidation (server-side)

\- HTML5 validation (client-side)



\*\*Email:\*\*

\- MailKit ou SendGrid (recuperação de senha)



\#### Critérios de Aceitação



\- \[ ] Tela de login funcional e integrada

\- \[ ] Validações client-side e server-side

\- \[ ] Mensagens de erro claras (genéricas)

\- \[ ] Recuperação de senha funcional

\- \[ ] Email de recuperação enviado

\- \[ ] Redefinição de senha com token seguro

\- \[ ] Design responsivo (mobile, tablet, desktop)

\- \[ ] Acessibilidade WCAG 2.1 AA

\- \[ ] Rate limiting visível para usuário

\- \[ ] Loading indicators

\- \[ ] Redirect para /authorize após login



\#### Testes



\*\*Testes de Integração:\*\*

\- Fluxo completo de login

\- Recuperação de senha end-to-end

\- Validações funcionando



\*\*Testes de UX:\*\*

\- Usabilidade em diferentes dispositivos

\- Mensagens de erro compreensíveis



\*\*Testes de Acessibilidade:\*\*

\- Validação com ferramentas (axe, WAVE)

\- Navegação por teclado



\#### Riscos



\- \*\*Médio:\*\* UX ruim pode frustrar usuários

\- \*\*Baixo:\*\* Envio de emails pode ter problemas em produção



---



\### PBI-016: UI de Cadastro de Usuários (Self-Service)



\*\*Prioridade:\*\* Média  

\*\*Story Points:\*\* 5  

\*\*Dependências:\*\* PBI-015



\#### Descrição



Desenvolver a interface para cadastro de novos usuários (self-service), permitindo que usuários criem suas próprias contas no SSO.



\#### Análise Técnica



\*\*Tela de Cadastro:\*\*



Campos:

\- Nome completo

\- Email

\- Senha

\- Confirmar senha

\- Checkbox: "Aceito os termos de uso"



Validações:

\- Email único no sistema

\- Senha forte (mín. 8 caracteres, maiúscula, minúscula, número, símbolo)

\- Senhas devem coincidir

\- Termos de uso obrigatórios



\*\*Fluxo:\*\*

1\. Usuário preenche formulário

2\. Validações client-side

3\. Submit para servidor

4\. Validações server-side

5\. Criar usuário com IsActive=false

6\. Enviar email de ativação

7\. Exibir mensagem de confirmação

8\. Usuário clica no link de ativação

9\. Conta ativada (IsActive=true)



\*\*Configuração:\*\*

\- Cadastro pode ser habilitado/desabilitado via configuração

\- Se desabilitado, apenas admins podem criar usuários



\*\*Segurança:\*\*

\- CAPTCHA (opcional, configurável)

\- Rate limiting (prevenir cadastros em massa)

\- Validação de email real (verificação de MX records)



\#### Ferramentas e Tecnologias



\*\*Frontend:\*\*

\- Razor Pages ou Blazor

\- Validação de senha em tempo real (indicador de força)



\*\*Backend:\*\*

\- Token de ativação (GUID ou JWT de curta duração)

\- Email service



\*\*CAPTCHA (opcional):\*\*

\- Google reCAPTCHA v3



\#### Critérios de Aceitação



\- \[ ] Tela de cadastro funcional

\- \[ ] Validações client-side e server-side

\- \[ ] Email de ativação enviado

\- \[ ] Link de ativação funcional (token seguro)

\- \[ ] Indicador de força de senha

\- \[ ] Validação de email único

\- \[ ] Rate limiting implementado

\- \[ ] Configuração de habilitar/desabilitar cadastro

\- \[ ] CAPTCHA implementado (opcional)

\- \[ ] Design responsivo

\- \[ ] Mensagens de erro claras



\#### Testes



\*\*Testes de Integração:\*\*

\- Fluxo completo de cadastro e ativação

\- Email duplicado bloqueado

\- Token de ativação válido e expiração



\*\*Testes de Segurança:\*\*

\- Rate limiting bloqueando cadastros em massa

\- Validação de força de senha



\#### Riscos



\- \*\*Médio:\*\* Spam e contas fake se sem CAPTCHA

\- \*\*Baixo:\*\* Emails podem cair em spam



---



\### PBI-017: UI Administrativa (Admin Dashboard)



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 13  

\*\*Dependências:\*\* PBI-012, PBI-013



\#### Descrição



Desenvolver a interface administrativa completa para gerenciamento de usuários, roles, permissions, clients e auditoria.



\#### Análise Técnica



\*\*Módulos da UI Admin:\*\*



1\. \*\*Dashboard:\*\*

&nbsp;  - Estatísticas: Total de usuários, logins hoje, tokens emitidos

&nbsp;  - Gráficos: Autenticações por dia (última semana)

&nbsp;  - Alertas: Tentativas de login falhas, contas bloqueadas



2\. \*\*Gestão de Usuários:\*\*

&nbsp;  - Listar usuários (paginação, busca, filtros)

&nbsp;  - Criar usuário

&nbsp;  - Editar usuário (nome, email, ativar/desativar)

&nbsp;  - Atribuir/remover roles

&nbsp;  - Atribuir/remover permissions diretas

&nbsp;  - Resetar senha (enviar email)

&nbsp;  - Visualizar permissions efetivas



3\. \*\*Gestão de Roles:\*\*

&nbsp;  - Listar roles

&nbsp;  - Criar role

&nbsp;  - Editar role

&nbsp;  - Associar/remover permissions

&nbsp;  - Deletar role (se não em uso)

&nbsp;  - Visualizar usuários com a role



4\. \*\*Gestão de Permissions:\*\*

&nbsp;  - Listar permissions

&nbsp;  - Criar permission

&nbsp;  - Editar permission

&nbsp;  - Deletar permission (se não em uso)

&nbsp;  - Visualizar onde está sendo usada (roles/usuários)



5\. \*\*Gestão de Clients:\*\*

&nbsp;  - Listar clients

&nbsp;  - Criar client

&nbsp;  - Editar client (nome, redirect\_uris, scopes)

&nbsp;  - Gerar novo client\_secret

&nbsp;  - Ativar/desativar client

&nbsp;  - Visualizar estatísticas de uso



6\. \*\*Auditoria:\*\*

&nbsp;  - Listar logs de auditoria

&nbsp;  - Filtros: Data, usuário, evento, client

&nbsp;  - Exportar logs (CSV/JSON)



7\. \*\*Permissions do Client\*\*

&nbsp;  - Seleção de permissões habilitadas

&nbsp;  - Visualização de impacto (quais usuários usam)



\*\*Autenticação e Autorização:\*\*

\- Admin UI protegida por autenticação

\- Apenas usuários com permission "admin.access" podem acessar

\- Diferentes níveis de acesso (view, edit) por módulo



\*\*Design:\*\*

\- Layout profissional (sidebar, top nav)

\- Tabelas com paginação

\- Modais para criação/edição

\- Feedback visual (toasts, confirmações)

\- Responsivo



\#### Ferramentas e Tecnologias



\*\*Frontend:\*\*

\- Blazor Server (opção 1, mais .NET)

\- React/Vue.js (opção 2, mais flexível)



\*\*UI Framework:\*\*

\- MudBlazor (para Blazor)

\- Ant Design ou Material-UI (para React)



\*\*Gráficos:\*\*

\- Chart.js ou Recharts



\*\*Tabelas:\*\*

\- DataTables.js ou AG Grid



\#### Critérios de Aceitação



\- \[ ] Todos os módulos implementados e funcionais

\- \[ ] CRUD completo em cada módulo

\- \[ ] Paginação, busca e filtros funcionando

\- \[ ] Autenticação e autorização por permission

\- \[ ] Gráficos e estatísticas exibidos

\- \[ ] Exportação de logs funcionando

\- \[ ] Design responsivo e profissional

\- \[ ] Feedback visual (loading, sucesso, erro)

\- \[ ] Confirmações antes de ações destrutivas

\- \[ ] Validações client-side e server-side



\#### Testes



\*\*Testes de Integração:\*\*

\- CRUD de cada entidade

\- Filtros e buscas

\- Autorização (usuário sem permission bloqueado)



\*\*Testes de UX:\*\*

\- Navegação intuitiva

\- Mensagens claras



\#### Riscos



\- \*\*Alto:\*\* Complexidade alta (muitas telas)

\- \*\*Médio:\*\* Performance com muitos dados (paginação essencial)

\- Escopo pode aumentar (feature creep)



---



\### PBI-018: Sistema de Auditoria e Logs



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* PBI-003



\#### Descrição



Implementar sistema completo de auditoria para registrar eventos críticos de segurança e administração, garantindo rastreabilidade e conformidade.



\#### Análise Técnica



\*\*Eventos Auditáveis:\*\*



\*\*Autenticação:\*\*

\- LOGIN\_SUCCESS

\- LOGIN\_FAILED

\- LOGOUT

\- PASSWORD\_RESET\_REQUESTED

\- PASSWORD\_RESET\_COMPLETED

\- ACCOUNT\_LOCKED



\*\*Autorização:\*\*

\- CONSENT\_GRANTED

\- CONSENT\_REVOKED

\- TOKEN\_ISSUED

\- REFRESH\_TOKEN\_USED

\- REFRESH\_TOKEN\_REVOKED



\*\*Administração:\*\*

\- USER\_CREATED

\- USER\_UPDATED

\- USER\_DELETED

\- USER\_ACTIVATED

\- USER\_DEACTIVATED

\- ROLE\_CREATED

\- ROLE\_UPDATED

\- ROLE\_DELETED

\- PERMISSION\_CREATED

\- PERMISSION\_UPDATED

\- PERMISSION\_DELETED

\- CLIENT\_CREATED

\- CLIENT\_UPDATED

\- CLIENT\_DELETED



\*\*Estrutura do Log:\*\*

```csharp

{

&nbsp; "Id": "guid",

&nbsp; "EventType": "LOGIN\_SUCCESS",

&nbsp; "EntityType": "User",

&nbsp; "EntityId": "user-guid",

&nbsp; "UserId": "user-guid",

&nbsp; "ClientId": "client-id",

&nbsp; "Timestamp": "ISO 8601",

&nbsp; "IpAddress": "192.168.1.1",

&nbsp; "UserAgent": "Mozilla/5.0...",

&nbsp; "Data": {

&nbsp;   // Dados adicionais em JSON

&nbsp; }

}

```



\*\*Implementação:\*\*



1\. \*\*Middleware de Auditoria:\*\*

&nbsp;  - Captura IP, User-Agent automaticamente

&nbsp;  - Injeta contexto de auditoria



2\. \*\*Audit Service:\*\*

&nbsp;  - Método genérico: `LogEvent(eventType, entityType, entityId, data)`

&nbsp;  - Processamento assíncrono (não bloquear fluxo principal)



3\. \*\*Storage:\*\*

&nbsp;  - Tabela AuditLogs no banco

&nbsp;  - Append-only (imutável)

&nbsp;  - Índices em: Timestamp, UserId, ClientId, EventType



4\. \*\*Retenção:\*\*

&nbsp;  - Logs mantidos por tempo configurável (ex: 1 ano)

&nbsp;  - Processo de arquivamento/remoção automático



\#### Ferramentas e Tecnologias



\*\*Logging:\*\*

\- Serilog (structured logging)

\- NLog (alternativa)



\*\*Processamento Assíncrono:\*\*

\- Background services (IHostedService)

\- Queues (opcional: RabbitMQ, Azure Service Bus)



\*\*Análise:\*\*

\- Elasticsearch + Kibana (opcional para grandes volumes)



\#### Critérios de Aceitação



\- \[ ] Todos os eventos auditáveis registrados

\- \[ ] Middleware de auditoria implementado

\- \[ ] Audit service funcional

\- \[ ] Logs armazenados na tabela AuditLogs

\- \[ ] Processamento assíncrono (não bloqueia fluxo)

\- \[ ] IP e User-Agent capturados automaticamente

\- \[ ] Índices criados para performance

\- \[ ] UI Admin exibindo logs (ver PBI-017)

\- \[ ] Filtros e exportação funcionando

\- \[ ] Retenção configurável

\- \[ ] Dados sensíveis nunca logados (senhas, tokens completos)



\#### Testes



\*\*Testes de Integração:\*\*

\- Eventos registrados corretamente

\- Processamento assíncrono funcional

\- Busca e filtros performáticos



\*\*Testes de Performance:\*\*

\- Alto volume de logs não degrada sistema



\#### Riscos



\- \*\*Médio:\*\* Alto volume de logs pode impactar performance do banco

\- \*\*Baixo:\*\* Dados sensíveis podem vazar se mal implementado



---



\### PBI-019: Segurança Avançada e Hardening



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* PBI-004, PBI-006



\#### Descrição



Implementar medidas avançadas de segurança, incluindo proteções adicionais, headers de segurança, rate limiting avançado e monitoramento de segurança.



\#### Análise Técnica



\*\*Medidas de Segurança:\*\*



1\. \*\*Headers de Segurança:\*\*

&nbsp;  - Content-Security-Policy (CSP)

&nbsp;  - X-Content-Type-Options: nosniff

&nbsp;  - X-Frame-Options: DENY

&nbsp;  - X-XSS-Protection: 1; mode=block

&nbsp;  - Strict-Transport-Security (HSTS)

&nbsp;  - Referrer-Policy: no-referrer



2\. \*\*Rate Limiting Avançado:\*\*

&nbsp;  - Por endpoint (diferente para cada)

&nbsp;  - Por IP (global)

&nbsp;  - Por usuário (autenticado)

&nbsp;  - Por client (OAuth)

&nbsp;  - Sliding window algorithm



3\. \*\*Proteção Contra Ataques:\*\*

&nbsp;  - CSRF (anti-forgery tokens)

&nbsp;  - SQL Injection (parameterized queries)

&nbsp;  - XSS (output encoding)

&nbsp;  - Clickjacking (X-Frame-Options)

&nbsp;  - Open Redirect (validação estrita de redirect\_uri)



4\. \*\*Secrets Management:\*\*

&nbsp;  - Chaves privadas JWT em secrets manager

&nbsp;  - Client secrets hasheados

&nbsp;  - Connection strings em variáveis de ambiente

&nbsp;  - Rotação de secrets



5\. \*\*Monitoramento de Segurança:\*\*

&nbsp;  - Alertas em eventos suspeitos:

&nbsp;    \* Múltiplas tentativas de login falhas

&nbsp;    \* Tentativas de acesso não autorizado

&nbsp;    \* Padrões anômalos de requisições

&nbsp;  - Integração com SIEM (opcional)



6\. \*\*HTTPS Obrigatório:\*\*

&nbsp;  - Redirect HTTP → HTTPS

&nbsp;  - HSTS preload



\#### Ferramentas e Tecnologias



\*\*Headers:\*\*

\- NWebsec (ASP.NET Core middleware)



\*\*Rate Limiting:\*\*

\- AspNetCoreRateLimit (configuração avançada)

\- Redis (distributed cache para rate limiting)



\*\*Secrets:\*\*

\- Azure Key Vault, AWS Secrets Manager ou HashiCorp Vault

\- Environment variables



\*\*Monitoramento:\*\*

\- Application Insights ou Datadog

\- Sentry (error tracking)



\#### Critérios de Aceitação



\- \[ ] Todos os headers de segurança configurados

\- \[ ] Rate limiting configurado por endpoint

\- \[ ] CSRF protection em todas as formas

\- \[ ] Output encoding automático (XSS prevention)

\- \[ ] Secrets em secret manager ou env vars

\- \[ ] HTTPS obrigatório (redirect configurado)

\- \[ ] HSTS habilitado

\- \[ ] Alertas de segurança configurados

\- \[ ] Scan de segurança passando (OWASP ZAP ou similar)

\- \[ ] Documentação de segurança



\#### Testes



\*\*Testes de Segurança:\*\*

\- Scan com OWASP ZAP

\- Testes de penetração (pentest)

\- Validação de headers (securityheaders.com)



\*\*Testes de Rate Limiting:\*\*

\- Validar limites sendo aplicados

\- Diferentes cenários de throttling



\#### Riscos



\- \*\*Alto:\*\* Configurações incorretas podem criar vulnerabilidades

\- \*\*Médio:\*\* Rate limiting muito agressivo pode bloquear usuários legítimos



---



\### PBI-020: Documentação Completa e Deployment



\*\*Prioridade:\*\* Alta  

\*\*Story Points:\*\* 8  

\*\*Dependências:\*\* Todas as anteriores



\#### Descrição



Criar documentação completa do sistema (técnica e de usuário), guias de deployment e configuração para produção.



\#### Análise Técnica



\*\*Documentação a ser Criada:\*\*



1\. \*\*Documentação Técnica:\*\*

&nbsp;  - Arquitetura do sistema (diagramas)

&nbsp;  - Modelo de dados (ER diagram)

&nbsp;  - Fluxos OAuth 2.0 e OIDC (sequence diagrams)

&nbsp;  - API Reference (Swagger/OpenAPI)

&nbsp;  - Guia de desenvolvimento (setup local)

&nbsp;  - Guia de contribuição



2\. \*\*Documentação de Operações:\*\*

&nbsp;  - Guia de deployment (Docker, Kubernetes)

&nbsp;  - Configuração de ambiente (variáveis)

&nbsp;  - Backup e recuperação

&nbsp;  - Monitoramento e observabilidade

&nbsp;  - Troubleshooting

&nbsp;  - Runbooks para incidentes comuns



3\. \*\*Documentação de Integração:\*\*

&nbsp;  - Guia para desenvolvedores de clients

&nbsp;  - Exemplos de código (C#, JavaScript, Python)

&nbsp;  - Bibliotecas recomendadas

&nbsp;  - Fluxos de autenticação step-by-step

&nbsp;  - Tratamento de erros

&nbsp;  - Boas práticas



4\. \*\*Documentação de Usuário:\*\*

&nbsp;  - Guia de uso da UI Admin

&nbsp;  - Tutoriais em vídeo (opcional)

&nbsp;  - FAQ



\*\*Deployment:\*\*



1\. \*\*Containerização:\*\*

&nbsp;  - Dockerfile otimizado (multi-stage)

&nbsp;  - Docker Compose para stack completa



2\. \*\*Kubernetes:\*\*

&nbsp;  - Helm charts

&nbsp;  - Manifests (deployment, service, ingress)

&nbsp;  - ConfigMaps e Secrets

&nbsp;  - Health checks e readiness probes



3\. \*\*CI/CD:\*\*

&nbsp;  - Pipeline completo (build, test, deploy)

&nbsp;  - Ambientes: dev, staging, production

&nbsp;  - Blue-green deployment ou canary



4\. \*\*Configuração de Produção:\*\*

&nbsp;  - Checklist de segurança

&nbsp;  - Performance tuning

&nbsp;  - Scaling (horizontal e vertical)

&nbsp;  - Load balancing



\#### Ferramentas e Tecnologias



\*\*Documentação:\*\*

\- Markdown (docs gerais)

\- Mermaid (diagramas)

\- Swagger UI (API docs)

\- Docusaurus ou MkDocs (site de documentação)



\*\*Diagramas:\*\*

\- PlantUML ou Draw.io

\- Mermaid (inline em Markdown)



\*\*Deployment:\*\*

\- Docker e Docker Compose

\- Kubernetes + Helm

\- GitHub Actions ou GitLab CI



\*\*Monitoramento:\*\*

\- Prometheus + Grafana

\- Application Insights



\#### Critérios de Aceitação



\- \[ ] Documentação técnica completa

\- \[ ] Documentação de operações completa

\- \[ ] Guia de integração com exemplos de código

\- \[ ] Documentação de usuário (Admin UI)

\- \[ ] Dockerfile otimizado

\- \[ ] Helm charts para Kubernetes

\- \[ ] Pipeline CI/CD completo

\- \[ ] Ambientes dev, staging, prod configurados

\- \[ ] Health checks implementados

\- \[ ] Monitoramento configurado

\- \[ ] Checklist de produção criado

\- \[ ] Site de documentação publicado



\#### Testes



\*\*Testes de Deployment:\*\*

\- Deploy em ambiente de staging

\- Deploy em ambiente de produção (simulado)

\- Health checks funcionando

\- Rollback funcional



\*\*Testes de Documentação:\*\*

\- Seguir guias de integração (smoke test)

\- Validar exemplos de código



\#### Riscos



\- \*\*Médio:\*\* Documentação pode ficar desatualizada rapidamente

\- \*\*Baixo:\*\* Deployment complexo pode gerar problemas iniciais



---



\## 5. Resumo do Backlog



\### Distribuição por Prioridade



\- \*\*Alta:\*\* 15 PBIs

\- \*\*Média:\*\* 5 PBIs



\### Estimativa Total



\*\*Total de Story Points:\*\* 128



\*\*Estimativa de Sprints (assumindo 20-25 SP por sprint):\*\*

\- 5-7 sprints (10-14 semanas para um time de 3-5 devs)



\### Roadmap Visual



```

Sprint 1 (PBIs 1-4):   Fundação + Autenticação Básica

Sprint 2 (PBIs 5-8):   OAuth 2.0 Core

Sprint 3 (PBIs 9-11):  OpenID Connect

Sprint 4 (PBIs 12-14): Autorização + Consentimento

Sprint 5 (PBIs 15-17): UIs (Login, Cadastro, Admin)

Sprint 6 (PBIs 18-20): Segurança + Documentação + Deploy

```



---



\## 6. Próximos Passos



1\. \*\*Refinamento do Backlog:\*\* Revisar PBIs com time técnico

2\. \*\*Priorização Final:\*\* Ajustar prioridades conforme necessidades de negócio

3\. \*\*Definição de DoR/DoD:\*\* Definition of Ready e Definition of Done

4\. \*\*Setup do Projeto:\*\* Executar PBI-001

5\. \*\*Iniciar Sprint 1:\*\* Começar desenvolvimento



---



\## 7. Notas e Considerações



\### Dependências Externas

\- Servidor de email (para recuperação de senha e ativação)

\- Secret manager (Azure Key Vault, AWS Secrets, etc.)

\- Ferramentas de monitoramento



\### Decisões Arquiteturais a Serem Tomadas

\- Escolha de banco de dados (PostgreSQL vs SQL Server)

\- Frontend da Admin UI (Blazor vs React/Vue)

\- Infraestrutura de deployment (Cloud vs On-premise)



\### Riscos Globais do Projeto

\- \*\*Alto:\*\* Segurança é crítica - vulnerabilidades podem ser catastróficas

\- \*\*Médio:\*\* Performance sob alta carga (scaling horizontal essencial)

\- \*\*Médio:\*\* Complexidade de integração com múltiplos clients



\### Recomendações

1\. Priorizar segurança desde o início (não deixar para depois)

2\. Implementar testes automatizados desde a primeira PBI

3\. Realizar code reviews rigorosos

4\. Considerar pentest ao final do desenvolvimento

5\. Documentar decisões arquiteturais (ADRs - Architecture Decision Records)



---



\*\*Fim do Documento\*\*



