﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MinecraftServerWrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process ServerProc;

        public MainWindow()
        {
            InitializeComponent();

            string ServerFile;
            string ServerPath;

            if (Properties.Settings.Default.ServerPath == "" || Properties.Settings.Default.ServerFile == "")
            {
                // Initialize an Open File Dialog
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Title = "Locate your Minecraft Server";
                dialog.Filter = "Minecraft Server JAR|*.jar";
                if (dialog.ShowDialog() != true) { Close(); return; } // If the user didn't pick a file then quit

                // Store the Path and Filename in the Application Settings
                ServerFile = System.IO.Path.GetFileName(dialog.FileName);
                Properties.Settings.Default.ServerFile = ServerFile;

                ServerPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                Properties.Settings.Default.ServerPath = ServerPath;

                Properties.Settings.Default.Save();
            }
            else
            {
                // If the values are already there then just load them.
                ServerFile = Properties.Settings.Default.ServerFile;
                ServerPath = Properties.Settings.Default.ServerPath;
            }

            var startInfo = new ProcessStartInfo("java", "-Xmx1024M -Xms1024M -jar " + ServerFile + " nogui");
            // Replace the following with the location of your Minecraft Server
            startInfo.WorkingDirectory = ServerPath;
            // Notice that the Minecraft Server uses the Standard Error instead of the Standard Output
            startInfo.RedirectStandardInput = startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false; // Necessary for Standard Stream Redirection
            startInfo.CreateNoWindow = true; // You can do either this or open it with "javaw" instead of "java"

            ServerProc = new Process();
            ServerProc.StartInfo = startInfo;
            ServerProc.EnableRaisingEvents = true;
            ServerProc.ErrorDataReceived += new DataReceivedEventHandler(ServerProc_ErrorDataReceived);
            ServerProc.Exited += new EventHandler(ServerProc_Exited);

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(1, 0, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("save-all"); }
            catch { }
            CommandManager.InvalidateRequerySuggested();

        }

        private void ServerProc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            // You have to do this through the Dispatcher because this method is called by a different Thread
            Dispatcher.Invoke(new Action(() =>
            {
                ConsoleTextBlock.Text += e.Data + "\r\n";
                ConsoleScroll.ScrollToEnd();
            }));
        }

        private void ServerProc_Exited(object sender, EventArgs e)
        {
            // The order of these 2 lines is very important, reversing them will cause an exception
            // and you wont be able to read from the stream when you start the Process again !
            ServerProc.CancelErrorRead();
            ServerProc.Close();
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            ServerProc.StandardInput.WriteLine(CommandTextBox.Text);
            CommandTextBox.Clear();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                ServerProc.StandardInput.WriteLine("stop");
                ServerProc.WaitForExit(10000);
                if (!ServerProc.HasExited)
                {
                    ConsoleTextBlock.Text += "ERROR: The Server doesn't want to Stop !\r\n";
                    e.Cancel = true;
                }
            }
            catch { }
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            // This is my dirty way of making sure that I don't start the Server Twice :D
            // If the server isn't running then an exception will be thrown when accessing
            // ServerProc.StartTime and the method wont return ;-)
            try { var x = ServerProc.StartTime; return; }
            catch { }

            ServerProc.Start();
            ServerProc.BeginErrorReadLine();

        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("stop"); }
            catch { }
        }

        private void SaveOnBtn_Click(object sender, RoutedEventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("save-on"); }
            catch { }
        }

        private void SaveOffBtn_Click(object sender, RoutedEventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("save-off"); }
            catch { }
        }

        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("save-all"); }
            catch { }
        }

        private void SayBtn_Click(object sender, RoutedEventArgs e)
        {
            try { ServerProc.StandardInput.WriteLine("say " + CommandTextBox.Text); }
            catch { }
            CommandTextBox.Clear();
        }
    }
}
