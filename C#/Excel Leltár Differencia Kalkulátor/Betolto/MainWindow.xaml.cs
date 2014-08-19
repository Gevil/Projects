/* ******************************** *
 * Excel Adatbetöltő és Feldolgozó
 * ******************************** *
 * 
 * - Czippán András (Gevil)
 * - @2013. 04. 11.
 * 
 * - Immo leltár diff
 * - Update 2014. 02. 04. 
 * ******************************** */

using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Betolto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            Log = "";

        }

        private String[] _excelSheets;
        public String[] ExcelSheets
        {
            get { return _excelSheets; }
            set
            {
                _excelSheets = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ExcelSheets"));
                }
            }
        }

        private string _log;
        public string Log
        {
            get { return _log; }
            set
            {
                _log = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Log"));
                }
            }
        }

        private DataTable _dtExcel;
        public DataTable DtExcel
        {
            get { return _dtExcel; }
            set
            {
                _dtExcel = value;
                if (null != PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DtExcel"));
                }
            }
        }

        private void btnOutput_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            var dlg = new SaveFileDialog {FileName = "kimenet", DefaultExt = ".xls", Filter = "Excel file (.xls)|*.xls"};

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                txtOutput.Text = dlg.FileName;
            }
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            var openInputFileDialog = new OpenFileDialog { Filter = "Excel 97 - 2003 fájlok|*.xls|Excel 2007 - 2010 fájlok |*.xlsx" };

            // Show the dialog and get result.
            var result = openInputFileDialog.ShowDialog(); 
            if (result == true) // Test result.
            {
                txtInput.Text = openInputFileDialog.FileName;
            }

            WriteLog("A Betöltendő Excel Fájl útvonala: " + txtInput.Text);


            HSSFWorkbook hssfwb;
            using (var file = new FileStream(txtInput.Text, FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
            }

            var inputSheet = hssfwb.GetSheetAt(1);

            DtExcel = ConvertISheetToDataTable(inputSheet);

            dxgrid.ItemsSource = DtExcel;

            dxgrid.PopulateColumns();

        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //check if
            if (txtInput.Text == "" || txtOutput.Text == null) return;

            var count = 1;

            //try
            {
                var hssfwb = new HSSFWorkbook();

                hssfwb.CreateSheet("Munka1");

                var sheet = hssfwb.GetSheet("Munka1");
                var headerrow = sheet.CreateRow(0);
                headerrow.CreateCell(0).SetCellValue("id");
                headerrow.CreateCell(1).SetCellValue("Név 1");
                headerrow.CreateCell(2).SetCellValue("Név 2");
                headerrow.CreateCell(3).SetCellValue("Érték 1");
                headerrow.CreateCell(4).SetCellValue("Érték 2");
                headerrow.CreateCell(5).SetCellValue("Menny. 1");
                headerrow.CreateCell(6).SetCellValue("Menny. 2");

                //walking throuhg each row
                for (int i = Convert.ToInt16(spnStartingRow.Text); i < DtExcel.Rows.Count; i++)
                {
                    //try
                    {
                        //check if we have data in our row
                        if (DtExcel.Rows[i][7].ToString() != "")
                        {
                            /* 
                                 * B - név 4976 sor
                                 * E - érték 1
                                 * 
                                 * I - név 5362
                                 * G - érték 2
                                 * 
                                 * C - menny. 1
                                 * H - menny. 2
                                 */

                            //find the diffs
                            for (var j = 0; j < DtExcel.Rows.Count; j++)
                            {
                                var sourceRowName1 = DtExcel.Rows[j][1].ToString();
                                var sourceRowName2 = DtExcel.Rows[i][8].ToString();

                                //check the names
                                if (sourceRowName1 != sourceRowName2) continue;

                                //found matching name
                                //now compare values and return them if values are different

                                //get values from source
                                var sourceValue1 = DtExcel.Rows[j][4].ToString();
                                var sourceValue2 = DtExcel.Rows[i][6].ToString();
                                var sourceAmount1 = DtExcel.Rows[j][2].ToString();
                                var sourceAmount2 = DtExcel.Rows[i][7].ToString();

                                //compare values
                                if (sourceValue1 == sourceValue2 && sourceAmount1 == sourceAmount2) continue;
                                //found difference in values write their data out to file

                                var row = sheet.CreateRow(count);

                                //name 1
                                row.CreateCell(1).SetCellValue(sourceRowName1);
                                //name 2
                                row.CreateCell(2).SetCellValue(sourceRowName2);
                                //value 1
                                row.CreateCell(3).SetCellValue(sourceValue1);
                                //value 2
                                row.CreateCell(4).SetCellValue(sourceValue2);
                                //amount 1
                                row.CreateCell(5).SetCellValue(sourceAmount1);
                                //amount 2
                                row.CreateCell(6).SetCellValue(sourceAmount2);

                                count++;
                            }
                        }
                    }
                    /*catch (Exception ex)
                            {
                                throw ex;
                            }*/
                }

                using (var file2 = new FileStream(txtOutput.Text, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    hssfwb.Write(file2);
                }
            }
            /*catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    WriteLog("Sikeresen feldolgoztam az adatokat. Kliensek száma: " + count.ToString() + "db.");
                 * 
                 * 
                }*/
        }

        public void WriteLog(string output)
        {
            Log = "" + DateTime.Now + ": " + output + "\n" + Log;
            //Log += "\n " + DateTime.Now.ToString() + ": " + output;

            
            txtLog.Select(txtLog.Text.Length, 0);
        }

        private void ComboBoxEdit_SelectedIndexChanged_1(object sender, RoutedEventArgs e)
        {
            try
            {
                dxgrid.ItemsSource = DtExcel;
                dxgrid.PopulateColumns();

            }
            
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                
            }

        }

        /// <summary>
        /// Read the contents an NPOI ISheet into a new DataTable object
        /// </summary>
        /// <param name="sheet">ISheet to read</param>
        /// <returns>DataTable populated with the contents of the ISheet</returns>
        private static DataTable ConvertISheetToDataTable(ISheet sheet)
        {
            // Temporarily set the thread culture to avoid conversion issues
            // http://stackoverflow.com/questions/15040567/c-xlsx-date-cell-import-to-datatable-by-npoi-2-0
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                var table = new DataTable();

                var colCount = 0;

                var headerRow = sheet.GetRow(sheet.FirstRowNum);
                foreach (var column in headerRow.Cells.Select(cell => cell.ToString()).Select(columnName => new DataColumn(columnName)))
                {
                    table.Columns.Add(column);
                    colCount++;
                }

                for (var rowNum = (sheet.FirstRowNum) + 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    var row = sheet.GetRow(rowNum);
                    var dataRow = table.NewRow();
                    var cellNumber = 0;
                    for (var colNum = 0; colNum < colCount; colNum++)
                    {
                        var cell = row.GetCell(colNum);
                        if (cell != null)
                        {
                            switch (cell.CellType)
                            {
                                case CellType.BOOLEAN:
                                    dataRow[cellNumber] = cell.BooleanCellValue;
                                    break;
                                case CellType.NUMERIC:
                                case CellType.FORMULA:
                                    dataRow[cellNumber] = cell.NumericCellValue;
                                    break;
                                default:
                                    dataRow[cellNumber] = cell.StringCellValue;
                                    break;
                            }
                        }
                        cellNumber++;
                    }
                    table.Rows.Add(dataRow);
                }
                return table;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = prevCulture;
            }

        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
