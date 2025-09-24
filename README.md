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

## 📚 Endpoints (resumo)

- `GET /api/Clientes` – lista com paginação e filtro por nome  
- `GET /api/Clientes/{id}` – detalhe  
- `POST /api/Clientes` – cria  
- `PUT /api/Clientes/{id}` – atualiza  
- `DELETE /api/Clientes/{id}` – remove

- `GET /api/Veiculos` – lista com paginação e filtro por placa  
- `GET /api/Veiculos/{id}` – detalhe  
- `POST /api/Veiculos` – cria  
- `PUT /api/Veiculos/{id}` – atualiza  
- `DELETE /api/Veiculos/{id}` – remove

- `GET /api/Patios` – lista com paginação  
- `GET /api/Patios/{id}` – detalhe  
- `POST /api/Patios` – cria  
- `PUT /api/Patios/{id}` – atualiza  
- `DELETE /api/Patios/{id}` – remove

## ✍️ Exemplos de payload (POST/PUT)

### Clientes (POST/PUT body)
```json
{
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
