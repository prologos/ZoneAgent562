using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ZoneAgent562
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close?", "ZoneAgent", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (ZoneAgent.ReqTick_Sender != null)
                    ZoneAgent.ReqTick_Sender.Change(Timeout.Infinite, Timeout.Infinite);
                if (ZoneAgent.dcClient_Checker != null)
                    ZoneAgent.dcClient_Checker.Change(Timeout.Infinite, Timeout.Infinite);
                //종료전 모든 사용를 로그아웃 처리한다.
                if (ZoneAgent._Players != null)
                {
                    foreach (var player in ZoneAgent._Players)
                    {
                        MSG_ZA2ZS_ACC_LOGOUT zLogout = new MSG_ZA2ZS_ACC_LOGOUT();
                        zLogout.MsgHeader.dwPCID = player.Value.Uid;
                        zLogout.byReason = 0x03;
                        ZoneServer.ZS[player.Value.ZoneStatus].Send(zLogout.Serialize());

                        MSG_ZA2LS_ACC_LOGOUT pLogout = new MSG_ZA2LS_ACC_LOGOUT();
                        pLogout.MsgHeader.dwPCID = player.Value.Uid;
                        pLogout.byReason = 0x03;
                        pLogout.szAccount = player.Value.Account;
                        LoginServer.LS.Send(pLogout.Serialize());

                        player.Value.TcpClient.Client.Disconnect(false);
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (Config.Initialization(@".\SvrInfo.ini", this))
            {
                UpdateLogMsg("=== Start Zone Agent ===");
                this.ckbNoLog.Checked = true;
                new ZoneServer(this);
                new ZoneAgent(this);
                new LoginServer(this);
            }
            else
            {
                UpdateLogMsg("=== Initialization Error ===");
            }
        }

        /// <summary>
        /// 로그인 서버 접속 상태 표시
        /// </summary>
        /// <param name="status"></param>
        internal void UpdateLSinfo(string status)
        {
            if (this.lbLsStatus.InvokeRequired)
                this.Invoke(new Action<string>(UpdateLSinfo), status);
            else
                this.lbLsStatus.Text = status;
        }
        /// <summary>
        /// ZA의 ServerID, AgentID, Port등의 정보 표시
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="aid"></param>
        /// <param name="port"></param>
        internal void UpdateZAinfo(int sid, int aid, int port)
        {
            if (this.lbServerID.InvokeRequired || this.lbAgentID.InvokeRequired || this.lbListenPort.InvokeRequired)
            {
                this.Invoke(new Action<int, int, int>(UpdateZAinfo), sid, aid, port);
            }
            else
            {
                this.lbServerID.Text = sid.ToString();
                this.lbAgentID.Text = aid.ToString();
                this.lbListenPort.Text = port.ToString();
            }
        }
        /// <summary>
        /// 접속자 수 표시
        /// </summary>
        /// <param name="cCount"></param>
        /// <param name="mCount"></param>
        internal void UpdateConnectionCount(int cCount, int mCount)
        {
            if (this.lbCurrentCount.InvokeRequired || this.lbMaxCount.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(UpdateConnectionCount), cCount, mCount);
            }
            else
            {
                lbCurrentCount.Text = cCount.ToString();
                lbMaxCount.Text = mCount.ToString();
            }
        }
        /// <summary>
        /// 존서버들의 접속상태 표시
        /// </summary>
        internal void UpdateConnectedZs()
        {
            if (this.lbConnectedZS.InvokeRequired || this.logZSlist.InvokeRequired)
            {
                this.Invoke(new Action(UpdateConnectedZs));
            }
            else
            {
                int cCount = 0;
                StringBuilder sb = new StringBuilder();
                foreach (var zs in Config.ZSList)
                {
                    sb.AppendFormat("{0}:{1}:{2} {3}{4}", zs.Value.IP, zs.Value.Port, zs.Value.aID, zs.Value.Status, Environment.NewLine);
                    if (zs.Value.Status == "Connected")
                        cCount++;
                }
                logZSlist.Text = sb.ToString();
                sb.Clear();
                lbConnectedZS.Text = sb.AppendFormat("{0}/{1}", cCount, Config.ZSList.Count).ToString();
            }
        }
        /// <summary>
        /// 로그 표시
        /// </summary>
        /// <param name="msg"></param>
        internal void UpdateLogMsg(string msg)
        {
            if (this.ckbNoLog.Checked) return;
            if (this.logZAmsg.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateLogMsg), msg);
            }
            else
            {
                if (logZAmsg.Lines.Length >= 100)
                {
                    int start_index = logZAmsg.GetFirstCharIndexFromLine(0);
                    int count = logZAmsg.Lines[0].Length;

                    if (0 < logZAmsg.Lines.Length - 1)
                    {
                        count += logZAmsg.GetFirstCharIndexFromLine(0 + 1) - ((start_index + count - 1) + 1);
                    }
                    logZAmsg.Text = logZAmsg.Text.Remove(start_index, count);
                }
                logZAmsg.AppendText(string.Format("{0}{1}", msg, Environment.NewLine));
                logZAmsg.ScrollToCaret();
            }
        }

        private void btnLoadLmap_Click(object sender, EventArgs e)
        {
            bool status;
            if (status = this.ckbNoLog.Checked)
                this.ckbNoLog.Checked = !this.ckbNoLog.Checked;
            Config.LoadLockedMap(this);
            this.ckbNoLog.Checked = status;
        }
    }
}
