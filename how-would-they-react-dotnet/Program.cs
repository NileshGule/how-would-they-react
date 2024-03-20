using OllamaSharp;

var uri = new Uri("http://localhost:11434");
var ollama = new OllamaApiClient(uri);

// select a model which should be used for further operations
ollama.SelectedModel = "llama2";

Console.WriteLine("Which famous person would you like to impersonate today?");
var celebrityName = Console.ReadLine();

Console.WriteLine();
Console.WriteLine($"How would {celebrityName} reply to the following Tweet?");
var tweet = Console.ReadLine();

ConversationContext context = null;
context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} ?", context, stream => Console.Write(stream.Response));

Console.WriteLine();
Console.WriteLine();

Console.WriteLine("Would you like to continue the conversation and try some other celebrity? : Yes (Y) / No (N)");
var continueConversation = Console.ReadLine();

var moods = new List<string> { "happy", "sad", "angry", "excited", "bored", "confused", "surprised", "scared", "disgusted", "neutral" };
Random random = new Random();

while (continueConversation.ToLower() == "yes" || continueConversation.ToLower() == "y") 
{
    Console.WriteLine();

    Console.WriteLine("Which famous person would you like to impersonate next?");
    celebrityName = Console.ReadLine();

    int randomIndex = random.Next(moods.Count);
    string selectedMood = moods[randomIndex];

    Console.WriteLine();
    Console.WriteLine($"This is how {celebrityName} would react in {selectedMood} mood.");

    context = await ollama.StreamCompletion($"How would {celebrityName} reply to the following Tweet which says {tweet} in {selectedMood} mood?", context, stream => Console.Write(stream.Response));

    Console.WriteLine();
    Console.WriteLine();

    Console.WriteLine("Would you like to continue the conversation and try some other celebrity? : Yes (Y) / No (N)");
    continueConversation = Console.ReadLine();
}

Console.WriteLine("Goodbye! But wait ...");
context = await ollama.StreamCompletion("Say Goodbye in a very funny way based on current time of the day", context, stream => Console.Write(stream.Response));


