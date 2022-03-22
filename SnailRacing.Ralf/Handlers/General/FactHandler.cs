using MediatR;
using Microsoft.Extensions.Logging;
using SnailRacing.Ralf.Discord.Handlers;
using SnailRacing.Ralf.Providers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SnailRacing.Ralf.Handlers.General
{
    public class FactHandler : IRequestHandler<FactRequest, FactResponse>
    {
        private IMediator _mediator;
        private IStorageProvider _storageProvider;
        private ILogger<ReactionAddedHandler> _logger;
        private readonly AppConfig _appConfig;

        public FactHandler(IMediator mediator, IStorageProvider storageProvider, ILogger<ReactionAddedHandler> logger, AppConfig appConfig)
        {
            _mediator = mediator;
            _storageProvider = storageProvider;
            _logger = logger;
            _appConfig = appConfig;
        }

        public async Task<FactResponse> Handle(FactRequest request, CancellationToken cancellationToken)
        {
            var fact = await GetFactAsync("/fact/random", _appConfig.FactsApiKey);
            return new FactResponse
            {
                Content = fact?.Contents?.Fact ?? "Larry is awesome, just saying."
            };
        }

        private static async Task<FactModel> GetFactAsync(string basePath, string apiKey)
        {
            var client = CreateFactsHttpClient(apiKey);
            FactModel? fact = null;

            HttpResponseMessage response = await client.GetAsync(basePath);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                fact = await JsonSerializer.DeserializeAsync<FactModel>(responseContent, options);
            }
            return fact ?? new ();
        }

        private static HttpClient CreateFactsHttpClient(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentException($"{nameof(apiKey)} must have a value");
            var client = new HttpClient();
            client.BaseAddress = new("https://api.fungenerators.com");
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Fungenerators-Api-Secret", apiKey);

            return client;
        }
    }
}
