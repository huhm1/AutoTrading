using System;
using System.Windows.Forms;
using AutoTrading.Lib;
using System.Threading;

//using System.Diagnostics.SymbolStore;


namespace AutoTrading
{
    public partial class AutoTrading : Form 
    {
        public AutoTrading()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

        }

        private void AutoTrading_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.Close();
        }

        private void AutoTrading_Shown(object sender, EventArgs e)
        {
            Client.Form = this;
            Thread newThread = new Thread(Client.Start);
            newThread.Start();
        }
    }

}