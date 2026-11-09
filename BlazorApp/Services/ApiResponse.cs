using System.Text.Json;

namespace BlazorApp.Services;

internal static class ApiResponse
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T> ReadAsync<T>(HttpResponseMessage response)
    {
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        return JsonSerializer.Deserialize<T>(content, Options)!;
    }
}