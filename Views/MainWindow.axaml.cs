using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Teno.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}