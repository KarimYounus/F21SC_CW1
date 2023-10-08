using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CW1_F21SC;

//This class contains functions for sending and receiving HTTP requests
public class HttpFunctions
{
    private readonly HttpClient _httpClient = new();
    
    //Send a GET request to the specified URL and return the response as a string
    public async Task<(string content, HttpStatusCode statusCode)> SendGetRequest(string url)
    {
        var response = await _httpClient.GetAsync(url);

        //If the response is successful, return the response as a string
        var content = await response.Content.ReadAsStringAsync();

        return (content, response.StatusCode);

    }
}