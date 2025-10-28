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
            var controller = context.ApiDescription.ActionDescriptor?.RouteValues.TryGetValue("controller", out var ctrl)
                == true ? ctrl?.ToLower() : null;

            OpenApiObject? example = null;

            // Support versioned routes like api/v1/... and conventional ones
            bool isClientes = (controller == "clientes") || path.Contains("/clientes");
            bool isVeiculos = (controller == "veiculos") || path.Contains("/veiculos");
            bool isPatios   = (controller == "patios")   || path.Contains("/patios");

            if (isClientes && (method == "POST"))
            {
                example = new OpenApiObject
                {
                    ["nome"] = new OpenApiString("Luigi Berzaghi"),
                    ["cpf"] = new OpenApiString("123.456.789-00"),
                    ["email"] = new OpenApiString("luigi@example.com"),
                    ["endereco"] = new OpenApiString("Guarulhos - SP")
                };
            }
            else if (isClientes && (method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["nome"] = new OpenApiString("Luigi Berzaghi"),
                    ["cpf"] = new OpenApiString("123.456.789-00"),
                    ["email"] = new OpenApiString("luigi@example.com"),
                    ["endereco"] = new OpenApiString("Santo André - SP")
                };
            }
            else if (isVeiculos && (method == "POST"))
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
            else if (isVeiculos && (method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["modelo"] = new OpenApiString("CG 160"),
                    ["placa"]  = new OpenApiString("ABC1D23"),
                    ["cor"]    = new OpenApiString("Azul"),
                    ["ano"]    = new OpenApiString("2022"),
                    ["clienteId"] = new OpenApiInteger(1)
                };
            }            
            else if (isPatios && (method == "POST"))
            {
                example = new OpenApiObject
                {
                    ["nome"]     = new OpenApiString("Pátio Central"),
                    ["endereco"] = new OpenApiString("Av. Lins de Vasconcelos, 1000")
                };
            }
            else if (isPatios && (method == "PUT"))
            {
                example = new OpenApiObject
                {
                    ["nome"]     = new OpenApiString("Pátio Central"),
                    ["endereco"] = new OpenApiString("Av. Lins de Vasconcelos, 5790")
                };
            }
            // ML predict-risk (POST)
            else if (path.Contains("/ml") && path.Contains("predict-risk") && (method == "POST"))
            {
                example = new OpenApiObject
                {
                    ["ano"] = new OpenApiInteger(2022),
                    ["quilometragem"] = new OpenApiInteger(1000)
                };
            }

            if (example is null) return;

            foreach (var mediaType in operation.RequestBody.Content.Values)
                mediaType.Example = example;
        }
    }
}
