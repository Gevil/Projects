using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Windows.Shapes;

namespace Demo.WindowsPresentation.CustomMarkers
{
    public partial class PolygonMarker
    {
        GMapMarker Marker;
        MainWindow MainWindow;
        Brush color;
        Popup Popup;

        public PolygonMarker(MainWindow window, GMapMarker marker)
        {
            this.InitializeComponent();

            this.MainWindow = window;
            this.Marker = marker;

            Popup = new Popup();

            this.Loaded += new RoutedEventHandler(PolygonMarker_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(PolygonMarker_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove += new MouseEventHandler(PolygonMarker_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(PolygonMarker_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(PolygonMarker_MouseLeftButtonDown);

        }
        
        void PolygonMarker_Loaded(object sender, RoutedEventArgs e)
        {

        }

        void PolygonMarker_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void PolygonMarker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                Point p = e.GetPosition(MainWindow.MainMap);
                Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            }
        }

        void PolygonMarker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
        }

        void PolygonMarker_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (color != null)
            {
                (this.Marker.Shape as Path).Fill = color;
                (this.Marker.Shape as Path).Stroke = color;
            }
        }

        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            color = (this.Marker.Shape as Path).Fill;

            (this.Marker.Shape as Path).Fill = Brushes.Red;
            (this.Marker.Shape as Path).Stroke = Brushes.Red;
        }

        //Delete
        private void item1_Click(object sender, RoutedEventArgs e)
        {

        }

        //Edit
        private void item0_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}