using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace eqDKP_Extractor
{
    class Startup
    {
        public static bool auto = false;
        [STAThread]
        static void Main(String[] args)
        {
            if (args.Length > 0)
            {
                auto = true; 
                WriteFile();
                return;
            }

            App app = new App();
            app.MainWindow = new MainWindow();
            app.MainWindow.Show();
            app.Run();
        }

        static public void WriteFile()
        {
            try
            {
                if (String.IsNullOrEmpty(Properties.Settings.Default.WoWFolder))
                {
                    MessageBox.Show("You have to set the WoW-folder first");
                    return;
                }

                if (!Directory.Exists(System.IO.Path.Combine(Properties.Settings.Default.WoWFolder, @"RaidWatch")))
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(Properties.Settings.Default.WoWFolder, @"RaidWatch"));
                }

                using (FileStream fs = new FileStream(System.IO.Path.Combine(Properties.Settings.Default.WoWFolder, @"RaidWatch\DKPData.lua"), FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    WebClient wc = new WebClient();
                    String data = GetDKPData();
                    if (String.IsNullOrEmpty(data))
                    {
                        return;
                    }
                    sw.Write(data);
                    sw.Close();
                    sw.Dispose();
                }

                if (!auto)
                    MessageBox.Show("Done!\n File written to: " + System.IO.Path.Combine(Properties.Settings.Default.WoWFolder, @"Interface\AddOns\RaidWatch\DKPata.lua"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static public String GetDKPData()
        {
            WebClient wc = new WebClient();
            String res = String.Empty;
            try
            {
                res = wc.DownloadString(Properties.Settings.Default.gdkpAddress);
                String final = String.Empty;
                // multiTable
                int start = res.IndexOf("multiTable");
                int end = res.IndexOf("DKPInfo");
                int count = end - start;
                int last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                String sub = res.Substring(start, (last + 1) - start);
                final = "RW_DKP_DATA = {\n\t";
                final += sub + ",\n\n\t";

                // DKPInfo
                start = end;
                end = res.IndexOf("gdkp");
                count = end - start;
                last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                sub = res.Substring(start, (last + 1) - start);
                final += sub + ",\n\n\t";

                // DKP
                start = end;
                end = res.IndexOf("DKP_ITEMS");
                count = end - start;
                last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                sub = res.Substring(start, (last + 1) - start);
                final += sub + ",\n\n\t";

                // DKP_ITEMS
                start = end;
                int itemidEnd = res.IndexOf("getdkp_itemids");
                if (itemidEnd != -1)
                {
                    end = itemidEnd;
                    count = end - start;
                    last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                    sub = res.Substring(start, (last + 1) - start);
                    final += sub + ",\n\n\t";
                }

                // getdkp_itemids
                start = end;
                end = res.IndexOf("GetDKPRaidPlaner");
                if (end == -1)
                    end = res.Length;
                count = end - start;
                last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                sub = res.Substring(start, (last + 1) - start);
                final += sub + ",\n\n\t";

                if (end != res.Length)
                {
                    // GetDKPRaidPlaner
                    start = end;
                    end = res.LastIndexOf("END");
                    count = end - start;
                    last = res.LastIndexOf("}", end, count, StringComparison.InvariantCultureIgnoreCase);

                    sub = res.Substring(start, (last + 1) - start);
                    final += sub + ",\n";
                }
                else
                {
                    final += "GetDKPRaidPlaner = {\n\t}\n";
                }

                final += "}";

                return final;
            }
            catch (Exception ex)
            {
                if (!auto)
                    MessageBox.Show(ex.Message, "Error while downloading data");
                return null;
            }
        }
    }
}
