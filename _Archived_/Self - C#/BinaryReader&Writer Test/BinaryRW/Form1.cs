using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BinaryRW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FileStream fs = File.Create("test.dat");
            //BinaryWriter bw = new BinaryWriter(fs);
            FileStream fs = File.OpenRead("AttackAnimTypes.dbc");
            BinaryReader br = new BinaryReader(fs);

            //br.ReadBytes(fs.Length);



            //int c = Convert.ToInt32(txtCharNum.Text.ToString());
            //byte[] InputArray = br.ReadString; //ReadFully(fs, 32768);

            string strf = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(32));

            //byte[] s = br.ReadBytes(20);

            // by varying the number of the array we get out the charaters - progress made here
            //for (int i = 0; i == c;)
            
            //txtIn.Text = InputArray[Convert.ToInt32(txtCharNum.Text.ToString())].ToString();
            txtIn.Text = strf;

            br.Close();
            fs.Close();
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                chunk = stream.Read(buffer, read, buffer.Length - read);
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read > buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
    }
}