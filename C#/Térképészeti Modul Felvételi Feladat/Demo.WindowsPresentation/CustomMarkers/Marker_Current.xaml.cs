using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;

namespace Demo.WindowsPresentation.CustomMarkers
{
   public partial class Marker_Current
   {
      GMapMarker Marker;
      MainWindow MainWindow;

      public Marker_Current(MainWindow window, GMapMarker marker, string tooltip)
      {
         this.InitializeComponent();

         this.MainWindow = window;
         this.Marker = marker;

         this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
         this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
         this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
         this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
         this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
         this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
         this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

         //create tooltip
         TextBlock textPinName = new TextBlock();
         textPinName.Foreground = Brushes.White;
         textPinName.HorizontalAlignment = HorizontalAlignment.Center;
         textPinName.VerticalAlignment = VerticalAlignment.Center;
         textPinName.Text = tooltip;

         Grid toolTipPanel = new Grid();
         //toolTipPanel.Children.Add(i); //can even add images and stuff
         toolTipPanel.Children.Add(textPinName);
         ToolTipService.SetToolTip(this, toolTipPanel);
      }

      void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
      {

      }

      void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         Marker.Offset = new Point(-e.NewSize.Width/2, -e.NewSize.Height/2);
      }

      void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
      {
         if(e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
         {
            Point p = e.GetPosition(MainWindow.MainMap);
            Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int) p.X, (int) p.Y);
         }
      }

      void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if(!IsMouseCaptured)
         {
            Mouse.Capture(this);
         }
      }

      void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         if(IsMouseCaptured)
         {
            Mouse.Capture(null);
         }
      }

      void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
      {

      }

      void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
      {

      }
   }
}