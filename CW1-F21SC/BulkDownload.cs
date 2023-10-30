using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CW1_F21SC;

/// <summary>
/// This class is responsible for bulk downloading files from a list of URLs. It reads the URLs from a file and
/// sends a GET request to each URL using the HttpFunctions class. It then returns the status code, file size and URL
/// to the console.
/// </summary>

// Class to store the download information
public class Download
{
    public HttpStatusCode StatusCode { get; } // The status code of the request
    public long FileSize { get; } // The size of the file
    public string DownloadUrl { get; } // The URL of the file

    // Constructor
    public Download(HttpStatusCode statusCode, long fileSize, string downloadUrl)
    {
        StatusCode = statusCode;
        FileSize = fileSize;
        DownloadUrl = downloadUrl;
    }
}

// Class to bulk download files
public class BulkDownload
{
    private string _fileName; // The name of the file containing the URLs
    private HttpFunctions _httpFunctions; // The HttpFunctions object to send the GET requests

    
    // Constructor with default file name
    public BulkDownload(string fileName = "bulk.txt")
    {
        _fileName = fileName;
        _httpFunctions = new HttpFunctions(new ViewModel());
    }
    

    // Method to read URLs from the specified file
    private IEnumerable<string>? ReadUrlsFile()
    {
        // Check if the file exists, if not return null
        return !File.Exists(_fileName) ? null : File.ReadLines(_fileName);
    }

    
    // Method to download files from the URLs in the file
    public async Task<List<Download>>? DownloadFilesAsync()
    {
        var downloads = new List<Download>();
        
        // Read the URLs from the file
        var urls = ReadUrlsFile();
        // If the file doesn't exist, return empty list
        if(urls == null)
        {
            Console.WriteLine("Error Reading File");
            return new List<Download>();
        }

        // Loop through each URL
        foreach (var url in ReadUrlsFile())
        {
            try
            {
                // Send a GET request to the current URL with download set to true
                var response = await _httpFunctions.SendGetRequest(url, true);
                
                // Add the download to the list
                downloads.Add(new Download(response.statusCode, response.fileSize, url));
                Console.WriteLine($"Status Code: {response.statusCode}, File Size: {response.fileSize} bytes, URL: {url}");
            }
            catch (Exception ex)
            {
                // If an exception is thrown, return null
                Console.WriteLine($"Download Error: {ex.Message}");
                return null;
            }
        }
        // Return the list of downloads containing the status code, file size and URL
        return downloads;
    }
}