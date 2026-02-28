# 🤖 AI Chat Assistant - Multi-Provider Console Application

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue?style=flat-square)](LICENSE)

A flexible, production-ready console application for interacting with multiple AI model providers through a unified interface. Built with .NET 10 and Microsoft.Extensions.AI abstractions.

## ✨ Features

- 🔌 **Multi-Provider Support** - GitHub Models, Azure OpenAI, OpenAI, Local Models (Ollama/LM Studio)
- 🎨 **Interactive Console UI** - Beautiful menus with color-coded output
- ⚙️ **Runtime Configuration** - Select provider and adjust parameters without code changes
- 🔒 **Secure Secrets Management** - Built-in support for User Secrets
- 🏗️ **Clean Architecture** - Provider pattern with factory for easy extensibility
- 📦 **Microsoft.Extensions.AI** - Uses official Microsoft abstractions for AI services
- 🚀 **Ready to Run** - Complete setup guide with troubleshooting

---

## 📋 Table of Contents

1. [Quick Start (5 Minutes)](#-quick-start-5-minutes)
2. [What This Application Does](#-what-this-application-does)
3. [Architecture Overview](#-architecture-overview)
4. [Detailed Setup Guide](#-detailed-setup-guide)
5. [Provider Setup Guides](#-provider-setup-guides)
6. [Understanding Key Concepts](#-understanding-key-concepts)
7. [Learning Resources](#-learning-resources)
8. [Code Structure](#-code-structure)
9. [Configuration Reference](#-configuration-reference)
10. [Troubleshooting](#-troubleshooting)
11. [Contributing](#-contributing)

---

## 🚀 Quick Start (5 Minutes)

Get up and running with GitHub Models in under 5 minutes!

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed
- [GitHub Account](https://github.com) (free tier works)
- Terminal/PowerShell

### Steps

```bash
# 1. Clone the repository
git clone https://github.com/Rahul1994jh/genai_with_dotnet.git
cd genai_with_dotnet/genai_with_dotnet/01_SimpleChat

# 2. Restore packages
dotnet restore

# 3. Get GitHub token from: https://github.com/settings/tokens
# (Click "Generate new token (classic)", no scopes needed)

# 4. Store your token securely
dotnet user-secrets set "GitHub:Token" "ghp_your_token_here"

# 5. Run the application
dotnet run
```

**That's it!** 🎉 You'll see an interactive menu - select provider, configure options, and start chatting with AI!

---

## 🎯 What This Application Does

### The Problem It Solves

Different AI providers (GitHub, Azure, OpenAI, Ollama) have different APIs and configurations. This makes it:
- ❌ Hard to switch between providers
- ❌ Difficult to compare models
- ❌ Time-consuming to prototype
- ❌ Complex to learn AI development

### The Solution

This application provides a **unified interface** to interact with any AI provider:

```
┌─────────────────────────────────────────────────────┐
│  You type a question                                │
└────────────────┬────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────┐
│  Unified Interface (Microsoft.Extensions.AI)        │
└────────────────┬────────────────────────────────────┘
                 │
        ┌────────┴────────┬────────┬────────┐
        │                 │        │        │
        ▼                 ▼        ▼        ▼
   ┌────────┐      ┌────────┐  ┌────────┐  ┌────────┐
   │ GitHub │      │ Azure  │  │ OpenAI │  │  Local │
   │ Models │      │  AI    │  │  API   │  │  Model │
   └────────┘      └────────┘  └────────┘  └────────┘
```

### Real-World Use Cases

- 🎓 **Learning AI Development** - Experiment with different models
- 🔬 **Prototyping** - Quick proof-of-concepts with various providers
- 📊 **Model Comparison** - Test same prompt across different models
- 💰 **Cost Optimization** - Start with free (GitHub), scale to production (Azure)
- 🏠 **Privacy-First** - Use local models for sensitive data

---

## 🏗️ Architecture Overview

### Design Patterns Used

This application demonstrates several important software design patterns:

#### 1️⃣ **Provider Pattern** (Strategy Pattern)

**What it is:** Defines a family of algorithms (providers) and makes them interchangeable.

**Why it matters:** Add new AI providers without changing existing code.

```csharp
// All providers implement this interface
public interface IModelProvider
{
    string ProviderName { get; }
    string ModelName { get; }
    IChatClient CreateChatClient();
}

// Each provider has its own implementation
public class GitHubModelProvider : IModelProvider { ... }
public class AzureOpenAIProvider : IModelProvider { ... }
public class OpenAIProvider : IModelProvider { ... }
public class LocalModelProvider : IModelProvider { ... }
```

📚 **Learn More:**
- [Provider Pattern Explained](https://refactoring.guru/design-patterns/strategy)
- [Video: Strategy Pattern in C#](https://www.youtube.com/watch?v=v9ejT8FO-7I)

#### 2️⃣ **Factory Pattern**

**What it is:** Creates objects without specifying exact classes.

**Why it matters:** Centralize object creation logic and make it extensible.

```csharp
public static class ModelProviderFactory
{
    public static IModelProvider CreateProvider(string type, ...)
    {
        return type.ToLower() switch
        {
            "github" => new GitHubModelProvider(...),
            "azure" => new AzureOpenAIProvider(...),
            "openai" => new OpenAIProvider(...),
            "local" => new LocalModelProvider(...),
            _ => throw new NotSupportedException()
        };
    }
}
```

📚 **Learn More:**
- [Factory Pattern Explained](https://refactoring.guru/design-patterns/factory-method)
- [Video: Factory Pattern in C#](https://www.youtube.com/watch?v=EcFVTgRHJLM)

#### 3️⃣ **Dependency Injection (DI)**

**What it is:** Objects receive dependencies rather than creating them.

**Why it matters:** Makes code testable and loosely coupled.

```csharp
// Configuration is injected, not created inside
public static async Task RunAsync()
{
    var configuration = BuildConfiguration();  // DI container would handle this
    var settings = LoadSettings(configuration);
    var provider = Factory.CreateProvider(..., configuration);
}
```

📚 **Learn More:**
- [Dependency Injection Explained](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Video: DI in .NET](https://www.youtube.com/watch?v=YR6HkvNBpX4)

### System Architecture Diagram

```mermaid
graph TB
    subgraph "🎨 Presentation Layer"
        Console[Console UI<br/>Interactive Menus]
        Menu1[Provider Selection]
        Menu2[Options Configuration]
    end

    subgraph "💼 Application Layer"
        SimpleChat[SimpleChat.cs<br/>Main Orchestrator]
        Config[Configuration Manager]
    end

    subgraph "⚙️ Business Logic Layer"
        Factory[ModelProviderFactory<br/>Creates Providers]
        IProvider[IModelProvider<br/>Interface]
    end

    subgraph "🔌 Provider Implementations"
        GitHub[GitHubModelProvider]
        Azure[AzureOpenAIProvider]
        OpenAI[OpenAIProvider]
        Local[LocalModelProvider]
    end

    subgraph "🌐 External Services"
        GitHubAPI[GitHub Models API]
        AzureAPI[Azure OpenAI API]
        OpenAIAPI[OpenAI API]
        LocalAPI[Local Server<br/>Ollama/LM Studio]
    end

    Console --> SimpleChat
    SimpleChat --> Menu1
    SimpleChat --> Menu2
    SimpleChat --> Config
    SimpleChat --> Factory

    Factory --> IProvider
    IProvider -.implements.-> GitHub
    IProvider -.implements.-> Azure
    IProvider -.implements.-> OpenAI
    IProvider -.implements.-> Local

    GitHub --> GitHubAPI
    Azure --> AzureAPI
    OpenAI --> OpenAIAPI
    Local --> LocalAPI

    style Console fill:#e1f5ff
    style SimpleChat fill:#f3e5f5
    style Factory fill:#e8f5e9
    style GitHub fill:#fff3e0
    style Azure fill:#e3f2fd
    style OpenAI fill:#fce4ec
    style Local fill:#fff9c4
```

### Application Flow

```mermaid
sequenceDiagram
    participant User
    participant Console
    participant SimpleChat
    participant Factory
    participant Provider
    participant AIService

    User->>Console: Start App
    Console->>SimpleChat: RunAsync()

    SimpleChat->>User: Show Provider Menu
    User->>SimpleChat: Select GitHub

    SimpleChat->>User: Show Options Menu
    User->>SimpleChat: Configure (or use defaults)

    SimpleChat->>Factory: CreateProvider("GitHub", settings)
    Factory->>Provider: new GitHubModelProvider()
    Provider->>SimpleChat: return IChatClient

    loop Chat Session
        User->>SimpleChat: Type Question
        SimpleChat->>Provider: CompleteAsync(messages)
        Provider->>AIService: HTTP Request
        AIService->>Provider: AI Response
        Provider->>SimpleChat: ChatCompletion
        SimpleChat->>User: Display Answer
    end

    User->>SimpleChat: Type 'exit'
    SimpleChat->>User: Goodbye!
```

---

## 📦 Detailed Setup Guide

### Step 1: Install Prerequisites

#### Option A: Windows

1. **Install .NET 10 SDK**
   - Download: https://dotnet.microsoft.com/download/dotnet/10.0
   - Run the installer
   - Verify installation:
     ```powershell
     dotnet --version
     # Should output: 10.x.x
     ```

2. **Install Git** (if not installed)
   - Download: https://git-scm.com/download/win
   - Run installer with defaults

#### Option B: macOS

```bash
# Install .NET 10
brew install dotnet-sdk

# Verify
dotnet --version

# Git comes pre-installed, but you can update it
brew install git
```

#### Option C: Linux (Ubuntu/Debian)

```bash
# Install .NET 10
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 10.0

# Add to PATH (add to ~/.bashrc for persistence)
export PATH="$PATH:$HOME/.dotnet"

# Verify
dotnet --version

# Install Git
sudo apt-get update
sudo apt-get install git
```

### Step 2: Clone and Setup Project

```bash
# Clone repository
git clone https://github.com/Rahul1994jh/genai_with_dotnet.git

# Navigate to project
cd genai_with_dotnet/genai_with_dotnet/01_SimpleChat

# Restore NuGet packages
dotnet restore

# Build project (verify everything compiles)
dotnet build
```

**Expected output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Step 3: Understanding Project Structure

```
01_SimpleChat/
├── 📄 Program.cs                    # Entry point (Main method)
├── 📄 SimpleChat.cs                 # Core application logic
│   ├── RunAsync()                   # Main orchestrator
│   ├── DisplayProviderSelectionMenu()
│   ├── ConfigureChatOptions()
│   └── BuildMessages()
│
├── 📄 AppSettings.cs                # Configuration classes
│   ├── AppSettings                  # Root settings
│   ├── ProviderSettings             # Provider config
│   ├── ChatOptionsSettings          # Chat parameters
│   └── UiSettings                   # UI messages
│
├── 📄 appsettings.json             # Configuration file
│
├── 📁 Providers/                    # Provider implementations
│   ├── IModelProvider.cs           # Interface definition
│   ├── GitHubModelProvider.cs      # GitHub Models
│   ├── AzureOpenAIProvider.cs      # Azure OpenAI
│   ├── OpenAIProvider.cs           # OpenAI API
│   ├── LocalModelProvider.cs       # Local models
│   └── ModelProviderFactory.cs     # Factory pattern
│
├── 📄 README.md                     # This file!
├── 📄 ARCHITECTURE.md              # Detailed architecture diagrams
└── 📄 COMPLETE_GUIDE.md            # In-depth tutorials
```

### Step 4: Understanding User Secrets

**What are User Secrets?**

User Secrets is a .NET feature for securely storing sensitive data (like API keys) during development.

**Key Benefits:**
- ✅ **Not in source control** - Never accidentally commit API keys
- ✅ **Machine-specific** - Each developer has their own secrets
- ✅ **Easy to manage** - Simple CLI commands
- ✅ **Secure storage** - Stored outside project directory

**How it works:**

1. **Your project has a unique ID** (in `01_SimpleChat.csproj`):
   ```xml
   <UserSecretsId>2e90f260-646c-46da-8da8-50f0a867cf12</UserSecretsId>
   ```

2. **Secrets are stored separately:**
   - Windows: `%APPDATA%\Microsoft\UserSecrets\{id}\secrets.json`
   - macOS/Linux: `~/.microsoft/usersecrets/{id}/secrets.json`

3. **Application reads them at runtime:**
   ```csharp
   var configuration = new ConfigurationBuilder()
       .AddJsonFile("appsettings.json")
       .AddUserSecrets<Program>()  // ← Adds user secrets!
       .Build();
   ```

**Managing User Secrets:**

```bash
# Navigate to project directory first!
cd 01_SimpleChat

# Set a secret
dotnet user-secrets set "GitHub:Token" "your_token_value"

# List all secrets
dotnet user-secrets list

# Remove a specific secret
dotnet user-secrets remove "GitHub:Token"

# Clear all secrets
dotnet user-secrets clear
```

📚 **Learn More:**
- [Microsoft Docs: User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Video: Managing Secrets in .NET](https://www.youtube.com/watch?v=PkLLP2tcd28)

---

## 🔌 Provider Setup Guides

### 🟢 GitHub Models Setup

**Why GitHub Models?**
- ✅ **Free tier** - No credit card required
- ✅ **Multiple models** - Access to various AI models
- ✅ **Easy setup** - Just need a GitHub token
- ✅ **Great for learning** - Perfect for experimentation

#### Step 1: Create GitHub Personal Access Token

1. **Sign in to GitHub**  
   Go to https://github.com

2. **Navigate to Token Settings**  
   Profile Picture → Settings → Developer settings → Personal access tokens → Tokens (classic)

3. **Generate New Token**
   - Click "**Generate new token (classic)**"
   - Note: "AI Chat Assistant - GitHub Models"
   - Expiration: 90 days (recommended)
   - **Scopes: Leave all unchecked** (GitHub Models doesn't need permissions)
   - Click "**Generate token**"

4. **Copy Your Token**
   - Format: `ghp_1234567890abcdefghijklmnopqrstuvwxyzABCD`
   - ⚠️ **IMPORTANT**: Copy NOW - you can't see it again!
   - Store temporarily in a secure note

#### Step 2: Store Token in User Secrets

```bash
cd 01_SimpleChat

# Set the GitHub token
dotnet user-secrets set "GitHub:Token" "ghp_paste_your_token_here"

# Verify it's stored
dotnet user-secrets list
# Output: GitHub:Token = ghp_...
```

#### Step 3: Configure Provider

`appsettings.json` should have (already configured):

```json
{
  "SelectedProvider": "GitHub",
  "Providers": {
    "GitHub": {
      "EndpointUrl": "https://models.github.ai/inference",
      "ModelName": "mistral-ai/Ministral-3B",
      "TokenConfigKey": "GitHub:Token"
    }
  }
}
```

#### Step 4: Try Different Models

Edit `ModelName` in `appsettings.json`:

| Model Name | Size | Speed | Quality | Best For |
|------------|------|-------|---------|----------|
| `mistral-ai/Ministral-3B` | 3B | ⚡⚡⚡ Fast | ⭐⭐⭐ Good | General chat |
| `meta-llama/Llama-3.2-3B-Instruct` | 3B | ⚡⚡ Medium | ⭐⭐⭐⭐ Great | Instruction following |
| `microsoft/Phi-3-mini-4k-instruct` | 3.8B | ⚡⚡ Medium | ⭐⭐⭐⭐ Great | Code & technical |
| `openai/gpt-4o-mini` | N/A | ⚡ Slow | ⭐⭐⭐⭐⭐ Excellent | Advanced reasoning |

Browse all models: https://github.com/marketplace/models

#### Step 5: Run!

```bash
dotnet run
```

Select `[1] GitHub` when prompted, or press **Enter** for default.

---

### 🔵 Azure OpenAI Setup

**Why Azure OpenAI?**
- ✅ **Enterprise-grade** - SLA guarantees
- ✅ **Security** - GDPR, SOC 2 compliant
- ✅ **Latest models** - GPT-4, GPT-4 Turbo, etc.
- ✅ **Integration** - Works with Azure ecosystem

#### Step 1: Create Azure OpenAI Resource

1. **Sign in to Azure Portal**  
   https://portal.azure.com

2. **Create Resource**
   - Click "**Create a resource**"
   - Search for "**Azure OpenAI**"
   - Click "**Create**"

3. **Configure Resource**
   - **Subscription**: Choose your subscription
   - **Resource Group**: Create new or select existing
   - **Region**: Choose closest region (e.g., East US)
   - **Name**: `my-openai-resource` (must be unique)
   - **Pricing Tier**: Standard S0
   - Click "**Review + Create**" → "**Create**"

4. **Deploy a Model**
   - Once deployed, go to resource
   - Click "**Go to Azure OpenAI Studio**"
   - Navigate to "**Deployments**" → "**Create new deployment**"
   - Model: Select "**gpt-4**" or "**gpt-35-turbo**"
   - Deployment Name: `gpt-4` (remember this!)
   - Click "**Create**"

5. **Get Keys and Endpoint**
   - In Azure Portal, go to your OpenAI resource
   - Navigate to "**Keys and Endpoint**"
   - Copy:
     - **KEY 1** (your API key)
     - **Endpoint** URL

#### Step 2: Configure Application

```bash
# Store API key in user secrets
dotnet user-secrets set "Azure:ApiKey" "your_azure_key_here"
```

Edit `appsettings.json`:

```json
{
  "SelectedProvider": "Azure",
  "Providers": {
    "Azure": {
      "EndpointUrl": "https://your-resource.openai.azure.com",
      "ModelName": "gpt-4",
      "DeploymentName": "gpt-4",
      "TokenConfigKey": "Azure:ApiKey"
    }
  }
}
```

⚠️ **Important**: 
- `EndpointUrl` = Your endpoint from Azure Portal
- `DeploymentName` = Name you gave when deploying model
- `ModelName` = Display name (can be anything)

#### Step 3: Run

```bash
dotnet run
```

Select `[2] Azure` when prompted.

📚 **Learn More:**
- [Azure OpenAI Documentation](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
- [Video: Getting Started with Azure OpenAI](https://www.youtube.com/watch?v=Wby1-MYKudE)

---

### 🟣 OpenAI Setup

**Why OpenAI Direct?**
- ✅ **Latest models** - Fastest access to new releases
- ✅ **Simple setup** - Just API key needed
- ✅ **Great documentation** - Extensive examples
- ✅ **Pay-as-you-go** - No upfront costs

#### Step 1: Get OpenAI API Key

1. **Sign up at OpenAI**  
   https://platform.openai.com/signup

2. **Navigate to API Keys**  
   https://platform.openai.com/api-keys

3. **Create New Key**
   - Click "**Create new secret key**"
   - Name: "AI Chat Assistant"
   - Click "**Create**"

4. **Copy Key**
   - Format: `sk-proj-...` or `sk-...`
   - ⚠️ **Copy now** - can't see it again!

#### Step 2: Configure Application

```bash
# Store API key
dotnet user-secrets set "OpenAI:ApiKey" "sk-your_key_here"
```

Edit `appsettings.json`:

```json
{
  "SelectedProvider": "OpenAI",
  "Providers": {
    "OpenAI": {
      "ModelName": "gpt-4",
      "TokenConfigKey": "OpenAI:ApiKey"
    }
  }
}
```

**Available Models:**

| Model Name | Cost | Speed | Quality |
|------------|------|-------|---------|
| `gpt-4-turbo` | $$$ | ⚡ | ⭐⭐⭐⭐⭐ |
| `gpt-4` | $$$ | ⚡ | ⭐⭐⭐⭐⭐ |
| `gpt-3.5-turbo` | $ | ⚡⚡⚡ | ⭐⭐⭐⭐ |

#### Step 3: Run

```bash
dotnet run
```

Select `[3] OpenAI`.

📚 **Learn More:**
- [OpenAI API Documentation](https://platform.openai.com/docs)
- [Video: OpenAI API Quickstart](https://www.youtube.com/watch?v=3jJ2DQYL3H8)

---

### 🟡 Local Models Setup (Ollama)

**Why Local Models?**
- ✅ **Privacy** - Data never leaves your machine
- ✅ **No cost** - Run unlimited queries
- ✅ **Offline** - Works without internet
- ✅ **Customizable** - Fine-tune models

#### Step 1: Install Ollama

**Windows:**
1. Download: https://ollama.ai/download/windows
2. Run installer
3. Verify: Open PowerShell and type `ollama --version`

**macOS:**
```bash
brew install ollama
```

**Linux:**
```bash
curl -fsSL https://ollama.ai/install.sh | sh
```

#### Step 2: Download a Model

```bash
# Start Ollama service (runs in background)
ollama serve

# In another terminal, pull a model
ollama pull llama2           # 4GB model
# OR
ollama pull mistral          # 7GB model
# OR
ollama pull codellama        # 7GB model (great for code)

# List downloaded models
ollama list
```

#### Step 3: Verify Ollama is Running

```bash
# Should see: Ollama is running
curl http://localhost:11434
```

#### Step 4: Configure Application

Edit `appsettings.json`:

```json
{
  "SelectedProvider": "Local",
  "Providers": {
    "Local": {
      "EndpointUrl": "http://localhost:11434",
      "ModelName": "llama2",
      "TokenConfigKey": ""
    }
  }
}
```

**Note:** `TokenConfigKey` is empty because local models don't need authentication.

#### Step 5: Run

```bash
dotnet run
```

Select `[4] Local`.

📚 **Learn More:**
- [Ollama Documentation](https://github.com/ollama/ollama)
- [Video: Running AI Models Locally](https://www.youtube.com/watch?v=Wjrdr0NU4Sk)
- [Ollama Models Library](https://ollama.ai/library)

---

## 💡 Understanding Key Concepts

### 1. Microsoft.Extensions.AI

**What is it?**

Microsoft's official abstraction layer for AI services - think of it as a "standard interface" for talking to different AI providers.

**Why use it?**

Without Microsoft.Extensions.AI:
```csharp
// Different code for each provider 😩
if (provider == "Azure")
    var response = await azureClient.GetChatCompletionsAsync(...);
else if (provider == "OpenAI")
    var response = await openAiClient.CreateChatCompletionAsync(...);
else if (provider == "GitHub")
    var response = await githubClient.Complete(...);
```

With Microsoft.Extensions.AI:
```csharp
// Same code for all providers! 🎉
IChatClient client = GetClient(provider);
var response = await client.CompleteAsync(messages, options);
```

**Key Abstractions:**

| Type | Purpose | Example |
|------|---------|---------|
| `IChatClient` | Main interface for chat | `await client.CompleteAsync(...)` |
| `ChatMessage` | Represents a message | `new ChatMessage(ChatRole.User, "Hello")` |
| `ChatRole` | Message sender | `System`, `User`, `Assistant` |
| `ChatOptions` | Configuration | Temperature, MaxTokens, etc. |

📚 **Learn More:**
- [Microsoft.Extensions.AI Docs](https://learn.microsoft.com/en-us/dotnet/ai/)
- [Video: Introduction to Microsoft.Extensions.AI](https://www.youtube.com/watch?v=example)
- [Blog: Building AI Apps with .NET](https://devblogs.microsoft.com/dotnet/introducing-microsoft-extensions-ai-preview/)

### 2. Chat Roles

**What are Roles?**

In AI conversations, each message has a "role" that tells the AI who said it:

```csharp
// SYSTEM: Instructions to the AI
new ChatMessage(ChatRole.System, 
    "You are a helpful coding assistant. Provide concise answers.")

// USER: Your questions
new ChatMessage(ChatRole.User, 
    "How do I reverse a string in C#?")

// ASSISTANT: AI's responses (usually generated, not sent)
new ChatMessage(ChatRole.Assistant, 
    "You can use Array.Reverse() or LINQ...")
```

**Why does it matter?**

- 🎯 **System** - Sets behavior and personality
- 👤 **User** - Your actual questions
- 🤖 **Assistant** - For conversation history (multi-turn)

**Example Flow:**

```
[System] "You are a friendly teacher"
[User] "What is a variable?"
[Assistant] "A variable is like a labeled box..."
[User] "Can you give an example?"  ← Uses previous context!
[Assistant] "Sure! Think of name = 'John'..."
```

📚 **Learn More:**
- [OpenAI Guide: Chat Roles](https://platform.openai.com/docs/guides/chat)
- [Video: Understanding AI Chat Roles](https://www.youtube.com/watch?v=example)

### 3. Temperature

**What is Temperature?**

A number (0.0 to 2.0) that controls how "creative" or "random" the AI is.

**The Scale:**

```
0.0 ════════════ 0.7 ════════════ 1.5 ════════════ 2.0
Focused         Balanced       Creative         Chaotic
Deterministic   Varied         Imaginative      Random
Factual         Conversational Artistic         Unpredictable
```

**Examples:**

**Temperature = 0.0** (Focused)
```
Q: What is 2 + 2?
A: 4
A: 4
A: 4  (always same answer)
```

**Temperature = 0.7** (Balanced)
```
Q: Describe a sunset
A: "The sky fills with warm hues of orange and pink..."
A: "Golden rays paint the horizon as day turns to night..."
A: "Vibrant colors dance across the evening sky..."
```

**Temperature = 1.5** (Very Creative)
```
Q: Describe a sunset
A: "Celestial fire dances with liquid gold, painting dreams..."
A: "The sun whispers farewell in a symphony of colors..."
A: "Time melts into an ocean of amber light..."
```

**When to Use:**

| Task | Temperature | Why |
|------|-------------|-----|
| Math, Code, Facts | 0.0 - 0.3 | Need accuracy |
| Chatbot, Tutoring | 0.5 - 0.8 | Balance creativity/accuracy |
| Creative Writing | 0.8 - 1.2 | Want variety |
| Brainstorming | 1.0 - 1.5 | Maximum ideas |

📚 **Learn More:**
- [OpenAI: Temperature Parameter](https://platform.openai.com/docs/guides/text-generation/temperature)
- [Video: AI Temperature Explained](https://www.youtube.com/watch?v=example)

### 4. Max Output Tokens

**What are Tokens?**

Tokens are pieces of words. The AI thinks in tokens, not characters.

**Examples:**
- `"Hello"` = 1 token
- `"ChatGPT"` = 2 tokens (`Chat` + `GPT`)
- `"unbelievable"` = 3 tokens (`un` + `believ` + `able`)

**Rule of Thumb:**
- 1 token ≈ 0.75 words
- 100 tokens ≈ 75 words
- 1,000 tokens ≈ 750 words

**What is MaxOutputTokens?**

Limits how long the AI's response can be.

```csharp
new ChatOptions 
{
    MaxOutputTokens = 100  // AI can respond with ~75 words max
}
```

**Choosing the Right Value:**

| MaxTokens | ~Words | Use Case |
|-----------|--------|----------|
| 50-100 | 35-75 | Short answers, quick replies |
| 200-500 | 150-375 | Explanations, tutorials |
| 500-1000 | 375-750 | Detailed guides, essays |
| 1000-2000 | 750-1500 | Long-form content |

**Cost Consideration:**

Most APIs charge per token:
- OpenAI GPT-4: ~$0.03 per 1K tokens
- OpenAI GPT-3.5: ~$0.002 per 1K tokens
- Azure: Similar to OpenAI
- GitHub Models: **FREE** (with limits)
- Local: **FREE** (unlimited)

📚 **Learn More:**
- [OpenAI Tokenizer Tool](https://platform.openai.com/tokenizer)
- [Video: Understanding AI Tokens](https://www.youtube.com/watch?v=example)

### 5. Prompt Engineering

**What is it?**

The art of writing instructions that make AI give better responses.

**System Prompt (in BuildMessages()):**

```csharp
new ChatMessage(ChatRole.System,
    "You are a helpful, knowledgeable, and professional AI assistant. " +
    "Your goal is to provide accurate, clear, and useful responses.\n\n" +
    "Guidelines:\n" +
    "- Be concise but thorough\n" +
    "- Use friendly and professional tone\n" +
    "- Structure responses with bullet points\n" +
    "- Acknowledge limitations honestly")
```

**Effective Prompting Techniques:**

#### ✅ Be Specific
```
❌ Bad: "Explain variables"
✅ Good: "Explain variables in C# with code examples for beginners"
```

#### ✅ Provide Context
```
❌ Bad: "Fix this code"
✅ Good: "This C# code should reverse a string but throws NullReferenceException. 
         I'm a beginner. Please explain what's wrong and how to fix it."
```

#### ✅ Use Examples (Few-Shot Learning)
```
Prompt:
"Convert sentences to title case.
Examples:
- 'hello world' → 'Hello World'
- 'the quick brown fox' → 'The Quick Brown Fox'

Now convert: 'artificial intelligence rocks'"
```

📚 **Learn More:**
- [OpenAI Prompt Engineering Guide](https://platform.openai.com/docs/guides/prompt-engineering)
- [Video: Master Prompt Engineering](https://www.youtube.com/watch?v=example)
- [Blog: Prompt Engineering Best Practices](https://www.promptingguide.ai/)

---

## 📚 Learning Resources

### Official Documentation

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/ai/)
- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)

### Design Patterns

**Books:**
- [Design Patterns: Elements of Reusable Object-Oriented Software](https://www.amazon.com/Design-Patterns-Elements-Reusable-Object-Oriented/dp/0201633612) (Gang of Four)
- [Head First Design Patterns](https://www.amazon.com/Head-First-Design-Patterns-Brain-Friendly/dp/0596007124)

**Online:**
- [Refactoring Guru](https://refactoring.guru/design-patterns) - Interactive pattern catalog
- [Source Making](https://sourcemaking.com/design_patterns) - Patterns with examples

**Videos:**
- [Design Patterns in C# (Derek Banas)](https://www.youtube.com/playlist?list=PLF206E906175C7E07)
- [SOLID Principles (Tim Corey)](https://www.youtube.com/watch?v=example)

### AI/LLM Concepts

**Courses:**
- [DeepLearning.AI - ChatGPT Prompt Engineering](https://www.deeplearning.ai/short-courses/chatgpt-prompt-engineering-for-developers/)
- [Microsoft Learn - AI Fundamentals](https://learn.microsoft.com/en-us/training/paths/get-started-with-artificial-intelligence-on-azure/)

**Videos:**
- [But what is a GPT?](https://www.youtube.com/watch?v=wjZofJX0v4M) by 3Blue1Brown
- [Large Language Models Explained](https://www.youtube.com/watch?v=example)

**Blogs:**
- [OpenAI Blog](https://openai.com/blog/)
- [Microsoft AI Blog](https://blogs.microsoft.com/ai/)
- [Hugging Face Blog](https://huggingface.co/blog)

### .NET & C# Learning

**For Beginners:**
- [C# Fundamentals (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
- [.NET Tutorial (Tim Corey)](https://www.youtube.com/c/IAmTimCorey)

**For Intermediate:**
- [Async/Await in C#](https://docs.microsoft.com/en-us/dotnet/csharp/async)
- [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

**For Advanced:**
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://www.amazon.com/Domain-Driven-Design-Tackling-Complexity-Software/dp/0321125215)

### Community & Forums

- [Stack Overflow - C# Tag](https://stackoverflow.com/questions/tagged/c%23)
- [Reddit - r/csharp](https://www.reddit.com/r/csharp/)
- [Reddit - r/dotnet](https://www.reddit.com/r/dotnet/)
- [.NET Discord](https://discord.gg/dotnet)
- [C# Corner](https://www.c-sharpcorner.com/)

---

## 🗂️ Code Structure

### Project Files Explained

#### `Program.cs` - Entry Point

```csharp
// This is where the application starts
await SimpleChat.RunAsync();
```

**Purpose:** Minimal entry point that starts the application.

#### `SimpleChat.cs` - Main Orchestrator

**Methods:**

1. **`RunAsync()`** - Main application flow
   ```csharp
   public static async Task RunAsync()
   {
       // 1. Load configuration
       // 2. Display provider menu
       // 3. Configure chat options
       // 4. Create provider
       // 5. Start chat loop
   }
   ```

2. **`DisplayProviderSelectionMenu()`** - Interactive provider selection
   - Shows available providers
   - Validates user input
   - Returns selected provider name

3. **`ConfigureChatOptions()`** - Configure temperature & max tokens
   - Shows current defaults
   - Allows customization
   - Validates input ranges

4. **`BuildMessages()`** - Creates message list for AI
   - Adds system prompt
   - Adds user question
   - Returns formatted messages

5. **`BuildConfiguration()`** - Loads configuration
   - Reads appsettings.json
   - Merges user secrets
   - Returns IConfiguration

6. **`LoadSettings()`** - Binds configuration to classes
   - Uses Configuration.Bind()
   - Returns strongly-typed AppSettings

#### `AppSettings.cs` - Configuration Classes

```csharp
public class AppSettings
{
    public string SelectedProvider { get; set; }
    public Dictionary<string, ProviderSettings> Providers { get; set; }
    public ChatOptionsSettings ChatOptions { get; set; }
    public UiSettings UI { get; set; }
}
```

**Purpose:** Strongly-typed configuration objects.

#### Provider Files

**`IModelProvider.cs`** - Interface

```csharp
public interface IModelProvider
{
    string ProviderName { get; }      // Display name
    string ModelName { get; }          // Model being used
    IChatClient CreateChatClient();    // Creates chat client
}
```

**`GitHubModelProvider.cs`** - GitHub Implementation

```csharp
public class GitHubModelProvider : IModelProvider
{
    public IChatClient CreateChatClient()
    {
        // Uses Azure.AI.Inference (GitHub uses Azure format)
        var credential = new AzureKeyCredential(_token);
        return new ChatCompletionsClient(new Uri(_settings.EndpointUrl), credential)
            .AsIChatClient(_settings.ModelName);
    }
}
```

**`AzureOpenAIProvider.cs`** - Azure Implementation

```csharp
public class AzureOpenAIProvider : IModelProvider
{
    public IChatClient CreateChatClient()
    {
        // Uses Azure.AI.OpenAI
        var credential = new AzureKeyCredential(_token);
        var client = new AzureOpenAIClient(new Uri(_settings.EndpointUrl), credential);
        return client.GetChatClient(_settings.DeploymentName).AsIChatClient();
    }
}
```

**`OpenAIProvider.cs`** - OpenAI Implementation

```csharp
public class OpenAIProvider : IModelProvider
{
    public IChatClient CreateChatClient()
    {
        // Uses OpenAI SDK
        var client = new OpenAIClient(new ApiKeyCredential(_token));
        return client.GetChatClient(_settings.ModelName).AsIChatClient();
    }
}
```

**`LocalModelProvider.cs`** - Local Models Implementation

```csharp
public class LocalModelProvider : IModelProvider
{
    public IChatClient CreateChatClient()
    {
        // Uses OpenAI-compatible local endpoint
        var client = new OpenAIClient(
            new ApiKeyCredential("local-key"),  // Dummy key
            new OpenAIClientOptions { Endpoint = new Uri(_settings.EndpointUrl) });
        return client.GetChatClient(_settings.ModelName).AsIChatClient();
    }
}
```

**`ModelProviderFactory.cs`** - Factory

```csharp
public static IModelProvider CreateProvider(
    string providerType,
    ProviderSettings settings,
    IConfiguration configuration)
{
    // Get token from user secrets if needed
    var token = GetTokenIfNeeded(settings.TokenConfigKey, configuration);

    // Create appropriate provider
    return providerType.ToLower() switch
    {
        "github" => new GitHubModelProvider(settings, token),
        "azure" => new AzureOpenAIProvider(settings, token),
        "openai" => new OpenAIProvider(settings, token),
        "local" => new LocalModelProvider(settings),
        _ => throw new NotSupportedException($"Provider '{providerType}' not supported")
    };
}
```

### NuGet Packages Used

| Package | Purpose |
|---------|---------|
| `Microsoft.Extensions.AI` | Core AI abstractions |
| `Microsoft.Extensions.AI.OpenAI` | OpenAI integration |
| `Microsoft.Extensions.AI.AzureAIInference` | Azure AI Inference integration |
| `Azure.AI.Inference` | Azure AI Inference SDK |
| `Azure.AI.OpenAI` | Azure OpenAI SDK |
| `Microsoft.Extensions.Configuration` | Configuration framework |
| `Microsoft.Extensions.Configuration.Json` | JSON configuration |
| `Microsoft.Extensions.Configuration.Binder` | Configuration binding |
| `Microsoft.Extensions.Configuration.UserSecrets` | User secrets support |

---

## ⚙️ Configuration Reference

### appsettings.json Structure

```json
{
  "SelectedProvider": "GitHub",  // Default provider at startup

  "Providers": {
    // Provider configurations
    "GitHub": {
      "EndpointUrl": "https://models.github.ai/inference",
      "ModelName": "mistral-ai/Ministral-3B",
      "TokenConfigKey": "GitHub:Token"  // Key in user secrets
    },
    "Azure": {
      "EndpointUrl": "https://your-resource.openai.azure.com",
      "ModelName": "gpt-4",
      "DeploymentName": "gpt-4",  // Azure-specific
      "TokenConfigKey": "Azure:ApiKey"
    },
    "OpenAI": {
      "ModelName": "gpt-4",
      "TokenConfigKey": "OpenAI:ApiKey"
    },
    "Local": {
      "EndpointUrl": "http://localhost:11434",
      "ModelName": "llama2",
      "TokenConfigKey": ""  // No token needed for local
    }
  },

  "ChatOptions": {
    "MaxOutputTokens": 300,  // Default max response length
    "Temperature": 0.2       // Default creativity level
  },

  "UI": {
    "WelcomeTitle": "Welcome to AI Chat Assistant (Q&A)",
    "ExitMessage": "Thank you for using AI Chat Assistant. Goodbye! 👋"
  }
}
```

### User Secrets Structure

```json
{
  "GitHub:Token": "ghp_your_github_token_here",
  "Azure:ApiKey": "your_azure_api_key_here",
  "OpenAI:ApiKey": "sk-your_openai_key_here"
}
```

### Environment Variables (Alternative to User Secrets)

```bash
# Set environment variables
export GitHub__Token="ghp_your_token"
export Azure__ApiKey="your_key"
export OpenAI__ApiKey="sk_your_key"

# Note: Double underscore (__) replaces colon (:) in environment variables
```

---

## 🐛 Troubleshooting

### Common Issues

#### 1. "Token not found" Error

**Error:**
```
❌ Error initializing provider: Token not found for provider 'GitHub'. 
   Please set 'GitHub:Token' in user secrets.
```

**Solutions:**

✅ **Check if secret is set:**
```bash
cd 01_SimpleChat
dotnet user-secrets list
```

✅ **Set the secret:**
```bash
dotnet user-secrets set "GitHub:Token" "your_token"
```

✅ **Verify token format:**
- GitHub: `ghp_...`
- OpenAI: `sk-...`
- Azure: Usually alphanumeric

✅ **Check appsettings.json:**
Ensure `TokenConfigKey` matches the secret name:
```json
"TokenConfigKey": "GitHub:Token"  // Must match!
```

#### 2. "Provider not found" Error

**Error:**
```
❌ Error: Provider 'github' not found in configuration.
```

**Solutions:**

✅ **Check spelling in appsettings.json:**
```json
{
  "Providers": {
    "GitHub": { ... }  // Case-sensitive!
  }
}
```

✅ **Ensure provider exists in appsettings.json**

#### 3. HTTP 401/403 (Authentication Errors)

**Error:**
```
❌ Error: HTTP 401 (Unauthorized)
```

**Solutions:**

✅ **Regenerate token:**
- GitHub: https://github.com/settings/tokens
- OpenAI: https://platform.openai.com/api-keys
- Azure: Azure Portal → Your Resource → Keys

✅ **Check token expiration:**
Tokens can expire! Generate a new one.

✅ **Verify endpoint URL:**
For Azure:
```json
"EndpointUrl": "https://YOUR-RESOURCE.openai.azure.com"
//              ^^^^^^^^^^^^^^^^ Must match your resource name
```

#### 4. HTTP 429 (Rate Limit)

**Error:**
```
❌ Error: HTTP 429 (Too Many Requests)
```

**Solutions:**

✅ **Wait a few minutes** and try again

✅ **Check your usage limits:**
- GitHub Models: Free tier has limits
- OpenAI: Check your plan at https://platform.openai.com/usage
- Azure: Check quota in Azure Portal

✅ **Reduce MaxOutputTokens:**
```json
"ChatOptions": {
  "MaxOutputTokens": 100  // Lower value = fewer tokens used
}
```

#### 5. "Model not found" Error (Azure)

**Error:**
```
❌ Error: The API deployment for this resource does not exist.
```

**Solutions:**

✅ **Verify deployment name:**
In Azure Portal → Your OpenAI Resource → Deployments

✅ **Update appsettings.json:**
```json
"DeploymentName": "your-actual-deployment-name"
```

✅ **Ensure model is deployed:**
In Azure OpenAI Studio → Deployments → Check if model exists

#### 6. Local Model Connection Failed

**Error:**
```
❌ Error: Connection refused to localhost:11434
```

**Solutions:**

✅ **Start Ollama service:**
```bash
ollama serve
```

✅ **Verify Ollama is running:**
```bash
curl http://localhost:11434
# Should return: Ollama is running
```

✅ **Check if model is downloaded:**
```bash
ollama list
```

✅ **Pull model if needed:**
```bash
ollama pull llama2
```

#### 7. Build Errors

**Error:**
```
error CS0246: The type or namespace name 'X' could not be found
```

**Solutions:**

✅ **Restore packages:**
```bash
dotnet restore
```

✅ **Clean and rebuild:**
```bash
dotnet clean
dotnet build
```

✅ **Check .NET version:**
```bash
dotnet --version
# Should be 10.x.x
```

### Debug Mode

To see detailed error messages, run in debug mode:

```bash
dotnet run --configuration Debug
```

Or in Visual Studio: F5 (Start Debugging)

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### Adding a New Provider

1. **Create provider class** in `Providers/`:
```csharp
public class MyNewProvider : IModelProvider
{
    public string ProviderName => "My New Provider";
    public string ModelName => _settings.ModelName;

    public IChatClient CreateChatClient()
    {
        // Your implementation
    }
}
```

2. **Add to factory** (`ModelProviderFactory.cs`):
```csharp
return providerType.ToLower() switch
{
    "github" => new GitHubModelProvider(settings, token),
    "mynew" => new MyNewProvider(settings, token),  // ← Add here
    // ...
};
```

3. **Add configuration** (`appsettings.json`):
```json
"MyNew": {
  "EndpointUrl": "https://api.mynew.com",
  "ModelName": "model-name",
  "TokenConfigKey": "MyNew:ApiKey"
}
```

4. **Update README** with setup instructions

### Reporting Issues

- Use GitHub Issues: https://github.com/Rahul1994jh/genai_with_dotnet/issues
- Include:
  - Error message (full stack trace if possible)
  - Steps to reproduce
  - Operating system
  - .NET version (`dotnet --version`)

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -am 'Add my feature'`)
4. Push to branch (`git push origin feature/my-feature`)
5. Create Pull Request

---

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **Microsoft** - For .NET and Microsoft.Extensions.AI
- **OpenAI** - For pioneering LLM technology
- **Azure** - For enterprise AI services
- **GitHub** - For free model access
- **Ollama** - For making local AI easy

---

## 📞 Support & Contact

- **Issues**: [GitHub Issues](https://github.com/Rahul1994jh/genai_with_dotnet/issues)
- **Discussions**: [GitHub Discussions](https://github.com/Rahul1994jh/genai_with_dotnet/discussions)
- **Email**: [Your email if you want to share]

---

## 🚀 What's Next?

After mastering this application, consider:

1. **Add conversation history** - Store chat history across sessions
2. **Implement streaming responses** - See AI type in real-time
3. **Add function calling** - Let AI use tools/APIs
4. **Build a web UI** - Convert to ASP.NET Core app
5. **Add RAG** (Retrieval Augmented Generation) - Chat with your documents
6. **Multi-modal support** - Add image/audio capabilities

Check out the advanced tutorials in `COMPLETE_GUIDE.md`!

---

**⭐ If you find this helpful, please star the repository!**

**Made with ❤️ using .NET 10**
