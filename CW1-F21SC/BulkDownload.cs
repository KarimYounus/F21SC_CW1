using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CW1_F21SC;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public class BulkDownload
{
    private readonly string _fileName;
    private HttpFunctions _httpFunctions;

    public BulkDownload(string fileName = "bulk.txt")
    {
        _fileName = fileName;
        _httpFunctions = new HttpFunctions(new ViewModel());
    }

    // Method to read URLs from the specified file
    private IEnumerable<string> ReadUrlsFile()
    {
        if (!File.Exists(_fileName))
        {
            throw new FileNotFoundException($"The file {_fileName} was not found.");
        }
        return File.ReadLines(_fileName);
    }

    public async Task DownloadFilesAsync()
    {
        var urls = ReadUrlsFile();
        foreach (var url in ReadUrlsFile())
        {
            long fileSize = 0;
            // Send request and get content
            try
            {
                var response = await _httpFunctions.SendGetRequest(url, true);
                
                // Output or Logging mechanism
                Console.WriteLine($"Status Code: {response.statusCode}, File Size: {response.fileSize} bytes, URL: {url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}