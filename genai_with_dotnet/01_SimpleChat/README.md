# AI Chat Assistant - Multi-Provider Support

This project supports multiple AI model providers with easy switching between them.

## Supported Providers

### 1. **GitHub Models** (Default)
- Uses GitHub's AI model inference endpoint
- Requires: GitHub token
- Free tier available for testing

### 2. **Azure OpenAI**
- Uses Azure's OpenAI service
- Requires: Azure OpenAI resource and API key
- Enterprise-grade with SLA guarantees

### 3. **OpenAI**
- Direct connection to OpenAI API
- Requires: OpenAI API key
- Access to latest GPT models

### 4. **Local Models**
- Run models locally (Ollama, LM Studio, etc.)
- No API key required
- Full privacy and offline capability

## Quick Start

### 1. Configure Your Provider

Edit `appsettings.json` and set the `SelectedProvider`:

```json
{
  "SelectedProvider": "GitHub"  // Options: GitHub, Azure, OpenAI, Local
}
```

### 2. Set Up Authentication

Set your API keys in user secrets (recommended) or environment variables:

#### For GitHub Models:
```bash
dotnet user-secrets set "GitHub:Token" "your_github_token" --project 01_SimpleChat
```

#### For Azure OpenAI:
```bash
dotnet user-secrets set "Azure:ApiKey" "your_azure_key" --project 01_SimpleChat
```

#### For OpenAI:
```bash
dotnet user-secrets set "OpenAI:ApiKey" "your_openai_key" --project 01_SimpleChat
```

#### For Local Models (no key needed):
Just make sure your local model server is running (e.g., Ollama on `localhost:11434`)

### 3. Run the Application
```bash
dotnet run --project 01_SimpleChat
```

## Configuration Details

### appsettings.json Structure

```json
{
  "SelectedProvider": "GitHub",
  "Providers": {
    "GitHub": {
      "EndpointUrl": "https://models.github.ai/inference",
      "ModelName": "mistral-ai/Ministral-3B",
      "TokenConfigKey": "GitHub:Token"
    },
    "Azure": {
      "EndpointUrl": "https://YOUR-RESOURCE.openai.azure.com",
      "ModelName": "gpt-4",
      "DeploymentName": "gpt-4",
      "TokenConfigKey": "Azure:ApiKey"
    },
    "OpenAI": {
      "ModelName": "gpt-4",
      "TokenConfigKey": "OpenAI:ApiKey"
    },
    "Local": {
      "EndpointUrl": "http://localhost:11434",
      "ModelName": "llama2",
      "TokenConfigKey": ""
    }
  }
}
```

## Switching Between Providers

Simply change the `SelectedProvider` value in `appsettings.json`:

```json
"SelectedProvider": "Azure"  // Switch to Azure OpenAI
```

Or use environment variable:
```bash
export SelectedProvider="Local"
dotnet run --project 01_SimpleChat
```

## Provider-Specific Setup

### GitHub Models
1. Get token from: https://github.com/settings/tokens
2. No specific scopes required for GitHub Models
3. Free tier available

### Azure OpenAI
1. Create Azure OpenAI resource in Azure Portal
2. Deploy a model (e.g., gpt-4, gpt-35-turbo)
3. Get endpoint URL and API key
4. Update `EndpointUrl` and `DeploymentName` in config

### OpenAI
1. Sign up at: https://platform.openai.com
2. Create API key in dashboard
3. Choose from available models: gpt-4, gpt-3.5-turbo, etc.

### Local Models (Ollama Example)
1. Install Ollama: https://ollama.ai
2. Pull a model: `ollama pull llama2`
3. Ollama runs on `localhost:11434` by default
4. Update `ModelName` to match pulled model

## Chat Options

Adjust model behavior in `appsettings.json`:

```json
"ChatOptions": {
  "MaxOutputTokens": 300,    // Max response length
  "Temperature": 0.2         // 0-1, lower = more focused
}
```

## Architecture

The project uses the **Provider Pattern** for flexibility:

- `IModelProvider` - Interface for all providers
- `GitHubModelProvider` - GitHub Models implementation
- `AzureOpenAIProvider` - Azure OpenAI implementation
- `OpenAIProvider` - OpenAI implementation
- `LocalModelProvider` - Local model implementation
- `ModelProviderFactory` - Creates the appropriate provider

## Adding New Providers

To add a new provider:

1. Create a class implementing `IModelProvider`
2. Add configuration to `appsettings.json`
3. Update `ModelProviderFactory` with new case
4. Set up authentication if needed

## Troubleshooting

### "Token not found" Error
- Ensure you've set the user secret for your selected provider
- Check `TokenConfigKey` matches in appsettings.json

### "Provider not found" Error
- Verify `SelectedProvider` value exists in `Providers` section
- Check spelling (case-insensitive)

### Connection Errors (Local Models)
- Ensure your local model server is running
- Verify endpoint URL and port
- Check model name matches exactly

## Best Practices

1. **Use User Secrets** for API keys (never commit to source control)
2. **Environment-Specific Config** - Create `appsettings.Development.json` for dev settings
3. **Provider Costs** - Be aware of token costs for cloud providers
4. **Rate Limits** - Different providers have different rate limits
5. **Model Capabilities** - Not all models support the same features

## License

MIT License - See LICENSE file for details
