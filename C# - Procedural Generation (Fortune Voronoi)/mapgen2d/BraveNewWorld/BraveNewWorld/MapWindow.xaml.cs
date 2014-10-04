using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BraveNewWorld.Models;

namespace BraveNewWorld
{
        /// <summary>
        /// Interaction logic for MapWindow.xaml
        /// </summary>
        public partial class MapWindow : Window
        {
            private class ImageCapturer
            {
                public static void SaveToBmp(FrameworkElement visual, string fileName)
                {
                    var encoder = new BmpBitmapEncoder();
                    SaveUsingEncoder(visual, fileName, encoder);
                }

                public static void SaveToPng(FrameworkElement visual, string fileName)
                {
                    var encoder = new PngBitmapEncoder();
                    SaveUsingEncoder(visual, fileName, encoder);
                }

                private static void SaveUsingEncoder(FrameworkElement visual,
                    string fileName, BitmapEncoder encoder)
                {
                    RenderTargetBitmap bitmap = new RenderTargetBitmap(
                        (int) visual.Width,
                        (int) visual.Height, 96, 96, PixelFormats.Pbgra32);
                    bitmap.Render(visual);
                    BitmapFrame frame = BitmapFrame.Create(bitmap);
                    encoder.Frames.Add(frame);

                    using (var stream = File.Create(fileName))
                    {
                        encoder.Save(stream);
                    }
                }
            }

            public MapWindow()
            {
                InitializeComponent();

                var generator = new ColourGenerator();
                foreach (Center center in App.AppMap.Centers.Values)
                {
                    string colour = "#" + generator.NextColour();

                    //var brush = new SolidColorBrush(Color.FromArgb(255, (byte) R, (byte) G, (byte) B));
                    //Color.FromRgb(Convert.ToByte(colour.Substring(1, 2), 16),Convert.ToByte(colour.Substring(3, 2), 16),Convert.ToByte(colour.Substring(5, 2), 16));
                    
                    var brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(colour));
                    var polygonPoints = new PointCollection();

                    center.OrderCorners();

                    //foreach (var border in center.Borders)
                    //{
                    //    polygonPoints.Add(new Point(border.Point.X * 3840, border.Point.Y * 2176));
                    //}

                    foreach (var corner in center.Corners)
                    {
                        polygonPoints.Add(new Point(corner.Point.X*3840, corner.Point.Y*2176));
                    }

                    var poly = new Polygon()
                    {
                        Points = polygonPoints,
                        //Stroke = brush,
                        //StrokeThickness = 1,
                        Fill = brush

                    };

                    canvas1.Children.Add(poly);
                    canvas1.InvalidateVisual();
                    canvas1.UpdateLayout();


                }


            }

            public void ExportToPng(string path, Canvas surface)
            {
                if (path == null) return;

                // Save current canvas transform
                Transform transform = surface.LayoutTransform;
                // reset current transform (in case it is scaled or rotated)
                surface.LayoutTransform = null;

                // Get the size of canvas
                Size size = new Size(surface.Width, surface.Height);
                // Measure and arrange the surface
                // VERY IMPORTANT
                surface.Measure(size);
                surface.Arrange(new Rect(size));

                // Create a render bitmap and push the surface to it
                RenderTargetBitmap renderBitmap =
                    new RenderTargetBitmap(
                        (int) size.Width,
                        (int) size.Height,
                        96d,
                        96d,
                        PixelFormats.Pbgra32);
                renderBitmap.Render(surface);

                // Create a file stream for saving image
                using (FileStream outStream = new FileStream(path, FileMode.Create))
                {
                    // Use png encoder for our data
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(outStream);
                }

                // Restore previously saved layout
                surface.LayoutTransform = transform;
            }

            private void Canvas1_OnMouseDown(object sender, MouseButtonEventArgs e)
            {
                ExportToPng("capture.png", canvas1);
            }
        }
    }

    public class ColourGenerator
    {

        private int index = 0;
        private IntensityGenerator intensityGenerator = new IntensityGenerator();

        public string NextColour()
        {
            string colour = string.Format(PatternGenerator.NextPattern(index),
                intensityGenerator.NextIntensity(index));
            index++;
            return colour;
        }
    }

    public class PatternGenerator
    {
        public static string NextPattern(int index)
        {
            switch (index % 7)
            {
                case 0: return "{0}0000";
                case 1: return "00{0}00";
                case 2: return "0000{0}";
                case 3: return "{0}{0}00";
                case 4: return "{0}00{0}";
                case 5: return "00{0}{0}";
                case 6: return "{0}{0}{0}";
                default: throw new Exception("Math error");
            }
        }
    }

    public class IntensityGenerator
    {
        private IntensityValueWalker walker;
        private int current;

        public string NextIntensity(int index)
        {
            if (index == 0)
            {
                current = 255;
            }
            else if (index % 7 == 0)
            {
                if (walker == null)
                {
                    walker = new IntensityValueWalker();
                }
                else
                {
                    walker.MoveNext();
                }
                current = walker.Current.Value;
            }
            string currentText = current.ToString("X");
            if (currentText.Length == 1) currentText = "0" + currentText;
            return currentText;
        }
    }

    public class IntensityValue
    {

        private IntensityValue mChildA;
        private IntensityValue mChildB;

        public IntensityValue(IntensityValue parent, int value, int level)
        {
            if (level > 7) throw new Exception("There are no more colours left");
            Value = value;
            Parent = parent;
            Level = level;
        }

        public int Level { get; set; }
        public int Value { get; set; }
        public IntensityValue Parent { get; set; }

        public IntensityValue ChildA
        {
            get
            {
                return mChildA ?? (mChildA = new IntensityValue(this, this.Value - (1 << (7 - Level)), Level + 1));
            }
        }

        public IntensityValue ChildB
        {
            get
            {
                return mChildB ?? (mChildB = new IntensityValue(this, Value + (1 << (7 - Level)), Level + 1));
            }
        }
    }

public class IntensityValueWalker
{

    public IntensityValueWalker()
    {
        Current = new IntensityValue(null, 1 << 7, 1);
    }

    public IntensityValue Current { get; set; }

    public void MoveNext()
    {
        if (Current.Parent == null)
        {
            Current = Current.ChildA;
        }
        else if (Current.Parent.ChildA == Current)
        {
            Current = Current.Parent.ChildB;
        }
        else
        {
            int levelsUp = 1;
            Current = Current.Parent;
            while (Current.Parent != null && Current == Current.Parent.ChildB)
            {
                Current = Current.Parent;
                levelsUp++;
            }
            if (Current.Parent != null)
            {
                Current = Current.Parent.ChildB;
            }
            else
            {
                levelsUp++;
            }
            for (int i = 0; i < levelsUp; i++)
            {
                Current = Current.ChildA;
            }

        }
    }
}
