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
using System.Security.Cryptography.X509Certificates;
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
        string? name = req.Query.Get("name");
        var response = req.CreateResponse();;


        if(name == null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            await response.WriteStringAsync("Please enter the name of the cocktail to search by.");
        }
        else
        {
            using HttpClient client = new();;

            string cocktails = await Helper_GetCocktailsByName(client, name);

            response.StatusCode = HttpStatusCode.OK;
            response.Headers.Add("Content-type", "application/json");
            await response.WriteStringAsync(cocktails);     
            
        }
        return response;

    }

    private async Task<string> Helper_GetCocktailsByName(HttpClient client, String cocktailName)
    {
        var json = await client.GetStringAsync($"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={cocktailName}");
        return json;

    }
}