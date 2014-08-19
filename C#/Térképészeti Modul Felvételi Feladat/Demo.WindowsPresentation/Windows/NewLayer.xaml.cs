using System;
using System.Collections.Generic;
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
    /// Interaction logic for NewLayer.xaml
    /// </summary>
    public partial class NewLayer : Window
    {
        public NewLayer()
        {
            InitializeComponent();

        }

        public event EventHandler btnAddLayerClicked;

        private void btnAddLayer_Click(object sender, RoutedEventArgs e)
        {
            Layer l = new Layer
            {
                IsLayerShown = true,
                LayerName = txtLayerName.Text,
                LayerID = System.Guid.NewGuid(),
                LayerColor = brushPicker.SelectedBrush.ToString()
            };

            MainWindow._LayerCollection.Add(l);
            MainWindow.data.Layers.InsertOnSubmit(l);
            MainWindow.data.SubmitChanges();

            btnAddLayerClicked(this, new EventArgs());

            this.Close();
        }
    }
}
