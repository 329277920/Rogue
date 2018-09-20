using Rogue.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rogue
{
    public partial class frmMain : Form
    {
        private CustomeWebBrowser _browser;

        public frmMain()
        {
            InitializeComponent();

            InitWebBrowser();

        }

        private void InitWebBrowser()
        {
            this._browser = new CustomeWebBrowser();
            
            this._browser.Dock = DockStyle.Fill;
             
            // this._browser.BackColor = Color.Red;
            // this._browser.
            // this._browser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.panelBrowser.Controls.Add(this._browser);
            this._browser.Load("http://www.qq.com");
        }
    }
}
