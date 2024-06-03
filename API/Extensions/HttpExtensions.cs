using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
    {
        // JsonSerializerOptions converts from C# object to JSON object so that it can go back with the Http Header
        var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions)); 
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}
}