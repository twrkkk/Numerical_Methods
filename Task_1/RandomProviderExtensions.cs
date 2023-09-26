using NumericalMethods.Core.Utils.Interfaces;
using System.Drawing;

namespace Task1
{
    static class RandomProviderExtensions
    {
        public static T[,] GenerateMatrix<T>(this IRangedRandomProvider<T> random, int rowsCount, int columnsCount, T minValue, T maxValue)
        {
            _ = random ?? throw new ArgumentNullException(nameof(random));
            _ = rowsCount < 0 ? throw new ArgumentOutOfRangeException(nameof(rowsCount), "The number of rows must not be negative.") : true;
            _ = columnsCount < 0 ? throw new ArgumentOutOfRangeException(nameof(columnsCount), "The number of columns must not be negative.") : true;

            var matrix = new T[rowsCount, columnsCount];
            for (var i = 0; i < matrix.GetLength(0); ++i)
            {
                //b
                matrix[i, rowsCount - i - 1] = random.Next(minValue, maxValue);

                //c
                if (rowsCount - i - 2 >= 0)
                    matrix[i, rowsCount - i - 2] = random.Next(minValue, maxValue);
                //a
                if (i > 0 && rowsCount - i > 0)
                    matrix[rowsCount - i, i] = random.Next(minValue, maxValue);
                //e
                matrix[i, 5] = random.Next(minValue, maxValue);
                //d
                matrix[i, 6] = random.Next(minValue, maxValue);
            }
            
            return matrix;
        }
    }
}
