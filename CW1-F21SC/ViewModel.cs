using System.ComponentModel;
using System.Net;

namespace CW1_F21SC;

/// <summary>
/// This class is responsible for the view model of the application. It is used to store the status code of the current page.
/// </summary>

public class ViewModel : INotifyPropertyChanged
{
    private HttpStatusCode _statusCode;
    public HttpStatusCode StatusCode
    {
        get => _statusCode;
        set
        {
            _statusCode = value;
            OnPropertyChanged(nameof(StatusCode));
            OnPropertyChanged(nameof(FormatStatusCode));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public string FormatStatusCode => $"{(int)_statusCode} - {_statusCode}";

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}