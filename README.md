[README.md](https://github.com/user-attachments/files/22516892/README.md)
# Sprint2CSharp

API RESTful em .NET para controle de clientes, veículos e pátios.  
Inclui **Swagger/OpenAPI** com **descrição de endpoints e parâmetros**, **exemplos de payload** e **modelos de dados descritos**.

## ✅ Requisitos (slides 7–9)

- Swagger/OpenAPI configurado:
  - Descrição de endpoints e parâmetros (via XML comments).
  - Exemplos de payload (via `ExamplesOperationFilter`).
  - Modelos de dados descritos (Schemas com summary das propriedades).

## 🚀 Pré-requisitos

- [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
- Acesso a um **Oracle Database** (12c+ recomendado)

## 🔐 Configurar a Connection String (sem expor credenciais)

Personalize o comando conforme seu ambiente.  
> **Não** use seus dados reais no README. Substitua os placeholders `SEU_USUARIO`, `SUA_SENHA`, `HOST`, `PORTA` e `SERVICE_NAME`.

### Windows (PowerShell)
```powershell
$env:ConnectionStrings__OracleConnection = "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORTA/SERVICE_NAME"
```

### Linux/macOS (bash)
```bash
export ConnectionStrings__OracleConnection="User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORTA/SERVICE_NAME"
```

Exemplo comum (ajuste para o seu host/porta/service):
```
User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.seu-dominio.com:1521/orcl
```

> A aplicação também aceita `ORACLE_CONNECTION` como fallback, se você preferir:
> - PowerShell: `$env:ORACLE_CONNECTION = "User Id=..."`
> - bash: `export ORACLE_CONNECTION="User Id=..."`

## ▶️ Como rodar

Na pasta do projeto (onde está o `.csproj`):

```bash
dotnet restore
dotnet run
```

O terminal mostrará a URL local (ex.: `http://localhost:5xxx`).  
Acesse o **Swagger** em:

```
http://localhost:{PORT}/swagger
```

> Se estiver em `Development`, a UI do Swagger já estará habilitada.

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

> Os **exemplos aparecem no Swagger** automaticamente (Operation Filter).  
> Em **POST**, **não envie `id`** — o banco gera.

## 🧪 Dicas de verificação rápida (pra nota)

- No `/swagger`, confira:
  - **Descriptions** nos endpoints e seus **parâmetros** (summary/param).
  - **Example Value** nos **POST/PUT** (Clientes, Veículos, Pátios).
  - **Schemas** dos modelos com descrições das propriedades.
- Teste um CRUD completo (criar, listar, detalhar, atualizar, deletar) para cada entidade.

## 🛠️ Solução de problemas

- **ORA-00942 (tabela não existe)**: confirme que as tabelas foram criadas no **mesmo schema** do usuário da connection string.
- **ORA-00904 ("Id" inválido)**: nomes de colunas devem estar em **UPPERCASE** (ex.: `ID`) e sem aspas no DDL.
- **Sem acesso ao DB**: valide host/porta/service e firewall da sua base Oracle.
