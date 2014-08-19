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

namespace WPFListViewColumnsAsRows
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			DataContext = new object[]
			{
				new { Name="John",	Height=180, Weight=75, Class=2 },
				new { Name="Peter",	Height=182, Weight=83, Class=3 },
				new { Name="Marc",	Height=191, Weight=91, Class=2 }
			};

			InitializeComponent();
		}
	}
}
