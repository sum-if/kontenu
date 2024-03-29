﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Kontenu.Design
{
    public partial class RptFormalLetter : DevExpress.XtraReports.UI.XtraReport
    {
        public RptFormalLetter()
        {
            InitializeComponent();
        }

        private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String text = xrLabel4.Text;
            text = text.Replace("#tanggalberlaku", this.Parameters["ProyekTanggalBerlaku"].Value.ToString());
            xrLabel4.Text = text;
        }

    }
}
