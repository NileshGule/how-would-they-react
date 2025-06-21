using Spectre.Console;

using Microsoft.AI.Foundry.Local;
using System.ClientModel;
using System.Text;

using OpenAI;
using OpenAI.Chat;

// Configurable settings (can be moved to appsettings or env vars)
var uri = new Uri(Environment.GetEnvironmentVariable("LLM_ENDPOINT") ?? "http://localhost:5273");
// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "deepseek-r1-distill-qwen-7b-generic-gpu";
// var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "qwen2.5-1.5b-instruct-generic-gpu";

var aliasOrModelId = Environment.GetEnvironmentVariable("LLM_MODEL_ID") ?? "mistralai-Mistral-7B-Instruct-v0-2-generic-gpu";


var moods = new List<string>
{
    "happy",
    "sad",
    "angry",
    "excited",
    "bored",
    "confused",
    "surprised",
    "scared",
    "disgusted",
    "neutral"
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




// System message for LLM context
var systemMessage = "You are an expert impersonator of celebrities. Always reply in the style, tone, and personality of the requested celebrity, and use emojis and slang to make it realistic.";

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


    // var prompt = $"{systemMessage}{Environment.NewLine}User: How would {celebrityName} reply to the following Tweet which says {tweet}? Use emojis where appropriate to make the tweet sound funny.";
    var prompt = $"How would {celebrityName} reply to the following Tweet which says {tweet}? ";
    await StreamAndPrintResponse(openAiClient, aliasOrModelId, prompt);

    Console.WriteLine();
    Console.WriteLine();

    var random = new Random();

    await ContinueConversationLoop(openAiClient, aliasOrModelId, moods, random, tweet, celebrityName);

    Console.WriteLine("Goodbye! But wait ...");
    
    prompt = $"Say Goodbye in a very funny way based on current time of the day";
    await StreamAndPrintResponse(openAiClient, aliasOrModelId, prompt);
}

async Task ContinueConversationLoop(OpenAIClient openAiClient, string aliasOrModelId, List<string> moods, Random random, string tweet, string celebrityName)
{
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

            await StreamAndPrintResponse(openAiClient, aliasOrModelId, prompt);

            Console.WriteLine();
            Console.WriteLine();
        }
    } while (continueConversation);
}

await RunConversationAsync();


// Helper method for streaming and printing results with truncation handling
async Task<string> StreamAndPrintResponse(OpenAIClient client, string deploymentOrModelName, string prompt, int maxTokens = 1024*8)
{

    var chatClient = client.GetChatClient(model?.ModelId);
    
    List<ChatMessage> messages = 
    [
        // System messages represent instructions or other guidance about how the assistant should behave
        new SystemChatMessage($"{systemMessage }, Use emojis where appropriate to make the tweet sound funny"),
        // User messages represent user input, whether historical or the most recen tinput
        new UserChatMessage(prompt)
    ];

    AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = chatClient.CompleteChatStreamingAsync(messages);

    await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
    {
        if (completionUpdate.ContentUpdate.Count > 0)
        {
            AnsiConsole.Markup($"[green]{completionUpdate.ContentUpdate[0].Text.EscapeMarkup()}[/]");

        }

    }


    return string.Empty;
}
