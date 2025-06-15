using OllamaSharp;
using Microsoft.AspNetCore.Mvc;

record RequestModel(string CelebrityName, string Tweet);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// var uri = new Uri("http://localhost:11434");
var uri = new Uri("http://localhost:1234");
var ollama = new OllamaApiClient(uri);
ollama.SelectedModel = "llama3.2";

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

app.MapPost("/react", async ([FromBody] RequestModel request) =>
{
    var response = "";
    await foreach (var stream in ollama.GenerateAsync($"How would {request.CelebrityName} reply to the following Tweet which says {request.Tweet} ?"))
    {
        response += stream.Response;
    }
    return Results.Ok(response);
});

app.MapPost("/react-with-mood", async ([FromBody] RequestModel request) =>
{
    int randomIndex = random.Next(moods.Count);
    string selectedMood = moods[randomIndex];

    var response = "";
    await foreach (var stream in ollama.GenerateAsync($"How would {request.CelebrityName} reply to the following Tweet which says {request.Tweet} in {selectedMood} mood?"))
    {
        response += stream.Response;
    }
    return Results.Ok(new { Mood = selectedMood, Response = response });
});

app.Run();

