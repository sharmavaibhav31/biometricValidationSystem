using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TMF20DemoApp
{
    public partial class TMF20Demo : Form
    {
        TMF20FPLibrary fpLibrary = new TMF20FPLibrary();                    
        CaptureResult captrslt1, captrslt2;
        DeviceInfo deviceInfo;
        byte[] rawBytes = new byte[300 * 400];

        public TMF20Demo()
        {
            InitializeComponent();
        }                      

        private void btn_deviceinfo_Click(object sender, EventArgs e)
        {
            int respCode = -1;
            string respMsg = "";
            deviceInfo = new DeviceInfo();
            respCode = fpLibrary.getDeviceInfo(deviceInfo);
            if (TMF20ErrorCodes.SUCCESS == respCode)
            {               
                respMsg = "Serial No : "+deviceInfo.serialNumber + "\n";
                respMsg += "Model : " + deviceInfo.model + "\n";
                respMsg += "Make : " + deviceInfo.make;
                statusBox.Text = respMsg;
            }
            else
            {
                respMsg = "Error code : " + deviceInfo.errorCode + "\n";
                respMsg += deviceInfo.errorString;
                statusBox.Text = respMsg;
            }
        }

        private void btn_capture_Click(object sender, EventArgs e)
        {
            if (rBtn_fingerprint1.Checked)
            {
                fingerCapture1();
            }
            else if (rBtn_fingerprint2.Checked)
            {
                fingerCapture2();
            }
        }
        void fingerCapture1()
        {
            captrslt1 = new CaptureResult();
            int respCode;
            respCode = capture(captrslt1);
            if (TMF20ErrorCodes.SUCCESS == respCode)
            {
                pictureBox.ImageLocation = @"..\image\right.bmp";                          
                pictureBox1.ImageLocation = @"..\image\right1.bmp";
                string msg = captrslt1.errorString + "\n";
                msg += "NFIQ : " + captrslt1.nfiq + "\n";
                msg += "Minutiae count : " + captrslt1.minutiaeCount;
                statusBox.Text = msg;
            }
            else
            {
                pictureBox.ImageLocation = @"..\image\wrong.bmp";
                pictureBox1.ImageLocation = @"..\image\wrong1.bmp";
                statusBox.Text = captrslt1.errorString;
            }            
        }
        void fingerCapture2()
        {
            captrslt2 = new CaptureResult();
            int respCode;
            respCode = capture(captrslt2);
            if (TMF20ErrorCodes.SUCCESS == respCode)
            {
                pictureBox.ImageLocation = @"..\image\right.bmp";                                             
                pictureBox2.ImageLocation = @"..\image\right1.bmp";
                string msg = captrslt2.errorString + "\n";
                msg += "NFIQ : " + captrslt2.nfiq + "\n";
                msg += "Minutiae count : " + captrslt2.minutiaeCount;
                statusBox.Text = msg;
            }
            else
            {
                pictureBox.ImageLocation = @"..\image\wrong.bmp";
                pictureBox2.ImageLocation = @"..\image\wrong1.bmp";
                statusBox.Text = captrslt2.errorString;
            }            
        }

        int capture(CaptureResult captrslt)
        {
            int response = -1;
            response = fpLibrary.captureFingerprint(captrslt, 10000);
            rawBytes = captrslt.rawImageBytes;
            btn_save.Enabled = true;
            return response;
        }
               
        private void btn_match_Click(object sender, EventArgs e)
        {
            string rslt = "Fingerprint not matched";
            Boolean isMatched = false;
            try
            {
                if (null != captrslt1 && null != captrslt2)
                {
                    if (null != captrslt1.fmrBytes && null != captrslt2.fmrBytes)
                    {
                        isMatched = fpLibrary.matchIsoTemplates(captrslt1.fmrBytes, captrslt2.fmrBytes);
                        if (isMatched)
                        {
                            rslt = "Fingerprint match successful";
                        }                        
                    }
                    else
                    {
                        rslt = "Capture the fingerprint";
                    }
                }
                else
                {
                    rslt = "Capture the fingerprint";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            statusBox.Text = rslt;
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            captrslt1 = null;
            captrslt2 = null;            
            statusBox.Text = "";
            pictureBox.ImageLocation = @"..\image\blank.bmp";
            pictureBox1.ImageLocation = @"..\image\blank.bmp";
            pictureBox2.ImageLocation = @"..\image\blank.bmp";            
        }

        private void btn_isDeviceConnected_Click(object sender, EventArgs e)
        {
            //statusBox.SelectionAlignment = HorizontalAlignment.Center;
            if (fpLibrary.isDeviceConnected())
            {                
                statusBox.Text = "Sucess";
            }
            else
            {
                statusBox.Text = "failed";
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {            
            Bitmap bmp = CreateGreyscaleBitmap(rawBytes,300,400);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Bitmap image |*.bmp";            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                String path =Path.GetFullPath(sfd.FileName);
                bmp.Save(path);            
            }                        
        }
        public static Bitmap CreateGreyscaleBitmap(byte[] buffer, int width, int height)
        {
            try
            {
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);               
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmpData.Scan0, width * height);
                bmp.UnlockBits(bmpData);
                
                ColorPalette pal = bmp.Palette;
                for (int i = 0; i < 256; i++)
                    pal.Entries[i] = Color.FromArgb(i, i, i);
                bmp.Palette = pal;
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ":" + ex.StackTrace, "Tatvik TMF20");
                return null;
            }
        }

    }
}
