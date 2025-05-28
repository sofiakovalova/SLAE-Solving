using SLAESolver.Utilities;

namespace SLAESolver.Solvers
{
    public interface ISolver
    {
        IterationResult Solve(double[,] matrix, double[] b, double epsilon, int maxIterations);
    }
}