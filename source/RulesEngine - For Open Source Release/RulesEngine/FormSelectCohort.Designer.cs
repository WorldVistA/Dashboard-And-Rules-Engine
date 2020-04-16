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
    partial class FormSelectCohort
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
            this.labelReminderCohort = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxSelectCohort = new System.Windows.Forms.ComboBox();
            this.checkBoxPTOnly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelReminderCohort
            // 
            this.labelReminderCohort.AutoSize = true;
            this.labelReminderCohort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReminderCohort.Location = new System.Drawing.Point(81, 43);
            this.labelReminderCohort.Name = "labelReminderCohort";
            this.labelReminderCohort.Size = new System.Drawing.Size(188, 20);
            this.labelReminderCohort.TabIndex = 13;
            this.labelReminderCohort.Text = "Select Reminder Cohort";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(182, 402);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(70, 36);
            this.buttonSave.TabIndex = 19;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(279, 402);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 36);
            this.buttonCancel.TabIndex = 20;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBoxSelectCohort
            // 
            this.comboBoxSelectCohort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSelectCohort.FormattingEnabled = true;
            this.comboBoxSelectCohort.Location = new System.Drawing.Point(26, 77);
            this.comboBoxSelectCohort.Name = "comboBoxSelectCohort";
            this.comboBoxSelectCohort.Size = new System.Drawing.Size(323, 26);
            this.comboBoxSelectCohort.TabIndex = 17;
            // 
            // checkBoxPTOnly
            // 
            this.checkBoxPTOnly.AutoSize = true;
            this.checkBoxPTOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPTOnly.Location = new System.Drawing.Point(29, 214);
            this.checkBoxPTOnly.Name = "checkBoxPTOnly";
            this.checkBoxPTOnly.Size = new System.Drawing.Size(314, 22);
            this.checkBoxPTOnly.TabIndex = 18;
            this.checkBoxPTOnly.Text = "Evaluate Pass-Through Reminders Anyway";
            this.checkBoxPTOnly.UseVisualStyleBackColor = true;
            // 
            // FormSelectCohort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 450);
            this.Controls.Add(this.checkBoxPTOnly);
            this.Controls.Add(this.comboBoxSelectCohort);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelReminderCohort);
            this.Name = "FormSelectCohort";
            this.Text = "FormSelectCohort";
            this.Load += new System.EventHandler(this.FormSelectCohort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelReminderCohort;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxSelectCohort;
        private System.Windows.Forms.CheckBox checkBoxPTOnly;
    }
}