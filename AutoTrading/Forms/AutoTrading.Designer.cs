namespace AutoTrading
{
    partial class AutoTrading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_tickers = new System.Windows.Forms.TextBox();
            this.m_TWS = new System.Windows.Forms.TextBox();
            this.m_errors = new System.Windows.Forms.TextBox();
            this.inform = new System.Windows.Forms.Label();
            this.lL = new System.Windows.Forms.Label();
            this.lV = new System.Windows.Forms.Label();
            this.lB = new System.Windows.Forms.Label();
            this.lA = new System.Windows.Forms.Label();
            this.lBV = new System.Windows.Forms.Label();
            this.lAV = new System.Windows.Forms.Label();
            this.lT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_tickers
            // 
            this.m_tickers.Location = new System.Drawing.Point(13, 13);
            this.m_tickers.Multiline = true;
            this.m_tickers.Name = "m_tickers";
            this.m_tickers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_tickers.Size = new System.Drawing.Size(500, 120);
            this.m_tickers.TabIndex = 0;
            // 
            // m_TWS
            // 
            this.m_TWS.Location = new System.Drawing.Point(12, 155);
            this.m_TWS.Multiline = true;
            this.m_TWS.Name = "m_TWS";
            this.m_TWS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_TWS.Size = new System.Drawing.Size(500, 120);
            this.m_TWS.TabIndex = 1;
            // 
            // m_errors
            // 
            this.m_errors.Location = new System.Drawing.Point(12, 296);
            this.m_errors.Multiline = true;
            this.m_errors.Name = "m_errors";
            this.m_errors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_errors.Size = new System.Drawing.Size(500, 120);
            this.m_errors.TabIndex = 2;
            // 
            // inform
            // 
            this.inform.AutoSize = true;
            this.inform.Location = new System.Drawing.Point(39, 517);
            this.inform.Name = "inform";
            this.inform.Size = new System.Drawing.Size(0, 13);
            this.inform.TabIndex = 3;
            // 
            // lL
            // 
            this.lL.AutoSize = true;
            this.lL.Location = new System.Drawing.Point(549, 24);
            this.lL.Name = "lL";
            this.lL.Size = new System.Drawing.Size(0, 13);
            this.lL.TabIndex = 4;
            // 
            // lV
            // 
            this.lV.AutoSize = true;
            this.lV.Location = new System.Drawing.Point(603, 24);
            this.lV.Name = "lV";
            this.lV.Size = new System.Drawing.Size(0, 13);
            this.lV.TabIndex = 5;
            // 
            // lB
            // 
            this.lB.AutoSize = true;
            this.lB.Location = new System.Drawing.Point(549, 59);
            this.lB.Name = "lB";
            this.lB.Size = new System.Drawing.Size(0, 13);
            this.lB.TabIndex = 6;
            // 
            // lA
            // 
            this.lA.AutoSize = true;
            this.lA.Location = new System.Drawing.Point(603, 59);
            this.lA.Name = "lA";
            this.lA.Size = new System.Drawing.Size(0, 13);
            this.lA.TabIndex = 7;
            // 
            // lBV
            // 
            this.lBV.AutoSize = true;
            this.lBV.Location = new System.Drawing.Point(549, 92);
            this.lBV.Name = "lBV";
            this.lBV.Size = new System.Drawing.Size(0, 13);
            this.lBV.TabIndex = 8;
            // 
            // lAV
            // 
            this.lAV.AutoSize = true;
            this.lAV.Location = new System.Drawing.Point(603, 92);
            this.lAV.Name = "lAV";
            this.lAV.Size = new System.Drawing.Size(0, 13);
            this.lAV.TabIndex = 9;
            // 
            // lT
            // 
            this.lT.AutoSize = true;
            this.lT.Location = new System.Drawing.Point(586, 131);
            this.lT.Name = "lT";
            this.lT.Size = new System.Drawing.Size(0, 13);
            this.lT.TabIndex = 10;
            // 
            // AutoTranding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.lT);
            this.Controls.Add(this.lAV);
            this.Controls.Add(this.lBV);
            this.Controls.Add(this.lA);
            this.Controls.Add(this.lB);
            this.Controls.Add(this.lV);
            this.Controls.Add(this.lL);
            this.Controls.Add(this.inform);
            this.Controls.Add(this.m_errors);
            this.Controls.Add(this.m_TWS);
            this.Controls.Add(this.m_tickers);
            this.Name = "AutoTranding";
            this.Text = "IB AutoTranding";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoTrading_FormClosing);
            this.Shown += new System.EventHandler(this.AutoTrading_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox m_tickers;
        public System.Windows.Forms.TextBox m_TWS;
        public System.Windows.Forms.TextBox m_errors;
        public System.Windows.Forms.Label inform;
        private System.Windows.Forms.Label lL;
        private System.Windows.Forms.Label lV;
        private System.Windows.Forms.Label lB;
        private System.Windows.Forms.Label lA;
        private System.Windows.Forms.Label lBV;
        private System.Windows.Forms.Label lAV;
        private System.Windows.Forms.Label lT;
    }
}

