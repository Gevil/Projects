using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BenTools.Mathematics;
using Vector = BenTools.Mathematics.Vector;

namespace mrci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var brush = new ImageBrush {ImageSource = new BitmapImage(new Uri(@"terrain.bmp", UriKind.Relative))};
            canvas1.Background = brush;
            
        }

        private void Canvas1_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var rand = new Random();
            var result = new List<int>();
            var check = new HashSet<int>();
            for (int i = 0; i < 2000; i++)
            {
                Int32 curValue = rand.Next(0, 4000);
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(0, 4000);
                }
                result.Add(curValue);
                check.Add(curValue);
            }

            //generate points inside our rectangle for our voronoi generator
            var datapointlist = new List<Vector>();

            for (int i = 0; i < 1000; i++)
            {
                datapointlist.Add(new Vector(result[i],result[i+1000]));
            }

            IEnumerable<Vector> datapoints = datapointlist;
           


            var vgraph = new VoronoiGraph();
            vgraph = Fortune.ComputeVoronoiGraph(datapoints);

            foreach (var vertex in vgraph.Vertizes)
            {

            }

            var R = 0;
            var G = 0;
            var B = 0;

            foreach (var edge in vgraph.Edges)
            {
                
                if (R < 255)
                    R++;
                if (R == 255 && G < 255)
                    G++;
                if (R == 255 && G == 255 && B < 255)
                    B++;

                var brush = new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));

                var poly = new Line()
                {
                    X1 = edge.LeftData[0],
                    Y1 = edge.LeftData[1],
                    X2 = edge.RightData[0],
                    Y2 = edge.RightData[1],
                    Stroke = brush,
                    StrokeThickness = 1
                };

                canvas1.Children.Add(poly);

                canvas1.InvalidateVisual();
                canvas1.UpdateLayout();
            }

        }

        public double GetDoubleFromString(string input)
        {

            double output = 0;

            if (input == "Infinity")
                output = 4000;
            else
                Double.TryParse(input, out output);

            return output;
        }
    }
}
