using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

namespace _01_SimpleChat
{
    internal class SimpleChat
    {
        private const string TokenConfigKey = "GitHub:Token";
        private const string EndpointUrl = "https://models.github.ai/inference";
        private const string ModelName = "mistral-ai/Ministral-3B";

        public static async Task RunAsync(string? qustion = null)
        {
            var configuration = BuildConfiguration();
            var token = GetTokenOrExit(configuration);

            var credential = new AzureKeyCredential(token);

            IChatClient client = new ChatCompletionsClient(new Uri(EndpointUrl), credential)
                .AsIChatClient(ModelName);

            var userQuestion = qustion ?? "What is the meaning of life?";
            var messages = BuildMessages(userQuestion);
            var chatOptions = new ChatOptions()
            {
                MaxOutputTokens = 300,
                Temperature = 0.2f
            };

            var response = await client.GetResponseAsync(messages, chatOptions);
            Console.WriteLine(response.Text);
        }

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

        private static string GetTokenOrExit(IConfiguration configuration)
        {
            var token = configuration[TokenConfigKey];
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Please set the {TokenConfigKey} in user secrets and try again.");
                Environment.Exit(1);
            }

            return token;
        }

        private static IEnumerable<ChatMessage> BuildMessages(string userQuestion) =>
            new List<ChatMessage>()
            {
                new(Microsoft.Extensions.AI.ChatRole.System,
                    "You are a concise balanced assistant. " +
                    "You don't have access to real-time information or the open web. " +
                    "If asked for current or live data, say you don't have real-time access. " +
                    "Be clear and structured and avoid definitive claims unless clearly justified"),

                new(Microsoft.Extensions.AI.ChatRole.User, userQuestion)
            };
    }
}
