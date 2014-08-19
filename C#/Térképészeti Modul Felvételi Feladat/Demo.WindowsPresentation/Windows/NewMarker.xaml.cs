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
    /// Interaction logic for NewMarker.xaml
    /// </summary>
    public partial class NewMarker : Window
    {
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

        public NewMarker()
        {
            InitializeComponent();

            DataContext = this;

            foreach (Layer l in MainWindow._LayerCollection)
            {
                if((bool)l.IsLayerShown)
                    DropDownValues.Add(new KeyValuePair<string, string>(l.LayerID.ToString(), l.LayerName));
            }


        }

        public event EventHandler btnAddMarkerClicked;

        private void btnAddMarker_Click(object sender, RoutedEventArgs e)
        {
            if (txtMarkerName.Text != null)
            {
                Marker m = new Marker
                {
                    MarkerName = txtMarkerName.Text,
                    MarkerNote = txtNotes.Text,
                    MarkerLat = MainWindow.currentposition.Lat,
                    MarkerLng = MainWindow.currentposition.Lng,
                    MarkerID = System.Guid.NewGuid(),
                    MarkerGEO = txtGEO.Text,
                    LayerID = cmbLayerList.SelectedValue.ToString(),
                    MarkerType = 0
                };
                MainWindow._MarkerCollection.Add(m);
                MainWindow.data.Markers.InsertOnSubmit(m);
                MainWindow.data.SubmitChanges();
                btnAddMarkerClicked(this, new EventArgs());

                this.Close();
            }
            else 
            {
                MessageBoxResult result = MessageBox.Show("A Név mezőt mindenképp ki kell tölteni!", "Hiba!");
            }


        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            txtGEO.Text = MainWindow.GetGEO(MainWindow.currentposition);
            cmbLayerList.SelectedIndex = 0;
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
    }
}
