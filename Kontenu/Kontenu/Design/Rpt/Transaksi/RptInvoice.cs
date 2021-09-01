using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Kontenu.Design
{
    public partial class RptInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        public RptInvoice()
        {
            InitializeComponent();
        }

        private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //Osw
            //String text = xrLabel4.Text;
            //text = text.Replace("#tanggalberlaku", this.Parameters["ProyekTanggalBerlaku"].ToString());
            //xrLabel4.Text = "ssss";
        }

    }
}
