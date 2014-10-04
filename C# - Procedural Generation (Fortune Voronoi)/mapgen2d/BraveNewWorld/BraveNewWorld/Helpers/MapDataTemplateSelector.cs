using System.Windows;
using System.Windows.Controls;
using BraveNewWorld.Models;

namespace BraveNewWorld.Helpers
{
    public class MapDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is Center)
            {
                Center center = item as Center;
                Window window = Application.Current.MainWindow;
                DataTemplate ret = new DataTemplate();

                //return window.FindResource(center.Biome) as DataTemplate;
                return window.FindResource("LandZone") as DataTemplate;
                
            }
            else if (item != null && item is Edge)
            {
                //Edge edge = item as Edge;
                Window window = Application.Current.MainWindow;
                Edge ed = (Edge)item;

                //if (ed.River > 0)
                //    return window.FindResource("RiverEdge") as DataTemplate;

                //if (ed.Coast)
                //    return window.FindResource("CoastEdge") as DataTemplate;

                return window.FindResource("Edge") as DataTemplate;
            }
            else if (item != null && item is Corner)
            {
                Window window = Application.Current.MainWindow;
                Corner crn = (Corner)item;
                
                //if (crn.Land)
                //    return window.FindResource("LandCorner") as DataTemplate;

                //if (crn.Water)
                //    return window.FindResource("WaterCorner") as DataTemplate;


                //if (crn.River > 0)
                //    return window.FindResource("RiverCorner") as DataTemplate;

                return window.FindResource("Corner") as DataTemplate;
            }

            return null;
        }
    }
}
