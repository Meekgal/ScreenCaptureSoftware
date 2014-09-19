using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace OaklandScreenCapture
{
    public partial class printForm : Form
    {
        PrintDocument printDoc = new PrintDocument();
        double maxHeight = 11;
        double maxWidth = 8;
        int format = 0;
        Graphics gfxScreenshot;
        Bitmap bmpScreenshot;
        public printForm(Bitmap bmpScreenshot)
        {
            this.bmpScreenshot = bmpScreenshot;
            InitializeComponent();
        }

        private void printForm_Load(object sender, EventArgs e)
        {
            //
            try
            {

                PrinterSettings settings = new PrinterSettings();
                string strDefaultPrint = settings.PrinterName;
                foreach (String printer in PrinterSettings.InstalledPrinters)
                {
                    printersList.Items.Add(printer.ToString());
                    if (printer.ToString().Equals(strDefaultPrint))
                    {
                        printersList.SelectedIndex = (printersList.Items.Count - 1);
                    }
                }
                cmbStyle.SelectedIndex = 0;
                this.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error accoured while loading the printers! \nPlease try again or restart the program", "Error", MessageBoxButtons.OK);
            }
                
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bmpScreenshot.Dispose();
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.PageUnit = GraphicsUnit.Inch;


                printDoc.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
                //
                if (cmbStyle.SelectedItem.ToString().ToLower() == "landscape")
                {
                    format = 1;
                    printDoc.DefaultPageSettings.Landscape = true;
                }
                else if (cmbStyle.SelectedItem.ToString().ToLower() == "portrait")
                {
                    format = 0;
                    printDoc.DefaultPageSettings.Landscape = false;
                }
                //
                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error accoured while printing! \nPlease try again or restart the program", "Error", MessageBoxButtons.OK);
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(ResizeImage(), 0, 0);
        }


        private Image ResizeImage()
        {
            try
            {
                Graphics graphic = Graphics.FromImage(bmpScreenshot);


                int resizeHeight = bmpScreenshot.Height;
                int resizeWidth = bmpScreenshot.Width;
                if (format == 0)
                {
                    resizeHeight = (int)(resizeHeight * .60);
                    resizeWidth = (int)(resizeWidth * .60);

                }
                else
                {
                    resizeHeight = (int)(resizeHeight * .70);
                    resizeWidth = (int)(resizeWidth * .70);

                }
                
                Bitmap newImage = new Bitmap(resizeWidth, resizeHeight);
                using (Graphics g = Graphics.FromImage((Image)newImage))
                {

                    g.DrawImage(bmpScreenshot, 0, 0, resizeWidth, resizeHeight);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                }
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //g.DrawImage(bmpScreenshot, 0, 0, resizeWidth,resizeHeight);


                //bmpScreenshot = newImage;

                //graphic.DrawImage(newImage, 0, 0, resizeWidth, resizeHeight);

                return newImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error accoured while resizing the screenshot! \nPlease try again or restart the program", "Error", MessageBoxButtons.OK);
                return null;

            }
        }

        private void printersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            printDoc.PrinterSettings.PrinterName = printersList.SelectedItem.ToString();
        }
    }
}
