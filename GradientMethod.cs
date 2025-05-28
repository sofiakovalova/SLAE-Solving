using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLAESolver.Utilities;

namespace SLAESolver.Solvers
{
    internal class GradientMethod : ISolver
    {
        public IterationResult Solve(double[,] matrix, double[] b, double epsilon, int maxIterations)
        {
            MatrixValidator.Validate(matrix, b, requireSymmetric: true, requirePositiveDefinite: true);

            int n = b.Length;
            double[] x = new double[n];
            int iterationCount = 0;
            double error = 0;

            for (int k = 0; k < maxIterations; k++)
            {
                double[] r = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                        sum += matrix[i, j] * x[j];
                    r[i] = sum - b[i];
                }

                double numerator = r.Sum(val => val * val);
                double denominator = 0;

                for (int i = 0; i < n; i++)
                {
                    double temp = 0;
                    for (int j = 0; j < n; j++)
                        temp += matrix[i, j] * r[j];
                    denominator += r[i] * temp;
                }

                if (denominator == 0)
                    throw new Exception("Ділення на нуль при обчисленні τ.");

                double tau = numerator / denominator;

                error = 0;
                for (int i = 0; i < n; i++)
                {
                    double newX = x[i] - tau * r[i];
                    error = Math.Max(error, Math.Abs(newX - x[i]));
                    x[i] = newX;
                }

                iterationCount++;

                if (error < epsilon)
                    return new IterationResult(x, iterationCount, error, "Метод найшвидшого спуску");
            }

            throw new Exception("Метод градієнтного спуску не досяг збіжності за максимально дозволену кількість ітерацій (10 000). Спробуйте зменшити точність або перевірити коректність вхідних даних.");
        }
    }
}
