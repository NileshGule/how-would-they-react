﻿using Spectre.Console;

using Microsoft.AI.Foundry.Local;
using System.ClientModel;
using System.Text;

using OpenAI;
using OpenAI.Chat;

// Configurable settings (can be moved to appsettings or env vars)
var uri = new Uri(Environment.GetEnvironmentVariable("LLM_ENDPOINT") ?? "http://localhost:5273");
// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "deepseek-r1-distill-qwen-7b-generic-gpu";
// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "qwen2.5-1.5b-instruct-generic-gpu";

// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "phi-4";

var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "Phi-4-mini-reasoning-generic-gpu";

// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "mistralai-Mistral-7B-Instruct-v0-2-generic-gpu";


var moods = new List<string>
{
    "joyful",
    "sad",
    "angry",
    "fearful",
    "surprised",
    "excited",
    "curious",
    "bored",
    "confident",
    "nervous",
};


// Foundry Local Initializations
var manager = await FoundryLocalManager.StartModelAsync(aliasOrModelId);
var model = await manager.GetModelInfoAsync(aliasOrModelId);

if (manager == null || model == null)
    throw new ArgumentNullException("Trouble initializing model");

var localEndpoint = manager.Endpoint;
var localApiKey = manager.ApiKey;


var key = new ApiKeyCredential(manager.ApiKey);
var openAiClient = new OpenAIClient(key, new OpenAIClientOptions
{
    Endpoint = manager.Endpoint
});

await RunConversationAsync();

void PrintWelcomeMessage()
{
    AnsiConsole.MarkupLine("[bold underline blue]Welcome to the How Would They React?[/]");
    AnsiConsole.MarkupLine("This is a fun way to see how famous personalities would react to tweets.");
    AnsiConsole.MarkupLine("You can ask how a celebrity would respond to a tweet, and even change their mood for different reactions.");

    Console.WriteLine();

    AnsiConsole.MarkupLine("Let's get started!");

    Console.WriteLine();

    AnsiConsole.MarkupLine("You can type in the name of a celebrity, and I will generate a response as if they were replying to a tweet.");
    Console.WriteLine();

    AnsiConsole.MarkupLine("You can also specify a mood, and I will generate a response based on that mood.");
    Console.WriteLine();
}

// Conversation loop logic
async Task RunConversationAsync()
{
    PrintWelcomeMessage();
    // Prints the welcome and instructions to the user

    var celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate today? : ");
    Console.WriteLine();

    AnsiConsole.Markup($"How would [bold underline blue]{celebrityName}[/] reply to the following [bold underline blue]Tweet[/]?{Environment.NewLine}");

    var tweet = Console.ReadLine();

    var prompt = $"How would {celebrityName} reply to the following Tweet which says {tweet}? ";

    await StreamAndPrintResponse(openAiClient, prompt);

    Console.WriteLine();
    Console.WriteLine();


    await ContinueConversationLoop(openAiClient, moods, tweet, celebrityName);

    Console.WriteLine("Goodbye! But wait ...");

    prompt = $"Say Goodbye in a very funny way based on current time of the day";
    await StreamAndPrintResponse(openAiClient, prompt);
}

async Task ContinueConversationLoop(OpenAIClient openAiClient, List<string> moods, string tweet, string celebrityName)
{
    var random = new Random();
    bool continueConversation;
    do
    {
        continueConversation = AnsiConsole.Confirm("Would you like to continue and try to impersonate some other celebrity?");
        Console.WriteLine();
        if (continueConversation)
        {
            celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate next? : ");

            int randomIndex = random.Next(moods.Count);
            string selectedMood = moods[randomIndex];

            Console.WriteLine();

            AnsiConsole.Markup($"This is how [bold underline blue]{celebrityName}[/] would react in [bold underline red]{selectedMood}[/] mood.");

            var prompt = $"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood?";

            Console.WriteLine();

            await StreamAndPrintResponse(openAiClient, prompt);

            Console.WriteLine();
            Console.WriteLine();
        }
    } while (continueConversation);
}




// Helper method for streaming and printing results with truncation handling
async Task StreamAndPrintResponse(OpenAIClient client, string prompt)
{
    var chatClient = client.GetChatClient(model?.ModelId);

    var systemMessage = "You are an expert impersonator of celebrities. Always reply in the style, tone, and personality of the requested celebrity, and use emojis and slang to make it realistic. Limit the response to 300 characters, as if it were a tweet. If the response is too long, truncate it to fit within the character limit.";

    List<ChatMessage> messages =
    [
        // System messages represent instructions or other guidance about how the assistant should behave
        new SystemChatMessage($"{systemMessage }, Use emojis where appropriate to make the tweet sound more realistic."),
        // User messages represent user input, whether historical or the most recent input
        new UserChatMessage(prompt)
    ];

    var chatCompletionsOptions = new ChatCompletionOptions
    {
        MaxOutputTokenCount = 1024 * 4, // Adjust as needed
    };

    AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = chatClient.CompleteChatStreamingAsync(messages, chatCompletionsOptions);

    await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
    {
        if (completionUpdate.ContentUpdate.Count > 0)
        {
            AnsiConsole.Markup($"[green]{completionUpdate.ContentUpdate[0].Text.EscapeMarkup()}[/]");
        }
    }
}
