using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Kontenu.Umum.Laporan {
    public partial class LaporanPrintPreview : DevExpress.XtraEditors.XtraForm {
        public LaporanPrintPreview() {
            InitializeComponent();
        }

        private void LaporanPrintPreview_Load(object sender, EventArgs e) {
            ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
        }
    }
}