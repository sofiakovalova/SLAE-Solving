using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAESolver.Utilities
{
    public class IterationResult
    {
        public double[] Solution { get; }
        public int Iterations { get; }
        public double Error { get; }
        public string MethodName { get; }

        public IterationResult(double[] solution, int iterations, double error, string methodName)
        {
            Solution = solution;
            Iterations = iterations;
            Error = error;
            MethodName = methodName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Розв’язок:");
            for (int i = 0; i < Solution.Length; i++)
            {
                sb.AppendLine($"x{i + 1} = {Solution[i]:0.######}");
            }
            sb.AppendLine($"Кількість ітерацій: {Iterations}");
            sb.AppendLine($"Похибка: {Error:0.##########}");
            return sb.ToString();
        }
    }
}