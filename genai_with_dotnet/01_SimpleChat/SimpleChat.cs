using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using _01_SimpleChat.Providers;

namespace _01_SimpleChat
{
    internal class SimpleChat
    {
        public static async Task RunAsync(string? qustion = null)
        {
            var configuration = BuildConfiguration();
            var settings = LoadSettings(configuration);

            // Display provider selection menu
            var selectedProvider = DisplayProviderSelectionMenu(settings);
            if (!settings.Providers.TryGetValue(selectedProvider, out var providerSettings))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error: Provider '{selectedProvider}' not found in configuration.");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            // Create the model provider
            IModelProvider provider;
            try
            {
                provider = ModelProviderFactory.CreateProvider(selectedProvider, providerSettings, configuration);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Error initializing provider: {ex.Message}");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            // Get chat client from provider
            IChatClient client = provider.CreateChatClient();

            // Configure chat options
            var chatOptions = ConfigureChatOptions(settings);

            // Display welcome message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine($"║  {settings.UI.WelcomeTitle.PadRight(46)}║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine($"\nProvider: {provider.ProviderName}");
            Console.WriteLine($"Model: {provider.ModelName}");
            Console.WriteLine("Type your questions and press Enter. Type 'exit' or 'quit' to end the session.\n");

            // Interactive Q&A loop
            while (true)
            {
                // Get user input
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("You: ");
                Console.ResetColor();
                var userQuestion = Console.ReadLine();

                // Check for exit commands
                if (string.IsNullOrWhiteSpace(userQuestion) || 
                    userQuestion.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    userQuestion.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n{settings.UI.ExitMessage}");
                    Console.ResetColor();
                    break;
                }

                try
                {
                    // Build messages and get response
                    var messages = BuildMessages(userQuestion);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\nAssistant: ");
                    Console.ResetColor();

                    var response = await client.GetResponseAsync(messages, chatOptions);
                    Console.WriteLine(response.Text);
                    Console.WriteLine(); // Add blank line for readability
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n❌ Error: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine(); // Add blank line for readability
                }
            }
        }

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

        private static AppSettings LoadSettings(IConfiguration configuration)
        {
            var settings = new AppSettings();
            configuration.Bind(settings);
            return settings;
        }

        private static string DisplayProviderSelectionMenu(AppSettings settings)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║         Select AI Model Provider              ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            var providers = settings.Providers.ToList();

            // Display available providers
            for (int i = 0; i < providers.Count; i++)
            {
                var provider = providers[i];
                var isDefault = provider.Key.Equals(settings.SelectedProvider, StringComparison.OrdinalIgnoreCase);

                Console.ForegroundColor = isDefault ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.Write($"  [{i + 1}] {provider.Key}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($" - {provider.Value.ModelName}");
                Console.ResetColor();

                if (isDefault)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"      (Default)");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Enter your choice [1-{providers.Count}] (or press Enter for default): ");
            Console.ResetColor();

            var input = Console.ReadLine();

            // If empty, use default from settings
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Using default provider: {settings.SelectedProvider}");
                Console.ResetColor();
                Console.WriteLine();
                Thread.Sleep(1000); // Brief pause to show selection
                return settings.SelectedProvider;
            }

            // Parse selection
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= providers.Count)
            {
                var selected = providers[choice - 1].Key;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Selected: {selected}");
                Console.ResetColor();
                Console.WriteLine();
                Thread.Sleep(1000); // Brief pause to show selection
                return selected;
            }

            // Invalid selection - use default
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Invalid selection. Using default provider: {settings.SelectedProvider}");
            Console.ResetColor();
            Console.WriteLine();
            Thread.Sleep(1500); // Brief pause to show warning
            return settings.SelectedProvider;
        }

        private static ChatOptions ConfigureChatOptions(AppSettings settings)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║         Configure Chat Options                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            // Display current defaults
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Current Default Settings:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"  • Max Output Tokens: {settings.ChatOptions.MaxOutputTokens}");
            Console.WriteLine($"  • Temperature: {settings.ChatOptions.Temperature} (0.0 = focused, 1.0 = creative)");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press Enter to use defaults, or type 'c' to customize:");
            Console.ResetColor();
            Console.Write("> ");
            var input = Console.ReadLine();

            // Use defaults if Enter pressed
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Using default settings...");
                Console.ResetColor();
                Thread.Sleep(800);
                return new ChatOptions()
                {
                    MaxOutputTokens = settings.ChatOptions.MaxOutputTokens,
                    Temperature = settings.ChatOptions.Temperature
                };
            }

            // Customize options
            if (input.Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();

                // Get Max Output Tokens
                int maxTokens = settings.ChatOptions.MaxOutputTokens;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Enter Max Output Tokens (50-4000) [{settings.ChatOptions.MaxOutputTokens}]: ");
                Console.ResetColor();
                var tokensInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(tokensInput) && int.TryParse(tokensInput, out int parsedTokens))
                {
                    if (parsedTokens >= 50 && parsedTokens <= 4000)
                    {
                        maxTokens = parsedTokens;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✓ Set to {maxTokens} tokens");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠ Value out of range. Using default: {maxTokens}");
                        Console.ResetColor();
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tokensInput))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⚠ Invalid input. Using default: {maxTokens}");
                    Console.ResetColor();
                }

                Console.WriteLine();

                // Get Temperature
                float temperature = settings.ChatOptions.Temperature;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Enter Temperature (0.0-2.0) [{settings.ChatOptions.Temperature}]: ");
                Console.ResetColor();
                var tempInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(tempInput) && float.TryParse(tempInput, out float parsedTemp))
                {
                    if (parsedTemp >= 0.0f && parsedTemp <= 2.0f)
                    {
                        temperature = parsedTemp;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✓ Set to {temperature}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠ Value out of range. Using default: {temperature}");
                        Console.ResetColor();
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tempInput))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⚠ Invalid input. Using default: {temperature}");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Configuration complete! Max Tokens: {maxTokens}, Temperature: {temperature}");
                Console.ResetColor();
                Thread.Sleep(1500);

                return new ChatOptions()
                {
                    MaxOutputTokens = maxTokens,
                    Temperature = temperature
                };
            }

            // Default fallback
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Using default settings...");
            Console.ResetColor();
            Thread.Sleep(800);
            return new ChatOptions()
            {
                MaxOutputTokens = settings.ChatOptions.MaxOutputTokens,
                Temperature = settings.ChatOptions.Temperature
            };
        }

        private static IEnumerable<ChatMessage> BuildMessages(string userQuestion) =>
            new List<ChatMessage>()
            {
                new(Microsoft.Extensions.AI.ChatRole.System,
                    "You are a helpful, knowledgeable, and professional AI assistant. " +
                    "Your goal is to provide accurate, clear, and useful responses to user questions.\n\n" +
                    "Guidelines:\n" +
                    "- Be concise but thorough - provide enough detail to be helpful without being verbose\n" +
                    "- Use a friendly and professional tone\n" +
                    "- Break down complex topics into easy-to-understand explanations\n" +
                    "- If you're unsure about something, acknowledge it honestly\n" +
                    "- You don't have access to real-time information, current events, or the open web\n" +
                    "- If asked about current events or live data, politely explain your limitations\n" +
                    "- Structure your responses with bullet points or numbered lists when appropriate\n" +
                    "- Provide examples when they help clarify your explanations\n" +
                    "- If a question is ambiguous, ask for clarification\n" +
                    "- Be objective and balanced in your responses\n" +
                    "- Cite your reasoning when making recommendations"),

                new(Microsoft.Extensions.AI.ChatRole.User, userQuestion)
            };
    }
}
