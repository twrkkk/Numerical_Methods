using NumericalMethods.Core.Utils.RandomProviders;
using NumericalMethods.Core.Extensions;
using NumericalMethods.Core.Utils.Interfaces;
using NumericalMethods.Core.Utils;

namespace Task1
{
    public class Program
    {
        static void Main()
        {
            FirstPart();
            SecondPart();

            Console.ReadKey(true);
        }

        private static void FirstPart()
        {
            var matrix = new MatrixFromTextFile();
            Console.WriteLine($"Среднее значение оценки точности: {FindAccuracy(matrix.Count)}");
            Console.WriteLine();
        }

        private static void SecondPart()
        {
            const int TestCount = 3;
            var testCases = new (int n, int minValue, int maxValue)[]
            {
                (10, -10, 10),
                (10, -100, 100),
                (10, -1000, 1000),
                (100, -10, 10),
                (100, -100, 100),
                (100, -1000, 1000),
                (1000, -10, 10),
                (1000, -100, 100),
                (1000, -1000, 1000)
            };

            foreach (var (count, minValue, maxValue) in testCases)
            {
                IReadOnlyList<(double UnitAccuracy, double RandomAccuracy)> accuracies = Enumerable
                    .Range(1, TestCount)
                    .Select(_ => FindAccuracies(count, minValue, maxValue))
                    .ToArray();

                Console.WriteLine($"N = {count}; Min = {minValue}; Max = {maxValue};");
                Console.WriteLine($"Средняя относительная погрешность системы: {accuracies.Select(x => x.UnitAccuracy).Average()}");
                Console.WriteLine($"Среднее значение оценки точности: {accuracies.Select(x => x.RandomAccuracy).Average()}");
            }
        }

        const double NonZeroEps = 1e-5;
        static readonly IRangedRandomProvider<double> _random = ((IRangedRandomProvider<double>)new DoubleRandomProvider(NonZeroEps)).NotDefault();
            
        static (double UnitAccuracy, double RandomAccuracy) FindAccuracies(int count, double minValue, double maxValue)
        {
            _ = count < 0 ? throw new ArgumentOutOfRangeException(nameof(count), "The number of elements must not be negative.") : true;

            double[,] matrixWithoutRightSide = _random.GenerateMatrix(count, count, minValue, maxValue);
            IReadOnlyList<double> expectRandomSolution = _random.Repeat(count, minValue, maxValue).ToArray();
            IReadOnlyList<double> expectUnitSolution = Enumerable.Repeat(1, count).Select(x => (double)x).ToArray();

            var rightSideBuilder = new RightSideBuilder(matrixWithoutRightSide);
            var randomRightSide = rightSideBuilder.Build(expectRandomSolution);
            var unitRightSide = rightSideBuilder.Build(expectUnitSolution);

            var randomMatrix = new FirstTaskMatrix(matrixWithoutRightSide, randomRightSide);
            var actualRandomSolution = randomMatrix.Solve().Reverse().ToArray();

            var unitMatrix = new FirstTaskMatrix(matrixWithoutRightSide, unitRightSide);
            var actualUnitSolution = unitMatrix.Solve();

            return (AccuracyUtils.CalculateAccuracy(expectUnitSolution, actualUnitSolution, NonZeroEps),
                AccuracyUtils.CalculateAccuracy(expectRandomSolution, actualRandomSolution, NonZeroEps));
        }

        static double FindAccuracy(int count)
        {
            _ = count < 0 ? throw new ArgumentOutOfRangeException(nameof(count), "The number of elements must not be negative.") : true;
            IReadOnlyList<double> expectUnitSolution = Enumerable.Repeat(1, count).Select(x => (double)x).ToArray();

            var matrixFromTextFile = new MatrixFromTextFile();
            var matrix = matrixFromTextFile.Matrix;

            var rightSideBuilder = new RightSideBuilder(matrix);
            var unitRightSide = rightSideBuilder.Build(expectUnitSolution);

            var rightSide = matrixFromTextFile.RightSide;
            var firstTaskMatrix = new FirstTaskMatrix(matrix, rightSide);
            var rightSideSolution = firstTaskMatrix.Solve();
            firstTaskMatrix.MatrixToTextFile();

            var unitMatrix = new FirstTaskMatrix(matrix, unitRightSide);
            var unitRightSideSolution = unitMatrix.Solve();

            return AccuracyUtils.CalculateAccuracy(unitRightSideSolution, expectUnitSolution, NonZeroEps);
        }
    }
}