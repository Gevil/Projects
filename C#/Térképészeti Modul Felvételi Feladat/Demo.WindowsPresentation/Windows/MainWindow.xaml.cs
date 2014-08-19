using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Demo.WindowsPresentation.CustomMarkers;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace Demo.WindowsPresentation
{
    public partial class MainWindow : Window
    {
        PointLatLng start;
        PointLatLng end;

        public static Layer SelectedLayer;
        public static GMapMarker SelectedMarker;

        public static PointLatLng currentposition;
        public static string NewMarkerGEO;

        // marker
        public static GMapMarker currentMarker;

        // zones list
        List<GMapMarker> Circles = new List<GMapMarker>();

        //DB connection and binding shit to listview todo: add tab for viewing selected layers poligons and markers maybe
        public static DataBaseDataContext data = new DataBaseDataContext();

        public static ObservableCollection<Layer> _LayerCollection = new ObservableCollection<Layer>();
        public static ObservableCollection<Marker> _MarkerCollection = new ObservableCollection<Marker>();
        public static ObservableCollection<Polygon> _PolygonCollection = new ObservableCollection<Polygon>();
        public ObservableCollection<Layer> LayerCollection { get { return _LayerCollection; } }
        public ObservableCollection<Marker> MarkerCollection { get { return _MarkerCollection; } }

        public enum MarkerTypes : int
        {
            Normal = 0,
            Polygon = 1,
            PolyLine = 2
        }

        public MainWindow()
        {
            InitializeComponent();
            /*TODO:
             * Find topright for polygon positioning in a polygon coordinates layer array.
             * ADD dialog for adding polygon by gps coords and group
             * ADD group layering of polygons
             * ADD functionality to draw polygons by mouse and set each polygon's group ID by list or dialog.
             * ADD DB support for polygons by grouplayer IDs
             * fix layering by groups since overlay doesnt work
             * Can't add interactable polygons so will have to add 1 marker for each polygon to handle menu functions on them.
             * 
            */

            // add your custom map db provider
            //MySQLPureImageCache ch = new MySQLPureImageCache();
            //ch.ConnectionString = @"server=sql2008;User Id=trolis;Persist Security Info=True;database=gmapnetcache;password=trolis;";
            //MainMap.Manager.SecondaryCache = ch;

            // set your proxy here if need
            //GMapProvider.WebProxy = new WebProxy("10.2.0.100", 8080);
            //GMapProvider.WebProxy.Credentials = new NetworkCredential("ogrenci@bilgeadam.com", "bilgeada");

            // set cache mode only if no internet avaible
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.bing.com");
            }
            catch
            {
                MainMap.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET - Demo.WindowsPresentation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // config map
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(54.6961334816182, 25.2985095977783);

            // map events
            MainMap.OnPositionChanged += new PositionChanged(MainMap_OnCurrentPositionChanged);
            MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
            MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
            MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
            MainMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
            MainMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            MainMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);

            // get map types
            comboBoxMapType.ItemsSource = GMapProviders.List;
            comboBoxMapType.DisplayMemberPath = "Name";
            comboBoxMapType.SelectedItem = MainMap.MapProvider;

            // acccess mode
            comboBoxMode.ItemsSource = Enum.GetValues(typeof(AccessMode));
            comboBoxMode.SelectedItem = MainMap.Manager.Mode;

            // get cache modes
            checkBoxCacheRoute.IsChecked = MainMap.Manager.UseRouteCache;
            checkBoxGeoCache.IsChecked = MainMap.Manager.UseGeocoderCache;

            // setup zoom min/max
            sliderZoom.Maximum = MainMap.MaxZoom;
            sliderZoom.Minimum = MainMap.MinZoom;

            // get position
            textBoxLat.Text = MainMap.Position.Lat.ToString(CultureInfo.InvariantCulture);
            textBoxLng.Text = MainMap.Position.Lng.ToString(CultureInfo.InvariantCulture);

            // get marker state
            checkBoxCurrentMarker.IsChecked = true;

            // can drag map
            checkBoxDragMap.IsChecked = MainMap.CanDragMap;

            //Load from Database to collections
            LoadDatabaseToCollections();

            LayerListView.ItemsSource = LayerCollection;
            cmbLayerList.ItemsSource = LayerCollection;

            //load Markers To Map
            LoadMarkersToMap();

            ////if(false)
            //{
            //    // add my city location for demo
            //    GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;

            //    PointLatLng? city = GMapProviders.GoogleMap.GetPoint("Lithuania, Vilnius", out status);
            //    if (city != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            //    {
            //        GMapMarker it = new GMapMarker(city.Value);
            //        {
            //            it.ZIndex = 55;
            //            it.Shape = new CustomMarkerDemo(this, it, "Welcome to Lithuania! ;}");
            //        }
            //        MainMap.Markers.Add(it);

            //        #region -- add some markers and zone around them --
            //        {
            //            List<PointAndInfo> objects = new List<PointAndInfo>();
            //            {
            //                string area = "Antakalnis";
            //                PointLatLng? pos = GMapProviders.GoogleMap.GetPoint("Lithuania, Vilnius, " + area, out status);
            //                if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            //                {
            //                    objects.Add(new PointAndInfo(pos.Value, area));
            //                }
            //            }
            //            {
            //                string area = "Senamiestis";
            //                PointLatLng? pos = GMapProviders.GoogleMap.GetPoint("Lithuania, Vilnius, " + area, out status);
            //                if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            //                {
            //                    objects.Add(new PointAndInfo(pos.Value, area));
            //                }
            //            }
            //            {
            //                string area = "Pilaite";
            //                PointLatLng? pos = GMapProviders.GoogleMap.GetPoint("Lithuania, Vilnius, " + area, out status);
            //                if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            //                {
            //                    objects.Add(new PointAndInfo(pos.Value, area));
            //                }
            //            }
            //            AddDemoZone(8.8, city.Value, objects);
            //        }
            //        #endregion
            //    }
            //}
        }

        public void LoadDatabaseToCollections()
        {
            _LayerCollection.Clear();
            _MarkerCollection.Clear();
            _PolygonCollection.Clear();

            foreach (Layer l in data.Layers)
            {
                _LayerCollection.Add(l);
            }

            foreach (Marker m in data.Markers)
            {
                _MarkerCollection.Add(m);
            }
            foreach (Polygon p in data.Polygons)
            {
                _PolygonCollection.Add(p);
            }
        }

        void LoadMarkersToMap()
        {
            MainMap.Markers.Clear();

            // set current marker
            currentMarker = new GMapMarker(MainMap.Position);
            {
                currentMarker.Shape = new Marker_Current(this, currentMarker, "custom position marker");
                currentMarker.Offset = new System.Windows.Point(-10, -10);
                currentMarker.ZIndex = int.MaxValue;
                MainMap.Markers.Add(currentMarker);
            }

            foreach (Marker m in _MarkerCollection)
            {
                if ((bool)LayerCollection.Single(l => l.LayerID.ToString() == m.LayerID).IsLayerShown)
                {
                    if (m.MarkerType == 1)
                    {
                        List<PointLatLng> coordlist = new List<PointLatLng>();

                        foreach(Polygon p in _PolygonCollection)
                        {
                            if (p.MarkerID == m.MarkerID.ToString())
                            {
                                coordlist.Add(new PointLatLng(p.Lat, p.Lng));
                            }
                        }
                        BrushConverter bc = new BrushConverter();
                        Brush color = (Brush)bc.ConvertFrom(LayerCollection.Single(l => l.LayerID.ToString() == m.LayerID).LayerColor);
                        AddPolygon(coordlist, new PointLatLng(m.MarkerLat, m.MarkerLng), color, color);
                    }
                    else if(m.MarkerType == 2)
                    {
                        List<PointLatLng> coordlist = new List<PointLatLng>();

                        foreach (Polygon p in _PolygonCollection)
                        {
                            if (p.MarkerID == m.MarkerID.ToString())
                            {
                                coordlist.Add(new PointLatLng(p.Lat, p.Lng));
                            }
                        }
                        BrushConverter bc = new BrushConverter();
                        Brush color = (Brush)bc.ConvertFrom(LayerCollection.Single(l => l.LayerID.ToString() == m.LayerID).LayerColor);

                        AddPolyline(coordlist, new PointLatLng(m.MarkerLat, m.MarkerLng), color);
                    }
                    GMapMarker marker = new GMapMarker(new PointLatLng(m.MarkerLat, m.MarkerLng));

                    string ToolTipText;
                    ToolTipText = "Név: " + m.MarkerName +
                                  "\nGEO: " + m.MarkerGEO +
                                  "\nMegj.: " + m.MarkerNote;

                    marker.Shape = new CustomMarkerDemo(this, marker, ToolTipText);
                    marker.ZIndex = 55;
                    marker.Tag = m.MarkerID.ToString();

                    MainMap.Markers.Add(marker);
                }
            }
        }

        public void AddPolygon(List<PointLatLng> CoordinatesList, PointLatLng Position, Brush FillColor, Brush StrokeColor)
        {
            GMapMarker m = new GMapMarker(Position);

            //m.Polygon.AddRange(CoordinatesList);
            foreach (PointLatLng p in CoordinatesList)
            {
                m.Polygon.Add(p);
                m.RegeneratePolygonShape(MainMap);
            }
            //m.Shape = new PolygonMarker(this, m);
            //m.RegeneratePolygonShape(MainMap);
            //m.Position = Position;

            (m.Shape as System.Windows.Shapes.Path).Fill = FillColor;
            (m.Shape as System.Windows.Shapes.Path).Stroke = StrokeColor;
            MainMap.Markers.Add(m);
            SelectedMarker = m;
        }

        public void AddPolyline(List<PointLatLng> CoordinatesList, PointLatLng Position, Brush Color)
        {
            GMapMarker m = new GMapMarker(Position);
            //m.Route.AddRange(CoordinatesList);
            foreach (PointLatLng p in CoordinatesList)
            {
                m.Route.Add(p);
                m.RegenerateRouteShape(MainMap);
            }
            //m.RegenerateRouteShape(MainMap);

            (m.Shape as System.Windows.Shapes.Path).Fill = Color;
            (m.Shape as System.Windows.Shapes.Path).Stroke = Color;
            MainMap.Markers.Add(m);
            SelectedMarker = m;
        }

        void MainMap_MouseEnter(object sender, MouseEventArgs e)
        {
            MainMap.Focus();
        }

        // add objects and zone around them
        void AddDemoZone(double areaRadius, PointLatLng center, List<PointAndInfo> objects)
        {
            var objectsInArea = from p in objects
                                where MainMap.MapProvider.Projection.GetDistance(center, p.Point) <= areaRadius
                                select new
                                {
                                    Obj = p,
                                    Dist = MainMap.MapProvider.Projection.GetDistance(center, p.Point)
                                };
            if (objectsInArea.Any())
            {
                var maxDistObject = (from p in objectsInArea
                                     orderby p.Dist descending
                                     select p).First();

                // add objects to zone
                foreach (var o in objectsInArea)
                {
                    GMapMarker it = new GMapMarker(o.Obj.Point);
                    {
                        it.ZIndex = 55;
                        var s = new CustomMarkerDemo(this, it, o.Obj.Info + ", distance from center: " + o.Dist + "km.");
                        it.Shape = s;
                    }

                    MainMap.Markers.Add(it);
                }

                // add zone circle
                {
                    GMapMarker it = new GMapMarker(center);
                    it.ZIndex = -1;

                    Circle c = new Circle();
                    c.Center = center;
                    c.Bound = maxDistObject.Obj.Point;
                    c.Tag = it;
                    c.IsHitTestVisible = false;

                    UpdateCircle(c);
                    Circles.Add(it);

                    it.Shape = c;
                    MainMap.Markers.Add(it);
                }
            }
        }

        // calculates circle radius
        void UpdateCircle(Circle c)
        {
            var pxCenter = MainMap.FromLatLngToLocal(c.Center);
            var pxBounds = MainMap.FromLatLngToLocal(c.Bound);

            double a = (double)(pxBounds.X - pxCenter.X);
            double b = (double)(pxBounds.Y - pxCenter.Y);
            var pxCircleRadius = Math.Sqrt(a * a + b * b);

            c.Width = 55 + pxCircleRadius * 2;
            c.Height = 55 + pxCircleRadius * 2;
            (c.Tag as GMapMarker).Offset = new System.Windows.Point(-c.Width / 2, -c.Height / 2);
        }

        // center markers on load
        void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        void MainMap_OnMapTypeChanged(GMapProvider type)
        {
            sliderZoom.Minimum = MainMap.MinZoom;
            sliderZoom.Maximum = MainMap.MaxZoom;
        }

        void MainMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(MainMap);
            currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            currentposition = currentMarker.Position;
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Point p = e.GetPosition(MainMap);
                currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
                currentposition = currentMarker.Position;
            }
        }

        // zoom max & center markers
        private void btnCenterView_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        // tile loading starts
        void MainMap_OnTileLoadStart()
        {
            System.Windows.Forms.MethodInvoker m = delegate()
            {
                progressBar1.Visibility = Visibility.Visible;
            };

            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, m);
            }
            catch
            {
            }
        }

        // tile loading stops
        void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
        {
            MainMap.ElapsedMilliseconds = ElapsedMilliseconds;

            System.Windows.Forms.MethodInvoker m = delegate()
            {
                progressBar1.Visibility = Visibility.Hidden;
                txtStatus_LoadTime.Text = "Betöltve " + MainMap.ElapsedMilliseconds + "ms alatt.";
            };

            try
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, m);
            }
            catch
            {
            }
        }

        // current location changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            //currentposition = MainMap.Position;
            //mapgroup.Header = "gmap: " + point;
            txtStatus_Latitude.Text = "Szélesség: " + point.Lat.ToString();
            txtStatus_Longitude.Text = "Hosszúság: " + point.Lng.ToString();
        }

        // reload
        private void btnReloadMap_Click(object sender, RoutedEventArgs e)
        {
            LoadDatabaseToCollections();
            LoadMarkersToMap();
            //MainMap.ReloadMap();
        }

        // enable current marker
        private void chkCurrentMarker_Checked(object sender, RoutedEventArgs e)
        {
            if (currentMarker != null)
            {
                MainMap.Markers.Add(currentMarker);
            }
        }

        // disable current marker
        private void chkCurrentMarker_Unchecked(object sender, RoutedEventArgs e)
        {
            if (currentMarker != null)
            {
                MainMap.Markers.Remove(currentMarker);
            }
        }

        // enable map dragging
        private void chkDragMap_Checked(object sender, RoutedEventArgs e)
        {
            MainMap.CanDragMap = true;
        }

        // disable map dragging
        private void chkDragMap_Unchecked(object sender, RoutedEventArgs e)
        {
            MainMap.CanDragMap = false;
        }

        // goto!
        private void btnGoToLatLong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double lat = double.Parse(textBoxLat.Text, CultureInfo.InvariantCulture);
                double lng = double.Parse(textBoxLng.Text, CultureInfo.InvariantCulture);

                MainMap.Position = new PointLatLng(lat, lng);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hibás Koordináta Formátum: " + ex.Message);
            }
        }

        // goto by geocoder
        private void txtGeo_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(textBoxGeo.Text);
                if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
                {
                    MessageBox.Show("Google Maps Geocoder can't find: '" + textBoxGeo.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    currentMarker.Position = MainMap.Position;
                }
            }
        }

        // zoom changed
        private void sliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // updates circles on map
            foreach (var c in Circles)
            {
                UpdateCircle(c.Shape as Circle);
            }
        }

        // zoom up
        private void czuZoomUp_Click(object sender, RoutedEventArgs e)
        {
            MainMap.Zoom = ((int)MainMap.Zoom) + 1;
        }

        // zoom down
        private void czuZoomDown_Click(object sender, RoutedEventArgs e)
        {
            MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
        }

        // prefetch
        private void btnPrefetch_Click(object sender, RoutedEventArgs e)
        {
            RectLatLng area = MainMap.SelectedArea;
            if (!area.IsEmpty)
            {
                for (int i = (int)MainMap.Zoom; i <= MainMap.MaxZoom; i++)
                {
                    MessageBoxResult res = MessageBox.Show("Ready ripp at Zoom = " + i + " ?", "GMap.NET", MessageBoxButton.YesNoCancel);

                    if (res == MessageBoxResult.Yes)
                    {
                        TilePrefetcher obj = new TilePrefetcher();
                        obj.Owner = this;
                        obj.ShowCompleteMessage = true;
                        obj.Start(area, i, MainMap.MapProvider, 100);
                    }
                    else if (res == MessageBoxResult.No)
                    {
                        continue;
                    }
                    else if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        // access mode
        private void cmbMode_DropDownClosed(object sender, EventArgs e)
        {
            MainMap.Manager.Mode = (AccessMode)comboBoxMode.SelectedItem;
            MainMap.ReloadMap();
        }

        // clear cache
        private void btnClearCache_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You sure?", "Clear GMap.NET cache?", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                try
                {
                    System.IO.Directory.Delete(MainMap.CacheLocation, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // export
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ShowExportDialog();
        }

        // import
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ShowImportDialog();
        }

        // use route cache
        private void checkBoxCacheRoute_Checked(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.UseRouteCache = checkBoxCacheRoute.IsChecked.Value;
        }

        // use geocoding cahce
        private void checkBoxGeoCache_Checked(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.UseGeocoderCache = checkBoxGeoCache.IsChecked.Value;
            GMaps.Instance.UsePlacemarkCache = GMaps.Instance.UseGeocoderCache;
        }

        // save currnt view
        private void btnSaveView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImageSource img = MainMap.ToImageSource();
                PngBitmapEncoder en = new PngBitmapEncoder();
                en.Frames.Add(BitmapFrame.Create(img as BitmapSource));

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "GMap.NET Image"; // Default file name
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "Image (.png)|*.png"; // Filter files by extension
                dlg.AddExtension = true;
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;

                    using (System.IO.Stream st = System.IO.File.OpenWrite(filename))
                    {
                        en.Save(st);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // clear all markers
        private void btnClearMarkers_Click(object sender, RoutedEventArgs e)
        {
            var clear = MainMap.Markers.Where(p => p != null && p != currentMarker);
            if (clear != null)
            {
                for (int i = 0; i < clear.Count(); i++)
                {
                    MainMap.Markers.Remove(clear.ElementAt(i));
                    i--;
                }
            }
        }

        public static string GetGEO(PointLatLng point)
        {
            Placemark p = null;
            GeoCoderStatusCode status;
            var plret = GMapProviders.GoogleMap.GetPlacemark(currentposition, out status);
            if (status == GeoCoderStatusCode.G_GEO_SUCCESS && plret != null)
            {
                p = plret;
            }

            if (p.Address != null)
                return p.Address;
            else
                return point.Lat.ToString() + ", " + point.Lng;
        }

        // add marker
        private void btnAddMarker_Click(object sender, RoutedEventArgs e)
        {
            NewMarker();
        }

        // sets route start
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            start = currentMarker.Position;
        }

        // sets route end
        private void btnEndRoute_Click(object sender, RoutedEventArgs e)
        {
            end = currentMarker.Position;
        }

        // adds route
        private void btnAddRoute_Click(object sender, RoutedEventArgs e)
        {
            RoutingProvider rp = MainMap.MapProvider as RoutingProvider;
            if (rp == null)
            {
                rp = GMapProviders.GoogleMap; // use google if provider does not implement routing
            }

            MapRoute route = rp.GetRoute(start, end, false, false, (int)MainMap.Zoom);
            if (route != null)
            {
                GMapMarker m1 = new GMapMarker(start);
                m1.Shape = new CustomMarkerDemo(this, m1, "Start: " + route.Name);

                GMapMarker m2 = new GMapMarker(end);
                m2.Shape = new CustomMarkerDemo(this, m2, "End: " + start.ToString());

                GMapMarker mRoute = new GMapMarker(start);
                {
                    mRoute.Route.AddRange(route.Points);
                    mRoute.RegenerateRouteShape(MainMap);

                    mRoute.ZIndex = -1;
                }

                MainMap.Markers.Add(m1);
                MainMap.Markers.Add(m2);
                MainMap.Markers.Add(mRoute);

                MainMap.ZoomAndCenterMarkers(null);
            }
        }

        // enables tile grid view
        private void chkEnableGrid_Checked(object sender, RoutedEventArgs e)
        {
            MainMap.ShowTileGridLines = true;
        }

        // disables tile grid view
        private void chkEnableGrid_Unchecked(object sender, RoutedEventArgs e)
        {
            MainMap.ShowTileGridLines = false;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int offset = 22;

            if (MainMap.IsFocused)
            {
                if (e.Key == Key.Left)
                {
                    MainMap.Offset(-offset, 0);
                }
                else if (e.Key == Key.Right)
                {
                    MainMap.Offset(offset, 0);
                }
                else if (e.Key == Key.Up)
                {
                    MainMap.Offset(0, -offset);
                }
                else if (e.Key == Key.Down)
                {
                    MainMap.Offset(0, offset);
                }
                else if (e.Key == Key.Add)
                {
                    czuZoomUp_Click(null, null);
                }
                else if (e.Key == Key.Subtract)
                {
                    czuZoomDown_Click(null, null);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MainMap.Bearing--;
            }
            else if (e.Key == Key.Z)
            {
                MainMap.Bearing++;
            }
        }

        private void btnNewPolygon_Click(object sender, RoutedEventArgs e)
        {
            NewPolygon();
        }

        //public Marker GetMarker(string ID)
        //{
        //    //return MainMap.Markers.Single(m => m.);   //data.Pins.Single(p => p.ID.ToString() == PinID);
        //}

        private void btnSearchGeo_Click(object sender, RoutedEventArgs e)
        {
            GeoCoderStatusCode status = MainMap.SetCurrentPositionByKeywords(textBoxGeo.Text);
            if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                MessageBox.Show("A keresett Geokód: '" + textBoxGeo.Text + "'nem található. Hibakód: " + status.ToString(), "Hiba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                currentMarker.Position = MainMap.Position;
                txtStatus_Field.Text = "Keresett Geokód: '" + textBoxGeo.Text + "' megjelölve!";
            }
        }

        private void btnNewLayer_Click(object sender, RoutedEventArgs e)
        {
            NewLayer();
        }

        #region ***Layer list context menus***
        private void MenuNewLayer_Click(object sender, RoutedEventArgs e)
        {
            NewLayer();
        }

        private void MenuEditLayer_Click(object sender, RoutedEventArgs e)
        {
            EditLayer();
        }

        private void MenuDeleteLayer_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedLayer();
        }
        #endregion


        #region *** General functions ***
        public void DeleteSelectedLayer()
        {
            if (LayerListView.SelectedItems.Count > 0)
            {
                Layer ldata = (Layer)LayerListView.SelectedItem;

                ObservableCollection<Marker> temp = new ObservableCollection<Marker>();
                foreach (Marker m in _MarkerCollection)
                {
                    if (m.LayerID == ldata.LayerID.ToString())
                        temp.Add(m);
                }

                if (temp.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show(this, "A kiválasztott réteg törlésével a réteg jelöléseit és alakzatait is törli. Biztos törli a réteget?", "Figyelem!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        foreach (Marker m in _MarkerCollection)
                        {
                            foreach (Polygon p in _PolygonCollection)
                            {
                                if (m.LayerID == ldata.LayerID.ToString())
                                {
                                    if (p.MarkerID == m.MarkerID.ToString())
                                    {
                                        //_PolygonCollection.Remove(p);
                                        data.Polygons.DeleteOnSubmit(p);
                                    }

                                    //_MarkerCollection.Remove(m);
                                    data.Markers.DeleteOnSubmit(m);
                                }
                            }
                        }
                        //(this.LayerListView.ItemsSource as ObservableCollection<Layer>).Remove(ldata);

                        data.Layers.DeleteOnSubmit(ldata);
                        data.SubmitChanges();
                    }
                    else
                    {

                    }
                }
                else 
                {
                    (this.LayerListView.ItemsSource as ObservableCollection<Layer>).Remove(ldata);

                    data.Layers.DeleteOnSubmit(ldata);
                    data.SubmitChanges();
                }

                LoadDatabaseToCollections();
                LoadMarkersToMap();
            }
        }

        public void NewLayer()
        {
            Windows.NewLayer NewLayerDialog = new Windows.NewLayer();
            NewLayerDialog.Show();
            NewLayerDialog.btnAddLayerClicked += new EventHandler(cw_btnAddLayerClicked);
        }

        public void EditLayer()
        {
 
        }

        public void NewMarker()
        {
            currentposition = MainMap.Position;

            NewMarkerGEO = GetGEO(currentposition);

            Windows.NewMarker NewMarkerDialog = new Windows.NewMarker();
            NewMarkerDialog.Show();
            NewMarkerDialog.btnAddMarkerClicked += new EventHandler(cw_btnAddMarkerClicked);
        }

        public void NewPolygon()
        {
            Windows.NewPolygon NewPolygonDialog = new Windows.NewPolygon();
            NewPolygonDialog.Show();

            //Events
            NewPolygonDialog.UpdatePolygonShape += new EventHandler(cw_UpdatePolygonShape);
            NewPolygonDialog.AddTempPolygon += new EventHandler(cw_AddTempPolygon);
            NewPolygonDialog.btnSavePolygonClicked += new EventHandler(cw_btnSavePolygonClicked);
        }

        public void NewPolyline()
        {
            Windows.NewPolyline NewPolylineDialog = new Windows.NewPolyline();
            NewPolylineDialog.Show();

            //Events
            NewPolylineDialog.UpdatePolylineShape += new EventHandler(cw_UpdatePolylineShape);
            NewPolylineDialog.AddTempPolyline += new EventHandler(cw_AddTempPolyline);
            NewPolylineDialog.btnSavePolylineClicked += new EventHandler(cw_btnSavePolylineClicked);
        }

        public void EditMarker()
        {
 
        }

        public void EditPolygon()
        {
 
        }

        public void EditPolyline()
        {

        }

        #endregion

        #region ***Event Handlers for Other Windows***
        //Event handlers from other windows
        public void cw_btnAddLayerClicked(object sender, EventArgs e)
        {
            SelectedLayer = null;
            LoadMarkersToMap();
        }

        public void cw_UpdatePolygonShape(object sender, EventArgs e)
        {
            MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].Polygon.Add(currentposition);
            if (MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].Polygon.Count > 0)
                MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].RegeneratePolygonShape(MainMap);
        }

        public void cw_UpdatePolylineShape(object sender, EventArgs e)
        {
            MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].Route.Add(currentposition);
            if (MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].Route.Count > 0)
                MainMap.Markers[MainMap.Markers.IndexOf(SelectedMarker)].RegenerateRouteShape(MainMap);
        }

        public void cw_btnSavePolygonClicked(object sender, EventArgs e)
        {
            SelectedMarker = null;
            LoadMarkersToMap();
        }

        public void cw_btnSavePolylineClicked(object sender, EventArgs e)
        {
            SelectedMarker = null;
            LoadMarkersToMap();
        }

        public void cw_AddTempPolygon(object sender, EventArgs e)
        {
            GMapMarker m = new GMapMarker(Windows.NewPolygon.polygonposition);
            {
                m.Polygon.Add(Windows.NewPolygon.polygonposition);
                m.ZIndex = 55;
            }
            MainMap.Markers.Add(m);
            SelectedMarker = m;
        }

        public void cw_AddTempPolyline(object sender, EventArgs e)
        {
            GMapMarker m = new GMapMarker(Windows.NewPolyline.polygonposition);
            {
                m.Route.Add(Windows.NewPolyline.polygonposition);
                m.ZIndex = 55;
            }
            MainMap.Markers.Add(m);
            SelectedMarker = m;
        }

        public void cw_btnAddMarkerClicked(object sender, EventArgs e)
        {
            LoadMarkersToMap();
        }
        #endregion

        //shiet
        private void cmbLayerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarkerListView.Items.Clear();

            foreach (Marker m in _MarkerCollection)
            {
                if (m.LayerID == cmbLayerList.SelectedValue.ToString())
                {
                    MarkerListView.Items.Add(m);
                }
            }
        }

        #region *** Layer Enabler checkbox functions ***
        private void chkLayerEnabled_Checked(object sender, RoutedEventArgs e)
        {
            LoadMarkersToMap();
        }

        private void chkLayerEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadMarkersToMap();
        }
        #endregion

        //On form load
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if(cmbLayerList.Items.Count != 0)
                cmbLayerList.SelectedIndex = 0;
        }

        #region *** Marker list Context Menus ***
        //marker menus
        private void MenuNewMarker_Click(object sender, RoutedEventArgs e)
        {
            NewMarker();
        }

        private void MenuEditMarker_Click(object sender, RoutedEventArgs e)
        {
            if ((MarkerListView.SelectedItem as Marker).MarkerType == 0)
            {
                //normal marker
                EditMarker();
            }
            else if ((MarkerListView.SelectedItem as Marker).MarkerType == 1)
            {
                //polygon
                EditPolygon();
            }
            else if ((MarkerListView.SelectedItem as Marker).MarkerType == 2)
            {
                //polyline
                EditPolyline();
            }
        }

        private void MenuDeleteMarker_Click(object sender, RoutedEventArgs e)
        {
            if ((MarkerListView.SelectedItem as Marker).MarkerType == 0)
            {
                //normal marker
                _MarkerCollection.Remove((Marker)MarkerListView.SelectedItem);
                data.Markers.DeleteOnSubmit((Marker)MarkerListView.SelectedItem);
                data.SubmitChanges();
            }
            else
            {
                //polygon
                foreach (Polygon p in _PolygonCollection)
                {
                    if ((MarkerListView.SelectedItem as Marker).MarkerID.ToString() == p.MarkerID)
                    {
                        data.Polygons.DeleteOnSubmit(p);
                    }
                }
                data.SubmitChanges();
                LoadDatabaseToCollections();
                LoadMarkersToMap();
            }
        }
        #endregion

        private void btnNewPolyline_Click(object sender, RoutedEventArgs e)
        {
            NewPolyline();
        }
    }
}
