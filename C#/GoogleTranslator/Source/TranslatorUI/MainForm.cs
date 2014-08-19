using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using AlexMG.GoogleTranslator;

namespace TranslatorUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cbDestLang.DataSource = GetLanguageTable();
            cbDestLang.DisplayMember = "Name";
            cbDestLang.ValueMember = "Value";
        }

        private Dictionary<string, string> ReadResourceValues(string filename)
        {
            var results = new Dictionary<string, string>();
            // Create a ResXResourceReader for the file items.resx.
            ResXResourceReader rsxr = new ResXResourceReader(filename);

            // Create an IDictionaryEnumerator to iterate through the resources.
            IDictionaryEnumerator id = rsxr.GetEnumerator();

            // Iterate through the resources and display the contents
            foreach (DictionaryEntry d in rsxr)
            {
                results.Add(d.Key.ToString(), d.Value.ToString());
            }

            return results;
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            pgBar.Value = 0;
            pgBar.Visible = true;
            btnSave.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("Source Key");
                table.Columns.Add("Source Value");
                table.Columns.Add("Translation");

                var resource = ReadResourceValues(txtResourceFile.Text);
                pgBar.Maximum = resource.Keys.Count;

                foreach (string key in resource.Keys)
                {
                    var row = table.NewRow();
                    row[0] = key;
                    row[1] = resource[key];

                    //try
                    {
                        TranslationResponse response = Google.Translate(resource[key], Language.Unknown,
                                                                        cbDestLang.SelectedValue.ToString(),
                                                                        TextFormat.Text);
                        row[2] = response.ResponseData.TranslatedText;
                    }
                    //catch
                    {
                        //row[2] = "?";
                    }                    
                    
                    table.Rows.Add(row);
                    pgBar.Value += 1;
                }

                grid.DataSource = table;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                pgBar.Visible = false;
                btnSave.Enabled = true;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Browse for resource file";
            dlg.Filter = "Resource File|*.resx";
            if (dlg.ShowDialog() != DialogResult.OK) return;
            txtResourceFile.Text = dlg.FileName;
        }

        private DataTable GetLanguageTable()
        {
            var table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            table.Rows.Add( "Afrikaans", "af" );
            table.Rows.Add( "Albanian",  "sq");
            table.Rows.Add( "Amharic",  "am");
            table.Rows.Add( "Arabic",  "ar");
            table.Rows.Add( "Armenian",  "hy");
            table.Rows.Add( "Azerbaijani",  "az");
            table.Rows.Add( "Basque",  "eu");
            table.Rows.Add( "Belarusian",  "be");
            table.Rows.Add( "Bengali",  "bn");
            table.Rows.Add( "Bihari",  "bh");
            table.Rows.Add( "Bulgarian",  "bg");
            table.Rows.Add( "Burmese",  "my");
            table.Rows.Add( "Catalan",  "ca");
            table.Rows.Add( "Cherokee",  "chr");
            table.Rows.Add( "Chinese",  "zh");
            table.Rows.Add( "ChineseSimplified",  "zh-CN");
            table.Rows.Add( "ChineseTraditional",  "zh-TW");
            table.Rows.Add( "Croatian",  "hr");
            table.Rows.Add( "Czech",  "cs");
            table.Rows.Add( "Danish",  "da");
            table.Rows.Add( "Dhivehi",  "dv");
            table.Rows.Add( "Dutch",  "nl");
            table.Rows.Add( "English",  "en");
            table.Rows.Add( "Esperanto",  "eo");
            table.Rows.Add( "Estonian",  "et");
            table.Rows.Add( "Filipino",  "tl");
            table.Rows.Add( "Finnish",  "fi");
            table.Rows.Add( "French",  "fr");
            table.Rows.Add( "Galician",  "gl");
            table.Rows.Add( "Georgian",  "ka");
            table.Rows.Add( "German",  "de");
            table.Rows.Add( "Greek",  "el");
            table.Rows.Add( "Guarani",  "gn");
            table.Rows.Add( "Gujarati",  "gu");
            table.Rows.Add( "Hebrew",  "iw");
            table.Rows.Add( "Hindi",  "hi");
            table.Rows.Add( "Hungarian",  "hu");
            table.Rows.Add( "Icelandic",  "is");
            table.Rows.Add( "Indonesian",  "id");
            table.Rows.Add( "Inuktitut",  "iu");
            table.Rows.Add( "Italian",  "it");
            table.Rows.Add( "Japanese",  "ja");
            table.Rows.Add( "Kannada",  "kn");
            table.Rows.Add( "Kazakh",  "kk");
            table.Rows.Add( "Khmer",  "km");
            table.Rows.Add( "Korean",  "ko");
            table.Rows.Add( "Kurdish",  "ku");
            table.Rows.Add( "Kyrgyz",  "ky");
            table.Rows.Add( "Laothian",  "lo");
            table.Rows.Add( "Latvian",  "lv");
            table.Rows.Add( "Lithuanian",  "lt");
            table.Rows.Add( "Macedonian",  "mk");
            table.Rows.Add( "Malay",  "ms");
            table.Rows.Add( "Malayalam",  "ml");
            table.Rows.Add( "Maltese",  "mt");
            table.Rows.Add( "Marathi",  "mr");
            table.Rows.Add( "Mongolian",  "mn");
            table.Rows.Add( "Nepali",  "ne");
            table.Rows.Add( "Norwegian",  "no");
            table.Rows.Add( "Oriya",  "or");
            table.Rows.Add( "Pashto",  "ps");
            table.Rows.Add( "Persian",  "fa");
            table.Rows.Add( "Polish",  "pl");
            table.Rows.Add( "Portuguese",  "pt-PT");
            table.Rows.Add( "Punjabi",  "pa");
            table.Rows.Add( "Romanian",  "ro");
            table.Rows.Add( "Sanskrit",  "sa");
            table.Rows.Add( "Serbian",  "sr");
            table.Rows.Add( "Sindhi",  "sd");
            table.Rows.Add( "Sinhalese",  "si");
            table.Rows.Add( "Slovak",  "sk");
            table.Rows.Add( "Slovenian",  "sl");
            table.Rows.Add( "Spanish",  "es");
            table.Rows.Add( "Swahili",  "sw");
            table.Rows.Add( "Swedish",  "sv");
            table.Rows.Add( "Tagalog",  "tl");
            table.Rows.Add( "Tajik",  "tg");
            table.Rows.Add( "Tamil",  "ta");
            table.Rows.Add( "Telugu",  "te");
            table.Rows.Add( "Thai",  "th");
            table.Rows.Add( "Tibetan",  "bo");
            table.Rows.Add( "Turkish",  "tr");
            table.Rows.Add( "Uighur",  "ug");
            table.Rows.Add( "Ukrainian",  "uk");            
            table.Rows.Add( "Urdu",  "ur");
            table.Rows.Add( "Uzbek",  "uz");
            table.Rows.Add( "Vietnamese",  "vi");
            return table;
        }

        private void txtResourceFile_TextChanged(object sender, EventArgs e)
        {
            btnTranslate.Enabled = System.IO.File.Exists(txtResourceFile.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save resource file";
            dlg.Filter = "Resource File|*.resx";
            dlg.FileName = cbDestLang.SelectedValue.ToString() + ".resx";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            ResXResourceWriter rsxw = new ResXResourceWriter(dlg.FileName);
            var table = (DataTable) grid.DataSource;
            foreach( DataRow row in table.Rows )
            {
                rsxw.AddResource( row[0].ToString(), row[2].ToString());    
            }
            rsxw.Generate();
            rsxw.Close();
        }
    }
}
