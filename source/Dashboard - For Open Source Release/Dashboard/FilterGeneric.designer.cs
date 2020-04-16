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
    partial class FilterGeneric
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
            this.listBoxShow = new System.Windows.Forms.ListBox();
            this.listBoxDoNotShow = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.buttonAllLeft = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonAllRight = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelShow = new System.Windows.Forms.Label();
            this.labelDoNotShow = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxShow
            // 
            this.listBoxShow.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxShow.FormattingEnabled = true;
            this.listBoxShow.Location = new System.Drawing.Point(434, 0);
            this.listBoxShow.Name = "listBoxShow";
            this.listBoxShow.Size = new System.Drawing.Size(388, 441);
            this.listBoxShow.TabIndex = 0;
            // 
            // listBoxDoNotShow
            // 
            this.listBoxDoNotShow.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxDoNotShow.FormattingEnabled = true;
            this.listBoxDoNotShow.Location = new System.Drawing.Point(0, 0);
            this.listBoxDoNotShow.Name = "listBoxDoNotShow";
            this.listBoxDoNotShow.Size = new System.Drawing.Size(368, 441);
            this.listBoxDoNotShow.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(822, 541);
            this.panel1.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.listBoxDoNotShow);
            this.panel4.Controls.Add(this.listBoxShow);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 27);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(822, 441);
            this.panel4.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.buttonAllLeft);
            this.panel5.Controls.Add(this.buttonLeft);
            this.panel5.Controls.Add(this.buttonRight);
            this.panel5.Controls.Add(this.buttonAllRight);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(368, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(66, 441);
            this.panel5.TabIndex = 2;
            // 
            // buttonAllLeft
            // 
            this.buttonAllLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAllLeft.Location = new System.Drawing.Point(6, 177);
            this.buttonAllLeft.Name = "buttonAllLeft";
            this.buttonAllLeft.Size = new System.Drawing.Size(54, 32);
            this.buttonAllLeft.TabIndex = 3;
            this.buttonAllLeft.Text = "<<";
            this.buttonAllLeft.UseVisualStyleBackColor = true;
            this.buttonAllLeft.Click += new System.EventHandler(this.buttonAllLeft_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeft.Location = new System.Drawing.Point(6, 141);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(54, 30);
            this.buttonLeft.TabIndex = 2;
            this.buttonLeft.Text = "<";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRight.Location = new System.Drawing.Point(6, 103);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(54, 32);
            this.buttonRight.TabIndex = 1;
            this.buttonRight.Text = ">";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonAllRight
            // 
            this.buttonAllRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAllRight.Location = new System.Drawing.Point(6, 66);
            this.buttonAllRight.Name = "buttonAllRight";
            this.buttonAllRight.Size = new System.Drawing.Size(54, 31);
            this.buttonAllRight.TabIndex = 0;
            this.buttonAllRight.Text = ">>";
            this.buttonAllRight.UseVisualStyleBackColor = true;
            this.buttonAllRight.Click += new System.EventHandler(this.buttonAllRight_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelShow);
            this.panel3.Controls.Add(this.labelDoNotShow);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(822, 27);
            this.panel3.TabIndex = 3;
            // 
            // labelShow
            // 
            this.labelShow.AutoSize = true;
            this.labelShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShow.Location = new System.Drawing.Point(470, 8);
            this.labelShow.Name = "labelShow";
            this.labelShow.Size = new System.Drawing.Size(38, 13);
            this.labelShow.TabIndex = 1;
            this.labelShow.Text = "Show";
            // 
            // labelDoNotShow
            // 
            this.labelDoNotShow.AutoSize = true;
            this.labelDoNotShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDoNotShow.Location = new System.Drawing.Point(39, 8);
            this.labelDoNotShow.Name = "labelDoNotShow";
            this.labelDoNotShow.Size = new System.Drawing.Size(82, 13);
            this.labelDoNotShow.TabIndex = 0;
            this.labelDoNotShow.Text = "Do Not Show";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 468);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(822, 73);
            this.panel2.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(591, 21);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 30);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(689, 21);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FilterGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 568);
            this.Controls.Add(this.panel1);
            this.Name = "FilterGeneric";
            this.Text = "Filter Results";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FilterOnService_FormClosing);
            this.Shown += new System.EventHandler(this.FilterOnService_Shown);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxShow;
        private System.Windows.Forms.ListBox listBoxDoNotShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelShow;
        private System.Windows.Forms.Label labelDoNotShow;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonAllLeft;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.Button buttonAllRight;
        public System.Windows.Forms.Button buttonSave;
        public System.Windows.Forms.Button buttonCancel;
    }
}