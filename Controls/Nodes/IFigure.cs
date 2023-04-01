using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Teno.Controls.Nodes;

public interface IFigure
{
    public void Update(double deltaX, double deltaY, double zoom);
    public void Add(Canvas canvas);
    public void Remove(Canvas canvas);
}

public class GeometryFigure : IFigure
{
    private readonly Path _shape;
    
    public PathGeometry Path { get; }

    public GeometryFigure(PathGeometry path, IBrush fill, IBrush stroke)
    {
        Path = path;

        _shape = new()
        {
            Fill = fill,
            Stroke = stroke
        };
    }

    public void Update(double deltaX, double deltaY, double zoom)
    {
        _shape.Data = Path;
        var transform = new TransformGroup();
        
        transform.Children.Add(new TranslateTransform(deltaX, deltaY));
        transform.Children.Add(new ScaleTransform(zoom, zoom));
        
        _shape.RenderTransform = transform;
    }

    public void Add(Canvas canvas)
    {
        canvas.Children.Add(_shape);
    }

    public void Remove(Canvas canvas)
    {
        canvas.Children.Remove(_shape);
    }
}