﻿using System;
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
    /// 
    /// [PROGRESS REPORT]
    /// Calendar project
    /// 
    /// [CURRENT TIER]: 1
    /// Will contain a grid 7x5 representing all the possible days in a month
    /// Each day block wil have a number associated with them (1, 2, ..., 31)
    /// Pressing 'a' will decrease month and 'd' will increase month
    /// Overflowing month error
    /// A top banner will represent the month/year currently selected
    /// The calendar itself resides an in a Table Layout Panel
    /// Both the top and calendar banners are added to the full banner
    /// Optimized day boxes so that date boxes are originally defaulted to ""
    /// Call ChangeText whenever needed
    /// 
    /// [TIER 1]
    /// Ways to switch months (only test version works)
    /// - [done (through parent form implementation)] a and d buttons change months
    /// - need to implement buttons to change month
    /// - Implement drop down box of dates to change month
    /// 
    /// [TIER 2]
    /// Clicking day boxes interacts with them
    /// - Add text to day box
    /// - Adding events on any day (basically specialized text objects)
    /// - Changing day block colors
    /// - Linking canvas lms feature
    /// 
    /// [TIER 3]
    /// Allow custom fonts
    /// Redesign all graphics
    /// Fancy animations
    /// Reminder feater
    /// 
    /// [TIER 4]
    /// Adding plugin supports
    /// User-level theme changing
    /// - ex: Dark theme/ light theme
    /// - ex: Uploading image and use it as bg for desired controls
    /// 
    /// [TIER 5]
    /// NETWORKED
    /// MAYBEM MULTITHREAD
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
                SuspendDrawing();

                numRow = 5;
                numCol = 7;
                pixWid = 0;
                pixHei = 0;

                //// This banner panel will contain every controls the application
                banfull = new TableLayoutPanel
                {
                    ColumnCount = 1,
                    RowCount = 2,
                    Dock = DockStyle.Fill,
                    BackColor = Color.White
                };

                // set up the main/full control's table columns and rows styles
                banfull.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
                banfull.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
                banfull.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                //////


                ///// Setup top banner
                bantop = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(this.Width, this.Height / 10),
                    BackColor = Color.FromArgb(0, 0, 0),
                    Dock = DockStyle.Fill,
                    Margin = Padding.Empty,
                };
                bantop.Controls.Add(new DateBlock($"{curMonth.Month}/{curMonth.Year}", Color.White, true));


                //// set up the panel that contains the calendar
                bancal = new TableLayoutPanel
                {
                    ColumnCount = numCol,
                    RowCount = numRow,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Beige,
                    Margin = Padding.Empty,
                };

                //// making calendar banner's row and columns of tablelayout styles to be percentages
                for (int r = 0; r < numRow; r++)
                    bancal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / numRow));
                for (int c = 0; c < numCol; c++)
                    bancal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / numCol));

                //// get the started weekday of the 1st of the month and max # of days in the month
                int first = (int)cal.GetDayOfWeek(curMonth);
                int maxday = cal.GetDaysInMonth(curMonth.Year, curMonth.Month);

                dayBoxes = new Panel[numRow * numCol];

                //// loop to add day boxes to each cell in table layout calendar banner
                int l = 0; //the current day counter (0-30)
                for (int i = 0; i < numRow * numCol; i++)
                {
                    //default basic style of each date block
                    dayBoxes[i] = new Panel
                    {
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = Padding.Empty,
                        Dock = DockStyle.Fill
                    };
                    //////


                    // if within range of the valid month days, then show the day number in day boxes
                    if (i >= first && l < maxday)
                    {
                        dayBoxes[i].BackColor = (i % 2 == 0) ? Color.FromArgb(215, 215, 215) : Color.FromArgb(235, 235, 235);
                        dayBoxes[i].Controls.Add(new DateBlock($"{l + 1}", Color.Black));
                        l++;
                    }
                    else
                    {
                        dayBoxes[i].BackColor = Color.White;
                        dayBoxes[i].Controls.Add(new DateBlock("", Color.Black));
                    }

                    // add the box to table cell
                    bancal.Controls.Add(dayBoxes[i], i % numCol, i / numCol);
                }


                banfull.Controls.Add(bantop, 0, 0); // add top banner to full banner
                banfull.Controls.Add(bancal, 0, 1); // add calendar banner to full banner
                this.Controls.Add(banfull);         // add the full banner to the main control

                ResumeDrawing();
                setup = true;
            }
        }

        public DateTime GetCurrentMonth()
        {
            return curMonth;
        }

        public void ChangeMonth(int year, int month)
        {
            SuspendDrawing();

            //// only change if the month and year isnt the same
            if (year != curMonth.Year || month != curMonth.Month)
            {
                // set the new cur month
                curMonth = new DateTime(year, month, 1);
                int first = (int)cal.GetDayOfWeek(curMonth);         // update first week day of month
                int maxday = cal.GetDaysInMonth(curMonth.Year, curMonth.Month);

                // update top banner text
                (bantop.Controls[0] as DateBlock).ChangeText($"{curMonth.Month}/{curMonth.Year}");

                //// redrawing dates on day boxes
                int l = 0;
                for (int i = 0; i < numRow * numCol; i++)
                {
                    if (i >= first && l < maxday)
                    {
                        dayBoxes[i].BackColor = (i % 2 == 0) ? Color.FromArgb(215, 215, 215) : Color.FromArgb(235, 235, 235);
                        (dayBoxes[i].Controls[0] as DateBlock).ChangeText($"{l + 1}");
                        l++;
                    }
                    else
                    {
                        dayBoxes[i].BackColor = Color.White;
                        (dayBoxes[i].Controls[0] as DateBlock).ChangeText("");
                    }
                }
            }

            ResumeDrawing();
        }


        private void TestCalendar_Resize(object sender, EventArgs e)
        {
        }


        #region Utility Functions
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        #endregion Utility Functions
    }
}
