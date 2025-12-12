using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Net.CocktailSearch;

public class GetCocktailsByName
{
    private readonly ILogger<GetCocktailsByName> _logger;

    public GetCocktailsByName(ILogger<GetCocktailsByName> logger)
    {
        _logger = logger;
    }

    [Function("GetCocktailsByName")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        using HttpClient client = new();;
        string cocktails = await Helper_GetCocktailsByName(client, "margarita");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-type", "application/json");
        await response.WriteStringAsync(cocktails);     

        return response;
    }

    private async Task<string> Helper_GetCocktailsByName(HttpClient client, String cocktailName)
    {
        var json = await client.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={cocktailName}");
        return json;

    }
}