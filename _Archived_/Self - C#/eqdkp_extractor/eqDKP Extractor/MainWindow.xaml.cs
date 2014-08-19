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
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Windows.Threading;
using System.Threading;

namespace eqDKP_Extractor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
			if (String.IsNullOrEmpty(Properties.Settings.Default.WoWFolder))
			{
				RegistryKey software = Registry.LocalMachine.CreateSubKey("software");
				foreach (String name in software.GetSubKeyNames())
				{
					if (name == "Blizzard Entertainment")
					{
						RegistryKey blizz = software.CreateSubKey(name);
						foreach (String game in blizz.GetSubKeyNames())
						{
							if (game == "World of Warcraft")
							{
								RegistryKey wow = blizz.CreateSubKey(game);
								Properties.Settings.Default.WoWFolder = System.IO.Path.Combine((String)wow.GetValue("InstallPath"), @"Interface\AddOns");
								Properties.Settings.Default.Save();
								wow.Close();
							}
						}
						blizz.Close();
					}
				}
				software.Close();
			}

			txtTargetFolder.Text = Properties.Settings.Default.WoWFolder;
			txtAddress.Text = Properties.Settings.Default.gdkpAddress;
		}

		private void Button_Write(object sender, RoutedEventArgs e)
		{
			Startup.WriteFile();
		}

		private void Button_ChangeTarget(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
			dlg.RootFolder = Environment.SpecialFolder.MyComputer;
			dlg.SelectedPath = txtTargetFolder.Text;
			dlg.ShowDialog();
			String selectedPath = dlg.SelectedPath;
			if (!String.IsNullOrEmpty(selectedPath))
			{
				//if (CheckFolder(selectedPath))
				txtTargetFolder.Text = selectedPath;
				Properties.Settings.Default.WoWFolder = selectedPath;
				Properties.Settings.Default.Save();
			}
		}

		private bool CheckFolder(String path)
		{
			if (!File.Exists(System.IO.Path.Combine(path, "WoW.exe")))
			{
				MessageBoxResult result =  System.Windows.MessageBox.Show("WoW.exe does not exist in " + path + ".\nAre you sure it is the correct folder where you installed World of Warcraft?", "Are you sure?", MessageBoxButton.OKCancel);
				if (result != MessageBoxResult.OK)
				   return false;
			}

			return true;
		}

		private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
		{
			Properties.Settings.Default.gdkpAddress = txtAddress.Text;
			Properties.Settings.Default.Save();
		}       
	}
}
