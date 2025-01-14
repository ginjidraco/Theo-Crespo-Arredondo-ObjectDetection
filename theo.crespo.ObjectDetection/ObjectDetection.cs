using System.Drawing;
using ObjectDetection;
using OpenCvSharp;      // Pour Cv2, Scalar, Point, etc.
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point; // Pour les conversions d'images


namespace theo.crespo.ObjectDetection;

public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        var results = new List<ObjectDetectionResult>();

        await Task.WhenAll(imagesSceneData.Select(async imageData =>
        {
            var yolo = new Yolo();
            YoloOutput detection = yolo.Detect(imageData);  // Récupère le résultat de détection
            using var img = Cv2.ImDecode(imageData, ImreadModes.Color);

            foreach (var box in detection.Boxes)
            {
                var startPoint = new Point(box.Dimensions.X, box.Dimensions.Y);
                var endPoint = new Point(box.Dimensions.X + box.Dimensions.Width, box.Dimensions.Y + box.Dimensions.Height);

                // Dessiner un rectangle rouge
                Cv2.Rectangle(img, startPoint, endPoint, Scalar.Red, 2);
                Cv2.PutText(img, $"{box.Label} ({box.Confidence:P1})", startPoint, HersheyFonts.HersheySimplex, 0.5, Scalar.Blue);
            }

            // Convertir l'image annotée en byte[]
            var annotatedImage = img.ToBytes(".png");
            results.Add(new ObjectDetectionResult
            {
                ImageData = annotatedImage,    // L'image d'entrée
                Box = detection.Boxes     // Les boîtes détectées
            });
        }));

        return results;
    }
}
