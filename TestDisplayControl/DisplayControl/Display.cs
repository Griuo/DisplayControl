using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace DisplayControl
{
    public partial class Display: UserControl
    {
        public delegate void DelegateDrawCircle();
        public static event DelegateDrawCircle EventDrawCircle;
        public delegate void DelegateDrawRect();
        public static event DelegateDrawRect EventDrawRect;
        public delegate void DelegateDrawPolygon();
        public static event DelegateDrawPolygon EventDrawPolygon;

        Point pointStart  = Point.Empty;//鼠标按下坐标
        Point pointEnd = Point.Empty;//鼠标终点坐标
        public Rectangle rect;//绘图矩形
        List<Rectangle> rectLst = new List<Rectangle>();//矩形链表
        public Pen p = new Pen(Color.Black, 2);
        public Brush br = new SolidBrush(Color.Black);
        public Graphics g;//picBox绘图
        public Graphics gs;//image绘图
        public Rectangle idxRect;//选中矩形框索引
        public RectangleF idxSmRect;//选中小矩形索引
        public bool isRect = false;
        public bool isCircle = false;
        public bool drawRect = false;
        public bool drawCircle = false;
        public bool isHand = false;
        public bool drag = false;
        bool isinRect = false;
        bool isMove = false;
        Bitmap bmpInit;//原始图片
        Bitmap bmpBack;
        float step ;//放大倍数
        Point mouseDownPoint;
        public class pic : PictureBox
        {
            public pic()
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
            }
        }

        public Display()
        {
            InitializeComponent();
            this.picBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseWheel);
        }
        public void SetImg(Image bmp)
        {
            bmpBack = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            gs = Graphics.FromImage(bmpBack);
            ////提高绘图质量 抗锯齿等
            gs.SmoothingMode = SmoothingMode.AntiAlias;
            gs.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gs.CompositingQuality = CompositingQuality.HighQuality;
            gs.DrawImage(bmp, 0, 0);
            picBox.Image = bmpBack;
            bmpInit = new Bitmap(bmp);
           
            PropertyInfo pInfo = picBox.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                BindingFlags.NonPublic);
            Rectangle rect = (Rectangle)pInfo.GetValue(picBox, null);
            //第③步
            picBox.Width = rect.Width;
            picBox.Height = rect.Height;
            step = (float)picBox.Image.Width / picBox.Width;
            g = picBox.CreateGraphics();
        }
        public void DrawCircle()
        {
            //this.picBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBox_MouseDown);
            isCircle = true;
            //Add your code
            if (EventDrawCircle != null)
                EventDrawCircle();
        }
        public void DrawRect()
        {
            isRect = true;
            //Add your code
            if (EventDrawRect != null)
                EventDrawRect();
        }
        public void DrawPolygon()
        {

            //MessageBox.Show(picBox.Image.Width.ToString());
            MessageBox.Show(picBox.Width.ToString());
            //Add your code
            if (EventDrawPolygon != null)
                EventDrawPolygon();
        }

        private void fitWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //六个小矩形
        RectangleF[] smRects(RectangleF rect)
        {
            RectangleF[] rects =
            {
                new RectangleF(rect.X - 3, rect.Y - 3, 6, 6),//0左上
                new RectangleF(rect.X - 3, rect.Y - 3 + rect.Height / 2, 6, 6),//1左中
                new RectangleF(rect.X - 3, rect.Y - 3 + rect.Height, 6, 6),//2左下
                new RectangleF(rect.X - 3 + rect.Width / 2, rect.Y - 3, 6, 6),//3中上
                new RectangleF(rect.X - 3 + rect.Width / 2, rect.Y - 3 + rect.Height, 6, 6),//4中下
                new RectangleF(rect.X + rect.Width - 3, rect.Y - 3, 6, 6),//5右上
                new RectangleF(rect.X + rect.Width - 3, rect.Y - 3 + rect.Height / 2, 6, 6),//6右中
                new RectangleF(rect.X + rect.Width - 3, rect.Y - 3 + rect.Height, 6, 6),//7右下
            };
            return rects;
        }
        //矩形转换
        Rectangle changeRect(Rectangle rect)
        {
            rect = new Rectangle(new Point(Convert.ToInt32(rect.X / step), Convert.ToInt32(rect.Y / step)), new Size(Convert.ToInt32(rect.Width / step), Convert.ToInt32(rect.Height / step)));
            return rect;
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            pointStart = e.Location;
            //坐标转换
            pointStart.X = Convert.ToInt32(step * pointStart.X);
            pointStart.Y = Convert.ToInt32(step * pointStart.Y);
            if (isRect)
            {
                drawRect = true;
            }
            else if (isCircle)
            {
                drawCircle = true;
            }
            //拖动
            else
            {
                if (this.Cursor == Cursors.Hand | isinRect)
                {
                    drag = true;
                    rectLst.Remove(idxRect);
                    gs.DrawImage(bmpInit, 0, 0);
                    foreach (Rectangle rect in rectLst)
                    {
                        gs.DrawRectangle(p, rect);
                        gs.FillRectangles(br, smRects(rect));
                    }
                    drawRect = true;
                }
            }
            //按中键移动
            if (e.Button == MouseButtons.Middle)
            {
                mouseDownPoint.X = Cursor.Position.X;   //记录鼠标左键按下时位置
                mouseDownPoint.Y = Cursor.Position.Y;
                isMove = true;
                picBox.Focus();    //鼠标滚轮事件(缩放时)需要picturebox有焦点
            }

        }
        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {

            label1.Text = e.Location.ToString();     
            picBox.Refresh();//刷新显示
            GC.Collect();
            pointEnd = e.Location;
            //坐标转换
            pointEnd.X = Convert.ToInt32(step * pointEnd.X);
            pointEnd.Y = Convert.ToInt32(step * pointEnd.Y);
            rect = new Rectangle(pointStart, new Size(pointEnd.X - pointStart.X, pointEnd.Y - pointStart.Y));
            
            if (drag)
            {
                //8个方向拖动
                switch (Array.IndexOf(smRects(idxRect), idxSmRect))
                {
                    case 0: rect = new Rectangle(pointEnd, new Size(idxRect.Width - pointEnd.X + pointStart.X, idxRect.Height - pointEnd.Y + pointStart.Y)); break;
                    case 1: rect = new Rectangle(new Point(pointEnd.X, idxRect.Y), new Size(idxRect.Width - pointEnd.X + pointStart.X, idxRect.Height)); break;
                    case 2: rect = new Rectangle(new Point(pointEnd.X, idxRect.Y), new Size(idxRect.Width - pointEnd.X + pointStart.X, idxRect.Height + pointEnd.Y - pointStart.Y)); break;
                    case 3: rect = new Rectangle(new Point(idxRect.X, pointEnd.Y), new Size(idxRect.Width, idxRect.Height - pointEnd.Y + pointStart.Y)); break;
                    case 4: rect = new Rectangle(idxRect.Location, new Size(idxRect.Width, idxRect.Height + pointEnd.Y - pointStart.Y)); break;
                    case 5: rect = new Rectangle(new Point(idxRect.X, pointEnd.Y), new Size(idxRect.Width + pointEnd.X - pointStart.X, idxRect.Height - pointEnd.Y + pointStart.Y)); break;
                    case 6: rect = new Rectangle(idxRect.Location, new Size(idxRect.Width + pointEnd.X - pointStart.X, idxRect.Height)); break;
                    case 7: rect = new Rectangle(idxRect.Location, new Size(pointEnd.X - idxRect.X, pointEnd.Y - idxRect.Y)); break;
                    default: rect = new Rectangle(pointStart, new Size(pointEnd.X - pointStart.X, pointEnd.Y - pointStart.Y)); break;
                }
                //平移
                if (isinRect)
                {
                    rect = new Rectangle(new Point(idxRect.X + pointEnd.X - pointStart.X, idxRect.Y + pointEnd.Y - pointStart.Y), idxRect.Size);
                }
            }
            else
            {
                //判断是否在框内
                foreach (Rectangle rec in rectLst)
                {
                    idxRect = rec;
                    if (rec.Contains(pointEnd))
                    {
                        isinRect = true;
                        break;
                    }
                    else
                    {
                        isinRect = false;
                        foreach (RectangleF rects in smRects(rec))
                        {
                            idxSmRect = rects;
                            if (rects.Contains(pointEnd))
                            {
                                this.Cursor = Cursors.Hand;
                                isHand = true;
                                break;
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                                isHand = false;
                            }
                        }
                        if (this.Cursor == Cursors.Hand)
                        {
                            break;
                        }
                    }
                }
            }

            //长宽小于零需重新定义起点
            if (rect.Width < 0)
            {
                rect = new Rectangle(new Point(rect.X + rect.Width, rect.Y), new Size(-rect.Width, rect.Height));
            }
            if (rect.Height < 0)
            {
                rect = new Rectangle(new Point(rect.X , rect.Y+rect.Height), new Size(rect.Width, -rect.Height));
            }
            //画矩形
            if (drawRect)
            {
                g.DrawRectangle(p, changeRect(rect));
                g.FillRectangles(br, smRects(changeRect(rect)));
            }
            else if (drawCircle)
            {
                g.DrawEllipse(p, rect);
                g.FillRectangles(br, smRects(rect));
            }
            //中键拖动图片
            picBox.Focus();    //鼠标在picturebox上时才有焦点，此时可以缩放
            if (isMove)
            {
                int x, y;           //新的picBox.Location(x,y)
                int moveX, moveY;   //X方向，Y方向移动大小。
                moveX = Cursor.Position.X - mouseDownPoint.X;
                moveY = Cursor.Position.Y - mouseDownPoint.Y;
                x = picBox.Location.X + moveX;
                y = picBox.Location.Y + moveY;
                picBox.Location = new Point(x, y);
                mouseDownPoint.X = Cursor.Position.X;
                mouseDownPoint.Y = Cursor.Position.Y;
            }

        }
        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            
            drag = false;
            //rect.Offset(picBox.Location.X,picBox.Location.Y);
            //MessageBox.Show(picBox.Location.ToString());
            if (e.Button == MouseButtons.Middle)
            {
                isMove = false;
            }

            if (drawRect)
            {
                
                this.Cursor = Cursors.Default;
                drawRect = false;
                isRect = false;
                gs.DrawRectangle(p, rect);
                gs.FillRectangles(br, smRects(rect)); 
                rectLst.Add(rect);
            }
            if (drawCircle)
            {
                drawCircle = false;
                isCircle = false;
                gs.DrawEllipse(p, rect);
                gs.FillRectangles(br, smRects(rect));
            }
        }
        private void picBox_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
            int x = e.Location.X;
            int y = e.Location.Y;
            int ow = picBox.Width;
            int oh = picBox.Height;
            int VX, VY;     //因缩放产生的位移矢量
            if (e.Delta > 0)    //放大
            {
                //第①步
                picBox.Width = Convert.ToInt32(picBox.Width * 1.2);
                picBox.Height = Convert.ToInt32(picBox.Height * 1.2);
                //第②步
                PropertyInfo pInfo = picBox.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                    BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(picBox, null);
                //第③步
                picBox.Width = rect.Width;
                picBox.Height = rect.Height;
            }
            if (e.Delta < 0)    //缩小
            {
                //防止一直缩成负值
                if (picBox.Width < picBox.Image.Width / 10)
                    return;

                picBox.Width = Convert.ToInt32(picBox.Width / 1.2);
                picBox.Height = Convert.ToInt32(picBox.Height / 1.2);
                PropertyInfo pInfo = picBox.GetType().GetProperty("ImageRectangle", BindingFlags.Instance |
                    BindingFlags.NonPublic);
                Rectangle rect = (Rectangle)pInfo.GetValue(picBox, null);
                picBox.Width = rect.Width;
                picBox.Height = rect.Height;
            }
            //第④步，求因缩放产生的位移，进行补偿，实现锚点缩放的效果
            VX = (int)((double)x * (ow - picBox.Width) / ow);
            VY = (int)((double)y * (oh - picBox.Height) / oh);
            picBox.Location = new Point(picBox.Location.X + VX, picBox.Location.Y + VY);
            step = (float)picBox.Image.Width / picBox.Width;
            g = picBox.CreateGraphics();
            //MessageBox.Show(step.ToString());
        }

        private void picBox_Click(object sender, EventArgs e)
        {

        }
    }
}
