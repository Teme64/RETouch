using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace RETouch
{
    public partial class frmVisualizer : Form
    {
        private int width;
        private int height;
        private Bitmap bm;
        private byte[] buffer;
        private float scaleFactor; // Map buffer.Length to width * height

        public frmVisualizer()
        {
            // Sys
            InitializeComponent();
            // Trap Esc and Enter to close form
            this.KeyPreview = true;
            this.KeyUp += frmVisual_KeyUp;
        }

        private void frmVisual_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        private void InitChart()
        {
            width = picBytePlot.Width;
            height = picBytePlot.Height;
            bm = new Bitmap(width, height);
        }

        private void InitChartData(string filename)
        {
            // Read file to plot
            try
            { 
                buffer = File.ReadAllBytes(filename);
                scaleFactor = (float)(width * height) / (float)buffer.Length;
            }
            catch(Exception)
            {
                // Do nothing, just catch
            }
        }

        private void PlotChartData()
        {
            Graphics g;
            int byteIndex;

            if (buffer == null) return;
            g = Graphics.FromImage(bm);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            g.Flush();

            byteIndex = 0;
            for (int pixelY = 0; pixelY < height; pixelY++)
            {
                for(int pixelX = 0; pixelX < width; pixelX++)
                { 
                    byteIndex = (int)((pixelY * width + pixelX) * scaleFactor);
                    if (byteIndex >= buffer.Length) byteIndex = buffer.Length - 1;
                    if (buffer[byteIndex] == 0xFF)
                    {
                        g.FillRectangle(Brushes.White, new Rectangle(pixelX, pixelY, 1, 1));
                    }
                    else if (buffer[byteIndex] == 0x00)
                    {
                        g.FillRectangle(Brushes.Black, new Rectangle(pixelX, pixelY, 1, 1));
                    }
                    else if ((buffer[byteIndex] & 0x80) > 0)
                    {
                        g.FillRectangle(Brushes.Yellow, new Rectangle(pixelX, pixelY, 1, 1));
                    }
                    else if (char.IsWhiteSpace((char)buffer[byteIndex]) || char.IsControl((char)buffer[byteIndex]))
                    {
                        g.FillRectangle(Brushes.LightGreen, new Rectangle(pixelX, pixelY, 1, 1));
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Blue, new Rectangle(pixelX, pixelY, 1, 1));
                    }
                }
            }
            // Finally show bitmap
            picBytePlot.Image = bm;
            // Colors for Byte Plot
            // Color.White = 0xFF;
            // Color.Black = 0x00;
            // Color.Yellow = non-ASCII (>127, 0x80 bit set)
            // Color.LightGreen = Invisible (WhiteSpace) ASCII
            // Color.Blue = Visible ASCII
        }

        public void PlotChartData(string filename)
        {
            InitChart();
            InitChartData(filename);
            PlotChartData();
        }

        // Colors for Entropy Plot
        // Color.DarkTurquoise = Entropy <= 2;
        // Color.MediumTurquese = Entropy 2 < entropy < 6;
        // Color.PaleTurquese = Entropy >= 6;

    } // Class
}
