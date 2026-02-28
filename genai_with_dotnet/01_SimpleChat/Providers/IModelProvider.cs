using Microsoft.Extensions.AI;

namespace _01_SimpleChat.Providers
{
    public interface IModelProvider
    {
        string ProviderName { get; }
        string ModelName { get; }
        IChatClient CreateChatClient();
    }
}
