using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLAESolver.Utilities;

namespace SLAESolver.Solvers
{
    internal class GaussSeidelMethod : ISolver
    {
        public IterationResult Solve(double[,] matrix, double[] b, double epsilon, int maxIterations)
        {
            MatrixValidator.Validate(matrix, b);

            int n = b.Length;
            double[] x = new double[n];
            double[] xOld = new double[n];
            int iterationCount = 0;
            double error = 0;

            for (int k = 0; k < maxIterations; k++)
            {
                Array.Copy(x, xOld, n);

                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i) sum += matrix[i, j] * x[j];
                    }
                    x[i] = (b[i] - sum) / matrix[i, i];
                }

                iterationCount++;
                error = 0;
                for (int i = 0; i < n; i++)
                {
                    error = Math.Max(error, Math.Abs(x[i] - xOld[i]));
                }

                if (error < epsilon)
                    return new IterationResult(x, iterationCount, error, "Метод Гауса-Зейделя");
            }

            throw new Exception("Метод Гауса-Зейделя не досяг збіжності за максимально дозволену кількість ітерацій (10 000). Спробуйте зменшити точність або перевірити коректність вхідних даних.");
        }
    }
}
