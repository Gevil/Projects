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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfColourSampler.Controls
{
    /// <summary>
    /// Interaction logic for BrushPickerComboControl.xaml
    /// </summary>
    public partial class BrushPickerComboControl : UserControl
    {
        #region Properties

        /// <summary>
        /// Get or set the selected brush
        /// </summary>
        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        #endregion

        #region DependencyProperties

        /// <summary>
        /// Get or set the selected brush. This is a dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(BrushPickerComboControl), new PropertyMetadata(Brushes.White));

        #endregion

        #region Methods

        /// <summary>
        /// Create a new BrushPickerComboControl
        /// </summary>
        public BrushPickerComboControl()
        {
            InitializeComponent();
        }

        #endregion
    }
}
