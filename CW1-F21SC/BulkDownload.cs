using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CW1_F21SC;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public class Download
{
    public HttpStatusCode StatusCode { get; set; }
    public long FileSize { get; set; }
    
    public string DownloadUrl { get; set; }

    public Download(HttpStatusCode statusCode, long fileSize, string downloadUrl)
    {
        StatusCode = statusCode;
        FileSize = fileSize;
        DownloadUrl = downloadUrl;
    }
}

public class BulkDownload
{
    private string _fileName;
    private HttpFunctions _httpFunctions;

    public BulkDownload(string fileName = "bulk.txt")
    {
        _fileName = fileName;
        _httpFunctions = new HttpFunctions(new ViewModel());
    }
    
    public void SetFileName(string fileName)
    {
        _fileName = fileName;
    }

    // Method to read URLs from the specified file
    private IEnumerable<string>? ReadUrlsFile()
    {
        return !File.Exists(_fileName) ? null : File.ReadLines(_fileName);
    }

    public async Task<List<Download>>? DownloadFilesAsync()
    {
        var urls = ReadUrlsFile();
        var downloads = new List<Download>();
        if(urls == null)
        {
            Console.WriteLine("Error Reading File");
            return new List<Download>();
        }

        foreach (var url in ReadUrlsFile())
        {
            long fileSize = 0;
            // Send request and get content
            try
            {
                var response = await _httpFunctions.SendGetRequest(url, true);
                
                downloads.Add(new Download(response.statusCode, response.fileSize, url));
                
                // Output or Logging mechanism
                Console.WriteLine($"Status Code: {response.statusCode}, File Size: {response.fileSize} bytes, URL: {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Download Error: {ex.Message}");
                return null;
            }
        }
        return downloads;
    }
}