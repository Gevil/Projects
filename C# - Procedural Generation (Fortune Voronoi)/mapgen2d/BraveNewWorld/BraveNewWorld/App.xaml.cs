using System.Windows;
using BraveNewWorld.Models;

namespace BraveNewWorld
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Map AppMap { get; set; }
        public static readonly int MapSize = 400;

        public static void ResetMap()
        {
            App.AppMap.Centers.Clear();
            App.AppMap.Edges.Clear();
            App.AppMap.Corners.Clear();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            AppMap = new Map();

            var mw = new MainWindow();
            mw.Show();
        }
    }
}
