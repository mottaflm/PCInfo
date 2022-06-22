namespace PCInfo.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.inputBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.pcInfoLabel = new System.Windows.Forms.Label();
            this.newTicketButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.msraButton = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeValue)).BeginInit();
            this.SuspendLayout();
            // 
            // inputBox
            // 
            this.inputBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputBox.ForeColor = System.Drawing.Color.Black;
            this.inputBox.Location = new System.Drawing.Point(12, 12);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(569, 26);
            this.inputBox.TabIndex = 1;
            this.inputBox.TabStop = false;
            this.inputBox.Text = "Informe o nome ou o IP do Computador";
            this.inputBox.Enter += new System.EventHandler(this.ClearBox);
            this.inputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBoxKeyDown);
            this.inputBox.Leave += new System.EventHandler(this.ResetInputBox);
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.searchButton.Location = new System.Drawing.Point(587, 12);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(185, 26);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "Consultar";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // pcInfoLabel
            // 
            this.pcInfoLabel.AutoSize = true;
            this.pcInfoLabel.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pcInfoLabel.Location = new System.Drawing.Point(7, 85);
            this.pcInfoLabel.Name = "pcInfoLabel";
            this.pcInfoLabel.Size = new System.Drawing.Size(196, 22);
            this.pcInfoLabel.TabIndex = 0;
            this.pcInfoLabel.Text = "Informações da Consulta";
            // 
            // newTicketButton
            // 
            this.newTicketButton.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newTicketButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.newTicketButton.Location = new System.Drawing.Point(586, 44);
            this.newTicketButton.Name = "newTicketButton";
            this.newTicketButton.Size = new System.Drawing.Size(185, 26);
            this.newTicketButton.TabIndex = 3;
            this.newTicketButton.Text = "Abrir Chamado";
            this.newTicketButton.UseVisualStyleBackColor = true;
            this.newTicketButton.Click += new System.EventHandler(this.NewTicketButtonClick);
            // 
            // restartButton
            // 
            this.restartButton.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restartButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.restartButton.Location = new System.Drawing.Point(205, 44);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(185, 26);
            this.restartButton.TabIndex = 5;
            this.restartButton.Text = "Reinicialização Remota";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.RemoteRestartClick);
            // 
            // msraButton
            // 
            this.msraButton.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msraButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.msraButton.Location = new System.Drawing.Point(396, 44);
            this.msraButton.Name = "msraButton";
            this.msraButton.Size = new System.Drawing.Size(185, 26);
            this.msraButton.TabIndex = 4;
            this.msraButton.Text = "Assistência Remota";
            this.msraButton.UseVisualStyleBackColor = true;
            this.msraButton.Click += new System.EventHandler(this.RemoteConnectionClick);
            // 
            // outputBox
            // 
            this.outputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputBox.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.outputBox.Location = new System.Drawing.Point(12, 110);
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(761, 430);
            this.outputBox.TabIndex = 7;
            this.outputBox.Text = "";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLabel.Location = new System.Drawing.Point(7, 45);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(66, 22);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "Tempo:";
            // 
            // timeValue
            // 
            this.timeValue.Font = new System.Drawing.Font("Trebuchet MS", 11F);
            this.timeValue.Location = new System.Drawing.Point(80, 45);
            this.timeValue.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.timeValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeValue.Name = "timeValue";
            this.timeValue.Size = new System.Drawing.Size(118, 25);
            this.timeValue.TabIndex = 6;
            this.timeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(483, 543);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Software Desenvolvido por: Fernando L. M. Motta";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.timeValue);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pcInfoLabel);
            this.Controls.Add(this.msraButton);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.newTicketButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.inputBox);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PCInfo";
            ((System.ComponentModel.ISupportInitialize)(this.timeValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label pcInfoLabel;
        private System.Windows.Forms.Button newTicketButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button msraButton;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.NumericUpDown timeValue;
        private System.Windows.Forms.Label label1;
    }
}

