using System.Globalization;
using System.Reflection;
using SLAESolver.Solvers;
using SLAESolver.Utilities;

namespace SLAESolver
{
    public partial class MainForm : Form
    {
        private IterationResult lastResult;

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
            this.Text = "SLAESolver";
        }

        private void InitializeUI()
        {
            dimensionInput.Minimum = 2;
            dimensionInput.Maximum = 10;
            dimensionInput.Value = 3;
            dimensionInput.ValueChanged += (s, e) => UpdateMatrixAndVectorGrids();

            MethodSelector.Items.AddRange(new string[] { "Якобі", "Гаус-Зейдель", "Градієнтний спуск" });
            MethodSelector.SelectedIndex = 0;

            epsilonBox.Text = "";

            solveButton.Click += SolveButton_Click;
            saveButton.Click += SaveButton_Click;

            UpdateMatrixAndVectorGrids();
        }

        private void UpdateMatrixAndVectorGrids()
        {
            int n = (int)dimensionInput.Value;
            if (n < 2 || n > 10)
            {
                MessageBox.Show("Розмірність повинна бути від 2 до 10", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowHeight = 21;
            int headerHeight = 28;
            int totalHeight = rowHeight * n + headerHeight + 2;

            Font cellFont = new Font("Segoe UI", 9F);

            // === Матриця A ===
            matrixGrid.Columns.Clear();
            matrixGrid.Rows.Clear();
            matrixGrid.RowHeadersVisible = false;
            matrixGrid.AllowUserToAddRows = false;
            matrixGrid.ScrollBars = ScrollBars.None;
            matrixGrid.BorderStyle = BorderStyle.None;
            matrixGrid.BackgroundColor = SystemColors.Control;
            matrixGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            matrixGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            matrixGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            matrixGrid.DefaultCellStyle.BackColor = Color.White;
            matrixGrid.DefaultCellStyle.Font = cellFont;
            matrixGrid.ColumnHeadersHeight = headerHeight;

            for (int i = 0; i < n; i++)
            {
                matrixGrid.Columns.Add($"colA{i}", $"A{i + 1}");
                matrixGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            matrixGrid.Rows.Add(n);
            foreach (DataGridViewRow row in matrixGrid.Rows)
                row.Height = rowHeight;

            matrixGrid.Height = totalHeight;

            // === Вектор b ===
            vectorGrid.Columns.Clear();
            vectorGrid.Rows.Clear();
            vectorGrid.RowHeadersVisible = false;
            vectorGrid.AllowUserToAddRows = false;
            vectorGrid.ScrollBars = ScrollBars.None;
            vectorGrid.BorderStyle = BorderStyle.None;
            vectorGrid.BackgroundColor = SystemColors.Control;
            vectorGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            vectorGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            vectorGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            vectorGrid.DefaultCellStyle.BackColor = Color.White;
            vectorGrid.DefaultCellStyle.Font = cellFont;
            vectorGrid.ColumnHeadersHeight = headerHeight;

            vectorGrid.Columns.Add("colB", "B");
            vectorGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            vectorGrid.Rows.Add(n);
            foreach (DataGridViewRow row in vectorGrid.Rows)
                row.Height = rowHeight;

            vectorGrid.Height = totalHeight;
        }





        private (double[,], double[]) ReadMatrixAndVector()
        {
            int n = (int)dimensionInput.Value;
            double[,] matrix = new double[n, n];
            double[] vector = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!double.TryParse(matrixGrid.Rows[i].Cells[j].Value?.ToString(), out double value))
                        throw new FormatException($"Невірне значення в матриці [{i + 1},{j + 1}]");

                    if (double.IsNaN(value) || double.IsInfinity(value))
                        throw new FormatException("Некоректне числове значення.");

                    if (Math.Abs(value) > 1e6)
                        throw new FormatException($"Занадто велике число [{i + 1},{j + 1}]");

                    matrix[i, j] = Math.Round(value, 10); // округлення до 10 знаків
                }

                if (!double.TryParse(vectorGrid.Rows[i].Cells[0].Value?.ToString(), out double bVal))
                    throw new FormatException($"Невірне значення у векторі b в рядку {i + 1}");

                if (double.IsNaN(bVal) || double.IsInfinity(bVal))
                    throw new FormatException("Некоректне числове значення у векторі b.");

                if (Math.Abs(bVal) > 1e6)
                    throw new FormatException($"Занадто велике число у векторі b [{i + 1}]");

                vector[i] = Math.Round(bVal, 10); // округлення до 10 знаків
            }

            return (matrix, vector);
        }

        private void SolveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(dimensionInput.Text, out int n) || n < 2 || n > 10)
                {
                    MessageBox.Show("Розмірність матриці має бути цілим числом від 2 до 10.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                string input = epsilonBox.Text.Replace(',', '.');
                if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double epsilon))
                {
                    MessageBox.Show("Невірний формат ?. Введіть десяткове число.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (epsilon < 1e-10 || epsilon > 0.1)
                {
                    MessageBox.Show("Значення ? повинно бути від 1e-10 до 0.1", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                var (matrix, vector) = ReadMatrixAndVector();

                
                string methodName = MethodSelector.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(methodName))
                {
                    MessageBox.Show("Будь ласка, оберіть метод розв’язання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ISolver solver;
                bool requireDiagonalDominance = false;
                bool requireSymmetric = false;
                bool requirePositiveDefinite = false;

                if (methodName.Contains("Якобі"))
                {
                    solver = new JacobiMethod();
                    requireDiagonalDominance = true;
                }
                else if (methodName.Contains("Гаус"))
                {
                    solver = new GaussSeidelMethod();
                    requireDiagonalDominance = true;
                }
                else if (methodName.Contains("градієнт", StringComparison.OrdinalIgnoreCase))
                {
                    solver = new GradientMethod();
                    requireSymmetric = true;
                    requirePositiveDefinite = true;
                }
                else
                {
                    MessageBox.Show("Невідомий метод розв’язання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                MatrixValidator.Validate(matrix, vector,
                                         requireSymmetric,
                                         requirePositiveDefinite);

                
                if (requireDiagonalDominance && !MatrixValidator.IsDiagonallyDominant(matrix))
                {
                    DialogResult dialogResult = MessageBox.Show(
                        "Матриця не є діагонально домінантною.\n" +
                        "Це може призвести до повільної або нестійкої збіжності методу Якобі / Гауса-Зейделя.\n" +
                        "Бажаєте продовжити?",
                        "Попередження",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (dialogResult == DialogResult.No)
                        return;
                }

                
                int maxIter = 10000;
                IterationResult result = solver.Solve(matrix, vector, epsilon, maxIter);
                textBox1.Text = result.ToString();

                
                lastResult = result;

                if (n == 2)                {
                    DrawGraph(matrix, vector, result.Solution);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (lastResult == null)
            {
                MessageBox.Show("Спочатку розв’яжіть систему.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                FileName = "result.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(sfd.FileName, lastResult.ToString());
                MessageBox.Show("Результат збережено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DrawGraph(double[,] matrix, double[] vector, double[] solution = null)
        {
            if (matrix.Length != 2)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
            }

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            Bitmap bmp = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            int centerX = width / 2;
            int centerY = height / 2;
            float scale = 40f; // pixels per unit

            Pen axisPen = new Pen(Color.Black, 1);
            Pen gridPen = new Pen(Color.LightGray, 1);
            Pen line1Pen = new Pen(Color.Red, 2);
            Pen line2Pen = new Pen(Color.Blue, 2);
            Pen solutionPen = new Pen(Color.Green, 3);
            Font font = new Font("Arial", 8);
            Brush textBrush = Brushes.Black;

            // --- Grid ---
            for (int x = centerX; x < width; x += (int)scale)
                g.DrawLine(gridPen, x, 0, x, height);
            for (int x = centerX; x > 0; x -= (int)scale)
                g.DrawLine(gridPen, x, 0, x, height);
            for (int y = centerY; y < height; y += (int)scale)
                g.DrawLine(gridPen, 0, y, width, y);
            for (int y = centerY; y > 0; y -= (int)scale)
                g.DrawLine(gridPen, 0, y, width, y);

            // After drawing grid:
            for (int x = -width / (int)(2 * scale); x <= width / (int)(2 * scale); x++)
            {
                float px = centerX + x * scale;
                g.DrawString(x.ToString(), font, textBrush, px - 10, centerY + 2);
            }
            for (int y = -height / (int)(2 * scale); y <= height / (int)(2 * scale); y++)
            {
                float py = centerY - y * scale;
                if (y != 0) // avoid overlapping "0" label
                    g.DrawString(y.ToString(), font, textBrush, centerX + 4, py - 6);
            }

            // --- Axes ---
            g.DrawLine(axisPen, 0, centerY, width, centerY); // X-axis
            g.DrawLine(axisPen, centerX, 0, centerX, height); // Y-axis
            g.DrawString("X", font, textBrush, width - 15, centerY + 5);
            g.DrawString("Y", font, textBrush, centerX + 5, 5);

            // --- Lines from matrix ---
            double a1 = matrix[0, 0];
            double b1 = matrix[0, 1];
            double c1 = vector[0];

            double a2 = matrix[1, 0];
            double b2 = matrix[1, 1];
            double c2 = vector[1];

            DrawEquationLine(g, a1, b1, c1, line1Pen, centerX, centerY, scale, width, height);
            DrawEquationLine(g, a2, b2, c2, line2Pen, centerX, centerY, scale, width, height);

            // --- Draw solution point ---
            if (solution != null && solution.Length >= 2)
            {
                float px = centerX + (float)(solution[0] * scale);
                float py = centerY - (float)(solution[1] * scale);
                g.FillEllipse(Brushes.Green, px - 4, py - 4, 8, 8);
                g.DrawString($"({solution[0]:0.00}, {solution[1]:0.00})", font, Brushes.Green, px + 5, py - 15);
            }

            pictureBox1.Image = bmp;
        }

         private void DrawEquationLine(Graphics g, double a, double b, double c, Pen pen,
            int centerX, int centerY, float scale, int width, int height)
         {
            if (Math.Abs(a) < 1e-8 && Math.Abs(b) < 1e-8)
                return;

            if (Math.Abs(b) < 1e-8)
            {
                float x = centerX + (float)(c / a * scale);
                g.DrawLine(pen, x, 0, x, height);
            }
            else if (Math.Abs(a) < 1e-8)
            {
                float y = centerY - (float)(c / b * scale);
                g.DrawLine(pen, 0, y, width, y);
            }
            else
            {
                List<PointF> points = new List<PointF>();
                for (float x = -width / (2 * scale); x <= width / (2 * scale); x += 0.1f)
                {
                    float y = (float)((c - a * x) / b);
                    float px = centerX + x * scale;
                    float py = centerY - y * scale;
                    points.Add(new PointF(px, py));
                }
                if (points.Count >= 2)
                    g.DrawLines(pen, points.ToArray());
            }
         }

        private void SLAESolver_Load(object sender, EventArgs e)
        {

        }
    }
}
