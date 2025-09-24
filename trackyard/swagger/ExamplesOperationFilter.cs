using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sprint1CSharp.Swagger
{
    /// <summary>
    /// Injeta exemplos de payload nos endpoints de POST/PUT (Clientes, Veículos, Pátios).
    /// </summary>
    public class ExamplesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.RequestBody?.Content == null) return;

            var path = context.ApiDescription.RelativePath?.ToLower() ?? "";
            var method = context.ApiDescription.HttpMethod?.ToUpper() ?? "";

            OpenApiObject? example = null;

            if (path.StartsWith("api/clientes") && (method == "POST" || method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["nome"] = new OpenApiString("Luigi Berzaghi"),
                    ["cpf"] = new OpenApiString("123.456.789-00"),
                    ["email"] = new OpenApiString("luigi@example.com"),
                    ["endereco"] = new OpenApiString("Guarulhos - SP")
                };
            }
            else if (path.StartsWith("api/veiculos") && (method == "POST" || method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["modelo"] = new OpenApiString("CG 160"),
                    ["placa"]  = new OpenApiString("ABC1D23"),
                    ["cor"]    = new OpenApiString("Preta"),
                    ["ano"]    = new OpenApiString("2022"),
                    ["clienteId"] = new OpenApiInteger(1)
                };
            }
            else if (path.StartsWith("api/patios") && (method == "POST" || method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["nome"]     = new OpenApiString("Pátio Central"),
                    ["endereco"] = new OpenApiString("Av. Lins de Vasconcelos, 1000")
                };
            }

            if (example is null) return;

            foreach (var mediaType in operation.RequestBody.Content.Values)
                mediaType.Example = example;
        }
    }
}
