using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CW1_F21SC;

//This class contains functions for sending and receiving HTTP requests
public class HttpFunctions
{
    private readonly HttpClient _httpClient = new();
    private readonly ViewModel _viewModel;
    
    public HttpFunctions(ViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.StatusCode = HttpStatusCode.OK;
    }
    
    //Send a GET request to the specified URL and return the response as a string
    public async Task<(string content, HttpStatusCode statusCode)> SendGetRequest(string url)
    {

        //Check if the URL is absolute, if not, add http:// to the start of the URL
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            url = "http://" + url;
        }
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            
            _viewModel.StatusCode = response.StatusCode;
        
            if(response.StatusCode != HttpStatusCode.OK) //If the response is not OK, return the status code
            {
                return (null, response.StatusCode)!;
            }

            //If the response is successful, return the response as a string
            var content = await response.Content.ReadAsStringAsync();

            return (content, response.StatusCode);
        }
        
        //Catch invalid URLs
        catch (System.InvalidOperationException e)
        {
            Console.WriteLine($"Invalid URL, check if URL is absolute: {url} {e.Message}");
            return (null, HttpStatusCode.BadRequest)!;
        }
        
        catch (System.Net.Http.HttpRequestException e)
        {
            Console.WriteLine($"Invalid URL, host does not exist: {url} {e.Message}");
            return (null, HttpStatusCode.NotFound)!;
        }
        
        //Catch other exceptions
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
}