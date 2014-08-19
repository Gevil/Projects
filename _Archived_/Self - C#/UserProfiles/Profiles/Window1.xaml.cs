using System;
using System.Collections.Generic;
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
using System.IO;
using System.Xml;
using System.Data;

namespace Profiles
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public DataSet dsUserOptions;
        public string UserName = null;


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            UserName = null;
            dsUserOptions = new DataSet("UserOptions");
            if (textBox1.Text != "")
            {
                UserName = textBox1.Text;
                if (UserNameExists(UserName))
                {
                    string filePath = UserName + ".xml";
                    dsUserOptions.ReadXml(filePath);
                    //fill listview
                    listBox1.DataContext = dsUserOptions.Tables["Options"].DefaultView;
                    //textBox1.IsReadOnly = true;
                }
                else
                {
                    //string msg = "A felhasználó nem létezik. Ha új felhasználót akar létrehozni, kattintson az Igen-re.";
                    //string caption = "Nemlétező vagy Hibás felhasználónév!";
                    //MessageBoxButton button = MessageBoxButton.YesNo;
                    //MessageBoxImage icon = MessageBoxImage.Warning;

                    // Display message box
                    MessageBoxResult result = MsgBoxResult("A felhasználó nem létezik. Ha új felhasználót akar létrehozni, kattintson az Igen-re.",
                        "Nemlétező vagy Hibás felhasználónév!",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    // Message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            XmlWrite(UserName);
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
            else
            {
                MessageBoxResult result = MsgBoxResult("Írja be a felhasználónevét!",
                    "Hiba!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (UserName != null)
            {
                MessageBoxResult result = MsgBoxResult("Biztosan Menti az adatokat?",
                    "Megerősítés",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                    );

                // Message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string filePath = UserName + ".xml";
                        dsUserOptions.WriteXml(filePath);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else 
            {
                MsgBoxResult("Írja be a felhasználónevét!",
                    "Hiba!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
            }
        }

        //void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        public MessageBoxResult MsgBoxResult(string msg, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(msg, caption, button, icon);
        }

        public static bool UserNameExists(string uName)
        {
            if (File.Exists(uName + ".xml"))
                return true;
            else 
                return false;
        }

        private void XmlWrite(string uName)
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(uName + ".xml", null);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Options");

            xmlWriter.WriteStartElement("Address", "");
            xmlWriter.WriteString("Default Address String");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Phone", "");
            xmlWriter.WriteString("Default Phone Number String");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Misc", "");
            xmlWriter.WriteString("Bla");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            // close writer
            xmlWriter.Close();
        }

    }
}
