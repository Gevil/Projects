using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ResxTranslator
{
	public partial class MainForm : Form
	{
		TextWriter _writer = null;

        public static readonly Dictionary<string, string> LanguageNamesList = new Dictionary<string, string>
		                                                                      	{
                                                                                    {"hr-HR", "hr"},
                                                                                    {"en-GB", "en"},		                                                                      		
                                                                                    {"ar-SA", "ar"},
		                                                                      		{"cs-CZ", "cs"},
		                                                                      		{"da-DK", "da"},
		                                                                      		{"de-DE", "de"},
		                                                                      		{"es-ES", "es"},
		                                                                      		{"fr-FR", "fr"},
		                                                                      		{"he-IL", "he"},
		                                                                      		{"hi-IN", "hi"},
		                                                                      		{"it-IT", "it"},
		                                                                      		{"nb-NO", "no"},
		                                                                      		{"nl-NL", "nl"},
		                                                                      		{"pl-PL", "pl"},
		                                                                      		{"pt-PT", "pt"},
		                                                                      		{"ru-RU", "ru"},
		                                                                      		{"sv-SE", "sv"},
		                                                                      		{"tr-TR", "tr"},
		                                                                      		{"zh-CN", "zh-CN"}
		                                                                      	};

		public MainForm()
		{
			InitializeComponent();

			foreach (string key in Translator.LanguageNamesList.Keys)
			{
				listLanguages.Items.Add(key, Settings.GetIfLanguageIsTranslated(key));
			}

			tbInputPath.Text = Settings.InputFolderPath;
			tbOutputPath.Text = Settings.OutputFolderPath;
			cbCreateSubFolderForEveryLang.Checked = Settings.CreateFoldersForEachLanguage;
			
			// Instantiate the writer
			_writer = new TextBoxStreamWriter(txtConsole);
			// Redirect the out Console stream
			//Console.SetOut(_writer);

		}

		private void btnInputBrowse_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				tbInputPath.Text = folderBrowserDialog1.SelectedPath;

				//default output path value - is a parent dir to "inputpath"
				if (tbInputPath.Text.LastIndexOf("\\") != tbInputPath.Text.IndexOf("\\")) //if its not a root drive
					tbOutputPath.Text = tbInputPath.Text.Substring(0, tbInputPath.Text.LastIndexOf("\\"));
				else
					tbOutputPath.Text = tbInputPath.Text;
			}
		}

		private void btnOutputBrowse_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				tbOutputPath.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(tbInputPath.Text)) return;
			if (!Directory.Exists(tbOutputPath.Text)) return;

			txtConsole.Text = "";

			//saving settings for next run
			Settings.InputFolderPath = tbInputPath.Text;
			Settings.OutputFolderPath = tbOutputPath.Text;
			Settings.CreateFoldersForEachLanguage = cbCreateSubFolderForEveryLang.Checked;
			foreach (var item in listLanguages.Items) //reset all to fasle
			{
				Settings.SaveIfLanguageIsTranslated(item.ToString(), false);
			}

			foreach (var item in listLanguages.CheckedItems) //save checked
			{
				Settings.SaveIfLanguageIsTranslated(item.ToString(), true);
			}


            //translate strings here
			//var translator = new Translator();
			//translator.Translate();

            var bw = new BackgroundWorker {WorkerSupportsCancellation = true, WorkerReportsProgress = true};
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            bw.RunWorkerAsync();

		}

        public void WriteLog(string log)
        {
            txtConsole.Text = log + Environment.NewLine + txtConsole.Text;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                WriteLog("Canceled!");
            }

            else if (e.Error != null)
            {
                WriteLog("Error: ");
            }

            else
            {
                WriteLog("Done!");
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteLog(e.UserState.ToString());
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            
            // Perform a time consuming operation and report progress.
            string folder = Settings.InputFolderPath;

            string[] files = Directory.GetFiles(folder, "*.resx");
            if (files.Length == 0) return;

            var locales = new List<string>(LanguageNamesList.Keys);
            foreach (string locale in locales)
            {
                if (!Settings.GetIfLanguageIsTranslated(locale)) continue;

                bool creatDirs = Settings.CreateFoldersForEachLanguage;
                string newDir = Settings.OutputFolderPath;
                if (creatDirs)
                {
                    newDir += "\\" + locale;

                    if (Directory.Exists(newDir))
                    {
                        worker.ReportProgress(0, "Directory " + newDir + " already exists.");
                    }
                    else
                    {
                        worker.ReportProgress(0, "Creating directory " + newDir);

                        Directory.CreateDirectory(newDir);
                    }
                }
                foreach (string file in files)
                {
                    if (Regex.IsMatch(file.ToLower(), @"\.[a-zA-Z]{2}\-[a-zA-Z]{2}\.resx$")) continue; //skip non-english files

                    string shortName = Path.GetFileName(file);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
                    string newname = nameWithoutExt + "." + locale + ".resx";
                    newname = newDir + "\\" + newname;

                    //if file already exists
                    bool fileExists = File.Exists(newname);
                    var existing = new Dictionary<string, string>();
                    if (fileExists)
                    {
                        worker.ReportProgress(0, "File " + newname + " already exists. Existing resources in it will be preserved.");
                        //get existing keys list
                        var readerNewFile = new ResXResourceReader(newname);
                        foreach (DictionaryEntry d in readerNewFile)
                            existing.Add(d.Key.ToString(), d.Value.ToString());
                        readerNewFile.Close();
                    }
                    else
                    {
                        worker.ReportProgress(0, "Creating file " + newname);
                    }

                    worker.ReportProgress(0, "Translating file " + shortName + " to " + locale + "....");

                    //Application.DoEvents(); //I know its bad but can't go multithreading, since I have to update UI

                    var reader = new ResXResourceReader(file);
                    var writer = new ResXResourceWriter(newname);
                    foreach (DictionaryEntry d in reader)
                    {
                        //leave existing text intact (if its not empty)
                        if (fileExists
                            && existing.Keys.Contains(d.Key.ToString())
                            && !string.IsNullOrEmpty(existing[d.Key.ToString()]))
                        {
                            writer.AddResource(d.Key.ToString(), existing[d.Key.ToString()]);
                        }
                        else
                        {
                            string originalString = d.Value.ToString();
                            if (!string.IsNullOrEmpty(originalString.Trim()))
                            {
                                string langPair = "hu|" + LanguageNamesList[locale];

                                string translatedString = GoogleTranslate.TranslateGoogle(originalString, "hu", LanguageNamesList[locale]);

                                worker.ReportProgress(0, "[" + originalString + " -> " + translatedString + "]");

                                writer.AddResource(d.Key.ToString(), translatedString);

                                //Thread.Sleep(100); //to prevent spam detector at google
                            }
                        }
                    }
                    writer.Close();
                    reader.Close();

                }
            }

            worker.ReportProgress(0, "Finished!");

        }


		private void lblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			for (int i = 0; i < listLanguages.Items.Count; i++)
				listLanguages.SetItemChecked(i, true);
		}

        public void Translate()
        {
            string folder = Settings.InputFolderPath;

            string[] files = Directory.GetFiles(folder, "*.resx");
            if (files.Length == 0) return;

            List<string> locales = new List<string>(LanguageNamesList.Keys);
            foreach (string locale in locales)
            {
                if (!Settings.GetIfLanguageIsTranslated(locale)) continue;

                bool creatDirs = Settings.CreateFoldersForEachLanguage;
                string newDir = Settings.OutputFolderPath;
                if (creatDirs)
                {
                    newDir += "\\" + locale;

                    if (Directory.Exists(newDir))
                    {
                        Console.WriteLine("Directory " + newDir + " already exists.");
                    }
                    else
                    {
                        Console.WriteLine("Creating directory " + newDir);
                        Directory.CreateDirectory(newDir);
                    }
                }
                foreach (string file in files)
                {
                    if (Regex.IsMatch(file.ToLower(), @"\.[a-zA-Z]{2}\-[a-zA-Z]{2}\.resx$")) continue; //skip non-english files

                    string shortName = Path.GetFileName(file);
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(file);
                    string newname = nameWithoutExt + "." + locale + ".resx";
                    newname = newDir + "\\" + newname;

                    //if file already exists
                    bool fileExists = File.Exists(newname);
                    Dictionary<string, string> existing = new Dictionary<string, string>();
                    if (fileExists)
                    {
                        Console.WriteLine("File " + newname + " already exists. Existing resources in it will be preserved.");
                        //get existing keys list
                        ResXResourceReader readerNewFile = new ResXResourceReader(newname);
                        foreach (DictionaryEntry d in readerNewFile)
                            existing.Add(d.Key.ToString(), d.Value.ToString());
                        readerNewFile.Close();
                    }
                    else
                    {
                        Console.WriteLine("Creating file " + newname);
                    }

                    Console.WriteLine("Translating file " + shortName + " to " + locale + "....");

                    Application.DoEvents(); //I know its bad but can't go multithreading, since I have to update UI

                    ResXResourceReader reader = new ResXResourceReader(file);
                    ResXResourceWriter writer = new ResXResourceWriter(newname);
                    foreach (DictionaryEntry d in reader)
                    {
                        //leave existing text intact (if its not empty)
                        if (fileExists
                            && existing.Keys.Contains(d.Key.ToString())
                            && !string.IsNullOrEmpty(existing[d.Key.ToString()]))
                        {
                            writer.AddResource(d.Key.ToString(), existing[d.Key.ToString()]);
                        }
                        else
                        {
                            string originalString = d.Value.ToString();
                            if (!string.IsNullOrEmpty(originalString.Trim()))
                            {
                                string langPair = "hu|" + LanguageNamesList[locale];
                                //string translatedString = GoogleTranslate.TranslateText(originalString, langPair);

                                string translatedString = GoogleTranslate.TranslateGoogle(originalString, "hu", LanguageNamesList[locale]);

                                Console.WriteLine("[" + originalString + " -> " + translatedString + "]");

                                writer.AddResource(d.Key.ToString(), translatedString);
                                //Thread.Sleep(100); //to prevent spam detector at google
                            }
                        }
                    }
                    writer.Close();
                    reader.Close();

                }
            }


            Console.Write("Finished!");

        }

        private void TranslateFiles(string folder)
        {
            string[] files = Directory.GetFiles(folder, "*.resx");
            if (files.Length == 0) return;

            List<string> locales = new List<string>(LanguageNamesList.Keys);
            foreach (string locale in locales)
            {
                if (!Settings.GetIfLanguageIsTranslated(locale)) continue;

                bool creatDirs = Settings.CreateFoldersForEachLanguage;
                string newDir = Settings.OutputFolderPath;
                if (creatDirs)
                {
                    newDir += "\\" + locale;

                    if (Directory.Exists(newDir))
                    {
                        Console.WriteLine("Directory " + newDir + " already exists.");
                    }
                    else
                    {
                        Console.WriteLine("Creating directory " + newDir);
                        Directory.CreateDirectory(newDir);
                    }
                }
                foreach (string file in files)
                {
                    if (Regex.IsMatch(file.ToLower(), @"\.[a-zA-Z]{2}\-[a-zA-Z]{2}\.resx$")) continue; //skip non-english files

                    TranslateFile(file, locale, newDir);
                }
            }
        }

        private void TranslateFile(string filename, string locale, string newDir)
        {
            string shortName = Path.GetFileName(filename);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            string newname = nameWithoutExt + "." + locale + ".resx";
            newname = newDir + "\\" + newname;

            //if file already exists
            bool fileExists = File.Exists(newname);
            Dictionary<string, string> existing = new Dictionary<string, string>();
            if (fileExists)
            {
                Console.WriteLine("File " + newname + " already exists. Existing resources in it will be preserved.");
                //get existing keys list
                ResXResourceReader readerNewFile = new ResXResourceReader(newname);
                foreach (DictionaryEntry d in readerNewFile)
                    existing.Add(d.Key.ToString(), d.Value.ToString());
                readerNewFile.Close();
            }
            else
            {
                Console.WriteLine("Creating file " + newname);
            }

            Console.WriteLine("Translating file " + shortName + " to " + locale + "....");

            Application.DoEvents(); //I know its bad but can't go multithreading, since I have to update UI

            ResXResourceReader reader = new ResXResourceReader(filename);
            ResXResourceWriter writer = new ResXResourceWriter(newname);
            foreach (DictionaryEntry d in reader)
            {
                //leave existing text intact (if its not empty)
                if (fileExists
                    && existing.Keys.Contains(d.Key.ToString())
                    && !string.IsNullOrEmpty(existing[d.Key.ToString()]))
                {
                    writer.AddResource(d.Key.ToString(), existing[d.Key.ToString()]);
                }
                else
                {
                    string originalString = d.Value.ToString();
                    if (!string.IsNullOrEmpty(originalString.Trim()))
                    {
                        string langPair = "hu|" + LanguageNamesList[locale];
                        //string translatedString = GoogleTranslate.TranslateText(originalString, langPair);

                        string translatedString = GoogleTranslate.TranslateGoogle(originalString, "hu", LanguageNamesList[locale]);

                        Console.WriteLine("[" + originalString + " -> " + translatedString + "]");



                        writer.AddResource(d.Key.ToString(), translatedString);
                        //Thread.Sleep(100); //to prevent spam detector at google
                    }
                }
            }
            writer.Close();
            reader.Close();
        }


		//this class redirect "Console.Write" to a textbox
		private class TextBoxStreamWriter : TextWriter
		{
			TextBox _output = null;

			public TextBoxStreamWriter(TextBox output)
			{
				_output = output;
			}

			public override void Write(char value)
			{
				base.Write(value);
				_output.AppendText(value.ToString()); // When character data is written, append it to the text box.
			}

			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}
		}
	}
}
