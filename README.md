# bling-ordens-producao

Aplicativo desktop (WinForms, .NET 9) que automatiza a criação e finalização de Ordens de Produção
no Bling. Foi feito pra substituir um processo manual repetitivo: uma produção com várias
impressoras 3D rodando ao longo do dia, onde cada lote precisava virar uma ordem de produção criada
e finalizada à mão no Bling, produto por produto.

A tela principal tem busca de produto com autocomplete (usando o catálogo carregado direto da conta
Bling), uma lista onde os itens do lote vão se acumulando, e um botão que cria e finaliza a ordem de
cada item de uma vez, com log de sucesso ou erro por item.

## Em resumo

**Problema:** numa produção com várias impressoras 3D rodando ao longo do dia, cada lote virava uma ordem de
produção criada e finalizada à mão no Bling, produto por produto.

**Solução:** um aplicativo de Windows em que você busca o produto, monta o lote e executa tudo de uma vez, com o
resultado de cada item na tela.

**Destaques**

- Integração com a API v3 do Bling usando OAuth2, com renovação automática do token.
- Tokens guardados criptografados (DPAPI), presos ao usuário do Windows.
- Entrega como `.exe` autocontido: quem usa não precisa instalar o .NET.

## Como rodar

```
dotnet run --project src/BlingOrdensProducao
```

## Cadastro do aplicativo no painel do Bling

A API v3 do Bling usa OAuth2, então antes de autorizar é preciso cadastrar um app em
https://developer.bling.com.br/aplicativos:

- **Link de redirecionamento**: `http://localhost:8765/callback/`, exatamente essa string (barra
  final incluída). É o endereço que o app abre localmente pra capturar o retorno da autorização.
- **Escopos**: marcar "Produtos" (leitura) e "Ordens de Produção" (leitura e escrita).
- Os demais campos do formulário (nome, categoria, descrição, dados do desenvolvedor) são só
  metadados do cadastro e não afetam o funcionamento.

Depois de salvar, o Bling mostra o Client ID e o Client Secret do app. Esses dois valores são
digitados uma única vez na tela do aplicativo pra iniciar a autorização.

## Autenticação

1. O usuário informa Client ID e Client Secret na tela principal e clica em "Autorizar acesso ao
   Bling".
2. O app abre o navegador padrão na URL de autorização do Bling.
3. Depois do login/aprovação, o Bling redireciona para `http://localhost:8765/callback/?code=...`.
4. Um `HttpListener` local, aberto só durante esse processo, captura o código e troca por
   `access_token`/`refresh_token`.
5. Os tokens são salvos criptografados com DPAPI, atrelados ao usuário do Windows, em
   `%AppData%\BlingOrdensProducao\auth.dat`.
6. Nas próximas execuções o app já abre conectado. O `access_token` é renovado automaticamente via
   `refresh_token` quando necessário, sem pedir login de novo.

O botão "Testar conexão" na tela principal confirma que os tokens salvos autenticam contra a API.

## Gerando o executável

O usuário final não tem o SDK do .NET instalado, então a entrega é um `.exe` autocontido:

```
dotnet publish src/BlingOrdensProducao -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish
```

Isso gera `publish/BlingOrdensProducao.exe` (por volta de 110 MB, porque embute o runtime do .NET
inteiro, sem precisar instalar nada na máquina de destino). O Client ID e o Client Secret não vão
junto no pacote: são digitados manualmente na primeira execução, na tela de autorização.

## Estrutura

```
src/BlingOrdensProducao/
├── Forms/       # MainForm: tela de conexão e tela de lote
├── Models/      # DTOs de autenticação, produto e ordem de produção
├── Services/    # BlingOAuthService, BlingApiClient, BlingProdutoService, BlingOrdemProducaoService
└── Program.cs
```
