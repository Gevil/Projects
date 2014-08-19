using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Diagnostics;
using System.Windows.Shapes;

namespace Demo.WindowsPresentation.CustomMarkers
{
   /// <summary>
   /// Interaction logic for CustomMarkerDemo.xaml
   /// </summary>
   public partial class CustomMarkerDemo
   {
      Popup Popup;
      Label Label;
      GMapMarker Marker;
      MainWindow MainWindow;

      public CustomMarkerDemo(MainWindow window, GMapMarker marker, string title)
      {
         this.InitializeComponent();

         this.MainWindow = window;
         this.Marker = marker;

         Popup = new Popup();
         Label = new Label();

         this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
         this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
         this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
         this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
         this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
         this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
         this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
         this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);
         //this.OnRender += new RoutedEventHandler(CustomMarkerDemo_OnRender);

         //Popup.Placement = PlacementMode.Mouse;
         //{
         //   Label.Background = Brushes.Blue;
         //   Label.Foreground = Brushes.White;
         //   Label.BorderBrush = Brushes.WhiteSmoke;
         //   Label.BorderThickness = new Thickness(2);
         //   Label.Padding = new Thickness(5);
         //   Label.FontSize = 22;
         //   Label.Content = title;
         //}
         //Popup.Child = Label;

         //create tooltip
         TextBlock textPinName = new TextBlock();
         //textPinName.Foreground = Brushes.White;
         textPinName.HorizontalAlignment = HorizontalAlignment.Center;
         textPinName.VerticalAlignment = VerticalAlignment.Center;
         textPinName.Text = title;

         Grid toolTipPanel = new Grid();
         //toolTipPanel.Children.Add(i); //can even add images and stuff
         toolTipPanel.Children.Add(textPinName);
         ToolTipService.SetToolTip(this, toolTipPanel);
      }

      void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
      {
         if(icon.Source.CanFreeze)
         {
            icon.Source.Freeze();
         }
      }

      void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
      {
         this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
         this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);
         this.SizeChanged-= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
         this.MouseEnter -= new MouseEventHandler(MarkerControl_MouseEnter);
         this.MouseLeave -= new MouseEventHandler(MarkerControl_MouseLeave);
         this.MouseMove -= new MouseEventHandler(CustomMarkerDemo_MouseMove);
         this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
         this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

         Marker.Shape = null;
         icon.Source = null;
         icon = null;
         Popup = null;
         Label = null;         
      }

      void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         Marker.Offset = new Point(-e.NewSize.Width/2, -e.NewSize.Height);
      }

      void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
      {
         if(e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
         {
            Point p = e.GetPosition(MainWindow.MainMap);
            Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int) (p.X), (int) (p.Y));
         }
      }

      void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if(!IsMouseCaptured && Keyboard.IsKeyDown(Key.LeftCtrl))
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
         Marker.ZIndex -= 10000;
         Popup.IsOpen = false;
      }

      void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
      {
         Marker.ZIndex += 10000;
         Popup.IsOpen = true;
      }

      void CustomMarkerDemo_OnRender(object sender, RoutedEventArgs e)
      {
          while (this.Marker.Shape.IsMouseOver)
          {
              (this.Marker.Shape as Path).Fill = Brushes.Red;
              (this.Marker.Shape as Path).Stroke = Brushes.Red;
          }
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