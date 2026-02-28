using Microsoft.Extensions.Configuration;

namespace _01_SimpleChat.Providers
{
    public class ModelProviderFactory
    {
        public static IModelProvider CreateProvider(
            string providerType, 
            ProviderSettings settings, 
            IConfiguration configuration)
        {
            // Get token if required (not needed for local models)
            var token = string.Empty;
            if (!string.IsNullOrEmpty(settings.TokenConfigKey))
            {
                token = configuration[settings.TokenConfigKey];
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException(
                        $"Token not found for provider '{providerType}'. " +
                        $"Please set '{settings.TokenConfigKey}' in user secrets.");
                }
            }

            return providerType.ToLower() switch
            {
                "github" => new GitHubModelProvider(settings, token),
                "azure" => new AzureOpenAIProvider(settings, token),
                "openai" => new OpenAIProvider(settings, token),
                "local" => new LocalModelProvider(settings),
                _ => throw new NotSupportedException($"Provider '{providerType}' is not supported. " +
                    $"Supported providers: GitHub, Azure, OpenAI, Local")
            };
        }
    }
}
