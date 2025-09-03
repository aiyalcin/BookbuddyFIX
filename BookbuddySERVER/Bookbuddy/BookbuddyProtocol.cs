using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class BookbuddyProtocol
{
    private string GOOGLEAPI = $"https://www.googleapis.com/books/v1/volumes?q=isbn:";
    private static readonly HttpClient client = new HttpClient();

    public string ProcessInput(string input)
    {
        string url = GOOGLEAPI + input;
        try
        {
            string response = client.GetStringAsync(url).GetAwaiter().GetResult();
            return response;
        }
        catch (Exception e)
        {
            return $"Error: {e.Message}";
        }
    }
}
