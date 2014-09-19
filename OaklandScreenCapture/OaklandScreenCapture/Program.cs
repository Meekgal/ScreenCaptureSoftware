using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace OaklandScreenCapture
{
    

    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        

        private static keyHooks Hooks = new keyHooks();
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            using (NotifyIcon icon = new NotifyIcon())
            {



                Hooks.HookedKeys.Add(Keys.PrintScreen);
                
                Hooks.KeyDown += new KeyEventHandler(Hooks_KeyDown);
                //icon.Icon = OaklandScreenCapture.Properties
                //icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon("OU.ico");
                icon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                icon.MouseDoubleClick += new MouseEventHandler(icon_MouseDoubleClick);
                icon.ContextMenu = new ContextMenu(new MenuItem[] {
                //new MenuItem("Show form", (s, e) => {new ScreenCapture().Show();}),
                new MenuItem("Exit", (s, e) => {
                    Hooks.unhook();
                    Application.Exit();}),
            });
                icon.Visible = true;

                Application.Run();
                icon.Visible = false;
            }
        }

        static void icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        static void Hooks_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                Graphics gfxScreenshot;
                Bitmap bmpScreenshot;
                //run screen capture
                bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
               
                //send to printForm

                gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                // Take the screenshot from the upper left corner to the right bottom corner
                gfxScreenshot.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //gfxScreenshot.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfxScreenshot.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfxScreenshot.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                new printForm(bmpScreenshot).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error accoured while taking the screenshot! \nPlease try again or restart the program", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
