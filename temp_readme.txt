# Sprint2CSharp

API RESTful em .NET para a Mottu.

## ðŸš€ PrÃ©-requisitos

- [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
- Acesso a um **Oracle Database** (12c+ recomendado)

## ðŸ” Configurar a Connection String (sem expor credenciais)

### Windows (PowerShell)

Configure as variÃ¡veis de ambiente de acordo com suas credenciais:

```powershell
$env:ConnectionStrings__OracleConnection = "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORTA/SERVICE_NAME"
```

## â–¶ï¸ Como rodar

Clone o repositÃ³rio no diretÃ³rio desejado:

```powershell
git clone https://github.com/LuigiBerzaghi/Sprint2CSharp.git
```

Navegue atÃ© o diretÃ³rio do projeto:

```powershell
cd Sprint2CSharp/trackyard
```
Restaure e execute o projeto:

```bash
dotnet restore
dotnet run
```

O terminal mostrarÃ¡ a URL local (ex.: `http://localhost:5xxx`).  
Acesse o **Swagger** em:

```
http://localhost:{PORT}/swagger
```

## ðŸ“š Endpoints (resumo)

- `GET /api/Clientes` â€“ lista com paginaÃ§Ã£o e filtro por nome  
- `GET /api/Clientes/{id}` â€“ detalhe  
- `POST /api/Clientes` â€“ cria  
- `PUT /api/Clientes/{id}` â€“ atualiza  
- `DELETE /api/Clientes/{id}` â€“ remove

- `GET /api/Veiculos` â€“ lista com paginaÃ§Ã£o e filtro por placa  
- `GET /api/Veiculos/{id}` â€“ detalhe  
- `POST /api/Veiculos` â€“ cria  
- `PUT /api/Veiculos/{id}` â€“ atualiza  
- `DELETE /api/Veiculos/{id}` â€“ remove

- `GET /api/Patios` â€“ lista com paginaÃ§Ã£o  
- `GET /api/Patios/{id}` â€“ detalhe  
- `POST /api/Patios` â€“ cria  
- `PUT /api/Patios/{id}` â€“ atualiza  
- `DELETE /api/Patios/{id}` â€“ remove

## âœï¸ Exemplos de payload (POST/PUT)

### Clientes (POST/PUT body)
```json
{
  "id" : 1,
  "nome": "Luigi Berzaghi",
  "cpf": "123.456.789-00",
  "email": "luigi@example.com",
  "endereco": "Guarulhos - SP"
}
```

### VeÃ­culos (POST/PUT body)
```json
{
  "modelo": "CG 160",
  "placa": "ABC1D23",
  "cor": "Preta",
  "ano": "2022",
  "clienteId": 1
}
```

### PÃ¡tios (POST/PUT body)
```json
{
  "nome": "PÃ¡tio Central",
  "endereco": "Av. Lins de Vasconcelos, 1000"
}
```

## Sprint 2 â€” Itens Entregues

- Health Checks: `GET /health` retorna 200 quando app e DB estÃ£o OK.
- Versionamento: `v1` nos caminhos. Ex.: `GET /api/v1/veiculos`.
- SeguranÃ§a (API Key): enviar header `X-API-KEY`.
  - Valor padrÃ£o em dev: `dev-api-key` (configure `ApiKey` em `trackyard/appsettings.json` ou a env `API_KEY`).
- ML.NET: `POST /api/v1/ml/predict-risk` com body `{ "ano": 2022, "quilometragem": 10000 }`.
  - Resposta: `{ "predicted": false, "probability": 0.23 }`.

## Como executar os testes

Os testes nÃ£o dependem de Oracle. Utilizam banco em memÃ³ria e `WebApplicationFactory`.

Comandos:

```bash
dotnet restore
dotnet test
```

ObservaÃ§Ã£o: ao usar a API manualmente, inclua o header `X-API-KEY` (o Swagger jÃ¡ expÃµe o botÃ£o Authorize com esse esquema).
