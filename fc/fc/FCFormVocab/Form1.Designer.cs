namespace FCFormVocab
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Translate = new System.Windows.Forms.Button();
            this.Mistake = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Good = new System.Windows.Forms.Button();
            this.Back = new System.Windows.Forms.Button();
            this.Forward = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(127, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(45, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(45, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 3;
            // 
            // Translate
            // 
            this.Translate.Location = new System.Drawing.Point(242, 30);
            this.Translate.Name = "Translate";
            this.Translate.Size = new System.Drawing.Size(27, 23);
            this.Translate.TabIndex = 4;
            this.Translate.Text = "T";
            this.Translate.UseVisualStyleBackColor = true;
            this.Translate.Click += new System.EventHandler(this.Translate_Click);
            // 
            // Mistake
            // 
            this.Mistake.Location = new System.Drawing.Point(242, 71);
            this.Mistake.Name = "Mistake";
            this.Mistake.Size = new System.Drawing.Size(27, 23);
            this.Mistake.TabIndex = 5;
            this.Mistake.Text = "M";
            this.Mistake.UseVisualStyleBackColor = true;
            this.Mistake.Click += new System.EventHandler(this.Mistake_Click);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(242, 113);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(27, 23);
            this.Remove.TabIndex = 6;
            this.Remove.Text = "R";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Good
            // 
            this.Good.Location = new System.Drawing.Point(242, 142);
            this.Good.Name = "Good";
            this.Good.Size = new System.Drawing.Size(27, 23);
            this.Good.TabIndex = 7;
            this.Good.Text = "G";
            this.Good.UseVisualStyleBackColor = true;
            this.Good.Click += new System.EventHandler(this.Good_Click);
            // 
            // Back
            // 
            this.Back.Location = new System.Drawing.Point(18, 213);
            this.Back.Name = "Back";
            this.Back.Size = new System.Drawing.Size(38, 23);
            this.Back.TabIndex = 8;
            this.Back.Text = "<--";
            this.Back.UseVisualStyleBackColor = true;
            this.Back.Click += new System.EventHandler(this.Back_Click);
            // 
            // Forward
            // 
            this.Forward.Location = new System.Drawing.Point(62, 213);
            this.Forward.Name = "Forward";
            this.Forward.Size = new System.Drawing.Size(38, 23);
            this.Forward.TabIndex = 9;
            this.Forward.Text = "-->";
            this.Forward.UseVisualStyleBackColor = true;
            this.Forward.Click += new System.EventHandler(this.Forward_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 260);
            this.Controls.Add(this.Forward);
            this.Controls.Add(this.Back);
            this.Controls.Add(this.Good);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Mistake);
            this.Controls.Add(this.Translate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Vocabulary";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Translate;
        private System.Windows.Forms.Button Mistake;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Good;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Button Forward;
    }
}

