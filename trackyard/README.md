# Sprint1CSharp (.NET 8 + Oracle EF Core)

API RESTful com entidades **Clientes**, **Veículos** e **Pátios**.

## Rodando local

1. Defina a variável de ambiente com sua connection string Oracle (Exemplo: `User Id=USR;Password=PWD;Data Source=HOST:1521/SEU_SERVICE_NAME`):

**Windows (PowerShell)**
```powershell
$env:ORACLE_CONNECTION = "User Id=USR;Password=PWD;Data Source=HOST:1521/ORCLPDB1"
```

**Linux/macOS (bash)**
```bash
export ORACLE_CONNECTION="User Id=USR;Password=PWD;Data Source=HOST:1521/ORCLPDB1"
```

2. Restaure pacotes e rode:
```bash
dotnet restore
dotnet run
```

Acesse `https://localhost:5001/swagger` (ou a URL que o console indicar).

> O app tenta `EnsureCreated()` no banco, criando tabelas se possível. Se você já possui o schema, isso é ignorado.

## Endpoints principais

- `GET /api/clientes?page=1&pageSize=10&nome=Lu`
- `GET /api/clientes/{id}`
- `POST /api/clientes`
- `PUT /api/clientes/{id}`
- `DELETE /api/clientes/{id}`

- `GET /api/veiculos?page=1&pageSize=10&placa=ABC`
- `GET /api/veiculos/{id}`
- `POST /api/veiculos`
- `PUT /api/veiculos/{id}`
- `DELETE /api/veiculos/{id}`

- `GET /api/patios?page=1&pageSize=10`
- `GET /api/patios/{id}`
- `POST /api/patios`
- `PUT /api/patios/{id}`
- `DELETE /api/patios/{id}`

## Observações

- Booleans não são usados no schema; Oracle não possui tipo boolean nativo em colunas, então evite mapear `bool` diretamente.
- Índices únicos: `CLIENTES.CPF` e `VEICULOS.PLACA`.
