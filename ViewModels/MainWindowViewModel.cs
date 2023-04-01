using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Media;
using Teno.Controls.Nodes;

namespace Teno.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<IFigure> Figures { get; }
    
    public MainWindowViewModel()
    {
        var points1 = new List<Point>
        {
            new(30,30),
            new(80,20),
            new(70,70),
            new(40,50)
        };
        var points2 = new List<Point>()
        {
            new(130,130),
            new(180,120),
            new(170,170),
        };
        
        var data1 = new PathGeometry
        {
            Figures = new PathFigures
            { 
                new () { Segments = new PathSegments{ new PolyLineSegment(points1)} }
            }
        };

        var data2 = new PathGeometry()
        {
            Figures = new PathFigures
            { 
                new () { Segments = new PathSegments{ new PolyLineSegment(points2)} }
            }
        };
        
        Figures = new ObservableCollection<IFigure>()
        {
            new GeometryFigure(data1, Brushes.Red, Brushes.Red),
            new GeometryFigure(data2, Brushes.White, Brushes.White)
        };
    }
}