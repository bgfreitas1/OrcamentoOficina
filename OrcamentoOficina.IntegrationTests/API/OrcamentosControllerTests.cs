using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OrcamentoOficina.IntegrationTests.Api;

public sealed class OrcamentosControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrcamentosControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ObterOrcamento_QuandoNaoExiste_DeveRetornar404()
    {
        var id = Guid.NewGuid();

        var response = await _client.GetAsync($"/orcamentos/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}