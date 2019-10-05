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
        private Panel bg;
        private Label text;
        private Color tColor;
        private bool centered;

        public DateBlock(Color textcol, bool center = false)
        {
            InitializeComponent();
            setup = false;
            this.bg = new Panel();
            this.text = new Label { Text = "" };
            this.tColor = textcol;
            this.centered = center;
            Setup();
            
        }

        public DateBlock(String val, Color textcol, bool center = false)
        {
            InitializeComponent();
            setup = false;
            this.bg = new Panel();
            this.text = new Label { Text = val };
            this.tColor = textcol;
            this.centered = center;
            Setup();
        }

        public DateBlock(int val, Color textcol, bool center = false)
        {
            InitializeComponent();
            setup = false;
            this.bg = new Panel();
            this.text = new Label { Text = val.ToString() };
            this.tColor = textcol;
            this.centered = center;
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
                this.text.Font = new Font(FontFamily.GenericSansSerif, 15F);
                this.text.Size = bg.Size;
                this.text.ForeColor = this.tColor;
                if (this.centered)
                {
                    //this.text.AutoSize = false;
                    this.text.TextAlign = ContentAlignment.MiddleCenter;
                    this.text.Dock = DockStyle.Fill;
                }

                this.bg.Controls.Add(this.text);
                this.bg.Dock = DockStyle.Fill;


                this.Dock = DockStyle.Fill;
                this.Controls.Add(this.bg);

                /*
                //SetStyle(ControlStyles.Opaque, true);
                this.BackColor = Color.Transparent;
                this.text.Size = this.Size;
                //this.text.Size = new Size(200,28);
                this.text.Font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);
                this.Controls.Add(this.text);
                */
            }
        }
        
    }
}
