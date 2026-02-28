using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace _01_SimpleChat.Providers
{
    public class OpenAIProvider : IModelProvider
    {
        private readonly ProviderSettings _settings;
        private readonly string _token;

        public string ProviderName => "OpenAI";
        public string ModelName => _settings.ModelName;

        public OpenAIProvider(ProviderSettings settings, string token)
        {
            _settings = settings;
            _token = token;
        }

        public IChatClient CreateChatClient()
        {
            var client = new OpenAIClient(new ApiKeyCredential(_token));
            var chatClient = client.GetChatClient(_settings.ModelName);
            return chatClient.AsIChatClient();
        }
    }
}
