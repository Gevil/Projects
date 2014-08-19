/* ******************************** *
 * Excel Maxsees String feldolgozó 
 * ******************************** *
 * 
 * - Czippán András (Gevil)
 * - 2013. 10. 30.
 * 
 * ******************************** */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Betolto.Model;
using NPOI.HSSF.UserModel;

namespace Betolto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("hu-HU");
            InitializeComponent();

            this.DataContext = this;

            Log = new ObservableCollection<LogItem>();
            //DataTable dtExcel = new DataTable();

            txtOutput.Text = AppDomain.CurrentDomain.BaseDirectory;
        }
        private ObservableCollection<string> _sheetNames;
        public ObservableCollection<string> SheetNames
        {
            get { return _sheetNames; }
            set
            {
                _sheetNames = value;
                OnPropertyChanged("SheetNames");
            }
        }

        private ObservableCollection<LogItem> _log;
        public ObservableCollection<LogItem> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged("Log");
            }
        }

        private DataTable _dtExcel;
        public DataTable DtExcel
        {
            get { return _dtExcel; }
            set
            {
                _dtExcel = value;
                OnPropertyChanged("DtExcel");
            }
        }

        private List<string> _filePaths;
        private Dictionary<string,string> _fileData;

        private Dictionary<string, string> _newFileData;

        public Dictionary<string, List<string>> _dictionaryOld;

        public Dictionary<string, string> _dictionaryNew;

        public Dictionary<string, string> _dictionaryFiltered;

        private void BtnOutputClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            var result = dlg.ShowDialog();

            //var openInputFileDialog = new OpenFileDialog { Filter = "Excel 97 - 2003 fájlok|*.xls|Excel 2007 - 2010 fájlok |*.xlsx" };

            // Show the dialog and get result.
            //DialogResult result = openInputFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) // Test result.
            {
                txtOutput.Text = dlg.SelectedPath;
            }
        }

        private void BtnInputClick(object sender, RoutedEventArgs e)
        {
            //var openInputFileDialog = new OpenFileDialog {Filter = "Excel 97 - 2003 fájlok|*.xls|Excel 2007 - 2010 fájlok |*.xlsx"};

            var openInputFileDialog = new OpenFileDialog { Filter = "Resx fájlok |*.resx" };

            // Show the dialog and get result.
            var result = openInputFileDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            txtInput.Text = openInputFileDialog.FileName;

            WriteLog("A Betöltendő Fájl útvonala: " + txtInput.Text);

            /*OleDbConnection objConn = null;
            DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you will search.
                var connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + txtInput.Text +
                                    ";Extended Properties=Excel 8.0;";

                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);

                // Open connection with the database.
                objConn.Open();

                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                SheetNames = new ObservableCollection<string>();

                // Add the sheet name to the string array.
                if (dt != null)
                    foreach (DataRow row in dt.Rows)
                    {
                        SheetNames.Add(row["TABLE_NAME"].ToString());
                    }
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }

                cmbSheets.SelectedItem = SheetNames[0];
            }*/
        }

        private void BtnLoadClick(object sender, RoutedEventArgs e)
        {
            tabcontrol.SelectedIndex = 1;

            //FixResxEntries();
            //GenerateStringsFromXamlFiles();
            CombineMultiLineStringsInCsFiles();
        }

        private void FixResxEntries()
        {
            if (txtInput.Text == null) return;

            //Read from existing file
            var readerNewFile = new ResXResourceReader(txtInput.Text);

            //create new keys list
            var writer = new ResXResourceWriter("C:\\maxsees\\newresources.resx");
            foreach (DictionaryEntry d in readerNewFile)
            {
                var newstring = d.Value.ToString().Replace(@"\n", "\n").Replace(@"\r","\r").Replace(@"\t", "\t");
                WriteLog("Új string érték: " + newstring);
                writer.AddResource(d.Key.ToString(), newstring);
            }

            readerNewFile.Close();
            writer.Close();
        }
        
        private void GenerateStringsFromCsFiles()
        {
            //get filelist from the whole maxsees directory
            if (txtOutput.Text != null)
            {
                _filePaths = new List<string>();
                //_filePaths.AddRange(Directory.GetFiles(txtOutput.Text, "*.cs", SearchOption.AllDirectories));
                //_filePaths.AddRange(Directory.GetFiles(txtOutput.Text, "*.xaml", SearchOption.AllDirectories));

                foreach (var filepath in Directory.GetFiles(txtOutput.Text, "*.cs", SearchOption.AllDirectories))
                {
                    if(!filepath.Contains("\\Report\\Finance") &&
                        !filepath.Contains("Designer.cs") &&
                        !filepath.Contains(".g") &&
                        !filepath.Contains("\\Converters\\"))
                        _filePaths.Add(filepath);
                }
            }

            _fileData = new Dictionary<string, string>();

            foreach (var filepath in _filePaths)
            {
                _fileData.Add(filepath, File.ReadAllText(filepath));
            }

            var j = 0;

            //Copy out the key collection as an array.
            var keys = _fileData.Keys.ToArray();

            var newStringsDictionary = new Dictionary<string, string>();

            //Do not use a foreach loop
            for (j = 0; j < keys.Length; j++)
            {
                if (_fileData[keys[j]] != null /*.Contains("�")*/)
                {
                    //WriteLog("Fájl: " + keys[j]);
                    
                    // Match all quoted fields
                    //var matchcollection = Regex.Matches(_fileData[keys[j]], "\"(.*?)\""); //shittier match can't handle escape \ character

                    //match with quotes included
                    //var matchcollection = Regex.Matches(_fileData[keys[j]], @"(?<="")[^\""]*(?="")");
                    //@"""[^\""]*"""
                    var matchcollection = Regex.Matches(_fileData[keys[j]], @"""[^\""]*""");


                    //@"(?<="")(?:\\.|[^""\\])*(?="")" //match without quotes
                    //var matchcollection = Regex.Matches(_fileData[keys[j]], @"(?<="")(?:\\.|[^""\\])*(?="")");

                    //this is for *.cs files only
                    foreach (Match match in matchcollection)
                    {
                        //if (match.Value.Contains("ó") || match.Value.Contains("Ó") ||
                        //    match.Value.Contains("é") || match.Value.Contains("É") ||
                        //    match.Value.Contains("á") || match.Value.Contains("Á") ||
                        //    match.Value.Contains("ü") || match.Value.Contains("Ü") ||
                        //    match.Value.Contains("ű") || match.Value.Contains("Ű") ||
                        //    match.Value.Contains("ö") || match.Value.Contains("Ö") ||
                        //    match.Value.Contains("ő") || match.Value.Contains("Ő") ||
                        //    match.Value.Contains("ú") || match.Value.Contains("Ú"))
                        if (!match.Value.Contains("_"))
                        {
                            if (!_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("==") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("!=") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("<=") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("<<") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("=>") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains(">>") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains(":=") &&
                            !_fileData[keys[j]].Substring(match.Index - 5, match.Length).Contains("?=") &&
                            !match.Value.Contains("Binding") && !match.Value.Contains("x:Static"))
                            {
                                {
                                    //WriteLog("String: " + match.Value);
                                    //WriteLog("\t" + _fileData[keys[j]].Substring(match.Index - 20, match.Length + 40));

                                    //Do processing with the proper string

                                    //Create Dictionary for key value pairs


                                    //Generate String Key
                                    var stringKey = match.Value;
                                    var ti = new CultureInfo("hu-HU", false).TextInfo;
                                    stringKey = stringKey.ToLowerInvariant();
                                    stringKey = stringKey.Replace(@"\t", "").Replace(@"\r", "").Replace(@"\n", "");

                                    stringKey = stringKey.ToLowerInvariant();

                                    stringKey = stringKey.Replace("ö", "o");
                                    stringKey = stringKey.Replace("ő", "o");
                                    stringKey = stringKey.Replace("ü", "u");
                                    stringKey = stringKey.Replace("ű", "u");
                                    stringKey = stringKey.Replace("ó", "o");
                                    stringKey = stringKey.Replace("ú", "u");
                                    stringKey = stringKey.Replace("é", "e");
                                    stringKey = stringKey.Replace("á", "a");
                                    stringKey = stringKey.Replace("í", "i");

                                    stringKey = ti.ToTitleCase(stringKey.Replace("\"", ""));

                                    stringKey = stringKey.Replace(" ", "_");
                                    stringKey = stringKey.Replace(",", "_");
                                    stringKey = stringKey.Replace(":", "_");
                                    stringKey = stringKey.Replace("[", "_");
                                    stringKey = stringKey.Replace("]", "_");
                                    stringKey = stringKey.Replace("{", "_");
                                    stringKey = stringKey.Replace("}", "_");
                                    stringKey = stringKey.Replace("?", "_");
                                    stringKey = stringKey.Replace("!", "_");
                                    stringKey = stringKey.Replace(".", "_");
                                    stringKey = stringKey.Replace("(", "_");
                                    stringKey = stringKey.Replace(")", "_");
                                    stringKey = stringKey.Replace("%", "_");
                                    stringKey = stringKey.Replace("*", "_");
                                    stringKey = stringKey.Replace("$", "_");
                                    stringKey = stringKey.Replace("-", "_");
                                    stringKey = stringKey.Replace("+", "_");
                                    stringKey = stringKey.Replace("\"", "_");
                                    stringKey = stringKey.Replace("\\", "_");
                                    stringKey = stringKey.Replace("/", "_");
                                    stringKey = stringKey.Replace("|", "_");
                                    stringKey = stringKey.Replace("<", "_");
                                    stringKey = stringKey.Replace(">", "_");

                                    if (!newStringsDictionary.ContainsKey(stringKey))
                                    {
                                        newStringsDictionary.Add(stringKey, match.Value.Substring(1, match.Value.Length - 2));
                                    }

                                    //Replace original_string to Utils.GetStringFromLanguageResource("New_Key")

                                    //Replace old string to new in file
                                    var oldstring_cs = match.Value;
                                    var newstring_cs = "Utils.GetStringFromLanguageResource(\"" + stringKey + "\")";

                                    //_fileData[keys[j]] = _fileData[keys[j]].Replace(oldstring_cs, newstring_cs);

                                    WriteLog("Generált Key: " + stringKey);
                                    WriteLog("-> String: " + match.Value.Substring(1, match.Value.Length - 2));


                                }

                            }
                        }
                        

                    }
                }
            }

            /*foreach (var filedata in _fileData)
            {
                File.WriteAllText(filedata.Key, filedata.Value);
            }

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Maxsees new string dictionary");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("String");
            row.CreateCell(1).SetCellValue("NewID");

            WriteLog("Excel fájl készítése.");
            foreach (var stringKey in newStringsDictionary)
            {
                WriteLog("Key: " + stringKey.Key);
                WriteLog("String: " + stringKey.Value);

                // Add data rows
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(stringKey.Key);
                row.CreateCell(1).SetCellValue(stringKey.Value);
                rowIndex++;
            }

            //Write the Workbook to a memory stream
            using (var fileData = new FileStream("C:\\Maxsees\\newstrings.xls", FileMode.Create))
            {
                workbook.Write(fileData);
            }
            */
        }

        private void CombineMultiLineStringsInCsFiles()
        {
            //get filelist from the whole maxsees directory
            if (txtOutput.Text != null)
            {
                _filePaths = new List<string>();
                foreach (var filepath in Directory.GetFiles(txtOutput.Text, "*.cs", SearchOption.AllDirectories).Where(filepath => !filepath.Contains("\\Report\\Finance") &&
                                                                                                                                   !filepath.Contains("Designer.cs") &&
                                                                                                                                   !filepath.Contains(".g") &&
                                                                                                                                   !filepath.Contains("\\Converters\\")))
                {
                    _filePaths.Add(filepath);
                }
            }

            _fileData = new Dictionary<string, string>();

            foreach (var filepath in _filePaths)
            {
                _fileData.Add(filepath, File.ReadAllText(filepath));
            }

            var j = 0;

            //Copy out the key collection as an array.
            var keys = _fileData.Keys.ToArray();

            var newStringsDictionary = new Dictionary<string, string>();

            //Do not use a foreach loop
            for (j = 0; j < keys.Length; j++)
            {
                if (_fileData[keys[j]] != null)
                {
                    WriteLog("Fájl: " + keys[j]);

                    var matchcollection = Regex.Matches(_fileData[keys[j]], @"""[^\""]*""");

                    foreach (Match match in matchcollection)
                    {
                        if(_fileData[keys[j]].Substring(match.Index - 38, 40).Contains("Utils.GetStringFromLanguageResource(") &&
                           _fileData[keys[j]].Substring(match.Index + match.Length, 44).Contains("Utils.GetStringFromLanguageResource("))
                        {
                            WriteLog(_fileData[keys[j]].Substring(match.Index - 38, match.Length + 40));
                            WriteLog(_fileData[keys[j]].Substring(match.Index + match.Length, 44));
                        }
                    }
                }
            }
        }

        private void GenerateStringsFromXamlFiles()
        {
            //get filelist from the whole maxsees directory
            if (txtOutput.Text != null)
            {
                _filePaths = new List<string>();

                foreach (var filepath in Directory.GetFiles(txtOutput.Text, "*.xaml", SearchOption.AllDirectories).Where(filepath => !filepath.Contains("\\Report\\Finance") &&
                                                                                                                                     !filepath.Contains("\\Converters\\") &&
                                                                                                                                     !filepath.Contains("\\ControlStyles\\") &&
                                                                                                                                     !filepath.Contains("LoginDialog.xaml")))
                {
                    _filePaths.Add(filepath);
                }
            }

            _fileData = new Dictionary<string, string>();
            _newFileData = new Dictionary<string, string>();

            foreach (var filepath in _filePaths)
            {
                _fileData.Add(filepath, File.ReadAllText(filepath));
            }

            var j = 0;

            //Copy out the key collection as an array.
            var keys = _fileData.Keys.ToArray();

            var newStringsDictionary = new Dictionary<string, string>();

            //Do not use a foreach loop
            for (j = 0; j < keys.Length; j++)
            {
                if (_fileData[keys[j]] != null)
                {
                    WriteLog("Fájl: " + keys[j]);

                    var matchcollection = Regex.Matches(_fileData[keys[j]], @"""[^\""]*""");

                    for (var i = matchcollection.Count-1; i > 0; i--)
                    {
                        if (!matchcollection[i].Value.Contains("_") &&
                            !matchcollection[i].Value.Contains("Binding") &&
                            !matchcollection[i].Value.Contains("Converter") &&
                            !matchcollection[i].Value.Contains("MAXsees.Resources;") &&
                            !matchcollection[i].Value.Contains("{") &&
                            !matchcollection[i].Value.Contains("}") &&
                            matchcollection[i].Value != "\" \"")
                        {
                            if (_fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Header =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Content =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Label =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Caption =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" NullText =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Text =") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Header=") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Content=") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Label=") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Caption=") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" NullText=") ||
                                _fileData[keys[j]].Substring(matchcollection[i].Index - 10, 10).Contains(" Text="))
                            {
                                if (!_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("==") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("!=") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("<=") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("<<") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("=>") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains(">>") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains(":=") &&
                                    !_fileData[keys[j]].Substring(matchcollection[i].Index - 5, matchcollection[i].Length).Contains("?="))
                                {
                                    {
                                        //Generate String Key
                                        var stringKey = matchcollection[i].Value;
                                        var ti = new CultureInfo("hu-HU", false).TextInfo;
                                        stringKey = stringKey.ToLowerInvariant();
                                        stringKey = stringKey.Replace(@"\t", "").Replace(@"\r", "").Replace(@"\n", "");

                                        stringKey = stringKey.ToLowerInvariant();

                                        stringKey = stringKey.Replace("ö", "o");stringKey = stringKey.Replace("ő", "o");stringKey = stringKey.Replace("ü", "u");
                                        stringKey = stringKey.Replace("ű", "u");stringKey = stringKey.Replace("ó", "o");stringKey = stringKey.Replace("ú", "u");
                                        stringKey = stringKey.Replace("é", "e");stringKey = stringKey.Replace("á", "a");stringKey = stringKey.Replace("í", "i");

                                        stringKey = ti.ToTitleCase(stringKey.Replace("\"", ""));

                                        stringKey = stringKey.Replace(" ", "_");stringKey = stringKey.Replace(",", "_");stringKey = stringKey.Replace(":", "_");
                                        stringKey = stringKey.Replace("[", "_");stringKey = stringKey.Replace("]", "_");stringKey = stringKey.Replace("{", "_");
                                        stringKey = stringKey.Replace("}", "_");stringKey = stringKey.Replace("?", "_");stringKey = stringKey.Replace("!", "_");
                                        stringKey = stringKey.Replace("(", "_");stringKey = stringKey.Replace(")", "_");stringKey = stringKey.Replace("%", "_");
                                        stringKey = stringKey.Replace("*", "_");stringKey = stringKey.Replace("$", "_");stringKey = stringKey.Replace("-", "_");
                                        stringKey = stringKey.Replace("+", "_");stringKey = stringKey.Replace("=", "_");stringKey = stringKey.Replace("\"", "_");
                                        stringKey = stringKey.Replace("\\", "_");stringKey = stringKey.Replace("/", "_");stringKey = stringKey.Replace("|", "_");
                                        stringKey = stringKey.Replace("<", "_");stringKey = stringKey.Replace(">", "_");stringKey = stringKey.Replace("©", "_");
                                        stringKey = stringKey.Replace("&", "_");stringKey = stringKey.Replace(".", "_");stringKey = stringKey.Replace("#", "_");
                                        stringKey = stringKey.Replace("@", "_");stringKey = stringKey.Replace("@", "_");

                                        WriteLog("Generált Key: " + stringKey);
                                        WriteLog("-> String: " + matchcollection[i].Value.Substring(1, matchcollection[i].Value.Length - 2));
                                        WriteLog("-> StringPrefix: " + _fileData[keys[j]].Substring(matchcollection[i].Index - 10, matchcollection[i].Value.Length + 12));

                                        if (!newStringsDictionary.ContainsKey(stringKey))
                                        {
                                            newStringsDictionary.Add(stringKey, matchcollection[i].Value.Substring(1, matchcollection[i].Value.Length - 2));
                                        }

                                        //Replace old string to new in file
                                        var oldstring_cs = matchcollection[i].Value;
                                        var newstring_cs = "\"{x:Static p:Languages." + stringKey + "}\"";

                                        var stringFirstPart = _fileData[keys[j]].Substring(0, matchcollection[i].Index);
                                        var stringSecondPart = _fileData[keys[j]].Substring(matchcollection[i].Index);

                                        if(oldstring_cs != "" && newstring_cs != "")
                                            stringSecondPart = stringSecondPart.Replace(oldstring_cs, newstring_cs);

                                        _fileData[keys[j]] = stringFirstPart + stringSecondPart;
                                    }
                                }
                            }
                        }
                    }

                    //xmlns:p="clr-namespace:MAXsees.Framework.Core.LanguageResources;assembly=MAXsees.Framework"
                    if(!_fileData[keys[j]].Contains("xmlns:p=\"clr-namespace:MAXsees.Framework.Core.LanguageResources;assembly=MAXsees.Framework\""))
                    {
                        const string find = "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
                        var newstr = "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"" + Environment.NewLine +
                            "             xmlns:p=\"clr-namespace:MAXsees.Framework.Core.LanguageResources;assembly=MAXsees.Framework\"";
                        _fileData[keys[j]] = _fileData[keys[j]].Replace(find, newstr);
                    }
                    
                }
            }

            foreach (var filedata in _fileData)
            {
                File.WriteAllText(filedata.Key, filedata.Value);
            }

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Maxsees new string dictionary");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("String");
            row.CreateCell(1).SetCellValue("NewID");

            WriteLog("Excel fájl készítése.");
            foreach (var stringKey in newStringsDictionary)
            {
                WriteLog("Key: " + stringKey.Key);
                WriteLog("String: " + stringKey.Value);

                // Add data rows
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(stringKey.Key);
                row.CreateCell(1).SetCellValue(stringKey.Value);
                rowIndex++;
            }

            //Write the Workbook to a memory stream
            using (var fileData = new FileStream("C:\\Maxsees\\newstrings.xls", FileMode.Create))
            {
                workbook.Write(fileData);
            }
            
        }

        private void ProcessStringsFromExcel()
        {
            if (txtInput.Text == "") return;
            var count = 0;

            //try
            {
                //Dictionary generálása: key=régiID, value1=újID, value2=String 
                WriteLog("Eredeti Stringek feldolgozásának megkezdése!");

                _dictionaryOld = new Dictionary<string, List<string>>();
                _dictionaryNew = new Dictionary<string, string>();

                //read rows
                for (var i = 0; i < DtExcel.Rows.Count; i++)
                {
                    var oldID = "";
                    if (DtExcel.Rows[i][0].ToString() != "")
                    {
                        //oldID
                        if (DtExcel.Rows[i][0].ToString().Replace(@"\", "").Replace("\"", "") != "")
                        {
                            oldID = DtExcel.Rows[i][0].ToString();
                        }
                    }
                    var oldString = "";
                    //oldSTRING
                    if (DtExcel.Rows[i][1].ToString().Replace(@"\", "").Replace("\"", "") != "")
                    {
                        oldString = DtExcel.Rows[i][1].ToString();
                    }

                    //Process Original String and create new ID from it and add it to dictionaries
                    var l = new List<string> {oldString}; //original string

                    //new identifier
                    var ti = new CultureInfo("hu-HU", false).TextInfo;
                    oldString = oldString.ToLowerInvariant();
                    oldString = oldString.Replace(@"\t", "").Replace(@"\r", "").Replace(@"\n", "");

                    oldString = oldString.ToLowerInvariant();

                    oldString = oldString.Replace("ö", "o");
                    oldString = oldString.Replace("ő", "o");
                    oldString = oldString.Replace("ü", "u");
                    oldString = oldString.Replace("ű", "u");
                    oldString = oldString.Replace("ó", "o");
                    oldString = oldString.Replace("ú", "u");
                    oldString = oldString.Replace("é", "e");
                    oldString = oldString.Replace("á", "a");
                    oldString = oldString.Replace("í", "i");

                    oldString = ti.ToTitleCase(oldString.Replace("\"", ""));

                    oldString = oldString.Replace(" ", "_");
                    oldString = oldString.Replace(",", "_");
                    oldString = oldString.Replace(":", "_");
                    oldString = oldString.Replace("[", "_");
                    oldString = oldString.Replace("]", "_");
                    oldString = oldString.Replace("{", "_");
                    oldString = oldString.Replace("}", "_");
                    oldString = oldString.Replace("?", "_");
                    oldString = oldString.Replace("!", "_");
                    oldString = oldString.Replace(".", "_");
                    oldString = oldString.Replace("(", "_");
                    oldString = oldString.Replace(")", "_");
                    oldString = oldString.Replace("%", "_");
                    oldString = oldString.Replace("*", "_");
                    oldString = oldString.Replace("$", "_");
                    oldString = oldString.Replace("-", "_");
                    oldString = oldString.Replace("+", "_");
                    oldString = oldString.Replace("\"", "_");
                    oldString = oldString.Replace("\\", "_");
                    oldString = oldString.Replace("/", "_");
                    oldString = oldString.Replace("|", "_");


                    l.Add(oldString);

                    WriteLog("Feldolgozás Alatt: ID - " + oldID);
                    WriteLog("-> EredetiString: " + oldString);
                    WriteLog("-> Új ID: " + l[1]);

                    //add to dictionaries

                    //oldID,original string
                    if(!_dictionaryOld.ContainsKey(oldID))
                    {
                        _dictionaryOld.Add(oldID, l);

                        //newID, original string
                        if (!_dictionaryNew.ContainsKey(l[1]))
                            _dictionaryNew.Add(l[1], l[0]);

                    }

                    count++;
                }

                WriteLog("Feldolgozott Eredeti Stringek száma: " + count.ToString(CultureInfo.InvariantCulture));

                //get filelist from the whole maxsees directory
                if(txtOutput.Text != null)
                {
                    _filePaths = new List<string>();
                    _filePaths.AddRange(Directory.GetFiles(txtOutput.Text, "*.cs", SearchOption.AllDirectories));
                    _filePaths.AddRange(Directory.GetFiles(txtOutput.Text, "*.xaml", SearchOption.AllDirectories));
                }

                //writing to files disable until we see the new generated IDs are okay
                
                // Display all the files.
                _fileData = new Dictionary<string, string>();

                foreach (var filepath in _filePaths)
                {
                    _fileData.Add(filepath, File.ReadAllText(filepath));
                    //Generate New String Identifier based on Original Content of Resx Value

                    //TODO STUFF

                    
                }

                var j = 0;

                //Copy out the key collection as an array.
                var keys = _fileData.Keys.ToArray();

                //Do not use a foreach loop
                for (j = 0; j < keys.Length; j++)
                {
                    WriteLog("Fájl: " + keys[j]);
                    //crawl string IDs in dictionary with key value pairs
                    foreach (var v in _dictionaryOld)
                    {
                        //Replace old string to new in file
                        var oldstring_xaml = "{x:Static p:Languages." + v.Key + "}";
                        var oldstring_cs = "GetStringFromLanguageResource(\"" + v.Key + "\")";

                        var newstring_xaml = "{x:Static p:Languages." + v.Value[1] + "}";
                        var newstring_cs = "GetStringFromLanguageResource(\"" + v.Value[1] + "\")";

                        _fileData[keys[j]] = _fileData[keys[j]].Replace(oldstring_xaml, newstring_xaml);
                        _fileData[keys[j]] = _fileData[keys[j]].Replace(oldstring_cs, newstring_cs);
                    }
                }

                foreach (var filedata in _fileData)
                {
                    File.WriteAllText(filedata.Key, filedata.Value);
                }
            }
            //finally
            {
                //WriteLog("Sikeresen feldolgoztam az adatokat. Stringek száma: " + count.ToString(CultureInfo.InvariantCulture) + "db.");
                //write excel of new strings dictionary
                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet("Maxsees new string dictionary");

                var rowIndex = 0;
                var row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue("String");
                row.CreateCell(1).SetCellValue("NewID");

                foreach (var v in _dictionaryNew)
                {
                    // Add data rows
                    row = sheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(v.Key);
                    row.CreateCell(1).SetCellValue(v.Value);
                    rowIndex++;
                }

                //Write the Workbook to a memory stream
                using (var fileData = new FileStream("C:\\Maxsees\\newstrings.xls", FileMode.Create))
                {
                    workbook.Write(fileData);
                }
            }

        }

        private void WriteLog(string output)
        {
            Log.Add(new LogItem {MessageDateTime = DateTime.Now, Message = output});
            Debug.WriteLine(output);
        }

        private void ComboBoxEditSelectedIndexChanged1(object sender, RoutedEventArgs e)
        {
            var sourceConstr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + txtInput.Text +
                                  "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

            // properties to make it writable
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Docs\Test.xls;Mode=ReadWrite;Extended Properties=""Excel 8.0;HDR=Yes""
            var con = new OleDbConnection(sourceConstr);
            var query = "Select * from [" + cmbSheets.SelectedItem + "]";
            var data = new OleDbDataAdapter(query, con);

            DtExcel = new DataTable();
            data.Fill(DtExcel);

            //dxgrid.ItemsSource = DtExcel;
            dxgrid.RefreshData();
            dxgrid.PopulateColumns();

            con.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
            
        }
    }
}