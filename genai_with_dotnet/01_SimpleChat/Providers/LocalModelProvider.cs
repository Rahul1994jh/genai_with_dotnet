using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace _01_SimpleChat.Providers
{
    /// <summary>
    /// Provider for local models (e.g., Ollama, LM Studio, or any OpenAI-compatible local endpoint)
    /// </summary>
    public class LocalModelProvider : IModelProvider
    {
        private readonly ProviderSettings _settings;

        public string ProviderName => "Local Model";
        public string ModelName => _settings.ModelName;

        public LocalModelProvider(ProviderSettings settings)
        {
            _settings = settings;
        }

        public IChatClient CreateChatClient()
        {
            // For local models, we can use a dummy API key since most don't require authentication
            var client = new OpenAIClient(
                new ApiKeyCredential("local-key"), 
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(_settings.EndpointUrl)
                });
            
            var chatClient = client.GetChatClient(_settings.ModelName);
            return chatClient.AsIChatClient();
        }
    }
}
