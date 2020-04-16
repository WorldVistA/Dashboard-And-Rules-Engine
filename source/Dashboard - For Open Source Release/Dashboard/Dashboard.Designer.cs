//Dashboard and Rules Engine
//Copyright [2020] [Central Regional Hospital, State of North Carolina]
// 
//Licensed under the Apache License, Version 2.0 (the "License")
//you may not use this file except in ;compliance with the License.
//You may obtain a copy of the License at
// 
//http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is ;distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Dashboard
{
    partial class Dashboard
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
            this.components = new System.ComponentModel.Container();
            this.paneltop = new System.Windows.Forms.Panel();
            this.buttonRefreshAll = new System.Windows.Forms.Button();
            this.panelmiddle = new System.Windows.Forms.Panel();
            this.panelmiddleright = new System.Windows.Forms.Panel();
            this.splitterhorizontal1 = new System.Windows.Forms.Splitter();
            this.splittervertical = new System.Windows.Forms.Splitter();
            this.panelmiddleleft = new System.Windows.Forms.Panel();
            this.splitterhorizontal2 = new System.Windows.Forms.Splitter();
            this.panelbottom = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.timerKeepAlive = new System.Windows.Forms.Timer(this.components);
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.paneltop.SuspendLayout();
            this.panelmiddle.SuspendLayout();
            this.panelmiddleright.SuspendLayout();
            this.panelmiddleleft.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // paneltop
            // 
            this.paneltop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.paneltop.Controls.Add(this.buttonRefreshAll);
            this.paneltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneltop.Location = new System.Drawing.Point(0, 28);
            this.paneltop.Name = "paneltop";
            this.paneltop.Size = new System.Drawing.Size(1153, 40);
            this.paneltop.TabIndex = 0;
            // 
            // buttonRefreshAll
            // 
            this.buttonRefreshAll.Location = new System.Drawing.Point(10, 2);
            this.buttonRefreshAll.Name = "buttonRefreshAll";
            this.buttonRefreshAll.Size = new System.Drawing.Size(90, 31);
            this.buttonRefreshAll.TabIndex = 0;
            this.buttonRefreshAll.Text = "Refresh All";
            this.buttonRefreshAll.UseVisualStyleBackColor = true;
            this.buttonRefreshAll.Click += new System.EventHandler(this.buttonRefreshAll_Click);
            // 
            // panelmiddle
            // 
            this.panelmiddle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelmiddle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelmiddle.Controls.Add(this.panelmiddleright);
            this.panelmiddle.Controls.Add(this.splittervertical);
            this.panelmiddle.Controls.Add(this.panelmiddleleft);
            this.panelmiddle.Location = new System.Drawing.Point(0, 70);
            this.panelmiddle.Name = "panelmiddle";
            this.panelmiddle.Size = new System.Drawing.Size(1153, 573);
            this.panelmiddle.TabIndex = 1;
            // 
            // panelmiddleright
            // 
            this.panelmiddleright.AutoScroll = true;
            this.panelmiddleright.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelmiddleright.Controls.Add(this.splitterhorizontal1);
            this.panelmiddleright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelmiddleright.Location = new System.Drawing.Point(5, 0);
            this.panelmiddleright.Name = "panelmiddleright";
            this.panelmiddleright.Size = new System.Drawing.Size(1144, 569);
            this.panelmiddleright.TabIndex = 0;
            // 
            // splitterhorizontal1
            // 
            this.splitterhorizontal1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitterhorizontal1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterhorizontal1.Location = new System.Drawing.Point(0, 560);
            this.splitterhorizontal1.Name = "splitterhorizontal1";
            this.splitterhorizontal1.Size = new System.Drawing.Size(1140, 5);
            this.splitterhorizontal1.TabIndex = 0;
            this.splitterhorizontal1.TabStop = false;
            this.splitterhorizontal1.Visible = false;
            // 
            // splittervertical
            // 
            this.splittervertical.Location = new System.Drawing.Point(0, 0);
            this.splittervertical.Name = "splittervertical";
            this.splittervertical.Size = new System.Drawing.Size(5, 569);
            this.splittervertical.TabIndex = 2;
            this.splittervertical.TabStop = false;
            // 
            // panelmiddleleft
            // 
            this.panelmiddleleft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelmiddleleft.Controls.Add(this.splitterhorizontal2);
            this.panelmiddleleft.Location = new System.Drawing.Point(0, 0);
            this.panelmiddleleft.Name = "panelmiddleleft";
            this.panelmiddleleft.Size = new System.Drawing.Size(412, 300);
            this.panelmiddleleft.TabIndex = 1;
            // 
            // splitterhorizontal2
            // 
            this.splitterhorizontal2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitterhorizontal2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterhorizontal2.Location = new System.Drawing.Point(0, 291);
            this.splitterhorizontal2.Name = "splitterhorizontal2";
            this.splitterhorizontal2.Size = new System.Drawing.Size(408, 5);
            this.splitterhorizontal2.TabIndex = 0;
            this.splitterhorizontal2.TabStop = false;
            this.splitterhorizontal2.Visible = false;
            // 
            // panelbottom
            // 
            this.panelbottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelbottom.AutoScroll = true;
            this.panelbottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelbottom.Location = new System.Drawing.Point(0, 649);
            this.panelbottom.Name = "panelbottom";
            this.panelbottom.Size = new System.Drawing.Size(1151, 80);
            this.panelbottom.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1153, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontSizeToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(58, 24);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // fontSizeToolStripMenuItem
            // 
            this.fontSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.fontSizeToolStripMenuItem.Name = "fontSizeToolStripMenuItem";
            this.fontSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.fontSizeToolStripMenuItem.Text = "Font Size";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(108, 26);
            this.toolStripMenuItem2.Text = "8";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(108, 26);
            this.toolStripMenuItem3.Text = "9";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(108, 26);
            this.toolStripMenuItem4.Text = "10";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(108, 26);
            this.toolStripMenuItem5.Text = "11";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(108, 26);
            this.toolStripMenuItem6.Text = "12";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // timerKeepAlive
            // 
            this.timerKeepAlive.Enabled = true;
            this.timerKeepAlive.Interval = 180000;
            this.timerKeepAlive.Tick += new System.EventHandler(this.timerKeepAlive_Tick);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 900000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1153, 729);
            this.Controls.Add(this.panelbottom);
            this.Controls.Add(this.panelmiddle);
            this.Controls.Add(this.paneltop);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(121, 100);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.Shown += new System.EventHandler(this.Dashboard_Shown);
            this.paneltop.ResumeLayout(false);
            this.panelmiddle.ResumeLayout(false);
            this.panelmiddleright.ResumeLayout(false);
            this.panelmiddleleft.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel paneltop;
        private System.Windows.Forms.Panel panelmiddle;
        private System.Windows.Forms.Panel panelbottom;
        private System.Windows.Forms.Splitter splittervertical;
        private System.Windows.Forms.Panel panelmiddleleft;
        private System.Windows.Forms.Panel panelmiddleright;
        private System.Windows.Forms.Splitter splitterhorizontal1;
        private System.Windows.Forms.Splitter splitterhorizontal2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.Timer timerKeepAlive;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Button buttonRefreshAll;
    }
}

