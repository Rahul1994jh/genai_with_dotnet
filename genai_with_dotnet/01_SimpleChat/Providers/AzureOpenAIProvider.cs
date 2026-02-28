using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace _01_SimpleChat.Providers
{
    public class AzureOpenAIProvider : IModelProvider
    {
        private readonly ProviderSettings _settings;
        private readonly string _token;

        public string ProviderName => "Azure OpenAI";
        public string ModelName => _settings.ModelName;

        public AzureOpenAIProvider(ProviderSettings settings, string token)
        {
            _settings = settings;
            _token = token;
        }

        public IChatClient CreateChatClient()
        {
            var credential = new AzureKeyCredential(_token);
            var client = new AzureOpenAIClient(new Uri(_settings.EndpointUrl), credential);
            
            // For Azure, we use the deployment name
            var chatClient = client.GetChatClient(_settings.DeploymentName);
            return chatClient.AsIChatClient();
        }
    }
}
