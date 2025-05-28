namespace SLAESolver
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            EnterSize = new Label();
            ChooseMethod = new Label();
            EnterMatrix = new Label();
            EnterB = new Label();
            dimensionInput = new NumericUpDown();
            vectorGrid = new DataGridView();
            MethodSelector = new ComboBox();
            solveButton = new Button();
            saveButton = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            epsilonBox = new TextBox();
            pictureBox1 = new PictureBox();
            matrixGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dimensionInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)vectorGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)matrixGrid).BeginInit();
            SuspendLayout();
            // 
            // EnterSize
            // 
            EnterSize.AutoSize = true;
            EnterSize.Location = new Point(535, 18);
            EnterSize.Name = "EnterSize";
            EnterSize.Size = new Size(191, 20);
            EnterSize.TabIndex = 0;
            EnterSize.Text = "Введіть розмірність СЛАР:";
            // 
            // ChooseMethod
            // 
            ChooseMethod.AutoSize = true;
            ChooseMethod.Location = new Point(535, 116);
            ChooseMethod.Name = "ChooseMethod";
            ChooseMethod.Size = new Size(202, 20);
            ChooseMethod.TabIndex = 1;
            ChooseMethod.Text = "Оберіть метод розв'язання:";
            // 
            // EnterMatrix
            // 
            EnterMatrix.AutoSize = true;
            EnterMatrix.Location = new Point(12, 18);
            EnterMatrix.Name = "EnterMatrix";
            EnterMatrix.Size = new Size(184, 20);
            EnterMatrix.TabIndex = 2;
            EnterMatrix.Text = "Матриця коефіціентів(А):";
            // 
            // EnterB
            // 
            EnterB.AutoSize = true;
            EnterB.Location = new Point(308, 18);
            EnterB.Name = "EnterB";
            EnterB.Size = new Size(185, 20);
            EnterB.TabIndex = 3;
            EnterB.Text = "Вектор вільних членів(b):";
            // 
            // dimensionInput
            // 
            dimensionInput.Location = new Point(535, 64);
            dimensionInput.Name = "dimensionInput";
            dimensionInput.Size = new Size(83, 27);
            dimensionInput.TabIndex = 4;
            // 
            // vectorGrid
            // 
            vectorGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            vectorGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            vectorGrid.ColumnHeadersVisible = false;
            vectorGrid.Location = new Point(308, 46);
            vectorGrid.Name = "vectorGrid";
            vectorGrid.RowHeadersVisible = false;
            vectorGrid.RowHeadersWidth = 51;
            vectorGrid.Size = new Size(110, 238);
            vectorGrid.TabIndex = 5;
            // 
            // MethodSelector
            // 
            MethodSelector.FormattingEnabled = true;
            MethodSelector.Location = new Point(535, 158);
            MethodSelector.Name = "MethodSelector";
            MethodSelector.Size = new Size(262, 28);
            MethodSelector.TabIndex = 7;
            // 
            // solveButton
            // 
            solveButton.Location = new Point(866, 429);
            solveButton.Name = "solveButton";
            solveButton.Size = new Size(94, 29);
            solveButton.TabIndex = 8;
            solveButton.Text = "Розв'язати";
            solveButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(866, 475);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(220, 29);
            saveButton.TabIndex = 9;
            saveButton.Text = "Зберегти розв'язок у файл";
            saveButton.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(18, 383);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(823, 144);
            textBox1.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(535, 209);
            label1.Name = "label1";
            label1.Size = new Size(182, 20);
            label1.TabIndex = 11;
            label1.Text = "Введіть точність(epsilon):";
            // 
            // epsilonBox
            // 
            epsilonBox.Location = new Point(535, 257);
            epsilonBox.Name = "epsilonBox";
            epsilonBox.Size = new Size(125, 27);
            epsilonBox.TabIndex = 12;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(841, 97);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(308, 177);
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // matrixGrid
            // 
            matrixGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            matrixGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            matrixGrid.ColumnHeadersVisible = false;
            matrixGrid.Location = new Point(18, 46);
            matrixGrid.Name = "matrixGrid";
            matrixGrid.RowHeadersVisible = false;
            matrixGrid.RowHeadersWidth = 51;
            matrixGrid.Size = new Size(241, 238);
            matrixGrid.TabIndex = 6;
            // 
            // SLAESolver
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1197, 555);
            Controls.Add(vectorGrid);
            Controls.Add(matrixGrid);
            Controls.Add(pictureBox1);
            Controls.Add(epsilonBox);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(saveButton);
            Controls.Add(solveButton);
            Controls.Add(MethodSelector);
            Controls.Add(dimensionInput);
            Controls.Add(EnterB);
            Controls.Add(EnterMatrix);
            Controls.Add(ChooseMethod);
            Controls.Add(EnterSize);
            Name = "SLAESolver";
            Text = "Form1";
            Load += SLAESolver_Load;
            ((System.ComponentModel.ISupportInitialize)dimensionInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)vectorGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)matrixGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label EnterSize;
        private Label ChooseMethod;
        private Label EnterMatrix;
        private Label EnterB;
        private NumericUpDown dimensionInput;
        private DataGridView vectorGrid;
        private ComboBox MethodSelector;
        private Button solveButton;
        private Button saveButton;
        private TextBox textBox1;
        private Label label1;
        private TextBox epsilonBox;
        private PictureBox pictureBox1;
        private DataGridView matrixGrid;
    }
}
