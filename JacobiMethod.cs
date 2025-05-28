using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLAESolver.Utilities;

namespace SLAESolver.Solvers
{
    internal class JacobiMethod : ISolver
    {
        public IterationResult Solve(double[,] matrix, double[] b, double epsilon, int maxIterations)
        {
            MatrixValidator.Validate(matrix, b);

            int n = b.Length;
            double[] xOld = new double[n];
            double[] xNew = new double[n];
            int iterationCount = 0;
            double error = 0;

            for (int k = 0; k < maxIterations; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i) sum += matrix[i, j] * xOld[j];
                    }
                    xNew[i] = (b[i] - sum) / matrix[i, i];
                }

                iterationCount++;
                error = 0;
                for (int i = 0; i < n; i++)
                {
                    error = Math.Max(error, Math.Abs(xNew[i] - xOld[i]));
                    xOld[i] = xNew[i];
                }

                if (error < epsilon)
                    return new IterationResult(xNew, iterationCount, error, "Метод Якобі");
            }

            throw new Exception("Метод Якобі не досяг збіжності за максимально дозволену кількість ітерацій (10 000). Спробуйте зменшити точність або перевірити коректність вхідних даних.");
        }
    }
}
