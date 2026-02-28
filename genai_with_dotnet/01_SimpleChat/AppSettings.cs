namespace _01_SimpleChat
{
    public class AppSettings
    {
        public string SelectedProvider { get; set; } = "GitHub";
        public Dictionary<string, ProviderSettings> Providers { get; set; } = new();
        public ChatOptionsSettings ChatOptions { get; set; } = new();
        public UiSettings UI { get; set; } = new();
    }

    public class ProviderSettings
    {
        public string EndpointUrl { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string TokenConfigKey { get; set; } = string.Empty;
        public string DeploymentName { get; set; } = string.Empty; // For Azure
    }

    public class ChatOptionsSettings
    {
        public int MaxOutputTokens { get; set; }
        public float Temperature { get; set; }
    }

    public class UiSettings
    {
        public string WelcomeTitle { get; set; } = string.Empty;
        public string ExitMessage { get; set; } = string.Empty;
    }
}
