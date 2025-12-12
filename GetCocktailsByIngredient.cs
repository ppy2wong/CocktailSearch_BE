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

public class GetCocktailsByIngredient
{
    private readonly ILogger<GetCocktailsByIngredient> _logger;

    public GetCocktailsByIngredient(ILogger<GetCocktailsByIngredient> logger)
    {
        _logger = logger;
    }

    [Function("GetCocktailsByIngredient")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        using HttpClient client = new();;
        string cocktails = await Helper_GetCocktailsByIngredient(client, "cointreau");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-type", "application/json");
        await response.WriteStringAsync(cocktails);     

        return response;
    }

    private async Task<string> Helper_GetCocktailsByIngredient(HttpClient client, String ingredientName)
    {
        var json = await client.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={ingredientName}");
        return json;

    }
}