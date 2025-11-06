namespace LAB6
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainControl = new System.Windows.Forms.TabControl();
            this.ToGuessGame = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.enterCountTryes = new System.Windows.Forms.TextBox();
            this.submitBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.inputB = new System.Windows.Forms.TextBox();
            this.inputA = new System.Windows.Forms.TextBox();
            this.formulaLabel = new System.Windows.Forms.Label();
            this.ArrayHelper = new System.Windows.Forms.TabPage();
            this.reqLength = new System.Windows.Forms.Label();
            this.inputLength = new System.Windows.Forms.TextBox();
            this.btnRandomArray = new System.Windows.Forms.Button();
            this.btnDefaultLength = new System.Windows.Forms.Button();
            this.btnLengthArray = new System.Windows.Forms.Button();
            this.Game = new System.Windows.Forms.TabPage();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.Author = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainControl.SuspendLayout();
            this.ToGuessGame.SuspendLayout();
            this.ArrayHelper.SuspendLayout();
            this.Game.SuspendLayout();
            this.Author.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainControl
            // 
            this.mainControl.Controls.Add(this.ToGuessGame);
            this.mainControl.Controls.Add(this.ArrayHelper);
            this.mainControl.Controls.Add(this.Game);
            this.mainControl.Controls.Add(this.Author);
            resources.ApplyResources(this.mainControl, "mainControl");
            this.mainControl.Name = "mainControl";
            this.mainControl.SelectedIndex = 0;
            // 
            // ToGuessGame
            // 
            this.ToGuessGame.Controls.Add(this.label2);
            this.ToGuessGame.Controls.Add(this.enterCountTryes);
            this.ToGuessGame.Controls.Add(this.submitBtn);
            this.ToGuessGame.Controls.Add(this.label4);
            this.ToGuessGame.Controls.Add(this.label3);
            this.ToGuessGame.Controls.Add(this.inputB);
            this.ToGuessGame.Controls.Add(this.inputA);
            this.ToGuessGame.Controls.Add(this.formulaLabel);
            resources.ApplyResources(this.ToGuessGame, "ToGuessGame");
            this.ToGuessGame.Name = "ToGuessGame";
            this.ToGuessGame.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // enterCountTryes
            // 
            resources.ApplyResources(this.enterCountTryes, "enterCountTryes");
            this.enterCountTryes.Name = "enterCountTryes";
            this.enterCountTryes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PressOnlyPositiveInteger);
            // 
            // submitBtn
            // 
            resources.ApplyResources(this.submitBtn, "submitBtn");
            this.submitBtn.Name = "submitBtn";
            this.submitBtn.UseVisualStyleBackColor = true;
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // inputB
            // 
            resources.ApplyResources(this.inputB, "inputB");
            this.inputB.Name = "inputB";
            this.inputB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PressOnlyRealNumbers);
            // 
            // inputA
            // 
            resources.ApplyResources(this.inputA, "inputA");
            this.inputA.Name = "inputA";
            this.inputA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PressOnlyRealNumbers);
            // 
            // formulaLabel
            // 
            resources.ApplyResources(this.formulaLabel, "formulaLabel");
            this.formulaLabel.Name = "formulaLabel";
            // 
            // ArrayHelper
            // 
            this.ArrayHelper.Controls.Add(this.reqLength);
            this.ArrayHelper.Controls.Add(this.inputLength);
            this.ArrayHelper.Controls.Add(this.btnRandomArray);
            this.ArrayHelper.Controls.Add(this.btnDefaultLength);
            this.ArrayHelper.Controls.Add(this.btnLengthArray);
            resources.ApplyResources(this.ArrayHelper, "ArrayHelper");
            this.ArrayHelper.Name = "ArrayHelper";
            this.ArrayHelper.UseVisualStyleBackColor = true;
            // 
            // reqLength
            // 
            resources.ApplyResources(this.reqLength, "reqLength");
            this.reqLength.Name = "reqLength";
            // 
            // inputLength
            // 
            resources.ApplyResources(this.inputLength, "inputLength");
            this.inputLength.Name = "inputLength";
            this.inputLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PressOnlyPositiveInteger);
            // 
            // btnRandomArray
            // 
            resources.ApplyResources(this.btnRandomArray, "btnRandomArray");
            this.btnRandomArray.Name = "btnRandomArray";
            this.btnRandomArray.UseVisualStyleBackColor = true;
            this.btnRandomArray.Click += new System.EventHandler(this.btnRandomArray_Click);
            // 
            // btnDefaultLength
            // 
            resources.ApplyResources(this.btnDefaultLength, "btnDefaultLength");
            this.btnDefaultLength.Name = "btnDefaultLength";
            this.btnDefaultLength.UseVisualStyleBackColor = true;
            this.btnDefaultLength.Click += new System.EventHandler(this.btnDefaultLength_Click);
            // 
            // btnLengthArray
            // 
            resources.ApplyResources(this.btnLengthArray, "btnLengthArray");
            this.btnLengthArray.Name = "btnLengthArray";
            this.btnLengthArray.UseVisualStyleBackColor = true;
            this.btnLengthArray.Click += new System.EventHandler(this.btnLengthArray_Click);
            // 
            // Game
            // 
            this.Game.Controls.Add(this.btnStartGame);
            resources.ApplyResources(this.Game, "Game");
            this.Game.Name = "Game";
            this.Game.UseVisualStyleBackColor = true;
            // 
            // btnStartGame
            // 
            resources.ApplyResources(this.btnStartGame, "btnStartGame");
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // Author
            // 
            this.Author.Controls.Add(this.label1);
            resources.ApplyResources(this.Author, "Author");
            this.Author.Name = "Author";
            this.Author.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.mainControl);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainControl.ResumeLayout(false);
            this.ToGuessGame.ResumeLayout(false);
            this.ToGuessGame.PerformLayout();
            this.ArrayHelper.ResumeLayout(false);
            this.ArrayHelper.PerformLayout();
            this.Game.ResumeLayout(false);
            this.Author.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainControl;
        private System.Windows.Forms.TabPage ArrayHelper;
        private System.Windows.Forms.TabPage Game;
        private System.Windows.Forms.TabPage Author;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLengthArray;
        private System.Windows.Forms.Button btnRandomArray;
        private System.Windows.Forms.Button btnDefaultLength;
        private System.Windows.Forms.Label reqLength;
        private System.Windows.Forms.TextBox inputLength;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TabPage ToGuessGame;
        private System.Windows.Forms.Button submitBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputB;
        private System.Windows.Forms.TextBox inputA;
        private System.Windows.Forms.Label formulaLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox enterCountTryes;
    }
}

