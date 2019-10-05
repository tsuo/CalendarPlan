using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;

namespace TestControls
{

    /// <summary>
    /// Calendar project
    /// Will contain a grid 7x5 representing all the possible days in a month
    /// Each day block wil have a number associated with them (1, 2, ..., 31)
    /// Pressing 'a' will decrease month and 'd' will increase month
    /// Overflowing month error
    /// Added two special Panel banners: 
    /// - One top for future menu
    /// - one center that is the viewport of the calendar
    /// 
    /// [TBA 1]
    /// Add a bar representing the current selected year/month
    /// Ways to switch months
    /// Adding events on any day
    /// Changing day block colors
    /// Linking canvas lms feature
    /// 
    /// [TBA 2]
    /// Redesign all graphics
    /// Fancy animations
    /// 
    /// </summary>

    public partial class TestCalendar : UserControl
    {
        private bool setup;
        private Panel[] dayBoxes;
        private int numRow, numCol, pixWid, pixHei;
        private GregorianCalendar cal;
        private DateTime curMonth;
        private Panel bantop;
        private TableLayoutPanel bancal;
        private TableLayoutPanel banfull;




        public TestCalendar()
        {
            InitializeComponent();

            setup = false;
            cal = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            curMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            Setup();
        }


        private void Setup()
        {
            if (!setup)
            {
                numRow = 5;
                numCol = 7;
                pixWid = 0;
                pixHei = 0;

                bantop = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(this.Width, this.Height / 10),
                    BackColor = Color.White
                };


                bancal = new TableLayoutPanel
                {
                    ColumnCount = numCol,
                    RowCount = numRow,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Beige,
                };

                //banfull = new TableLayoutPanel { RowCount = 10, ColumnCount = 1 };


                int first = (int)cal.GetDayOfWeek(curMonth);
                int maxday = cal.GetDaysInMonth(curMonth.Year, curMonth.Month);

                dayBoxes = new Panel[numRow * numCol];

                int l = 0;
                for (int i = 0; i < numRow * numCol; i++)
                {
                    //default basic style of each date block
                    dayBoxes[i] = new Panel
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = (i % 2 == 0) ? Color.LightPink : Color.LightGray,

                    };
                    //////

                    if (i >= first && l < maxday)
                    {
                        dayBoxes[i].Controls.Add(new DateBlock(l + 1));
                        l++;
                    }

                    //MessageBox.Show($"{i%numCol},{i/numCol}");
                    bancal.Controls.Add(dayBoxes[i], i % numCol, i / numCol);


                }

                this.Controls.Add(bancal);
                //drawing calendar


                setup = true;
            }
        }

        public void DrawCalendar()
        {
            SuspendDrawing();



            ResumeDrawing();
        }

        /*
        public void DrawCalendar()
        {
            SuspendDrawing();

            this.Controls.Clear();

            //drawing banners first
            bantop.Location = new Point(0, 0);
            bantop.Size = new Size(this.Width, this.Height/10);
            this.BackColor = Color.White;

            bancal.Location = new Point(0, bantop.Height);
            bancal.Size = new Size(this.Width, this.Height - bantop.Height);
            bancal.BackColor = Color.Beige;

            pixWid = bancal.Width/numCol;
            pixHei = bancal.Height/numRow;

            int first = (int)cal.GetDayOfWeek(curMonth);
            int maxday = cal.GetDaysInMonth(curMonth.Year, curMonth.Month);

            int i = 0;
            int l = 0;
            for(int y = 0; y < numRow; y++)
            {
                for(int x = 0; x < numCol; x++)
                {
                    //dayBoxes[i].Size = new Size(pixWid, pixHei);
                    //dayBoxes[i].Location = new Point(x*pixWid, y*pixHei);

                    if (!setup)
                    {
                        if (i >= first && l < maxday)
                        {
                            dayBoxes[i].Controls.Add(new DateBlock(l + 1));
                            l++;
                        }
                    }

                    bancal.Controls.Add(dayBoxes[i]);

                    i++;
                }
            }

            this.Controls.Add(bantop);
            this.Controls.Add(bancal);

            ResumeDrawing();
        }*/

        public DateTime GetCurrentMonth()
        {
            return curMonth;
        }

        public void ChangeMonth(int year, int month)
        {
            SuspendDrawing();

            if (year != curMonth.Year || month != curMonth.Month)
            {
                curMonth = new DateTime(year, month, 1);
                int first = (int)cal.GetDayOfWeek(curMonth);
                int maxday = cal.GetDaysInMonth(curMonth.Year, curMonth.Month);

                int i = 0;
                int l = 0;
                for (int y = 0; y < numRow; y++)
                {
                    for (int x = 0; x < numCol; x++)
                    {
                        dayBoxes[i].Controls.Clear();

                        if (i >= first && l < maxday)
                        {
                            dayBoxes[i].Controls.Add(new DateBlock(l + 1));
                            l++;
                        }
                        i++;
                    }
                }
            }

            ResumeDrawing();
        }


        private void TestCalendar_Resize(object sender, EventArgs e)
        {
            //SuspendDrawing();
            ///this.Controls.Clear();
            //this.Controls.Add(bancal);

            //ResumeDrawing();
        }



        // useful forms suspend/resume form drawing found on:
        // https://stackoverflow.com/questions/487661/how-do-i-suspend-painting-for-a-control-and-its-children
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private static int suspendCounter = 0;
        private int WM_SETREDRAW = 11;
        private void SuspendDrawing()
        {
            if (suspendCounter == 0)
                SendMessage(this.Handle, WM_SETREDRAW, false, 0);
            suspendCounter++;
        }

        private void ResumeDrawing()
        {
            suspendCounter--;
            if (suspendCounter == 0)
            {
                SendMessage(this.Handle, WM_SETREDRAW, true, 0);
                this.Refresh();
            }
        }
        ////////////////////////////////////////////////////
    }
}
