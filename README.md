# MeuProjeto 🏗️

**Portal do Cliente para negócios sob medida** — SaaS que permite marcenarias, serralherias, vidraçarias e outros negócios de projetos personalizados oferecerem um portal profissional onde seus clientes acompanham o andamento dos pedidos em tempo real.

## 💡 O Problema

O marceneiro faz o projeto, manda um PDF por WhatsApp, e depois fica respondendo mensagens o dia todo:

- *"Já começou a fazer?"*
- *"Quando fica pronto?"*
- *"Quanto eu ainda devo?"*
- *"Me manda aquela imagem de novo?"*

**MeuProjeto** resolve isso dando ao cliente um link exclusivo onde ele vê tudo sozinho.

## 🎯 O que o Cliente Final vê

Acesso por link (sem instalar nada):
- ✅ Imagens do projeto renderizado
- ✅ Timeline de status (Orçamento → Aprovado → Produção → Entrega → Instalação)
- ✅ Barra de progresso visual
- ✅ Valores e parcelas (pagas/pendentes)
- ✅ Galeria de fotos da produção
- ✅ Botão de WhatsApp direto

## 🔧 O que o Profissional vê

Painel de gestão:
- ✅ Cadastro de clientes
- ✅ Criação de projetos com valor, prazo e parcelas
- ✅ Upload de imagens (projeto + produção)
- ✅ Atualização de status com 1 clique
- ✅ Controle financeiro por projeto
- ✅ Link compartilhável por cliente

## 🏗️ Arquitetura

Clean Architecture com 4 camadas:

```
src/
├── MeuProjeto.Domain/          # Entidades, regras de negócio, interfaces
├── MeuProjeto.Application/     # Serviços, DTOs, mapeamentos
├── MeuProjeto.Infrastructure/  # EF Core, repositórios, banco de dados
└── MeuProjeto.Web/             # ASP.NET Core (UI + API)
tests/
├── MeuProjeto.Domain.Tests/    # Testes unitários do domínio
└── MeuProjeto.Application.Tests/ # Testes dos serviços
```

## 🛠️ Stack

- **Backend:** ASP.NET Core 9
- **ORM:** Entity Framework Core 9
- **Banco:** SQL Server (compatível com SmarterASP.NET)
- **Auth:** ASP.NET Identity
- **Testes:** xUnit
- **CI/CD:** GitHub Actions
- **Padrões:** Clean Architecture, Result Pattern, Repository + Unit of Work

## 🚀 Como rodar

```bash
# Clone
git clone https://github.com/Softprojetos/MeuProjeto.git
cd MeuProjeto

# Restaurar dependências
dotnet restore

# Rodar testes
dotnet test

# Rodar o projeto (usa InMemory DB por padrão em dev)
cd src/MeuProjeto.Web
dotnet run
```

## 📋 Modelo de Negócio

| Plano | Preço | Limites |
|-------|-------|---------|
| Trial | Grátis 30 dias | 5 projetos |
| Básico | R$ 49/mês | 30 projetos |
| Profissional | R$ 99/mês | Ilimitado |

## 📊 Mercado-alvo

- 70.000+ marcenarias no Brasil
- Serralherias, vidraçarias, marmorarias
- Qualquer negócio de projeto sob medida

---

Desenvolvido por [Soft Projetos](https://softprojetos.com)
