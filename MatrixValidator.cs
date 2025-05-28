using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAESolver.Utilities
{
    internal static class MatrixValidator
    {
        public static void Validate(double[,] matrix, double[] b, bool requireSymmetric = false, bool requirePositiveDefinite = false, bool requireDiagonalDominance = false, double minValue = -1e6, double maxValue = 1e6)
        {
            if (matrix == null || b == null)
                throw new ArgumentNullException("Матриця або вектор вільних членів не ініціалізовані.");

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (rows != cols)
                throw new ArgumentException("Матриця повинна бути квадратною.");

            if (b.Length != rows)
                throw new ArgumentException("Розмір вектора b має відповідати розміру матриці.");

            if (rows < 2 || rows > 10)
                throw new ArgumentException("Розмір матриці повинен бути від 2 до 10.");

            for (int i = 0; i < rows; i++)
            {
                if (matrix[i, i] == 0)
                    throw new InvalidOperationException($"Нульовий елемент на головній діагоналі в рядку {i}.");
            }

            if (requireSymmetric)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = i + 1; j < cols; j++)
                    {
                        if (matrix[i, j] != matrix[j, i])
                            throw new ArgumentException("Матриця повинна бути симетричною.");
                    }
                }
            }

            if (requirePositiveDefinite)
            {
                double[,] L = new double[rows, rows];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        double sum = 0;
                        for (int k = 0; k < j; k++)
                            sum += L[i, k] * L[j, k];

                        if (i == j)
                        {
                            double val = matrix[i, i] - sum;
                            if (val <= 0)
                                throw new ArgumentException("Матриця не є додатньо визначеною.");
                            L[i, j] = Math.Sqrt(val);
                        }
                        else
                        {
                            if (Math.Abs(L[j, j]) < 1e-12)
                                throw new ArgumentException("Ділення на дуже мале число під час перевірки додатньої визначеності.");

                            L[i, j] = (matrix[i, j] - sum) / L[j, j];
                        }
                    }
                }
            }


            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] < minValue || matrix[i, j] > maxValue)
                        throw new ArgumentException($"Значення матриці A[{i + 1},{j + 1}] виходить за межі ±1e6.");

                    if (matrix[i, j] != 0 && Math.Abs(matrix[i, j]) < 1e-15)
                    {
                        throw new FormatException($"Значення в матриці [{i + 1},{j + 1}] занадто мале.");
                    }
                }

                if (b[i] < minValue || b[i] > maxValue)
                    throw new ArgumentException($"Значення вектору b[{i + 1}] виходить за межі ±1e6.");
            }
        }

        public static bool IsDiagonallyDominant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                double diag = Math.Abs(matrix[i, i]);
                double rowSum = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                        rowSum += Math.Abs(matrix[i, j]);
                }
                if (diag <= rowSum)
                    return false;
            }
            return true;
        }
    }
}
