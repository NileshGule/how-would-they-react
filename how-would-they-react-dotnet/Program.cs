using Spectre.Console;
using Microsoft.Extensions.AI;
using Microsoft.AI.Foundry.Local;
using System.ClientModel;

using OpenAI;
using OpenAI.Chat;

// Configurable settings (can be moved to appsettings or env vars)
var uri = new Uri(Environment.GetEnvironmentVariable("LLM_ENDPOINT") ?? "http://localhost:5273");
// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "deepseek-r1-distill-qwen-7b-generic-gpu";
var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "qwen2.5-1.5b-instruct-generic-gpu";


var moods = new List<string> {
    "happy", "sad", "angry", "excited", "bored", "confused", "surprised", "scared", "disgusted", "neutral"
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




// Conversation loop logic
async Task RunConversationAsync()
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
    var celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate today? : ");
    Console.WriteLine();

    AnsiConsole.Markup($"How would [bold underline blue]{celebrityName}[/] reply to the following [bold underline blue]Tweet[/]?{Environment.NewLine}");

    var tweet = Console.ReadLine();

    // Instead of passing maxTokens here, use the default overload
    // var result = openAiClient.InvokePromptStreamingAsync($"How would {celebrityName} reply to the following Tweet which says {tweet} ?");

    var prompt = $"How would {celebrityName} reply to the following Tweet which says {tweet} ?";

    await StreamAndPrintOpenAIAsync(openAiClient, aliasOrModelId, prompt);

    Console.WriteLine();
    Console.WriteLine();

    var random = new Random();

    bool continueConversation;

    do
    {
        continueConversation = AnsiConsole.Confirm("Would you like to continue the conversation and try some other celebrity?");
        Console.WriteLine();
        if (continueConversation)
        {
            celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate next? : ");
            int randomIndex = random.Next(moods.Count);
            string selectedMood = moods[randomIndex];
            Console.WriteLine();
            AnsiConsole.Markup($"This is how [bold underline blue]{celebrityName}[/] would react in [bold underline red]{selectedMood}[/] mood.");
            prompt = $"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood?";
            await StreamAndPrintOpenAIAsync(openAiClient, aliasOrModelId, prompt);
            Console.WriteLine();
            Console.WriteLine();
        }
    } while (continueConversation);

    Console.WriteLine("Goodbye! But wait ...");

    prompt = "Say Goodbye in a very funny way based on current time of the day";
    await StreamAndPrintOpenAIAsync(openAiClient, aliasOrModelId, prompt);
}

await RunConversationAsync();


// Helper method for streaming and printing results with truncation handling
async Task<string> StreamAndPrintOpenAIAsync(OpenAIClient client, string deploymentOrModelName, string prompt, int maxTokens = 1024*8)
{

    var chatClient = client.GetChatClient(model?.ModelId);

    CollectionResult<StreamingChatCompletionUpdate> completionUpdates = chatClient.CompleteChatStreaming(prompt);

    foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
    {
        if (completionUpdate.ContentUpdate.Count > 0)
        {
            AnsiConsole.Markup($"[green]{completionUpdate.ContentUpdate[0].Text.EscapeMarkup()}[/]");
        }
    }
    
    return string.Empty;

    // var chatCompletionsOptions = new ChatCompletionsOptions()
    // {
    //     Messages = { new ChatMessage(ChatRole.User, prompt) },
    //     MaxTokens = maxTokens,
    //     Temperature = 0.7f,
    //     NucleusSamplingFactor = 0.95f,
    //     Stream = true
    // };
    // var response = new StringBuilder();
    // string? finishReason = null;
    // await foreach (StreamingChatCompletionsUpdate update in client.GetChatCompletionsStreaming(deploymentOrModelName, chatCompletionsOptions))
    // {
    //     foreach (var choice in update.Choices)
    //     {
    //         if (!string.IsNullOrEmpty(choice.Delta.Content))
    //         {
    //             AnsiConsole.Markup($"[green]{choice.Delta.Content.EscapeMarkup()}[/]");
    //             response.Append(choice.Delta.Content);
    //         }
    //         if (!string.IsNullOrEmpty(choice.FinishReason))
    //         {
    //             finishReason = choice.FinishReason;
    //         }
    //     }
    // }
    // Console.WriteLine();
    // // If truncated, request continuation
    // if (finishReason == "length")
    // {
    //     AnsiConsole.Markup("[yellow]\n[Truncated: requesting more...]\n[/]");
    //     response.Append(await StreamAndPrintOpenAIAsync(client, deploymentOrModelName, response.ToString(), maxTokens));
    // }
    // return response.ToString();
}
