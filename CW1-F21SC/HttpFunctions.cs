using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CW1_F21SC;

public class HttpFunctions
{
    private readonly HttpClient _httpClient = new();
    
    //Send a GET request to the specified URL and return the response as a string
    public async Task<string> SendGetRequest(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}