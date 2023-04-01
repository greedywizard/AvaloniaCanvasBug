using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Teno.Controls.Nodes;

public class NodeViewport : TemplatedControl, IStyleable
{
    private Canvas _canvas;
    
    private const string PART_MainCanvas = "PART_MainCanvas";
    
    public static StyledProperty<double> PointerXProperty =
        AvaloniaProperty.Register<NodeViewport, double>(nameof(PointerX));

    public static StyledProperty<double> PointerYProperty =
        AvaloniaProperty.Register<NodeViewport, double>(nameof(PointerY));

    public static StyledProperty<ObservableCollection<IFigure>> FiguresProperty =
        AvaloniaProperty.Register<NodeViewport, ObservableCollection<IFigure>>(nameof(Figures));

    private double _deltaX = 120;
    private double _deltaY = 120;
    private double _zoom;
    private bool _moving;
    private Point _lastPoint;

    public double PointerX
    {
        get => GetValue(PointerXProperty);
        set => SetValue(PointerXProperty, value);
    }

    public double PointerY
    {
        get => GetValue(PointerYProperty);
        set => SetValue(PointerYProperty, value);
    }

    public double DeltaX
    {
        get => _deltaX;
        set
        {
            _deltaX = value;
            Update();
        }
    }

    public double DeltaY
    {
        get => _deltaY; 
        set
        {
            _deltaY = value;
            Update();
        }
    }

    public double Zoom { get; set; } = 1;

    [Content]
    public ObservableCollection<IFigure> Figures
    {
        get => GetValue(FiguresProperty);
        set => SetValue(FiguresProperty, value);
    }

    Type IStyleable.StyleKey => typeof(NodeViewport);
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _canvas = e.NameScope.Find(PART_MainCanvas) as Canvas
                  ?? throw new Exception($"Cant find {PART_MainCanvas}");
        
        _canvas.PointerMoved += CanvasOnPointerMoved;
        _canvas.PointerPressed += CanvasOnPointerPressed;
        _canvas.PointerReleased += CanvasOnPointerReleased;

        foreach (var figure in Figures)
        {
            figure.Add(_canvas);
            figure.Update(DeltaX, DeltaY, Zoom);
        }
    }

    private void CanvasOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _moving = false;
    }

    private void CanvasOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _moving = true;
        e.Pointer.Capture(_canvas);
        _lastPoint = e.GetPosition(_canvas);
    }

    private void CanvasOnPointerMoved(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(_canvas);

        PointerX = pos.X;
        PointerY = pos.Y;

        if (_moving)
        {
            var point = e.GetPosition(_canvas);

            var (deltaX, deltaY) = point - _lastPoint;
            DeltaX += deltaX;
            DeltaY += deltaY;
            
            _lastPoint = point;
        }
    }
    
    private void Update()
    {
        foreach (var figure in Figures)
        {
            figure.Update(DeltaX, DeltaY, Zoom);
        }
    }
}