using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using BraveNewWorld.Models;
using BraveNewWorld.Services;

namespace BraveNewWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int MapSize = 1;
        private ObservableCollection<IMapItem> AllOfThem = new ObservableCollection<IMapItem>();
        private readonly IMapService MapHandler = new MapService();

        public MainWindow()
        {
            InitializeComponent();
            MainContainer.ItemsSource = AllOfThem;
            DotInput.Text = "896";
        }

        public void LoadMap(int DotCount)
        {
            var rnd = new Random();

            var points = new HashSet<BenTools.Mathematics.Vector>();
            points.Clear();
            for (int i = 0; i < DotCount; i++)
            {
                points.Add(new BenTools.Mathematics.Vector(rnd.NextDouble() * MapSize,
                                                            rnd.NextDouble() * MapSize));
            }

            AllOfThem.Clear();


            MapHandler.LoadMap(new LoadMapParams(points, true));

            AddThemAll();

            //ImageCapturer.SaveToPng(AnyControl, "Capture.png");

            var mw = new MapWindow();
            mw.Show();


        }

        private void AddThemAll()
        {
            foreach (Center o in App.AppMap.Centers.Values)
            {
                AllOfThem.Add(o);
            }

            foreach (Corner c in App.AppMap.Corners.Values)
            {
                AllOfThem.Add(c);
            }

            foreach (Edge ed in App.AppMap.Edges.Values)
            {
                AllOfThem.Add(ed);
            }
        }

        private void LoadMapStuff(object sender, RoutedEventArgs e)
        {
            App.ResetMap();
            LoadMap(int.Parse(DotInput.Text));
        }
    }
}
