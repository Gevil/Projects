using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace Demo.WindowsPresentation.Windows
{
    /// <summary>
    /// Interaction logic for NewPolyline.xaml
    /// </summary>
    public partial class NewPolyline : Window
    {
        public static PointLatLng polygonposition;
        public static Brush color;
        public System.Guid newguid = System.Guid.NewGuid();

        private ObservableCollection<KeyValuePair<string, string>> _dropDownValues = new ObservableCollection<KeyValuePair<string, string>>();
        public ObservableCollection<KeyValuePair<string, string>> DropDownValues
        {
            get
            {
                return _dropDownValues;
            }

            set
            {
                _dropDownValues = value;
                OnPropertyChanged("DropDownValues");
            }
        }

        public ObservableCollection<Polygon> PolygonCollection { get { return MainWindow._PolygonCollection; } }

        private string _selectedValue;
        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }

            set
            {
                _selectedValue = value;
                OnPropertyChanged("SelectedValue");
            }
        }


        public NewPolyline()
        {
            InitializeComponent();
            polygonposition = MainWindow.currentposition;

            DataContext = this;

            foreach (Layer l in MainWindow._LayerCollection)
            {
                if ((bool)l.IsLayerShown)
                    DropDownValues.Add(new KeyValuePair<string, string>(l.LayerID.ToString(), l.LayerName));
            }

            txtPosition.Text = polygonposition.Lat.ToString() + ", " + polygonposition.Lng.ToString();
            txtGEO.Text = MainWindow.GetGEO(polygonposition);
            cmbLayerList.SelectedIndex = 0;
        }

        public event EventHandler AddTempPolyline;
        public event EventHandler UpdatePolylineShape;
        public event EventHandler btnSavePolylineClicked;


        private void btnAddPointCoord_Click(object sender, RoutedEventArgs e)
        {
            Polygon p = new Polygon
            {
                PolygonID = System.Guid.NewGuid(),
                Lat = MainWindow.currentposition.Lat,
                Lng = MainWindow.currentposition.Lng,
                MarkerID = newguid.ToString()
            };
            MainWindow._PolygonCollection.Add(p);
            MainWindow.data.Polygons.InsertOnSubmit(p);
            MainWindow.data.SubmitChanges();

            //handle added polygon coords on main window
            UpdatePolylineShape(this, new EventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void btnAddPolygon_Click(object sender, RoutedEventArgs e)
        {
            if (txtPolygonName.Text != null)
            {
                Marker m = new Marker
                {
                    MarkerName = txtPolygonName.Text,
                    MarkerNote = txtNote.Text,
                    MarkerLat = polygonposition.Lat,
                    MarkerLng = polygonposition.Lng,
                    MarkerID = newguid,
                    MarkerGEO = txtGEO.Text,
                    LayerID = cmbLayerList.SelectedValue.ToString(),
                    MarkerType = 2
                };
                MainWindow._MarkerCollection.Add(m);
                MainWindow.data.Markers.InsertOnSubmit(m);
                MainWindow.data.SubmitChanges();

                Polygon p = new Polygon
                {
                    PolygonID = System.Guid.NewGuid(),
                    Lat = MainWindow.currentposition.Lat,
                    Lng = MainWindow.currentposition.Lng,
                    MarkerID = newguid.ToString()
                };
                MainWindow._PolygonCollection.Add(p);
                MainWindow.data.Polygons.InsertOnSubmit(p);
                MainWindow.data.SubmitChanges();

                //BrushConverter bc = new BrushConverter();
                //color = (Brush)bc.ConvertFrom(MainWindow._LayerCollection.Single(l => l.LayerID.ToString() == cmbLayerList.SelectedValue.ToString()).LayerColor);

                AddTempPolyline(this, new EventArgs());

                btnAddPolygon.Visibility = Visibility.Hidden;
                btnSavePolygon.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("A Név mezőt mindenképp ki kell tölteni!", "Hiba!");
            }
        }

        private void btnSavePolygon_Click(object sender, RoutedEventArgs e)
        {
            btnSavePolylineClicked(this, new EventArgs());
            this.Close();
        }
    }
}
