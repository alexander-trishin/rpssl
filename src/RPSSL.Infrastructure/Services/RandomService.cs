using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RPSSL.Infrastructure.Services;

public sealed class RandomService : IRandomService
{
    private readonly HttpClient _httpClient;

    public RandomService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public Task<GetRandomResponse> GetRandomAsync(CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<GetRandomResponse>("/random", cancellationToken);
    }
}
