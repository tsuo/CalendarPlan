using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestControls
{
    public partial class Form1 : Form
    {
        bool keyreleased;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.keyreleased = true;
            //this.Focus();
            //MessageBox.Show("t");
        }

        protected override void OnResize(EventArgs e)
        {
            
            base.OnResize(e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.keyreleased)
            {
                this.keyreleased = false;

                DateTime cur = testCalendar1.GetCurrentMonth();
                char key = e.KeyChar;

                if (key == 'a')
                {
                    if (cur.Month == 1)
                        testCalendar1.ChangeMonth(cur.Year - 1, 12);
                    else
                        testCalendar1.ChangeMonth(cur.Year, (cur.Month - 1));
                }
                else if (key == 'd')
                {
                    if (cur.Month == 12)
                        testCalendar1.ChangeMonth(cur.Year + 1, 1);
                    else
                        testCalendar1.ChangeMonth(cur.Year, (cur.Month + 1));
                }
                //testCalendar1.ChangeMonth(cur.Year, cur.Month + 1);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
                this.keyreleased = true;
        }
    }
}
