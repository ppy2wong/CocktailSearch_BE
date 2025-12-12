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

public class GetFullCocktailRecipe
{
    private readonly ILogger<GetFullCocktailRecipe> _logger;

    public GetFullCocktailRecipe(ILogger<GetFullCocktailRecipe> logger)
    {
        _logger = logger;
    }

    [Function("GetFullCocktailRecipe")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        using HttpClient client = new();;
        string cocktails = await Helper_GetFullCocktailRecipe(client, "11118");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-type", "application/json");
        await response.WriteStringAsync(cocktails);     

        return response;
    }

        private async Task<string> Helper_GetFullCocktailRecipe(HttpClient client, String drinkId)
    {
        var json = await client.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkId}");
        return json;

    }

}