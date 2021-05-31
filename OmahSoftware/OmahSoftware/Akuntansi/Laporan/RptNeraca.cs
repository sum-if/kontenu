using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace OmahSoftware.Akuntansi.Laporan {
    public partial class RptNeraca : DevExpress.XtraReports.UI.XtraReport {
        public RptNeraca() {
            InitializeComponent();
        }

        private void iniAset(object sender, System.Drawing.Printing.PrintEventArgs e) {
            ((RptNeracaSub)((XRSubreport)sender).ReportSource).Parameters["jenisakun"].Value = "ASET";
            ((RptNeracaSub)((XRSubreport)sender).ReportSource).Parameters["periode"].Value = this.Parameters["periode"].Value;
        }

        private void iniBukanAset(object sender, System.Drawing.Printing.PrintEventArgs e) {
            ((RptNeracaSub)((XRSubreport)sender).ReportSource).Parameters["jenisakun"].Value = "NONASET";
            ((RptNeracaSub)((XRSubreport)sender).ReportSource).Parameters["periode"].Value = this.Parameters["periode"].Value;
        }
    }
}
