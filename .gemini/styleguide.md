# Code Review Style Guide

## Regras Gerais
- Verificar segurança: SQL injection, credenciais expostas, XSS
- Verificar performance: queries N+1, loops desnecessários, memory leaks
- Código limpo: naming conventions, DRY, separação de responsabilidades
- Async/await em todas operações de I/O
- Validação de input em endpoints públicos
- Não expor secrets, tokens ou connection strings no código

## C# / .NET
- Nullable reference types habilitados
- PascalCase para métodos e propriedades
- AsNoTracking em queries read-only
- Usar Add ao invés de AddAsync (exceto HiLo generators)

## Segurança (CRÍTICO)
- NUNCA hardcode de credenciais
- Verificar SQL injection em queries raw
- CSRF protection em formulários POST
- Validar e sanitizar inputs do usuário
