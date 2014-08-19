using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace EE_DKP_updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.WriteFile(args[0],args[1]);
        }

        static public void WriteFile(string getdkpaddress, string wowfolder)
        {
            try
            {
                if (String.IsNullOrEmpty(wowfolder) || String.IsNullOrEmpty(getdkpaddress))
                {
                    Console.WriteLine("You have to start this application with these arguments dkp_extractor.exe -getdkp.phpaddress -wowfolderpath");
                    return;
                }

                if (!Directory.Exists(System.IO.Path.Combine(wowfolder, @"RaidWatch")))
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(wowfolder, @"RaidWatch"));
                }

                using (FileStream fs = new FileStream(System.IO.Path.Combine(wowfolder, @"RaidWatch\DKPData.lua"), FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    WebClient wc = new WebClient();
                    String data = GetDKPData(getdkpaddress);
                    if (String.IsNullOrEmpty(data))
                    {
                        return;
                    }
                    sw.Write(data);
                    sw.Close();
                    sw.Dispose();
                }
                    Console.WriteLine("Done!\n File written to: " + System.IO.Path.Combine(wowfolder, @"Interface\AddOns\RaidWatch\DKPata.lua"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public String GetDKPData(string getdkpaddress)
        {
            WebClient wc = new WebClient();
            String res = String.Empty;
            try
            {
                res = wc.DownloadString(getdkpaddress);
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
                Console.WriteLine(ex.Message, "Error while downloading data");
                return null;
            }
        }
    
    }
}
