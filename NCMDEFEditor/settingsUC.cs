using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCMDEFEditor
{
    public partial class settingsUC : UserControl
    {
        public settingsUC()
        {
            InitializeComponent();

            comboBox1.DataSource = new System.Globalization.CultureInfo[]{
                System.Globalization.CultureInfo.GetCultureInfo("ru-RU"),
                System.Globalization.CultureInfo.GetCultureInfo("en-US")
                };

            comboBox1.DisplayMember = "NativeName";
            comboBox1.ValueMember = "Name";

            if (!String.IsNullOrEmpty(Properties.Settings.Default.Language))            
                comboBox1.SelectedValue = Properties.Settings.Default.Language;            
        }
    }
}
