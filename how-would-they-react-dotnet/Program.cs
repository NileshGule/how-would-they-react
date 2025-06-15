using OllamaSharp;
using Spectre.Console;
using Microsoft.Extensions.AI;
using Microsoft.AI.Foundry.Local;
using Microsoft.SemanticKernel;

// var uri = new Uri("http://localhost:11434");
var uri = new Uri("http://localhost:5273");
// var ollama = new OllamaApiClient(uri);

// var aliasOrModelId = "qwen2.5-1.5b-instruct-generic-gpu";
var aliasOrModelId = "deepseek-r1-distill-qwen-7b-generic-gpu"; 

// Foundry Local Initializations
var modelAlias = "phi-4";
        // Note: If the model isn't already on-box, it will download and may take a few minutes for this step.
        var manager = await FoundryLocalManager.StartModelAsync(aliasOrModelId);
        var model = await manager.GetModelInfoAsync(aliasOrModelId);

        // "Negative Space" programming
        if(manager == null || model == null)
        {
            throw new ArgumentNullException("Trouble initializing model");
        }

        var localEndpoint = manager.Endpoint; // http://localhost:NNNNN
        var localApiKey = manager.ApiKey; // "OPEN AI APIKEY"

        // Semantic Kernel Initializations
        var builder = Kernel.CreateBuilder();

        // Add OpenAI service
        builder.Services.AddOpenAIChatCompletion(
            modelId: model.ModelId,
            endpoint: localEndpoint,
            apiKey: localApiKey
        );

        var kernel = builder.Build();

        var celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate today? : ");

Console.WriteLine();
AnsiConsole.Markup($"How would [bold underline blue]{celebrityName}[/] reply to the following [bold underline blue]Tweet[/]?{Environment.NewLine}");
var tweet = Console.ReadLine();

        var result = kernel.InvokePromptStreamingAsync($"How would {celebrityName} reply to the following Tweet which says {tweet} ?");

// Stream the result.
await foreach (var chunk in result)
{
    // Console.Write(chunk);
    AnsiConsole.Markup($"[green]{chunk.ToString().EscapeMarkup()}[/]");
}



// ConversationContext context = null;
// context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} ?", context, stream => AnsiConsole.Markup($"[green]{stream.Response.EscapeMarkup()}[/]"));

Console.WriteLine();
Console.WriteLine();


var moods = new List<string> { 
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

Random random = new Random();

bool continueConversation;

do {
    
    continueConversation = AnsiConsole.Confirm("Would you like to continue the conversation and try some other celebrity?");
    
    Console.WriteLine();
    
    if (continueConversation)
    {
        celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate next? : ");

        int randomIndex = random.Next(moods.Count);
        string selectedMood = moods[randomIndex];

        Console.WriteLine();
        AnsiConsole.Markup($"This is how [bold underline blue]{celebrityName}[/] would react in [bold underline red]{selectedMood}[/] mood.");

        result = kernel.InvokePromptStreamingAsync($"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood? {Environment.NewLine}?");

        // Stream the result.
        await foreach (var chunk in result)
        {
            // Console.Write(chunk);
            AnsiConsole.Markup($"[green]{chunk.ToString().EscapeMarkup()}[/]");
        }

        // context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood? {Environment.NewLine}", context, stream =>
        // {
        //     AnsiConsole.Markup($"[green]{stream.Response.EscapeMarkup()}[/]");
                    
        // });
        Console.WriteLine();
        Console.WriteLine();
    }
    
} while (continueConversation);

Console.WriteLine("Goodbye! But wait ...");

result = kernel.InvokePromptStreamingAsync($"Say Goodbye in a very funny way based on current time of the day");

        // Stream the result.
        await foreach (var chunk in result)
        {
            // Console.Write(chunk);
            AnsiConsole.Markup($"[green]{chunk.ToString().EscapeMarkup()}[/]");
        }

// context = await ollama.StreamCompletion("Say Goodbye in a very funny way based on current time of the day", context, stream => Console.Write(stream.Response));


