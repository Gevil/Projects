using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ESRI.ArcGIS.esriSystem;

namespace WPFMapViewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App: Application
	{
		public App ()
		{
			InitializeEngineLicense ();
		}

		private void InitializeEngineLicense ()
		{
			AoInitialize aoi = new AoInitializeClass ();

			//more license choices could be included here
			esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeEngine;
			if (aoi.IsProductCodeAvailable (productCode) == esriLicenseStatus.esriLicenseAvailable)
			{
				aoi.Initialize (productCode);
			}
		}
	}
}
