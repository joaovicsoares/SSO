# Documento de Requisitos – Sistema de Single Sign-On (SSO)

## 1. Visão Geral

Este documento descreve os requisitos funcionais, não funcionais e a estrutura de dados necessária para a implementação de um **Sistema de Single Sign-On (SSO)**, utilizando **OAuth 2.0 (Authorization Code Flow)** e **OpenID Connect (OIDC)**, com **UI de login própria** e **consentimento por sistema**.

O sistema terá como objetivo centralizar a autenticação de usuários e permitir que múltiplos sistemas clientes utilizem uma identidade única, de forma segura e padronizada.

---

## 2. Escopo do Projeto

### 2.1 Modelo Arquitetural de Autorização

O sistema de Single Sign-On (SSO) é responsável exclusivamente por:

* Autenticação de usuários
* Centralização de identidade
* Emissão de tokens contendo roles e permissions

O SSO **não possui conhecimento sobre regras de negócio nem sobre recursos específicos** dos sistemas clientes.

Cada sistema cliente é considerado **dono dos seus próprios recursos** e deve aplicar suas regras de autorização utilizando as **permissions recebidas no Access Token**.

Este modelo garante baixo acoplamento, clareza de responsabilidades e facilidade de evolução dos sistemas.

---

### 2.2 Funcionalidades incluídas

* OpenID Connect (OIDC)
* OAuth 2.0 – Authorization Code Flow
* Consentimento de identidade por sistema
* Interface de login própria (SSO UI)
* Tokens JWT (Access Token, ID Token e Refresh Token)

### 2.3 Funcionalidades fora do escopo

* Revogação de tokens em tempo real
* Autorização baseada em scopes de domínio
* Login social (Google, Microsoft, etc.)
* Autenticação multifator (MFA)

---

## 3. Atores do Sistema

### 3.1 Usuário Final

* Realiza login no SSO
* Concede consentimento para sistemas clientes
* Utiliza múltiplos sistemas com uma única autenticação

### 3.2 Sistema Cliente

* Redireciona o usuário para autenticação no SSO
* Solicita tokens ao SSO
* Consome informações de identidade do usuário

### 3.3 Administrador

* Gerencia usuários
* Gerencia sistemas clientes
* Gerencia permissões e scopes

---

## 4. Requisitos Funcionais

### 4.1 Autenticação de Usuários

* O sistema deve permitir autenticação via **email e senha**
* As senhas devem ser armazenadas utilizando **hash seguro com salt**
* O sistema deve manter sessão autenticada via cookie seguro (HttpOnly)
* Deve permitir logout do usuário

---

### 4.2 OAuth 2.0 – Authorization Code Flow

#### 4.2.1 Endpoint /authorize

* Validar `client_id`, `redirect_uri` e `scope`
* Verificar se o usuário está autenticado
* Redirecionar para a UI de login caso não esteja autenticado
* Gerar um **Authorization Code** de uso único

#### 4.2.2 Endpoint /token

* Validar Authorization Code
* Validar `client_secret`
* Emitir:

  * Access Token (JWT)
  * ID Token (JWT)
  * Refresh Token

---

### 4.3 OpenID Connect (OIDC)

O sistema deve implementar os principais recursos do OpenID Connect:

* Emissão de **ID Token** contendo informações de identidade do usuário
* Endpoint de descoberta:

  * `/.well-known/openid-configuration`
* Endpoint:

  * `/userinfo`

#### 4.3.1 Claims mínimas

* `sub` (identificador único do usuário)
* `email`
* `name`
* `iss` (issuer)
* `aud` (client_id)
* `exp` (expiração)

---

### 4.4 Consentimento por Sistema

O consentimento do usuário será utilizado **exclusivamente para autorização de identidade**, conforme o padrão OpenID Connect.

Scopes utilizados:

* openid
* profile
* email

O sistema **não utiliza scopes para controle de acesso a recursos de domínio**, uma vez que cada sistema cliente é responsável por seus próprios recursos.

A tela de consentimento possui caráter informativo, indicando que o sistema cliente utilizará a identidade do usuário para autenticação.

---

### 4.5 UI do SSO (Login, Cadastro e Administração)

O sistema deve possuir interfaces próprias para interação com usuários finais e administradores, centralizando toda a gestão de identidade no SSO.

#### 4.5.1 UI de Login

* Tela de login (email e senha)
* Tela de erro de autenticação
* Tela de consentimento de identidade
* Logout

Esta UI é o ponto central de autenticação para todos os sistemas clientes.

---

#### 4.5.2 UI de Cadastro de Usuários

O sistema deve permitir o **cadastro de usuários diretamente no SSO**, contendo:

* Criação de conta com email e senha
* Validações básicas (email único, senha forte)
* Ativação inicial do usuário
* Registro de auditoria da criação do usuário

O cadastro poderá ser:

* Público (self-service), ou
* Restrito a administradores (configurável)

---

#### 4.5.3 UI Administrativa

O sistema deve possuir uma **UI exclusiva para administradores**, permitindo:

##### Gestão de Usuários

* Criar, editar e desativar usuários
* Atribuir e remover roles
* Atribuir e remover permissões diretas
* Resetar senha

##### Gestão de Roles e Permissões

* Criar e editar roles
* Criar e editar permissões
* Associar permissões a roles

##### Gestão de Sistemas Clientes (Clients)

* Criar e editar sistemas clientes
* Gerenciar `redirect_uri`
* Gerenciar scopes permitidos
* Ativar ou desativar clientes

##### Auditoria

* Visualizar logs de auditoria
* Filtrar por data, usuário, evento e sistema cliente

Todas as ações administrativas devem ser **auditadas**.

---

### 4.6 Tokens

#### 4.6.1 Access Token

* Formato JWT
* Curta duração (ex: 5 a 15 minutos)
* Contém roles e permissions do usuário
* Utilizado exclusivamente para autorização nos sistemas clientes

#### 4.6.2 ID Token

* Formato JWT
* Contém apenas informações de identidade do usuário
* Utilizado para autenticação

#### 4.6.3 Refresh Token

* Armazenado no banco de dados
* Utilizado para obtenção de novos tokens
* Vinculado ao usuário e ao sistema cliente

---

## 5. Requisitos Não Funcionais

### 5.1 Segurança

O sistema de SSO deve seguir boas práticas modernas de segurança, considerando que se trata de um componente crítico de infraestrutura.

#### 5.1.1 Autenticação

* Senhas armazenadas utilizando **hash seguro com salt** (Argon2id ou BCrypt)
* Proteção contra brute force com rate limiting por IP e usuário
* Bloqueio temporário após múltiplas tentativas de login inválidas
* Mensagens de erro genéricas para evitar enumeração de usuários

#### 5.1.2 Sessão (UI do SSO)

* Cookies configurados como `HttpOnly` e `Secure`
* Política `SameSite` apropriada
* Expiração e invalidação de sessão no logout

#### 5.1.3 OAuth 2.0 / OpenID Connect

* Uso obrigatório de **PKCE** (Proof Key for Code Exchange)
* Validação estrita de `redirect_uri`
* Uso de parâmetros `state` (CSRF) e `nonce` (replay protection)

#### 5.1.4 Tokens

* Tokens JWT assinados com **RS256 ou ES256**
* Access Tokens com curta duração
* Refresh Tokens com **rotação a cada uso**
* Refresh Tokens vinculados ao usuário e sistema cliente
* Publicação de chaves públicas via `jwks_uri`

#### 5.1.5 APIs

* Rate limiting nos endpoints sensíveis (`/authorize`, `/token`, `/login`, `/userinfo`)
* Validação completa de tokens JWT nos sistemas clientes (`iss`, `aud`, `exp`, assinatura)

---

### 5.2 Performance

* Validação de Access Token sem acesso ao banco de dados
* Uso de JWT para evitar chamadas desnecessárias ao SSO

### 5.3 Escalabilidade

* Arquitetura stateless
* Suporte a múltiplas instâncias do SSO

---

## 6. Modelo de Dados – Tabelas do Banco

### 6.1 Users

Armazena os usuários do sistema.

* Id
* Email
* PasswordHash
* Name
* IsActive
* CreatedAt

---

### 6.2 Roles

Define perfis de acesso (agrupadores de permissões).

* Id
* Name
* Description
* CreatedAt

---

### 6.3 Permissions

Define permissões granulares do sistema.

* Id
* Name
* Description
* CreatedAt

---

### 6.4 RolePermissions

Relaciona roles às permissões.

* RoleId
* PermissionId

---

### 6.5 UserRoles

Relacionamento entre usuários e roles.

* UserId
* RoleId

---

### 6.6 UserPermissions

Permite atribuir permissões diretamente a um usuário, complementando as roles.

* UserId
* PermissionId
* GrantedAt
* GrantedBy

---

### 6.5 UserRoles

Relacionamento entre usuários e roles.

* UserId
* RoleId

---

### 6.6 Clients

Representa os sistemas clientes integrados ao SSO.

* Id
* Name
* ClientId
* ClientSecret
* RedirectUris
* IsActive
* CreatedAt

---

### 6.7 Scopes

Define os escopos disponíveis no sistema.

* Id
* Name
* Description

---

### 6.8 ClientScopes

Relaciona quais scopes cada sistema cliente pode solicitar.

* ClientId
* ScopeId

---

### 6.9 UserConsents

Registra o consentimento do usuário por sistema.

* Id
* UserId
* ClientId
* GrantedAt

---

### 6.10 AuthorizationCodes

Armazena os authorization codes gerados.

* Code
* UserId
* ClientId
* RedirectUri
* ExpiresAt
* IsUsed

---

### 6.11 RefreshTokens

Armazena os refresh tokens emitidos.

* Id
* Token
* UserId
* ClientId
* ExpiresAt
* CreatedAt
* IsRevoked

---

### 6.12 AuditLogs

Registra eventos relevantes para auditoria e segurança.

* Id
* EventType
* EntityType
* EntityId
* UserId
* ClientId
* Timestamp
* IpAddress
* UserAgent
* Data (JSON)

---

## 7. Controle de Acesso (RBAC + Permissions)

O sistema utiliza um modelo híbrido de controle de acesso:

* **RBAC (Role-Based Access Control)** para facilitar a atribuição de permissões em grupo
* **Permissões diretas por usuário** para exceções controladas

### Estratégia adotada

* Roles agrupam permissões comuns
* Permissões representam ações específicas
* Usuários podem receber permissões adicionais sem necessidade de novas roles
* Tokens contêm a união das permissões vindas das roles e das permissões diretas

Exemplo de permissões:

* users.create
* users.edit
* users.delete
* pedidos.read
* pedidos.approve

Essa abordagem oferece flexibilidade sem comprometer a organização do controle de acesso.

---

## 8. Auditoria

O sistema deve possuir um mecanismo de auditoria para registrar ações relevantes de segurança e administração.

### 8.1 Eventos auditáveis

#### Autenticação

* Login bem-sucedido
* Login falho
* Logout

#### Autorização

* Concessão de consentimento
* Emissão de tokens
* Uso de refresh token

#### Administração

* Criação, edição e desativação de usuários
* Alteração de roles
* Alteração de permissões (roles ou usuários)
* Criação e edição de sistemas clientes

### 8.2 Diretrizes

* Auditoria não deve impactar o fluxo principal
* Registros são imutáveis (append-only)
* Dados sensíveis nunca devem ser armazenados

---

## 9. Considerações Finais

Este documento define um **Sistema de Single Sign-On robusto, seguro e extensível**, alinhado aos padrões OAuth 2.0 e OpenID Connect.

A arquitetura proposta contempla boas práticas de segurança, controle de acesso flexível (RBAC + permissões diretas) e auditoria, permitindo evolução futura para recursos como MFA, login social e revogação de tokens em tempo real, sem necessidade de reestruturação profunda.

