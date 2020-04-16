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
    partial class FormRule
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
            this.labelSelectRule = new System.Windows.Forms.Label();
            this.comboBoxRules = new System.Windows.Forms.ComboBox();
            this.textBoxRuleText = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelDays = new System.Windows.Forms.Label();
            this.labelHours = new System.Windows.Forms.Label();
            this.textBoxDays = new System.Windows.Forms.TextBox();
            this.textBoxHours = new System.Windows.Forms.TextBox();
            this.labelRuleText = new System.Windows.Forms.Label();
            this.labelDayofWeek = new System.Windows.Forms.Label();
            this.comboBoxDayofWeek = new System.Windows.Forms.ComboBox();
            this.comboBoxDayofMonth = new System.Windows.Forms.ComboBox();
            this.labelDayofMonth = new System.Windows.Forms.Label();
            this.labelDateofYear = new System.Windows.Forms.Label();
            this.textBoxDateofYear = new System.Windows.Forms.TextBox();
            this.labelCalendarDate = new System.Windows.Forms.Label();
            this.dateTimePickerCalendarDate = new System.Windows.Forms.DateTimePicker();
            this.labelOrderableItem = new System.Windows.Forms.Label();
            this.comboBoxOrderableItem = new System.Windows.Forms.ComboBox();
            this.comboBoxSelectReminder = new System.Windows.Forms.ComboBox();
            this.labelSelectReminder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelSelectRule
            // 
            this.labelSelectRule.AutoSize = true;
            this.labelSelectRule.Location = new System.Drawing.Point(37, 32);
            this.labelSelectRule.Name = "labelSelectRule";
            this.labelSelectRule.Size = new System.Drawing.Size(70, 15);
            this.labelSelectRule.TabIndex = 0;
            this.labelSelectRule.Text = "Select Rule";
            // 
            // comboBoxRules
            // 
            this.comboBoxRules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRules.DropDownWidth = 550;
            this.comboBoxRules.FormattingEnabled = true;
            this.comboBoxRules.Location = new System.Drawing.Point(40, 48);
            this.comboBoxRules.MaxDropDownItems = 20;
            this.comboBoxRules.Name = "comboBoxRules";
            this.comboBoxRules.Size = new System.Drawing.Size(419, 21);
            this.comboBoxRules.TabIndex = 1;
            this.comboBoxRules.SelectedIndexChanged += new System.EventHandler(this.comboBoxRules_SelectedIndexChanged);
            // 
            // textBoxRuleText
            // 
            this.textBoxRuleText.Enabled = false;
            this.textBoxRuleText.Location = new System.Drawing.Point(43, 283);
            this.textBoxRuleText.Name = "textBoxRuleText";
            this.textBoxRuleText.Size = new System.Drawing.Size(416, 20);
            this.textBoxRuleText.TabIndex = 2;
            this.textBoxRuleText.Visible = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(335, 333);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(238, 333);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelDays
            // 
            this.labelDays.AutoSize = true;
            this.labelDays.Location = new System.Drawing.Point(40, 90);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(34, 15);
            this.labelDays.TabIndex = 5;
            this.labelDays.Text = "Days";
            this.labelDays.Visible = false;
            // 
            // labelHours
            // 
            this.labelHours.AutoSize = true;
            this.labelHours.Location = new System.Drawing.Point(235, 90);
            this.labelHours.Name = "labelHours";
            this.labelHours.Size = new System.Drawing.Size(40, 15);
            this.labelHours.TabIndex = 6;
            this.labelHours.Text = "Hours";
            this.labelHours.Visible = false;
            // 
            // textBoxDays
            // 
            this.textBoxDays.Location = new System.Drawing.Point(43, 107);
            this.textBoxDays.Name = "textBoxDays";
            this.textBoxDays.Size = new System.Drawing.Size(59, 20);
            this.textBoxDays.TabIndex = 7;
            this.textBoxDays.Visible = false;
            // 
            // textBoxHours
            // 
            this.textBoxHours.Location = new System.Drawing.Point(238, 106);
            this.textBoxHours.Name = "textBoxHours";
            this.textBoxHours.Size = new System.Drawing.Size(59, 20);
            this.textBoxHours.TabIndex = 8;
            this.textBoxHours.Visible = false;
            // 
            // labelRuleText
            // 
            this.labelRuleText.AutoSize = true;
            this.labelRuleText.Location = new System.Drawing.Point(43, 264);
            this.labelRuleText.Name = "labelRuleText";
            this.labelRuleText.Size = new System.Drawing.Size(59, 15);
            this.labelRuleText.TabIndex = 9;
            this.labelRuleText.Text = "Rule Text";
            this.labelRuleText.Visible = false;
            // 
            // labelDayofWeek
            // 
            this.labelDayofWeek.AutoSize = true;
            this.labelDayofWeek.Location = new System.Drawing.Point(105, 90);
            this.labelDayofWeek.Name = "labelDayofWeek";
            this.labelDayofWeek.Size = new System.Drawing.Size(75, 15);
            this.labelDayofWeek.TabIndex = 10;
            this.labelDayofWeek.Text = "Day of Week";
            this.labelDayofWeek.Visible = false;
            // 
            // comboBoxDayofWeek
            // 
            this.comboBoxDayofWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDayofWeek.FormattingEnabled = true;
            this.comboBoxDayofWeek.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.comboBoxDayofWeek.Location = new System.Drawing.Point(108, 106);
            this.comboBoxDayofWeek.Name = "comboBoxDayofWeek";
            this.comboBoxDayofWeek.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDayofWeek.TabIndex = 11;
            this.comboBoxDayofWeek.Visible = false;
            // 
            // comboBoxDayofMonth
            // 
            this.comboBoxDayofMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDayofMonth.FormattingEnabled = true;
            this.comboBoxDayofMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28"});
            this.comboBoxDayofMonth.Location = new System.Drawing.Point(301, 106);
            this.comboBoxDayofMonth.Name = "comboBoxDayofMonth";
            this.comboBoxDayofMonth.Size = new System.Drawing.Size(54, 21);
            this.comboBoxDayofMonth.TabIndex = 13;
            this.comboBoxDayofMonth.Visible = false;
            // 
            // labelDayofMonth
            // 
            this.labelDayofMonth.AutoSize = true;
            this.labelDayofMonth.Location = new System.Drawing.Point(297, 90);
            this.labelDayofMonth.Name = "labelDayofMonth";
            this.labelDayofMonth.Size = new System.Drawing.Size(79, 15);
            this.labelDayofMonth.TabIndex = 12;
            this.labelDayofMonth.Text = "Day of Month";
            this.labelDayofMonth.Visible = false;
            // 
            // labelDateofYear
            // 
            this.labelDateofYear.AutoSize = true;
            this.labelDateofYear.Location = new System.Drawing.Point(40, 130);
            this.labelDateofYear.Name = "labelDateofYear";
            this.labelDateofYear.Size = new System.Drawing.Size(121, 15);
            this.labelDateofYear.TabIndex = 14;
            this.labelDateofYear.Text = "Date of Year (mmdd)";
            this.labelDateofYear.Visible = false;
            // 
            // textBoxDateofYear
            // 
            this.textBoxDateofYear.Location = new System.Drawing.Point(44, 146);
            this.textBoxDateofYear.Name = "textBoxDateofYear";
            this.textBoxDateofYear.Size = new System.Drawing.Size(106, 20);
            this.textBoxDateofYear.TabIndex = 15;
            this.textBoxDateofYear.Visible = false;
            // 
            // labelCalendarDate
            // 
            this.labelCalendarDate.AutoSize = true;
            this.labelCalendarDate.Location = new System.Drawing.Point(153, 129);
            this.labelCalendarDate.Name = "labelCalendarDate";
            this.labelCalendarDate.Size = new System.Drawing.Size(86, 15);
            this.labelCalendarDate.TabIndex = 16;
            this.labelCalendarDate.Text = "Calendar Date";
            this.labelCalendarDate.Visible = false;
            // 
            // dateTimePickerCalendarDate
            // 
            this.dateTimePickerCalendarDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerCalendarDate.Location = new System.Drawing.Point(156, 146);
            this.dateTimePickerCalendarDate.Name = "dateTimePickerCalendarDate";
            this.dateTimePickerCalendarDate.Size = new System.Drawing.Size(101, 20);
            this.dateTimePickerCalendarDate.TabIndex = 17;
            this.dateTimePickerCalendarDate.Visible = false;
            this.dateTimePickerCalendarDate.ValueChanged += new System.EventHandler(this.dateTimePickerCalendarDate_ValueChanged);
            // 
            // labelOrderableItem
            // 
            this.labelOrderableItem.AutoSize = true;
            this.labelOrderableItem.Location = new System.Drawing.Point(41, 180);
            this.labelOrderableItem.Name = "labelOrderableItem";
            this.labelOrderableItem.Size = new System.Drawing.Size(244, 15);
            this.labelOrderableItem.TabIndex = 18;
            this.labelOrderableItem.Text = "Limit To Patients With Active Orderable Item";
            this.labelOrderableItem.Visible = false;
            // 
            // comboBoxOrderableItem
            // 
            this.comboBoxOrderableItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOrderableItem.FormattingEnabled = true;
            this.comboBoxOrderableItem.Location = new System.Drawing.Point(44, 194);
            this.comboBoxOrderableItem.Name = "comboBoxOrderableItem";
            this.comboBoxOrderableItem.Size = new System.Drawing.Size(212, 21);
            this.comboBoxOrderableItem.TabIndex = 19;
            this.comboBoxOrderableItem.Visible = false;
            // 
            // comboBoxSelectReminder
            // 
            this.comboBoxSelectReminder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectReminder.FormattingEnabled = true;
            this.comboBoxSelectReminder.Location = new System.Drawing.Point(44, 232);
            this.comboBoxSelectReminder.Name = "comboBoxSelectReminder";
            this.comboBoxSelectReminder.Size = new System.Drawing.Size(415, 21);
            this.comboBoxSelectReminder.TabIndex = 21;
            this.comboBoxSelectReminder.Visible = false;
            // 
            // labelSelectReminder
            // 
            this.labelSelectReminder.AutoSize = true;
            this.labelSelectReminder.Location = new System.Drawing.Point(41, 218);
            this.labelSelectReminder.Name = "labelSelectReminder";
            this.labelSelectReminder.Size = new System.Drawing.Size(99, 15);
            this.labelSelectReminder.TabIndex = 20;
            this.labelSelectReminder.Text = "Select Reminder";
            this.labelSelectReminder.Visible = false;
            // 
            // FormRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 386);
            this.Controls.Add(this.comboBoxSelectReminder);
            this.Controls.Add(this.labelSelectReminder);
            this.Controls.Add(this.comboBoxOrderableItem);
            this.Controls.Add(this.labelOrderableItem);
            this.Controls.Add(this.dateTimePickerCalendarDate);
            this.Controls.Add(this.labelCalendarDate);
            this.Controls.Add(this.textBoxDateofYear);
            this.Controls.Add(this.labelDateofYear);
            this.Controls.Add(this.comboBoxDayofMonth);
            this.Controls.Add(this.labelDayofMonth);
            this.Controls.Add(this.comboBoxDayofWeek);
            this.Controls.Add(this.labelDayofWeek);
            this.Controls.Add(this.labelRuleText);
            this.Controls.Add(this.textBoxHours);
            this.Controls.Add(this.textBoxDays);
            this.Controls.Add(this.labelHours);
            this.Controls.Add(this.labelDays);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxRuleText);
            this.Controls.Add(this.comboBoxRules);
            this.Controls.Add(this.labelSelectRule);
            this.Name = "FormRule";
            this.Text = "Add New Ward Rule";
            this.Load += new System.EventHandler(this.FormRule_Load);
            this.Shown += new System.EventHandler(this.FormRule_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSelectRule;
        private System.Windows.Forms.ComboBox comboBoxRules;
        private System.Windows.Forms.TextBox textBoxRuleText;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelDays;
        private System.Windows.Forms.Label labelHours;
        private System.Windows.Forms.TextBox textBoxDays;
        private System.Windows.Forms.TextBox textBoxHours;
        private System.Windows.Forms.Label labelRuleText;
        private System.Windows.Forms.Label labelDayofWeek;
        private System.Windows.Forms.ComboBox comboBoxDayofWeek;
        private System.Windows.Forms.ComboBox comboBoxDayofMonth;
        private System.Windows.Forms.Label labelDayofMonth;
        private System.Windows.Forms.Label labelDateofYear;
        private System.Windows.Forms.TextBox textBoxDateofYear;
        private System.Windows.Forms.Label labelCalendarDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerCalendarDate;
        private System.Windows.Forms.Label labelOrderableItem;
        private System.Windows.Forms.ComboBox comboBoxOrderableItem;
        private System.Windows.Forms.ComboBox comboBoxSelectReminder;
        private System.Windows.Forms.Label labelSelectReminder;
    }
}