# Product Backlog – Sistema de Single Sign-On (SSO)
## Abordagem por Vertical Slices

## Informações do Documento

**Versão:** 2.0  
**Data:** 19/03/2026  
**Projeto:** Sistema de Single Sign-On (SSO)  
**Metodologia:** Vertical Slices - Entregas incrementais de valor end-to-end

---

## Índice

1. [Visão Geral da Abordagem](#1-visão-geral-da-abordagem)
2. [Estrutura das Slices](#2-estrutura-das-slices)
3. [Roadmap de Entregas](#3-roadmap-de-entregas)
4. [Vertical Slices](#4-vertical-slices)

---

## 1. Visão Geral da Abordagem

Este backlog foi reorganizado usando **Vertical Slices** ao invés de camadas horizontais. Cada slice entrega uma funcionalidade completa e utilizável, atravessando todas as camadas da arquitetura.

### 1.1 Princípios de Vertical Slices

- **Valor end-to-end**: Cada slice entrega funcionalidade completa e testável
- **Independência**: Slices podem ser desenvolvidas em paralelo quando possível
- **Feedback rápido**: Funcionalidades podem ser validadas imediatamente
- **Redução de risco**: Problemas são descobertos cedo no ciclo

### 1.2 Diferença da Abordagem Anterior

**Antes (Horizontal):**
```
Sprint 1: Toda infraestrutura
Sprint 2: Todo banco de dados
Sprint 3: Todas entidades
Sprint 4: Todos repositories
```

**Agora (Vertical):**
```
Slice 1: Login básico funcionando (infra + DB + entidades + API + testes)
Slice 2: OAuth flow mínimo funcionando (tudo necessário para um client se autenticar)
Slice 3: Gestão de usuários funcionando (CRUD completo com UI)
```

---

## 2. Estrutura das Slices

Cada slice contém:

- **ID e Título**
- **Valor de Negócio**: O que o usuário/sistema ganha
- **Descrição**: Funcionalidade entregue
- **Story Points**: Estimativa de esforço
- **Dependências**: Slices predecessoras
- **Escopo Técnico**: O que será implementado em cada camada
- **Critérios de Aceitação**: Condições para conclusão
- **Demo**: Como demonstrar a funcionalidade
- **Riscos**: Pontos de atenção

---

## 3. Roadmap de Entregas

### Fase 1 - MVP Funcional (Slices 1-4)
SSO básico funcionando com OAuth 2.0 para um client web

### Fase 2 - Suporte Mobile (Slices 5-6)
PKCE e suporte completo para apps mobile

### Fase 3 - Autorização Avançada (Slices 7-9)
RBAC, permissions e consentimento

### Fase 4 - Gestão e Operação (Slices 10-12)
UIs administrativas e auditoria

---

## 4. Vertical Slices

---

### SLICE-001: Autenticação Básica End-to-End

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** Nenhuma

#### Valor de Negócio

Usuários podem fazer login no sistema com email/senha através de uma API REST segura, com sessão persistente e proteção contra ataques.

#### Descrição

Implementar o fluxo completo de autenticação básica, desde a infraestrutura até a API funcional, incluindo banco de dados, entidades, serviços e testes.


#### Escopo Técnico

**Infraestrutura:**
- Setup do projeto (.NET 10, Clean Architecture)
- Docker Compose (PostgreSQL + API)
- CI/CD básico (GitHub Actions)

**Banco de Dados:**
- Tabela Users (Id, Email, PasswordHash, Name, IsActive, CreatedAt)
- Migrations com EF Core
- Seeds iniciais

**Domain Layer:**
- Entidade User com comportamentos (SetPassword, Activate)
- Value Object Email
- Interface IUserRepository
- Interface IPasswordHasher

**Application Layer:**
- AuthenticationService
- DTO LoginRequest
- Validações com FluentValidation

**Infrastructure Layer:**
- UserRepository (EF Core)
- PasswordHasher (Argon2id ou BCrypt)
- DbContext configurado

**API Layer:**
- AuthController com endpoints:
  - POST /api/auth/login
  - POST /api/auth/logout
  - GET /api/auth/me
- Cookie authentication configurado
- Rate limiting (5 tentativas/min)
- CORS configurado

**Segurança:**
- Password hashing seguro
- HttpOnly, Secure, SameSite cookies
- Account lockout (5 tentativas)
- Mensagens de erro genéricas

**Testes:**
- Testes unitários (domain, application)
- Testes de integração (API completa)
- Cobertura mínima: 80%

#### Critérios de Aceitação

- [ ] Projeto compila e roda via Docker Compose
- [ ] Usuário pode fazer login com credenciais válidas
- [ ] Login retorna cookie de sessão seguro
- [ ] Endpoint /me retorna dados do usuário autenticado
- [ ] Logout invalida sessão
- [ ] Credenciais inválidas retornam 401
- [ ] Rate limiting bloqueia após 5 tentativas
- [ ] Account lockout funciona (15 min)
- [ ] Senhas hasheadas com Argon2id/BCrypt
- [ ] Testes passando (unitários + integração)
- [ ] CI/CD executando build e testes

#### Demo

```bash
# 1. Subir ambiente
docker-compose up

# 2. Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin@123"}' \
  -c cookies.txt

# 3. Acessar recurso protegido
curl http://localhost:5000/api/auth/me \
  -b cookies.txt

# 4. Logout
curl -X POST http://localhost:5000/api/auth/logout \
  -b cookies.txt
```

#### Riscos

- **Médio:** Configuração de Docker pode ter problemas em diferentes ambientes
- **Alto:** Vulnerabilidades de segurança se implementação incorreta

---

### SLICE-002: OAuth 2.0 Authorization Code Flow Completo

**Prioridade:** Alta  
**Story Points:** 21  
**Dependências:** SLICE-001

#### Valor de Negócio

Sistemas clientes podem integrar-se ao SSO usando o padrão OAuth 2.0, permitindo que usuários façam login uma vez e acessem múltiplas aplicações sem re-autenticar.

#### Descrição

Implementar o fluxo completo OAuth 2.0 Authorization Code Flow, incluindo endpoints /authorize e /token, geração de JWT, e toda infraestrutura necessária para um client web se autenticar.

#### Escopo Técnico

**Banco de Dados:**
- Tabela Clients (Id, Name, ClientId, ClientSecret, RedirectUris, IsActive)
- Tabela Scopes (Id, Name, Description)
- Tabela ClientScopes (ClientId, ScopeId)
- Tabela AuthorizationCodes (Code, UserId, ClientId, RedirectUri, ExpiresAt, IsUsed)
- Tabela RefreshTokens (Id, Token, UserId, ClientId, ExpiresAt, IsRevoked)
- Migrations e seeds (client padrão, scopes: openid, profile, email)

**Domain Layer:**
- Entidade Client com validações (ValidateRedirectUri, AddScope)
- Entidade AuthorizationCode (MarkAsUsed, IsExpired)
- Entidade RefreshToken (Rotate, Revoke)
- Entidade Scope
- Interfaces de repositórios

**Application Layer:**
- OAuthService (GenerateAuthorizationCode, ExchangeCodeForTokens)
- TokenService (GenerateAccessToken, GenerateIdToken, GenerateRefreshToken)
- DTOs (AuthorizeRequest, TokenRequest, TokenResponse)
- Validações OAuth

**Infrastructure Layer:**
- Repositories (Client, AuthorizationCode, RefreshToken, Scope)
- JwtTokenGenerator (RS256 com chaves assimétricas)
- KeyManagementService (geração e armazenamento de chaves)

**API Layer:**
- OAuthController:
  - GET /oauth/authorize
  - POST /oauth/token
  - GET /.well-known/jwks.json
  - GET /.well-known/openid-configuration
- Validações de OAuth (client_id, redirect_uri, scopes)
- Geração de authorization code (10 min expiração)
- Troca de code por tokens

**Tokens:**
- Access Token (JWT, 15 min, contém: sub, email, roles, permissions)
- ID Token (JWT, 15 min, contém: sub, email, name, email_verified)
- Refresh Token (opaco, 30 dias, armazenado no DB)

**Segurança:**
- Client authentication (client_secret)
- Redirect URI exact match
- State parameter (CSRF protection)
- Code uso único
- Tokens assinados com RS256

**Testes:**
- Testes unitários (geração de tokens, validações)
- Testes de integração (fluxo completo OAuth)
- Testes de segurança (redirect URI manipulation, code reuse)

#### Critérios de Aceitação

- [ ] Client pode ser registrado no banco
- [ ] Endpoint /authorize valida client_id e redirect_uri
- [ ] /authorize redireciona para login se não autenticado
- [ ] /authorize gera authorization code válido
- [ ] /authorize redireciona com code e state
- [ ] Endpoint /token valida code, client_secret, redirect_uri
- [ ] /token retorna access_token, id_token, refresh_token
- [ ] Access Token é JWT válido com claims corretas
- [ ] ID Token é JWT válido com claims de identidade
- [ ] Refresh Token pode renovar access_token
- [ ] Authorization code só pode ser usado uma vez
- [ ] Code expira em 10 minutos
- [ ] JWKS endpoint publica chaves públicas
- [ ] Discovery endpoint retorna configuração OpenID
- [ ] Testes de integração cobrem fluxo completo

#### Demo

```bash
# 1. Registrar client (via seed ou API futura)
# Client: web-app
# Secret: secret123
# Redirect: http://localhost:3000/callback

# 2. Iniciar fluxo OAuth (browser)
http://localhost:5000/oauth/authorize?
  client_id=web-app
  &redirect_uri=http://localhost:3000/callback
  &response_type=code
  &scope=openid profile email
  &state=xyz123

# 3. Usuário faz login (se não autenticado)
# 4. Redirect de volta com code
http://localhost:3000/callback?code=ABC123&state=xyz123

# 5. Client troca code por tokens
curl -X POST http://localhost:5000/oauth/token \
  -H "Content-Type: application/json" \
  -d '{
    "grant_type": "authorization_code",
    "code": "ABC123",
    "client_id": "web-app",
    "client_secret": "secret123",
    "redirect_uri": "http://localhost:3000/callback"
  }'

# Resposta:
{
  "access_token": "eyJhbGc...",
  "id_token": "eyJhbGc...",
  "refresh_token": "opaque_token",
  "token_type": "Bearer",
  "expires_in": 900
}

# 6. Validar JWT
curl http://localhost:5000/.well-known/jwks.json

# 7. Usar access_token
curl http://localhost:5000/api/protected \
  -H "Authorization: Bearer eyJhbGc..."

# 8. Renovar com refresh_token
curl -X POST http://localhost:5000/oauth/token \
  -H "Content-Type: application/json" \
  -d '{
    "grant_type": "refresh_token",
    "refresh_token": "opaque_token",
    "client_id": "web-app",
    "client_secret": "secret123"
  }'
```

#### Riscos

- **Alto:** Implementação incorreta de JWT pode gerar vulnerabilidades críticas
- **Alto:** Gerenciamento de chaves privadas requer cuidado especial
- **Médio:** Complexidade alta - muitos componentes interconectados

---

### SLICE-003: PKCE e Suporte para Apps Mobile/SPA

**Prioridade:** Alta  
**Story Points:** 8  
**Dependências:** SLICE-002

#### Valor de Negócio

Apps mobile nativos (iOS/Android) e SPAs (React/Vue/Angular) podem se autenticar de forma segura sem precisar armazenar client_secret, usando PKCE.

#### Descrição

Adicionar suporte obrigatório a PKCE para proteção contra interceptação de authorization code, essencial para aplicações públicas.

#### Escopo Técnico

**Banco de Dados:**
- Adicionar colunas em AuthorizationCodes: CodeChallenge, CodeChallengeMethod

**Domain/Application/Infrastructure:**
- PkceValidator (valida code_challenge e code_verifier)
- Atualizar OAuthService para suportar PKCE
- SHA256 hashing para S256 method

**API Layer:**
- Atualizar /authorize para aceitar code_challenge
- Atualizar /token para validar code_verifier
- PKCE obrigatório para public clients

**Configuração:**
- ClientType (confidential vs public)
- Public clients: PKCE obrigatório, sem client_secret

**Documentação:**
- Guias para mobile (iOS/Android)
- Guias para SPAs
- Exemplos de código

#### Critérios de Aceitação

- [ ] Client configurável como public ou confidential
- [ ] /authorize aceita code_challenge e code_challenge_method
- [ ] /token valida code_verifier
- [ ] Métodos S256 e plain funcionam
- [ ] PKCE obrigatório para public clients
- [ ] Documentação mobile completa
- [ ] Exemplos funcionais

#### Demo

```bash
# App gera code_verifier e code_challenge
code_verifier="dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk"
code_challenge=$(echo -n $code_verifier | sha256sum | xxd -r -p | base64 | tr -d '=' | tr '/+' '_-')

# Fluxo OAuth com PKCE
http://localhost:5000/oauth/authorize?
  client_id=mobile-app
  &redirect_uri=myapp://callback
  &response_type=code
  &code_challenge=$code_challenge
  &code_challenge_method=S256

# Troca code por tokens (SEM client_secret)
curl -X POST http://localhost:5000/oauth/token \
  -d '{
    "grant_type": "authorization_code",
    "code": "ABC123",
    "client_id": "mobile-app",
    "redirect_uri": "myapp://callback",
    "code_verifier": "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk"
  }'
```

#### Riscos

- **Médio:** Clients existentes precisarão adaptar
- **Baixo:** Padrão bem documentado (RFC 7636)

---

### SLICE-004: Sistema de Roles e Permissions com API

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** SLICE-002

#### Valor de Negócio

Administradores podem gerenciar roles e permissions via API REST. Access tokens incluem roles e permissions do usuário.

#### Descrição

Implementar RBAC completo com API para gerenciamento. Tokens passam a incluir roles e permissions calculadas.

#### Escopo Técnico

**Banco de Dados:**
- Tabelas: Roles, Permissions, RolePermissions, UserRoles, UserPermissions, ClientPermissions

**Domain/Application:**
- Entidades Role, Permission
- RoleService, PermissionService, UserRoleService
- PermissionCalculator (roles + diretas ∩ client)

**API Layer:**
- RolesController (CRUD + associar permissions)
- PermissionsController (CRUD)
- UsersController (atribuir roles/permissions, ver efetivas)
- ClientsController (gerenciar permissions do client)

**Tokens:**
- Access Token inclui: "roles": [...], "permissions": [...]
- Permissions calculadas: (user_roles + user_direct) ∩ client_permissions

**Autorização:**
- Policy-based authorization
- Apenas "admin.access" pode gerenciar

#### Critérios de Aceitação

- [ ] CRUD de Roles via API
- [ ] CRUD de Permissions via API
- [ ] Associação Role-Permission
- [ ] Atribuição User-Role
- [ ] Atribuição User-Permission direta
- [ ] Associação Client-Permission
- [ ] Cálculo de permissions efetivas correto
- [ ] Access Token inclui roles e permissions
- [ ] Apenas admins acessam endpoints
- [ ] Documentação Swagger completa

#### Demo

```bash
# Criar role
curl -X POST http://localhost:5000/api/roles \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -d '{"name":"editor","description":"Content Editor"}'

# Criar permission
curl -X POST http://localhost:5000/api/permissions \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -d '{"name":"posts.create","description":"Create posts"}'

# Associar permission à role
curl -X POST http://localhost:5000/api/roles/editor/permissions \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -d '{"permissionId":"posts.create"}'

# Atribuir role a usuário
curl -X POST http://localhost:5000/api/users/user-123/roles \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -d '{"roleId":"editor"}'

# Token contém:
{
  "sub": "user-123",
  "roles": ["editor"],
  "permissions": ["posts.create"]
}
```

#### Riscos

- **Médio:** Performance do cálculo pode ser gargalo
- **Médio:** Cache invalidation precisa ser bem implementado

---

### SLICE-005: Sistema de Consentimento de Usuário

**Prioridade:** Média  
**Story Points:** 8  
**Dependências:** SLICE-002

#### Valor de Negócio

Usuários visualizam e controlam quais informações compartilham com cada aplicação, aumentando transparência e conformidade LGPD/GDPR.

#### Descrição

Implementar tela de consentimento que aparece no primeiro acesso, mostrando dados que serão compartilhados.

#### Escopo Técnico

**Banco de Dados:**
- Tabela UserConsents (Id, UserId, ClientId, GrantedScopes, GrantedAt)

**Application:**
- ConsentService (CheckConsent, GrantConsent, RevokeConsent)

**UI (Razor Pages):**
- Página /consent
- Exibe: nome do client, scopes traduzidos, informações compartilhadas
- Botões: "Permitir" e "Cancelar"
- Design responsivo (Tailwind CSS)
- CSRF protection

**Fluxo:**
- /authorize verifica consentimento
- Se não existe → /consent
- Se existe → gera code
- Cancelamento retorna erro

**API:**
- GET /api/users/me/consents (listar)
- DELETE /api/users/me/consents/{clientId} (revogar)

#### Critérios de Aceitação

- [ ] Tela de consentimento responsiva
- [ ] Scopes traduzidos
- [ ] "Permitir" salva e prossegue
- [ ] "Cancelar" retorna erro
- [ ] /authorize verifica consentimento
- [ ] Usuário pode listar consentimentos
- [ ] Usuário pode revogar
- [ ] CSRF protection

#### Demo

```bash
# Primeiro acesso → Tela de consentimento
# -----------------------------------
# Blog App gostaria de acessar:
# ✓ Sua identidade (login)
# ✓ Seu nome e foto de perfil
# ✓ Seu endereço de email
#
# [Cancelar] [Permitir]
# -----------------------------------

# Próximo acesso → Pula consentimento

# Listar consentimentos
curl http://localhost:5000/api/users/me/consents \
  -H "Authorization: Bearer $TOKEN"

# Revogar
curl -X DELETE http://localhost:5000/api/users/me/consents/blog-app \
  -H "Authorization: Bearer $TOKEN"
```

#### Riscos

- **Baixo:** Funcionalidade padrão OIDC
- **Médio:** UX ruim pode confundir usuários

---

### SLICE-006: UI de Login e Cadastro de Usuários

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** SLICE-001, SLICE-005

#### Valor de Negócio

Usuários podem fazer login e criar contas através de interface web moderna e responsiva.

#### Descrição

Desenvolver interface completa de login, cadastro, recuperação e redefinição de senha.

#### Escopo Técnico

**Banco de Dados:**
- Adicionar em Users: EmailVerified, PasswordResetToken, PasswordResetExpires, ActivationToken, ActivationExpires

**Application:**
- UserRegistrationService
- PasswordResetService
- EmailService (SMTP)

**UI (Razor Pages):**
1. Login (/login)
2. Cadastro (/register)
3. Recuperação (/forgot-password)
4. Redefinição (/reset-password?token=...)
5. Ativação (/activate?token=...)

**Design:**
- Tailwind CSS
- Responsivo (mobile-first)
- Acessibilidade (WCAG 2.1 AA)
- Indicador de força de senha

**Segurança:**
- CSRF protection
- Rate limiting
- Tokens com expiração (24h)

**Integração OAuth:**
- /login aceita ?returnUrl=/authorize?...

#### Critérios de Aceitação

- [ ] Todas as telas funcionais e responsivas
- [ ] Validações client-side e server-side
- [ ] Indicador de força de senha
- [ ] Emails enviados
- [ ] Tokens com expiração
- [ ] Rate limiting
- [ ] CSRF protection
- [ ] Acessibilidade WCAG 2.1 AA
- [ ] Integração com OAuth (?returnUrl)

#### Demo

```bash
# 1. Cadastro
http://localhost:5000/register
Nome: João Silva
Email: joao@example.com
Senha: Senha@123

# 2. Email de ativação enviado
# 3. Clicar no link
http://localhost:5000/activate?token=abc123

# 4. Login
http://localhost:5000/login
Email: joao@example.com
Senha: Senha@123

# 5. Recuperação de senha
http://localhost:5000/forgot-password
Email: joao@example.com

# 6. Email com link de reset
http://localhost:5000/reset-password?token=xyz789
Nova senha: NovaSenha@456
```

#### Riscos

- **Médio:** Emails podem cair em spam
- **Médio:** UX ruim pode frustrar usuários

---

### SLICE-007: UI Administrativa - Gestão de Usuários

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** SLICE-004, SLICE-006

#### Valor de Negócio

Administradores gerenciam usuários através de interface web intuitiva.

#### Descrição

Desenvolver painel administrativo para gestão completa de usuários.

#### Escopo Técnico

**Application:**
- UserManagementService (CRUD, Search, Activate/Deactivate)

**API:**
- UsersController expandido (paginação, busca, filtros, CRUD, activate, deactivate, reset-password)

**UI (Blazor Server ou Razor + HTMX):**
1. Dashboard (/admin) - estatísticas
2. Lista de Usuários (/admin/users) - tabela com paginação, busca, filtros
3. Criar Usuário (modal)
4. Editar Usuário - nome, email, roles, permissions, ver efetivas

**Design:**
- Layout com sidebar
- Tabelas responsivas
- Modais para confirmações
- Toasts para feedback

**Autorização:**
- Apenas "admin.access"
- Logs de ações

#### Critérios de Aceitação

- [ ] Dashboard com estatísticas
- [ ] Lista com paginação
- [ ] Busca funcional
- [ ] Filtros funcionais
- [ ] Criar usuário
- [ ] Editar usuário
- [ ] Desativar/ativar
- [ ] Atribuir/remover roles
- [ ] Atribuir/remover permissions
- [ ] Ver permissions efetivas
- [ ] Resetar senha
- [ ] Soft delete
- [ ] Apenas admins acessam
- [ ] Ações logadas
- [ ] Design responsivo

#### Demo

```bash
# 1. Dashboard
http://localhost:5000/admin
→ Total: 150 usuários
→ Ativos: 142
→ Gráfico de cadastros

# 2. Listar usuários
→ Buscar: "joão"
→ Filtrar: Role = "editor"

# 3. Criar usuário
→ Preencher formulário
→ Selecionar roles

# 4. Editar
→ Adicionar role "admin"
→ Adicionar permission "reports.view"
→ Ver permissions efetivas

# 5. Resetar senha
→ Email enviado
```

#### Riscos

- **Médio:** Complexidade de UI
- **Baixo:** Performance com muitos usuários

---

### SLICE-008: UI Administrativa - Gestão de Roles, Permissions e Clients

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** SLICE-007

#### Valor de Negócio

Administradores gerenciam roles, permissions e clients através de interface web.

#### Descrição

Expandir painel administrativo com gestão completa.

#### Escopo Técnico

**Application:**
- ClientManagementService (CRUD, GenerateSecret)

**API:**
- ClientsController expandido

**UI:**
1. Gestão de Roles (/admin/roles)
2. Gestão de Permissions (/admin/permissions)
3. Gestão de Clients (/admin/clients)
4. Navegação sidebar

**Funcionalidades Especiais:**
- Regenerar client_secret (exibe uma vez)
- Múltiplos redirect URIs
- Seleção de permissions do client

#### Critérios de Aceitação

- [ ] CRUD de Roles via UI
- [ ] CRUD de Permissions via UI
- [ ] CRUD de Clients via UI
- [ ] Associar permissions a roles
- [ ] Ver usuários com role
- [ ] Ver onde permission é usada
- [ ] Criar client completo
- [ ] Editar redirect URIs
- [ ] Editar scopes
- [ ] Editar permissions do client
- [ ] Regenerar secret
- [ ] Secret exibido uma vez
- [ ] Copiar para clipboard
- [ ] Ativar/desativar client
- [ ] Navegação sidebar

#### Demo

```bash
# 1. Criar role
→ "content-manager"
→ Adicionar permissions: posts.create, posts.edit

# 2. Criar permission
→ "analytics.view"
→ Ver onde é usada

# 3. Criar client
→ "mobile-app"
→ Type: public
→ Redirect: myapp://callback
→ Scopes: openid, profile, email
→ Permissions: posts.read, posts.create

# 4. Regenerar secret
→ Novo secret: "abc123xyz..."
→ Copiar
→ Aviso: "Não será exibido novamente"
```

#### Riscos

- **Médio:** Complexidade de UI
- **Baixo:** Regeneração pode quebrar integrações

---

### SLICE-009: Sistema de Auditoria Completo

**Prioridade:** Alta  
**Story Points:** 8  
**Dependências:** SLICE-008

#### Valor de Negócio

Todas as ações críticas são registradas para rastreabilidade, conformidade (LGPD/GDPR) e investigação de incidentes.

#### Descrição

Implementar sistema completo de auditoria com UI para visualização.

#### Escopo Técnico

**Banco de Dados:**
- Tabela AuditLogs (Id, EventType, EntityType, EntityId, UserId, ClientId, Timestamp, IpAddress, UserAgent, Data)
- Índices: Timestamp, UserId, ClientId, EventType

**Application:**
- AuditService (LogEvent, Search)
- Background service para processamento assíncrono

**Middleware:**
- AuditMiddleware (captura IP, User-Agent)

**Eventos:**
- Autenticação: LOGIN_SUCCESS, LOGIN_FAILED, LOGOUT, etc.
- Autorização: CONSENT_GRANTED, TOKEN_ISSUED, etc.
- Administração: USER_CREATED, ROLE_CREATED, etc.

**API:**
- AuditController (GET /api/audit/logs, export CSV/JSON)

**UI:**
- Página /admin/audit
- Filtros: data, evento, usuário, client, IP
- Exportação
- Visualização de detalhes (JSON)

**Processamento:**
- Assíncrono (não bloqueia)
- Queue in-memory ou RabbitMQ

**Retenção:**
- Configurável (padrão: 1 ano)
- Job de limpeza automática

**Segurança:**
- Dados sensíveis não logados

#### Critérios de Aceitação

- [ ] Todos eventos registrados
- [ ] Middleware captura IP/User-Agent
- [ ] Processamento assíncrono
- [ ] UI funcional
- [ ] Filtros funcionais
- [ ] Exportação CSV/JSON
- [ ] Visualização de detalhes
- [ ] Paginação
- [ ] Dados sensíveis não logados
- [ ] Job de limpeza
- [ ] Performance adequada

#### Demo

```bash
# 1. Eventos registrados automaticamente

# 2. Acessar auditoria
http://localhost:5000/admin/audit

# 3. Filtrar
→ Data: Últimos 7 dias
→ Evento: LOGIN_FAILED
→ Usuário: joao@example.com

# 4. Ver detalhes
{
  "eventType": "LOGIN_FAILED",
  "userId": "user-123",
  "timestamp": "2026-03-19T10:30:00Z",
  "ipAddress": "192.168.1.100",
  "data": {"reason": "invalid_password"}
}

# 5. Exportar
→ Download: audit_logs_2026-03-19.csv
```

#### Riscos

- **Médio:** Alto volume pode impactar performance
- **Baixo:** Dados sensíveis podem vazar se mal implementado

---

### SLICE-010: Segurança Avançada e Hardening

**Prioridade:** Alta  
**Story Points:** 8  
**Dependências:** SLICE-009

#### Valor de Negócio

Sistema protegido contra ataques comuns e em conformidade com melhores práticas de segurança (OWASP).

#### Descrição

Implementar medidas avançadas de segurança, headers, rate limiting avançado e monitoramento.

#### Escopo Técnico

**Headers de Segurança:**
- Content-Security-Policy, X-Content-Type-Options, X-Frame-Options, X-XSS-Protection, HSTS, Referrer-Policy

**Rate Limiting Avançado:**
- Por endpoint, IP, usuário, client
- Sliding window algorithm
- Redis para storage distribuído

**Proteções:**
- CSRF, SQL Injection, XSS, Clickjacking, Open Redirect

**Secrets Management:**
- Chaves JWT em Azure Key Vault / AWS Secrets / HashiCorp Vault
- Client secrets hasheados
- Connection strings em env vars
- Rotação de chaves JWT

**Monitoramento:**
- Alertas: múltiplas tentativas falhas, acessos não autorizados, padrões anômalos
- Application Insights / Datadog

**HTTPS:**
- Redirect HTTP → HTTPS
- HSTS preload

#### Critérios de Aceitação

- [ ] Headers de segurança configurados
- [ ] CSP funcional
- [ ] HSTS habilitado (prod)
- [ ] Rate limiting por endpoint
- [ ] Rate limiting com Redis
- [ ] CSRF protection
- [ ] Secrets em secret manager
- [ ] Chaves JWT em secret manager
- [ ] Client secrets hasheados
- [ ] HTTPS obrigatório
- [ ] Rotação de chaves suportada
- [ ] Alertas configurados
- [ ] Monitoramento integrado
- [ ] Scan OWASP ZAP passando
- [ ] Documentação de segurança
- [ ] Checklist de produção

#### Demo

```bash
# 1. Headers
curl -I http://localhost:5000
→ Content-Security-Policy: default-src 'self'
→ X-Frame-Options: DENY
→ Strict-Transport-Security: max-age=31536000

# 2. Rate limiting
for i in {1..10}; do curl http://localhost:5000/api/auth/login; done
→ Primeiras 5: OK
→ Restantes: 429 Too Many Requests

# 3. HTTPS redirect
curl http://localhost:5000
→ 301 → https://localhost:5000

# 4. CSRF
curl -X POST http://localhost:5000/admin/users
→ 400 Bad Request (CSRF token missing)

# 5. Scan
zap-cli quick-scan http://localhost:5000
→ 0 high, 0 medium

# 6. Monitoramento
→ Dashboard: Tentativas falhas, alertas, bloqueios
```

#### Riscos

- **Alto:** Configurações incorretas podem criar vulnerabilidades
- **Médio:** Rate limiting agressivo pode bloquear usuários legítimos

---

### SLICE-011: Documentação Completa e Guias de Integração

**Prioridade:** Alta  
**Story Points:** 8  
**Dependências:** SLICE-010

#### Valor de Negócio

Desenvolvedores podem integrar sistemas ao SSO facilmente através de documentação clara e exemplos.

#### Descrição

Criar documentação técnica completa, guias de integração e exemplos de código.

#### Escopo Técnico

**Documentação Técnica:**
1. Arquitetura (diagramas C4)
2. Modelo de dados (ER diagram)
3. Fluxos OAuth/OIDC (sequence diagrams)
4. API Reference (Swagger completo)
5. Guia de desenvolvimento

**Guias de Integração:**
1. Web Apps (ASP.NET, Node.js, Python)
2. SPAs (React, Vue, Angular)
3. Mobile (iOS, Android, React Native, Flutter)
4. APIs/Microservices

**Exemplos de Código:**
- Repositórios GitHub:
  - sso-example-webapp
  - sso-example-spa
  - sso-example-mobile-ios
  - sso-example-mobile-android

**Documentação de Operações:**
1. Deployment (Docker, Kubernetes, Azure, AWS)
2. Configuração (env vars, secrets)
3. Monitoramento
4. Backup e recuperação
5. Troubleshooting

**Site:**
- Docusaurus ou MkDocs
- Hospedado (GitHub Pages)
- Busca integrada
- Versionamento

#### Critérios de Aceitação

- [ ] Documentação técnica completa
- [ ] Diagramas criados
- [ ] Modelo de dados documentado
- [ ] Fluxos documentados
- [ ] API Reference completa
- [ ] Guias de integração (web, SPA, mobile, API)
- [ ] Exemplos funcionais (4 repos)
- [ ] Documentação de deployment
- [ ] Guia de configuração
- [ ] Guia de monitoramento
- [ ] Guia de backup
- [ ] Troubleshooting guide
- [ ] Site publicado
- [ ] Busca funcionando
- [ ] Versionamento configurado

#### Demo

```bash
# 1. Site de documentação
https://sso-docs.example.com

# 2. Navegação
→ Getting Started
→ Architecture
→ API Reference
→ Integration Guides
  → Web Apps
  → SPAs
  → Mobile Apps
→ Operations
→ Troubleshooting

# 3. Seguir guia (React SPA)
npm install @example/sso-client

const sso = new SSOClient({
  issuer: 'https://sso.example.com',
  clientId: 'my-spa',
  redirectUri: 'http://localhost:3000/callback'
});

sso.login();

# 4. Clonar exemplo
git clone https://github.com/example/sso-example-spa
npm install
npm start
→ Funciona imediatamente

# 5. Buscar
→ "refresh token"
→ Resultados relevantes
```

#### Riscos

- **Médio:** Documentação pode ficar desatualizada
- **Baixo:** Exemplos podem quebrar com updates

---

### SLICE-012: Deployment e Configuração de Produção

**Prioridade:** Alta  
**Story Points:** 13  
**Dependências:** SLICE-011

#### Valor de Negócio

Sistema pode ser implantado em produção de forma segura, escalável e monitorada.

#### Descrição

Preparar sistema para produção com containerização, Kubernetes, CI/CD completo e monitoramento.

#### Escopo Técnico

**Containerização:**
1. Dockerfile otimizado (multi-stage, Alpine, non-root, health checks)
2. Docker Compose (stack completa)

**Kubernetes:**
1. Helm Charts (API, PostgreSQL, Redis)
2. Manifests (Deployment, Service, Ingress, ConfigMaps, Secrets, HPA)
3. Health Checks (liveness, readiness, startup)

**CI/CD (GitHub Actions):**
1. Pipeline: build, testes, análise (SonarQube), scan (Trivy), Docker build/push, deploy
2. Ambientes: dev (auto), staging (auto), prod (manual)
3. Estratégias: rolling update, blue-green, canary

**Configuração de Produção:**
1. Checklist de segurança
2. Performance tuning (connection pooling, Redis cache, compressão)
3. Scaling (HPA, recursos adequados)
4. Load balancing (Ingress NGINX)

**Monitoramento:**
1. Application Insights / Datadog
2. Prometheus + Grafana
3. Alertas (CPU, erros, latência, logins falhos)

**Backup:**
- PostgreSQL automático (diário)
- Chaves JWT
- Retenção: 30 dias
- Testes mensais

#### Critérios de Aceitação

- [ ] Dockerfile otimizado
- [ ] Docker Compose funcional
- [ ] Helm charts criados
- [ ] Manifests completos
- [ ] Health checks implementados
- [ ] Pipeline CI/CD completo
- [ ] Build e testes automatizados
- [ ] Análise de código (SonarQube)
- [ ] Scan de segurança (Trivy)
- [ ] Deploy automático (staging)
- [ ] Deploy manual (prod)
- [ ] Ambientes configurados
- [ ] HPA configurado
- [ ] Ingress com HTTPS
- [ ] Secrets em secret manager
- [ ] Monitoramento configurado
- [ ] Alertas configurados
- [ ] Backup automático
- [ ] Checklist criado
- [ ] Load testing executado
- [ ] Rollback testado

#### Demo

```bash
# 1. Build Docker
docker build -t sso-api:1.0.0 .
→ Imagem: 150MB

# 2. Docker Compose
docker-compose up
→ API, PostgreSQL, Redis rodando

# 3. Deploy Kubernetes (staging)
helm install sso ./helm/sso \
  --namespace staging \
  --values values-staging.yaml

# 4. Verificar pods
kubectl get pods -n staging
→ sso-api-xxx: Running (3/3)
→ postgresql-xxx: Running
→ redis-xxx: Running

# 5. Health checks
curl https://sso-staging.example.com/health/live
→ 200 OK

curl https://sso-staging.example.com/health/ready
→ 200 OK

# 6. Auto-scaling
kubectl get hpa -n staging
→ sso-api: 3/10 replicas (30% CPU)

# 7. Load test
k6 run load-test.js
→ 1000 req/s
→ HPA escala para 8 replicas

# 8. Monitoramento
→ Grafana dashboard
→ Métricas: Requests/s, Latência, Erros

# 9. Deploy production
git tag v1.0.0
git push --tags
→ Pipeline aguarda aprovação
→ Aprovar
→ Rolling update
→ Zero downtime

# 10. Verificar
curl https://sso.example.com/health/ready
→ 200 OK

# 11. Rollback (se necessário)
helm rollback sso -n production
```

#### Riscos

- **Alto:** Problemas em produção afetam múltiplos sistemas
- **Médio:** Configuração incorreta pode causar downtime
- **Médio:** Secrets management requer atenção

---

## 5. Resumo do Backlog

### Distribuição por Fase

- **Fase 1 - MVP Funcional:** Slices 1-4 (55 SP)
- **Fase 2 - Suporte Mobile:** Slices 5-6 (21 SP)
- **Fase 3 - Autorização Avançada:** Slices 7-9 (34 SP)
- **Fase 4 - Gestão e Operação:** Slices 10-12 (29 SP)

### Estimativa Total

**Total de Story Points:** 139

**Estimativa de Tempo:**
- Time de 3-5 devs
- Velocidade: 20-25 SP por sprint (2 semanas)
- Duração: 6-7 sprints (12-14 semanas)

### Roadmap Visual

```
Sprint 1-2: SLICE-001, SLICE-002 (MVP OAuth básico)
Sprint 3:   SLICE-003, SLICE-004 (Mobile + RBAC)
Sprint 4:   SLICE-005, SLICE-006 (Consentimento + UI Login)
Sprint 5:   SLICE-007, SLICE-008 (Admin UI completo)
Sprint 6:   SLICE-009, SLICE-010 (Auditoria + Segurança)
Sprint 7:   SLICE-011, SLICE-012 (Docs + Produção)
```

---

## 6. Vantagens da Abordagem por Vertical Slices

### Comparação com Abordagem Anterior

**Horizontal (Anterior):**
- ❌ Valor entregue apenas no final
- ❌ Integração tardia (problemas descobertos tarde)
- ❌ Difícil demonstrar progresso
- ❌ Risco alto de retrabalho

**Vertical (Atual):**
- ✅ Valor entregue a cada slice
- ✅ Integração contínua (problemas descobertos cedo)
- ✅ Fácil demonstrar progresso (demos funcionais)
- ✅ Risco reduzido (feedback rápido)

### Benefícios Práticos

1. **Feedback Rápido:** Cada slice pode ser testada e validada
2. **Flexibilidade:** Prioridades podem mudar entre slices
3. **Paralelização:** Slices independentes podem ser desenvolvidas em paralelo
4. **Motivação:** Time vê funcionalidades completas sendo entregues
5. **Redução de Risco:** Problemas arquiteturais descobertos cedo

---

## 7. Próximos Passos

1. **Refinamento:** Revisar slices com time técnico
2. **Priorização:** Confirmar ordem de implementação
3. **Setup:** Preparar ambiente de desenvolvimento
4. **Kick-off:** Iniciar SLICE-001

---

**Fim do Documento**
