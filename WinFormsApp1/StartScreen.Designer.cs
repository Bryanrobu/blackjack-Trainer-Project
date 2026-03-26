using System;
using System.Drawing;
using System.Windows.Forms;
namespace BlackjackOOP
{
    partial class StartScreen
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
            button1 = new Button();
            numericUpDownPlayers = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPlayers).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(327, 191);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += buttonStart_Click;
            // 
            // numericUpDownPlayers
            // 
            numericUpDownPlayers.Location = new Point(408, 191);
            numericUpDownPlayers.Name = "numericUpDownPlayers";
            numericUpDownPlayers.Size = new Size(120, 23);
            numericUpDownPlayers.TabIndex = 1;
            // 
            // StartScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(numericUpDownPlayers);
            Controls.Add(button1);
            Name = "StartScreen";
            Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)numericUpDownPlayers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private NumericUpDown numericUpDownPlayers;
    }
}