# Documento de Requisitos – Sistema de Single Sign-On (SSO)

## 1. Visão Geral

Este documento descreve os requisitos funcionais, não funcionais e a estrutura de dados necessária para a implementação de um **Sistema de Single Sign-On (SSO)**, utilizando **OAuth 2.0 (Authorization Code Flow)** e **OpenID Connect (OIDC)**, com **UI de login própria** e **consentimento por sistema**.

O sistema tem como objetivo centralizar a autenticação de usuários e permitir que múltiplos sistemas clientes utilizem uma identidade única, de forma segura, padronizada e preparada para **cenários multi-tenant**.

---

## 2. Escopo do Projeto

### 2.1 Modelo Arquitetural de Autorização

O sistema de Single Sign-On (SSO) é responsável exclusivamente por:

- Autenticação de usuários
- Centralização de identidade
- Emissão de tokens contendo roles e permissions

O SSO **não possui conhecimento sobre regras de negócio nem sobre recursos específicos** dos sistemas clientes.

Cada sistema cliente é considerado **dono dos seus próprios recursos** e deve aplicar suas regras de autorização utilizando as **permissions recebidas no Access Token**.

As permissões são definidas de forma centralizada no SSO, porém **cada sistema cliente declara explicitamente quais permissões reconhece**, garantindo isolamento lógico entre sistemas e preparando a arquitetura para um ambiente **multi-tenant**.

---

### 2.2 Funcionalidades incluídas

- OpenID Connect (OIDC)
- OAuth 2.0 – Authorization Code Flow
- Consentimento de identidade por sistema
- Interface de login própria (SSO UI)
- Tokens JWT (Access Token, ID Token e Refresh Token)
- Isolamento de permissões por sistema cliente

---

### 2.3 Funcionalidades fora do escopo

- Revogação de tokens em tempo real
- Autorização baseada em scopes de domínio
- Login social (Google, Microsoft, etc.)
- Autenticação multifator (MFA)

---

## 3. Atores do Sistema

### 3.1 Usuário Final

- Realiza login no SSO
- Concede consentimento para sistemas clientes
- Utiliza múltiplos sistemas com uma única autenticação

---

### 3.2 Sistema Cliente

- Redireciona o usuário para autenticação no SSO
- Solicita tokens ao SSO
- Consome informações de identidade do usuário
- Aplica autorização baseada nas permissões recebidas

---

### 3.3 Administrador

- Gerencia usuários
- Gerencia roles e permissões
- Gerencia sistemas clientes
- Gerencia permissões habilitadas por sistema

---

## 4. Requisitos Funcionais

### 4.1 Autenticação de Usuários

- Autenticação via **email e senha**
- Senhas armazenadas com **hash seguro + salt**
- Sessão autenticada via cookie seguro (`HttpOnly`)
- Logout do usuário

---

### 4.2 OAuth 2.0 – Authorization Code Flow

#### 4.2.1 Endpoint `/authorize`

- Validar `client_id`, `redirect_uri` e `scope`
- Verificar autenticação do usuário
- Redirecionar para login se necessário
- Gerar **Authorization Code** de uso único

---

#### 4.2.2 Endpoint `/token`

- Validar Authorization Code
- Validar `client_secret` e **PKCE**
- Emitir:
  - Access Token (JWT)
  - ID Token (JWT)
  - Refresh Token

---

### 4.3 OpenID Connect (OIDC)

- Emissão de ID Token
- Endpoint de descoberta:
  - `/.well-known/openid-configuration`
- Endpoint `/userinfo`

#### 4.3.1 Claims mínimas

- `sub`
- `email`
- `name`
- `iss`
- `aud`
- `exp`

---

### 4.4 Consentimento por Sistema

O consentimento do usuário é utilizado **exclusivamente para identidade**, conforme o padrão OpenID Connect.

Scopes utilizados:

- `openid`
- `profile`
- `email`

A tela de consentimento possui caráter informativo e **não concede permissões de domínio**.

---

### 4.5 UI do SSO

#### 4.5.1 UI de Login

- Login (email e senha)
- Erro de autenticação
- Consentimento de identidade
- Logout

---

#### 4.5.2 UI de Cadastro de Usuários

- Cadastro com email e senha
- Validações básicas
- Ativação inicial
- Auditoria da criação

Cadastro pode ser:

- Público, ou
- Restrito a administradores

---

#### 4.5.3 UI Administrativa

##### Gestão de Usuários

- Criar, editar e desativar usuários
- Atribuir roles
- Atribuir permissões diretas
- Resetar senha

##### Gestão de Roles e Permissões

- Criar roles
- Criar permissões
- Associar permissões a roles

##### Gestão de Sistemas Clientes

- Criar e editar clients
- Gerenciar `redirect_uri`
- Ativar/desativar clients
- Definir permissões habilitadas por client

##### Auditoria

- Visualização e filtro de logs

---

### 4.6 Tokens

#### 4.6.1 Access Token

- Formato JWT
- Curta duração
- Contém **apenas permissões válidas para o sistema cliente**
- Permissões emitidas:

---

#### 4.6.2 ID Token

- Formato JWT
- Contém apenas informações de identidade do usuário

---

#### 4.6.3 Refresh Token

- Persistido no banco de dados
- Vinculado ao usuário e ao client
- Rotação a cada uso

---

## 5. Requisitos Não Funcionais

### 5.1 Segurança

- Hash seguro (Argon2id ou BCrypt)
- Rate limiting
- Proteção contra brute force
- PKCE obrigatório
- Validação estrita de `redirect_uri`
- JWT assinado (RS256 ou ES256)
- Publicação de chaves via `jwks_uri`

---

### 5.2 Performance

- Validação de Access Token sem acesso ao banco
- Arquitetura stateless

---

### 5.3 Escalabilidade

- Suporte a múltiplas instâncias
- Preparado para multi-tenant

---

## 6. Modelo de Dados – Tabelas do Banco

### 6.1 Users

- Id
- Email
- PasswordHash
- Name
- IsActive
- CreatedAt

---

### 6.2 Roles

- Id
- Name
- Description
- CreatedAt

---

### 6.3 Permissions

- Id
- Name
- Description
- CreatedAt

---

### 6.4 RolePermissions

- RoleId
- PermissionId

---

### 6.5 UserRoles

- UserId
- RoleId

---

### 6.6 UserPermissions

- UserId
- PermissionId
- GrantedAt
- GrantedBy

---

### 6.7 Clients

- Id
- Name
- ClientId
- ClientSecret
- RedirectUris
- IsActive
- CreatedAt

---

### 6.8 ClientPermissions

Relaciona permissões reconhecidas por cada sistema cliente.

- ClientId
- PermissionId
- CreatedAt

> Apenas permissões presentes nesta tabela podem ser emitidas no Access Token para o respectivo client.

---

### 6.9 Scopes

- Id
- Name
- Description

---

### 6.10 ClientScopes

- ClientId
- ScopeId

---

### 6.11 UserConsents

- Id
- UserId
- ClientId
- GrantedAt

---

### 6.12 AuthorizationCodes

- Code
- UserId
- ClientId
- RedirectUri
- ExpiresAt
- IsUsed

---

### 6.13 RefreshTokens

- Id
- Token
- UserId
- ClientId
- ExpiresAt
- CreatedAt
- IsRevoked

---

### 6.14 AuditLogs

- Id
- EventType
- EntityType
- EntityId
- UserId
- ClientId
- Timestamp
- IpAddress
- UserAgent
- Data (JSON)

---

## 7. Controle de Acesso (RBAC + Permissions + Client Isolation)

- Roles agrupam permissões
- Usuários podem possuir permissões diretas
- Clients definem quais permissões reconhecem
- Tokens contêm apenas permissões válidas para o client

---

## 8. Auditoria

Eventos auditáveis:

- Login (sucesso e falha)
- Logout
- Consentimento
- Emissão de tokens
- Uso de refresh token
- Ações administrativas

Diretrizes:

- Logs append-only
- Sem dados sensíveis
- Baixo impacto no fluxo principal

---

## 9. Considerações Finais

Este documento define um **Sistema de Single Sign-On robusto, seguro e preparado para multi-tenant**, alinhado aos padrões **OAuth 2.0** e **OpenID Connect**.

A introdução da tabela **ClientPermissions** garante isolamento entre sistemas, controle fino de autorização e flexibilidade para evolução futura sem necessidade de reestruturação profunda.
