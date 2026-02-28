using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;

namespace _01_SimpleChat.Providers
{
    public class GitHubModelProvider : IModelProvider
    {
        private readonly ProviderSettings _settings;
        private readonly string _token;

        public string ProviderName => "GitHub Models";
        public string ModelName => _settings.ModelName;

        public GitHubModelProvider(ProviderSettings settings, string token)
        {
            _settings = settings;
            _token = token;
        }

        public IChatClient CreateChatClient()
        {
            var credential = new AzureKeyCredential(_token);
            
            return new ChatCompletionsClient(new Uri(_settings.EndpointUrl), credential)
                .AsIChatClient(_settings.ModelName);
        }
    }
}
