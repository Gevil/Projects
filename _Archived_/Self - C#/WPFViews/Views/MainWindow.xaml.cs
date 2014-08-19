using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//        private ICollectionView view;

        public MainWindow()
        {
            InitializeComponent();

            BeatlesDataContext dc = new BeatlesDataContext();

            var query = from s in dc.Songs
                        select s;

            dataListBox.ItemsSource = query.ToList();

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dataListBox.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription((sender as RadioButton).Tag.ToString(),ListSortDirection.Ascending));

            view.GroupDescriptions.Clear();
            if (sender == radioButton4)
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription("Album.Name"));
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
        }

        private void filterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dataListBox.ItemsSource);
            view.Filter = m => ((Song)m).Name.ToLower().Contains(filterBox.Text.ToLower());
        }

    }
    public class NametoURIConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Uri("..\\AlbumsBeatles\\" + value.ToString().Trim() + "-A.jpg", UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class TabToNewLineConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().Replace('\t','\n').Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



}
