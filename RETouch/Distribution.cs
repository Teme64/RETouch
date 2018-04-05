using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RETouch
{
    public partial class frmDistribution : Form
    {
        private int width;
        private int height;
        private Bitmap bm;
        private int minValue;
        private int maxValue;
        //private float scaleValue;

        private float[] _plotBuffer;

        public frmDistribution()
        {
            InitializeComponent();
            // Trap Esc and Enter to close form
            this.KeyPreview = true;
            this.KeyUp += frmDistribution_KeyUp; ;
        }

        private void frmDistribution_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        private void InitChart()
        {
            width = picChart.Width;
            height = picChart.Height;
            bm = new Bitmap(width, height);
        }

        private void InitChartData(int[] plotData)
        {
            minValue = 0;
            maxValue = 0;
            // Search max to scale data
            foreach (int i in plotData)
            {
                if (i > maxValue) maxValue = i;
            }
            _plotBuffer = new float[plotData.Length];
            // Map data 0 - maxValue -> plotdata 0 - 1
            for (int i = 0; i < plotData.Length; i++)
            {
                _plotBuffer[i] = plotData[i] / (float)(maxValue - minValue);
            }
        }

        private void PlotChartData()
        {
            Graphics g;
            float scaleWidth;
            float scaleHeight;
            int paddingLeft = 10;
            int paddingRight = 10;
            int paddingTop = 10;
            int paddingBottom = 5;
            int prevX;
            int prevY;
            int pixelX;
            int pixelY;
            Color bgColor = Color.White;
            Color graphColor = Color.Black;
            Point legendYBottom;
            Point legendYTop; ;
            string legendYTopText = maxValue.ToString();
            string legendYBottomText = minValue.ToString();

            g = Graphics.FromImage(bm);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            g.Flush();

            scaleWidth = (float)(width - paddingLeft - paddingRight) / (float)_plotBuffer.Length;
            scaleHeight = (float)(height - paddingTop - paddingBottom); // / maxValue;

            // Y-axis top- and bottom points
            legendYBottom = new Point(1, (int)scaleHeight); // Top
            legendYTop = new Point(1, (int)scaleHeight - (int)(maxValue * scaleHeight)); // Bottom
            // TODO: Fix
            if (legendYTop.Y < 0) legendYTop.Y = 5;

            prevX = 0;
            prevY = 0;
            for (int i = 0; i < _plotBuffer.Length; i++)
            {
                pixelY = (int)scaleHeight - (int)(_plotBuffer[i] * scaleHeight);
                // FIX 20180201:
                if (pixelY < 0) pixelY = 0;
                pixelX = (int)(i * scaleWidth);
                if (i == 0)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(pixelX, pixelY, 1, 1));
                    prevX = pixelX;
                    prevY = pixelY;
                    // Draw legend
                    g.DrawString(legendYBottomText, new Font("Consolas", 8.25F, FontStyle.Regular), 
                        Brushes.Black, legendYBottom.X, legendYBottom.Y);
                    g.DrawString(legendYTopText, new Font("Consolas", 8.25F, FontStyle.Regular),
                        Brushes.Black, legendYTop.X, legendYTop.Y);
                }
                else
                {
                    g.DrawLine(Pens.Black, new Point(prevX, prevY), new Point(pixelX, pixelY));
                    prevX = pixelX;
                    prevY = pixelY;
                }
            }
            // Finally show
            picChart.Image = bm;
        }

        public void PlotDistribution(int[] distributionData)
        {
            if (distributionData != null) this.DistributionData = distributionData;
            InitChart();
            if (DistributionData != null)
            {
                InitChartData(DistributionData);
                PlotChartData();
            }
        }

        public int[] DistributionData { get; set; }

    } // Class
}
