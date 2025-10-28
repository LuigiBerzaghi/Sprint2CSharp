# Sprint2CSharp

API RESTful em .NET para a Mottu.

## 🚀 Pré-requisitos

- [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
- Acesso a um **Oracle Database** (12c+ recomendado)

## 🔐 Configurar a Connection String (sem expor credenciais)

### Windows (PowerShell)

Configure as variáveis de ambiente de acordo com suas credenciais:

```powershell
$env:ConnectionStrings__OracleConnection = "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORTA/SERVICE_NAME"
```

## ▶️ Como rodar

Clone o repositório no diretório desejado:

```powershell
git clone https://github.com/LuigiBerzaghi/Sprint2CSharp.git
```

Navegue até o diretório do projeto:

```powershell
cd Sprint2CSharp/trackyard
```
Restaure e execute o projeto:

```bash
dotnet restore
dotnet run
```

O terminal mostrará a URL local (ex.: `http://localhost:5xxx`).  
Acesse o **Swagger** em:

```
http://localhost:{PORT}/swagger
```

## Auth
Para autenticar e ter acesso aos métodos, é necessário realizar a autenticação via API-KEY.
- Valor padrão em dev: `dev-api-key` (configure `ApiKey` em `trackyard/appsettings.json` ou a env `API_KEY`).

## Como executar os testes

Os testes não dependem de Oracle. Utilizam banco em memória e `WebApplicationFactory`.

Comandos:

```bash
dotnet restore
dotnet test
```


## 📚 Endpoints (resumo)

- `GET /api/v1/Clientes` – lista com paginação e filtro por nome  
- `GET /api/v1/Clientes/{id}` – detalhe  
- `POST /api/v1/Clientes` – cria  
- `PUT /api/v1/Clientes/{id}` – atualiza  
- `DELETE /api/v1/Clientes/{id}` – remove

- `GET /api/v1/Veiculos` – lista com paginação e filtro por placa  
- `GET /api/v1/Veiculos/{id}` – detalhe  
- `POST /api/v1/Veiculos` – cria  
- `PUT /api/v1/Veiculos/{id}` – atualiza  
- `DELETE /api/v1/Veiculos/{id}` – remove

- `GET /api/v1/Patios` – lista com paginação  
- `GET /api/v1/Patios/{id}` – detalhe  
- `POST /api/v1/Patios` – cria  
- `PUT /api/v1/Patios/{id}` – atualiza  
- `DELETE /api/v1/Patios/{id}` – remove

- `POST /api/v1/ml/predict-risk` - retorna uma classificação de risco
- `GET /health` - retorna a "saúde" da aplicação

## ✍️ Exemplos de payload (POST/PUT)

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

### Veículos (POST/PUT body)
```json
{
  "modelo": "CG 160",
  "placa": "ABC1D23",
  "cor": "Preta",
  "ano": "2022",
  "clienteId": 1
}
```

### Pátios (POST/PUT body)
```json
{
  "nome": "Pátio Central",
  "endereco": "Av. Lins de Vasconcelos, 1000"
}
```

### ML.NET (POST body)
```json
{
  "ano": 2022,
  "quilometragem": 1000
}
```

irá retornar:
```json
{ 
  "predicted": false,
  "probability": 0.23 
}
```

### Health (GET)
Caso o app e DB estejam OK, retonará:
```json
Healthy
```

caso contrário:

```json 
Unhealthy
```