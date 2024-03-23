using OllamaSharp;
using Spectre.Console;

var uri = new Uri("http://localhost:11434");
var ollama = new OllamaApiClient(uri);

// select a model which should be used for further operations
ollama.SelectedModel = "llama2";

var celebrityName = AnsiConsole.Ask<string>("Which famous person would you like to impersonate today? : ");

Console.WriteLine();
AnsiConsole.Markup($"How would [bold underline blue]{celebrityName}[/] reply to the following [bold underline blue]Tweet[/]?{Environment.NewLine}");
var tweet = Console.ReadLine();

ConversationContext context = null;
context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} ?", context, stream => AnsiConsole.Markup($"[green]{stream.Response.EscapeMarkup()}[/]"));

Console.WriteLine();
Console.WriteLine();


var moods = new List<string> { "happy", "sad", "angry", "excited", "bored", "confused", "surprised", "scared", "disgusted", "neutral" };
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

        context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood?", context, stream => AnsiConsole.Markup($"[green]{stream.Response.EscapeMarkup()}[/]"));

        Console.WriteLine();
        Console.WriteLine();
    }
    
} while (continueConversation); 

Console.WriteLine("Goodbye! But wait ...");
context = await ollama.StreamCompletion("Say Goodbye in a very funny way based on current time of the day", context, stream => Console.Write(stream.Response));


