using theo.crespo.ObjectDetection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Ajouter Swagger pour documenter l'API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activer Swagger uniquement en développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/ObjectDetection", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count < 1)
        return Results.BadRequest("Veuillez fournir une image.");

    // Lecture de l'image envoyée
    using var sceneSourceStream = files[0].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    await sceneSourceStream.CopyToAsync(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    // Utilisation de la librairie de détection
    var detector = new theo.crespo.ObjectDetection.ObjectDetection();
    var results = await detector.DetectObjectInScenesAsync(new List<byte[]> { imageSceneData });

    // Récupérer l'image annotée depuis le résultat
    var annotatedImage = results[0].ImageData;

    // Retourner l'image annotée
    return Results.File(annotatedImage, "image/png");
}).DisableAntiforgery();

app.Run();