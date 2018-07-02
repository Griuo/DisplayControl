using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DisplayControl;

namespace TestDisplayControl
{
    public partial class Form1 : Form
    {
        Display display = new Display();
        public Form1()
        {
            InitializeComponent();
            panel.Controls.Add(display);
            Display.EventDrawCircle += EventDrawCircleImp;
            Display.EventDrawRect += EventDrawRectImp;
            Display.EventDrawPolygon += EventDrawPolygonImp;
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            //finalMergeImg_
            //C:\Users\Administrator\Desktop\
            string strRawImg = "C:\\Users\\Administrator\\Desktop\\123.bmp";

            using (Bitmap bmpRaw = new Bitmap(strRawImg))
            {
                // Do something with the Bitmap object
                display.SetImg(bmpRaw);
            }

                
            
        }
        public void EventDrawCircleImp()
        {

        }
        public void EventDrawRectImp()
        {

        }
        public void EventDrawPolygonImp()
        {

        }
        private void butDrawCircle_Click(object sender, EventArgs e)
        {
            display.DrawCircle();
        }

        private void butDrawRect_Click(object sender, EventArgs e)
        {
            display.DrawRect();
        }

        private void butDrawPolygon_Click(object sender, EventArgs e)
        {
            display.DrawPolygon();
        }
    }
}
