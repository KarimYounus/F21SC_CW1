using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CW1_F21SC;

//This class contains functions for sending HTTP requests
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
    public async Task<(string content, HttpStatusCode statusCode, long fileSize)> SendGetRequest(string url, bool download = false)
    {
        
        //Check if the URL is absolute, if not, add http:// to the start of the URL
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) url = "http://" + url;
        
        try
        {
            //Send a GET request to the URL
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);
            //Store the status code in the view model
            _viewModel.StatusCode = response.StatusCode;
            
            //If the response is not OK, return the status code
            if(response.StatusCode != HttpStatusCode.OK) 
            {
                return (null, response.StatusCode, 0)!;
            }
            
            //If the response is successful, store the response as a string
            var content = await response.Content.ReadAsStringAsync();

            //If the download flag is set, return the content and the file size
            if (download)
            {
                var length = response.Content.Headers.ContentLength;
                //Check for the Content-Length header, if it doesn't exist, return 0
                var fileSize = length ?? 0;
                return (content, response.StatusCode, fileSize);
            }
            
            //If the download flag is not set, return the content and the status code
            return (content, response.StatusCode, 0);
        }
        
        //Catch invalid URLs
        catch (System.InvalidOperationException e)
        {
            Console.WriteLine($"Invalid URL, check if URL is absolute: {url} {e.Message}");
            return (null, HttpStatusCode.BadRequest, 0)!;
        }
        
        catch (System.Net.Http.HttpRequestException e)
        {
            Console.WriteLine($"Invalid URL, host does not exist: {url} {e.Message}");
            return (null, HttpStatusCode.NotFound, 0)!;
        }
        
        //Catch other exceptions
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    
}