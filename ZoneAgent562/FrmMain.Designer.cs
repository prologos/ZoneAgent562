namespace ZoneAgent562
{
    partial class FrmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.lbCurrentCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMaxCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbServerID = new System.Windows.Forms.Label();
            this.lbAgentID = new System.Windows.Forms.Label();
            this.lbListenPort = new System.Windows.Forms.Label();
            this.lbConnectedZS = new System.Windows.Forms.Label();
            this.logZSlist = new System.Windows.Forms.RichTextBox();
            this.logZAmsg = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ckbNoLog = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbLsStatus = new System.Windows.Forms.Label();
            this.btnLoadLmap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection Count     :";
            // 
            // lbCurrentCount
            // 
            this.lbCurrentCount.AutoSize = true;
            this.lbCurrentCount.Location = new System.Drawing.Point(165, 5);
            this.lbCurrentCount.Name = "lbCurrentCount";
            this.lbCurrentCount.Size = new System.Drawing.Size(14, 14);
            this.lbCurrentCount.TabIndex = 1;
            this.lbCurrentCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Max Connection Count :";
            // 
            // lbMaxCount
            // 
            this.lbMaxCount.AutoSize = true;
            this.lbMaxCount.Location = new System.Drawing.Point(165, 20);
            this.lbMaxCount.Name = "lbMaxCount";
            this.lbMaxCount.Size = new System.Drawing.Size(14, 14);
            this.lbMaxCount.TabIndex = 3;
            this.lbMaxCount.Text = "0";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(2, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(367, 2);
            this.label3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "Server ID  :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 14);
            this.label5.TabIndex = 5;
            this.label5.Text = "Agent ID:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "Listen Port:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(168, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "Connected Zone Servers:";
            // 
            // lbServerID
            // 
            this.lbServerID.AutoSize = true;
            this.lbServerID.Location = new System.Drawing.Point(92, 40);
            this.lbServerID.Name = "lbServerID";
            this.lbServerID.Size = new System.Drawing.Size(14, 14);
            this.lbServerID.TabIndex = 6;
            this.lbServerID.Text = "0";
            // 
            // lbAgentID
            // 
            this.lbAgentID.AutoSize = true;
            this.lbAgentID.Location = new System.Drawing.Point(276, 40);
            this.lbAgentID.Name = "lbAgentID";
            this.lbAgentID.Size = new System.Drawing.Size(14, 14);
            this.lbAgentID.TabIndex = 6;
            this.lbAgentID.Text = "0";
            // 
            // lbListenPort
            // 
            this.lbListenPort.AutoSize = true;
            this.lbListenPort.Location = new System.Drawing.Point(92, 55);
            this.lbListenPort.Name = "lbListenPort";
            this.lbListenPort.Size = new System.Drawing.Size(14, 14);
            this.lbListenPort.TabIndex = 6;
            this.lbListenPort.Text = "0";
            // 
            // lbConnectedZS
            // 
            this.lbConnectedZS.AutoSize = true;
            this.lbConnectedZS.Location = new System.Drawing.Point(170, 70);
            this.lbConnectedZS.Name = "lbConnectedZS";
            this.lbConnectedZS.Size = new System.Drawing.Size(14, 14);
            this.lbConnectedZS.TabIndex = 6;
            this.lbConnectedZS.Text = "0";
            // 
            // logZSlist
            // 
            this.logZSlist.BackColor = System.Drawing.Color.Black;
            this.logZSlist.ForeColor = System.Drawing.Color.DarkGray;
            this.logZSlist.Location = new System.Drawing.Point(5, 85);
            this.logZSlist.Name = "logZSlist";
            this.logZSlist.ReadOnly = true;
            this.logZSlist.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.logZSlist.Size = new System.Drawing.Size(354, 50);
            this.logZSlist.TabIndex = 7;
            this.logZSlist.Text = "logZSlist";
            // 
            // logZAmsg
            // 
            this.logZAmsg.BackColor = System.Drawing.Color.Black;
            this.logZAmsg.ForeColor = System.Drawing.Color.DarkGray;
            this.logZAmsg.Location = new System.Drawing.Point(5, 177);
            this.logZAmsg.Name = "logZAmsg";
            this.logZAmsg.ReadOnly = true;
            this.logZAmsg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.logZAmsg.Size = new System.Drawing.Size(354, 120);
            this.logZAmsg.TabIndex = 7;
            this.logZAmsg.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 5;
            this.label8.Text = "Log Message:";
            // 
            // ckbNoLog
            // 
            this.ckbNoLog.AutoSize = true;
            this.ckbNoLog.Location = new System.Drawing.Point(262, 159);
            this.ckbNoLog.Name = "ckbNoLog";
            this.ckbNoLog.Size = new System.Drawing.Size(96, 18);
            this.ckbNoLog.TabIndex = 8;
            this.ckbNoLog.Text = "No Refresh";
            this.ckbNoLog.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(175, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 14);
            this.label9.TabIndex = 9;
            this.label9.Text = "Login Server:";
            // 
            // lbLsStatus
            // 
            this.lbLsStatus.AutoSize = true;
            this.lbLsStatus.Location = new System.Drawing.Point(268, 136);
            this.lbLsStatus.Name = "lbLsStatus";
            this.lbLsStatus.Size = new System.Drawing.Size(91, 14);
            this.lbLsStatus.TabIndex = 9;
            this.lbLsStatus.Text = "Disconnected";
            // 
            // btnLoadLmap
            // 
            this.btnLoadLmap.Location = new System.Drawing.Point(5, 303);
            this.btnLoadLmap.Name = "btnLoadLmap";
            this.btnLoadLmap.Size = new System.Drawing.Size(120, 40);
            this.btnLoadLmap.TabIndex = 10;
            this.btnLoadLmap.Text = "Reload\r\nLockedMap.ini";
            this.btnLoadLmap.UseVisualStyleBackColor = true;
            this.btnLoadLmap.Click += new System.EventHandler(this.btnLoadLmap_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 423);
            this.Controls.Add(this.btnLoadLmap);
            this.Controls.Add(this.lbLsStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ckbNoLog);
            this.Controls.Add(this.logZAmsg);
            this.Controls.Add(this.logZSlist);
            this.Controls.Add(this.lbConnectedZS);
            this.Controls.Add(this.lbListenPort);
            this.Controls.Add(this.lbAgentID);
            this.Controls.Add(this.lbServerID);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbMaxCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbCurrentCount);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Text = "Zone Agent v562";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCurrentCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbMaxCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbServerID;
        private System.Windows.Forms.Label lbAgentID;
        private System.Windows.Forms.Label lbListenPort;
        private System.Windows.Forms.Label lbConnectedZS;
        private System.Windows.Forms.RichTextBox logZSlist;
        private System.Windows.Forms.RichTextBox logZAmsg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox ckbNoLog;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbLsStatus;
        private System.Windows.Forms.Button btnLoadLmap;
    }
}

