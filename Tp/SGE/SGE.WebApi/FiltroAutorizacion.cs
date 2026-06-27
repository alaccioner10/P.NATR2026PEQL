using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SGE.WebApi;

public sealed class FiltroAutorizacion : IDocumentFilter
{
	public void Apply(OpenApiDocument documento, DocumentFilterContext contexto)
	{
		foreach (var apiDescription in contexto.ApiDescriptions)
		{
			var metadatosDelEndpoint = apiDescription.ActionDescriptor.EndpointMetadata;
			var tieneAutorizacion = metadatosDelEndpoint.OfType<AuthorizeAttribute>().Any();
			var permiteAnonimo = metadatosDelEndpoint.OfType<AllowAnonymousAttribute>().Any();

			if (!tieneAutorizacion || permiteAnonimo)
			{
				continue;
			}

			if (string.IsNullOrWhiteSpace(apiDescription.RelativePath) || string.IsNullOrWhiteSpace(apiDescription.HttpMethod))
			{
				continue;
			}

			var ruta = "/" + apiDescription.RelativePath.Split('?')[0].TrimEnd('/');
			if (ruta == "//")
			{
				ruta = "/";
			}

			if (!documento.Paths.TryGetValue(ruta, out var pathItem))
			{
				continue;
			}

			if (!pathItem.Operations.TryGetValue(new System.Net.Http.HttpMethod(apiDescription.HttpMethod), out var operation))
			{
				continue;
			}

			operation.Security ??= [];

			var yaMarcado = operation.Security.Any(requisito =>
				requisito.Keys.OfType<OpenApiSecuritySchemeReference>().Any(referencia =>
					string.Equals(referencia.Reference?.Id, "Bearer", StringComparison.Ordinal)));

			if (yaMarcado)
			{
				continue;
			}

			operation.Security.Add(new OpenApiSecurityRequirement
			{
				[new OpenApiSecuritySchemeReference("Bearer", documento)] = new List<string>()
			});
		}
	}
}