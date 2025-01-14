namespace theo.crespo.ObjectDetection;
public record BoundingBoxDimensions
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}

public record BoundingBox
{
    public string Label { get; set; }
    public float Confidence { get; set; }
    public BoundingBoxDimensions Dimensions { get; set; }
}

public record ObjectDetectionResult
{
    public byte[] ImageData { get; set; }
    public IList<global::ObjectDetection.BoundingBox> Box { get; set; }
}
