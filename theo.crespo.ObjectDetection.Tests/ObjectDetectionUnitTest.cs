using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace theo.crespo.ObjectDetection.Tests;
public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageScenesData);
        Assert.Equal("[{\"Dimensions\":{\"X\":63.104874,\"Y\":129.16133,\"Height\":154.22751,\"Width\":231.16074},\"Label\":\"tvmonitor\",\"Confidence\":0.8109468}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Box));
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        return Path.GetDirectoryName(executingAssemblyPath);
    }
}
