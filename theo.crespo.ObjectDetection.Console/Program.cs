using theo.crespo.ObjectDetection;
using System.Text.Json;

var folderPath = args[0];
var imageFiles = Directory.GetFiles(folderPath, "*.jpg");

var imagesData = imageFiles.Select(File.ReadAllBytes).ToList();
var detection = new theo.crespo.ObjectDetection.ObjectDetection();
var results = await detection.DetectObjectInScenesAsync(imagesData);

foreach (var result in results)
{
    Console.WriteLine(JsonSerializer.Serialize(result.Box));
}