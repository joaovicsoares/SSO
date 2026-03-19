# Contributing

Obrigado por contribuir com este projeto 🎉

Este documento descreve **como colaborar corretamente**, seguindo a estratégia de branches, padrões de commits e boas práticas adotadas no projeto.

---

## 🌳 Branching Strategy

Este projeto utiliza uma variação **simplificada do Gitflow**, adequada para times pequenos e com foco em previsibilidade e CI/CD.

### Branches principais

| Branch    | Ambiente   | Descrição                             |
| --------- | ---------- | ------------------------------------- |
| `main`    | Production | Código estável e pronto para produção |
| `develop` | Staging    | Integração contínua e homologação     |

Essas branches são **protegidas** e não devem receber commits diretos.

---

### Branches de feature

```
feature/<descricao-curta>
```
Caso seja uma branch criada para uma PBI do Board
```
feature/<titulo-da-pbi>
```
Exemplos:

* `feature/auth-jwt`
* `feature/user-registration`
* `feature/ci-docker-build`

Regras:

* Criadas a partir da `develop`
* Devem conter apenas alterações relacionadas ao escopo da feature
* Devem ser curtas e focadas

---

### Fluxo de trabalho

1. Criar branch a partir da `develop`
2. Desenvolver a feature localmente
3. Garantir que o build e testes passam
4. Abrir Pull Request para `develop`
5. Após validação, a feature é integrada em **staging**
6. Quando estável, a `develop` é promovida para `main`

---

## 🔀 Pull Requests

Todos os Pull Requests devem:

* Ter um título claro e objetivo
* Descrever **o que foi feito** e **por quê**
* Referenciar PBIs ou issues quando aplicável
* Passar pelo CI automaticamente

Checklist recomendado:

* [ ] Código compila
* [ ] Build Docker executa com sucesso
* [ ] Não quebra funcionalidades existentes

---

## 📝 Padrão de Commits

Utilize commits semânticos para manter o histórico claro.

Formato:

```
<tipo>: <descrição curta>
```

Tipos aceitos:

* `feat:` nova funcionalidade
* `fix:` correção de bug
* `refactor:` refatoração sem mudança de comportamento
* `chore:` tarefas técnicas (configs, CI, deps)
* `docs:` documentação
* `test:` testes

Exemplos:

```
feat: add jwt authentication flow
fix: correct token expiration validation
chore: configure docker for staging
```

---

## 🧪 Qualidade de Código

Guidelines gerais:

* Seguir os princípios da **Clean Architecture**
* Evitar dependências diretas entre camadas
* Priorizar código legível e explícito
* Métodos pequenos e bem nomeados
* Evitar lógica de negócio na camada de API

---

## 🐳 Docker e Ambiente

* Utilize o mesmo `Dockerfile` para todos os ambientes
* Diferenças de ambiente devem ser tratadas via **variáveis de ambiente**
* Nunca commitar secrets no repositório

---

## 🚦 CI/CD

O pipeline executa automaticamente:

* Build da aplicação
* Build da imagem Docker

Regras:

* Commits que quebram o CI não devem ser mergeados
* Pull Requests só devem ser aprovados com CI verde

---

## 🔐 Segurança

* Nunca commitar senhas, tokens ou secrets
* Utilizar variáveis de ambiente e secrets do GitHub
* Revisar mudanças que impactem autenticação e autorização com atenção extra

---

## 💬 Comunicação

* Discussões técnicas devem ser documentadas
* Decisões arquiteturais relevantes devem gerar um ADR
* Dúvidas devem ser levantadas antes de grandes mudanças

---

## ✅ Resumo

* Branches curtas e focadas
* PRs claros e revisados
* CI sempre verde
* Código limpo e sustentável

Seguindo essas diretrizes, garantimos um projeto consistente, seguro e fácil de evoluir 🚀
