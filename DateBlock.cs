using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestControls
{
    public partial class DateBlock : UserControl
    {
        private bool setup;
        private Label text;

        public DateBlock()
        {
            InitializeComponent();
            setup = false;
            this.text = new Label { Text = "" };
            Setup();
            
        }

        public DateBlock(String d)
        {
            InitializeComponent();
            setup = false;
            this.text = new Label { Text = d };
            Setup();
        }

        public DateBlock(int d)
        {
            InitializeComponent();
            setup = false;
            this.text = new Label { Text = d.ToString() };
            Setup();
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }

        public void Setup()
        {
            if(!setup)
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                //SetStyle(ControlStyles.Opaque, true);
                this.BackColor = Color.Transparent;
                this.text.Size = this.Size;
                //this.text.Size = new Size(200,28);
                this.text.Font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);
                this.Controls.Add(this.text);
            }
        }
        
    }
}
