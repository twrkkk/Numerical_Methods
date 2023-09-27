using NumericalMethods.Core.Utils.RandomProviders;
using NumericalMethods.Core.Extensions;
using System;
using System.Text;
using NumericalMethods.Core.Utils.Interfaces;
using NumericalMethods.Core.Utils;

namespace Task1
{
    public class Program
    {
        static void Main()
        {
            const int TestCount = 1;
            var testCases = new (int n, int minValue, int maxValue)[]
            {
                (10, -10, 10),
                //(10, -100, 100),
                //(10, -1000, 1000),
                //(100, -10, 10),
                //(100, -100, 100),
                //(100, -1000, 1000),
                //(1000, -10, 10),
                //(1000, -100, 100),
                //(1000, -1000, 1000)
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

            Console.ReadKey(true);
        }
        private static void EnterKHandler(int size, bool correct, ref int k)
        {
            do
            {
                Console.WriteLine("Enter k");
                try
                {
                    k = int.Parse(Console.ReadLine());
                    if (k > 0 && k < size) correct = true;
                    if (!correct) Console.WriteLine($"Enter 0 < k < {size}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (!correct);
            --k;
        }

        const double NonZeroEps = 1e-5;

        static readonly IRangedRandomProvider<double> _random = ((IRangedRandomProvider<double>)new DoubleRandomProvider(NonZeroEps)).NotDefault();

        static (double UnitAccuracy, double RandomAccuracy) FindAccuracies(int count, double minValue, double maxValue)
        {
            _ = count < 0 ? throw new ArgumentOutOfRangeException(nameof(count), "The number of elements must not be negative.") : true;

            double[,] matrixWithoutRightSide = _random.GenerateMatrix(count, count, minValue, maxValue);
            //   {
            //    { 0.0,0.0,0.0,0.0,0.0,7.8,9.9,0.0,9.0,-8.9 },
            //    { 0.0,0.0,0.0,0.0,0.0,-4.3,-7.6,8.7,0.2,-5.5},
            //    { 0.0,0.0,0.0,0.0,0.0,6.2,-1.8,2.2,3.7,0.0},
            //    { 0.0,0.0,0.0,0.0,0.0,-9.6,-0.9,4.5,0.0,0.0},
            //    { 0.0,0.0,0.0,0.0,0.0,-5.2,-9.7,0.0,0.0,0.0},
            //    { 0.0,0.0,0.0,-6.1,3.8,2.9,6.0,0.0,0.0,0.0},
            //    { 0.0,0.0,7.6,3.7,2.2,-8.2,-8.6,0.0,0.0,0.0},
            //    { 0.0,-9.2,7.8,6.2,0.0,-2.7,-0.2,0.0,0.0,0.0},
            //    { 6.2,5.2,1.0,0.0,0.0,-4.7,7.3,0.0,0.0,0.0},
            //    { 6.6,-8.1,0.0,0.0,0.0,-3.9,6.0,0.0,0.0,0.0}
            //};

            IReadOnlyList<double> expectRandomSolution = _random.Repeat(count, minValue, maxValue).ToArray();
            IReadOnlyList<double> expectUnitSolution = Enumerable.Repeat(1, count).Select(x => (double)x).ToArray();

            var rightSideBuilder = new RightSideBuilder(matrixWithoutRightSide);
            var randomRightSide = rightSideBuilder.Build(expectRandomSolution);
            var unitRightSide = rightSideBuilder.Build(expectUnitSolution);

            var randomMatrix = new FirstTaskMatrix(matrixWithoutRightSide, randomRightSide);
            var actualRandomSolution = randomMatrix.Solve();

            //var unitMatrix = new FirstTaskMatrix(matrixWithoutRightSide, unitRightSide);
            //var actualUnitSolution = unitMatrix.Solve();
            return (0, 0);
            //return (AccuracyUtils.CalculateAccuracy(expectUnitSolution, actualUnitSolution, NonZeroEps), AccuracyUtils.CalculateAccuracy(expectRandomSolution, actualRandomSolution, NonZeroEps));
        }
    }
}