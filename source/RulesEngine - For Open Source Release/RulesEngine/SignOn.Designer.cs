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

namespace RulesEngine
{
    partial class SignOn
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.LstDivisions = new System.Windows.Forms.ListBox();
            this.txtuname = new System.Windows.Forms.TextBox();
            this.txtpword = new System.Windows.Forms.TextBox();
            this.labeluname = new System.Windows.Forms.Label();
            this.labelpword = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(100, 126);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // LstDivisions
            // 
            this.LstDivisions.FormattingEnabled = true;
            this.LstDivisions.Location = new System.Drawing.Point(29, 15);
            this.LstDivisions.Name = "LstDivisions";
            this.LstDivisions.Size = new System.Drawing.Size(231, 121);
            this.LstDivisions.TabIndex = 1;
            this.LstDivisions.Visible = false;
            // 
            // txtuname
            // 
            this.txtuname.AcceptsTab = true;
            this.txtuname.Enabled = false;
            this.txtuname.Location = new System.Drawing.Point(29, 29);
            this.txtuname.Name = "txtuname";
            this.txtuname.Size = new System.Drawing.Size(183, 20);
            this.txtuname.TabIndex = 0;
            // 
            // txtpword
            // 
            this.txtpword.Location = new System.Drawing.Point(29, 85);
            this.txtpword.Name = "txtpword";
            this.txtpword.PasswordChar = '*';
            this.txtpword.Size = new System.Drawing.Size(183, 20);
            this.txtpword.TabIndex = 1;
            // 
            // labeluname
            // 
            this.labeluname.AutoSize = true;
            this.labeluname.Location = new System.Drawing.Point(29, 10);
            this.labeluname.Name = "labeluname";
            this.labeluname.Size = new System.Drawing.Size(77, 15);
            this.labeluname.TabIndex = 6;
            this.labeluname.Text = "Access Code";
            // 
            // labelpword
            // 
            this.labelpword.AutoSize = true;
            this.labelpword.Location = new System.Drawing.Point(29, 66);
            this.labelpword.Name = "labelpword";
            this.labelpword.Size = new System.Drawing.Size(68, 15);
            this.labelpword.TabIndex = 7;
            this.labelpword.Text = "Verify Code";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(100, 142);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // SignOn
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 172);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.labelpword);
            this.Controls.Add(this.labeluname);
            this.Controls.Add(this.txtpword);
            this.Controls.Add(this.txtuname);
            this.Controls.Add(this.LstDivisions);
            this.Controls.Add(this.btnConnect);
            this.Name = "SignOn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VistA Rules Engine";
            this.Load += new System.EventHandler(this.SignOn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox LstDivisions;
        private System.Windows.Forms.TextBox txtuname;
        private System.Windows.Forms.TextBox txtpword;
        private System.Windows.Forms.Label labeluname;
        private System.Windows.Forms.Label labelpword;
        private System.Windows.Forms.Button btnSelect;
    }
}

