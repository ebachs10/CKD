using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKD_No_1337
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

private void button1_Click(object sender, EventArgs e)
{
    using (var client = new TwinCAT.Ads.TcAdsClient())
    {
        client.Connect(851);
        client.WriteSymbol("HmiCom.MyBoolVariable", true, 
            reloadSymbolInfo: true);
    }
}
  }
}
