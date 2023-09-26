using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class FirstTaskMatrix
    {
        public readonly int Size;
        private readonly double[] _c, _b, _a, _e, _d, _f, _result;
        private bool _solved;

        public IReadOnlyList<double> Result => _result;

        public FirstTaskMatrix(string fileName)
        {
            string[] s = File.ReadAllLines(fileName);
            Size = (s.Length - 1) / 2;

            _c = new double[Size - 1];
            _b = new double[Size];
            _a = new double[Size - 1];
            _e = new double[Size];
            _d = new double[Size];
            _f = new double[Size];
            _result = new double[Size];

            for (int i = 0; i < Size; ++i)
            {
                string[] str = s[i].Split(new char[] { ' ', '\t' });
                _b[i] = int.Parse(str[Size - i - 1]);
                _e[i] = int.Parse(str[5]);
                _d[i] = int.Parse(str[6]);
                if (Size - i - 2 >= 0)
                    _c[i] = int.Parse(str[Size - i - 2]);
                if (i > 0 && Size - i > 0)
                    _a[i - 1] = int.Parse(str[Size - i]);
            }

            int j = 0;
            for (int i = Size + 1; i < s.Length; ++i)
                _f[j++] = int.Parse(s[i]);
        }

        public FirstTaskMatrix(double[,] matrix, IReadOnlyList<double> rightSight)
        {
            Size = matrix.GetLength(0);

            _c = new double[Size - 1];
            _b = new double[Size];
            _a = new double[Size - 1];
            _e = new double[Size];
            _d = new double[Size];
            _f = new double[Size];
            _result = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                _b[i] = matrix[i, Size - i - 1];
                _e[i] = matrix[i, 5];
                _d[i] = matrix[i, 6];
                if (Size - i - 2 >= 0)
                    _c[i] = matrix[i, Size - i - 2];
                if (i > 0 && Size - i > 0)
                    _a[i - 1] = matrix[i, Size - i];
            }

            for (int i = 0; i < Size; i++)
                _f[i] = rightSight[i];
        }

        public void MatrixToTextFile(string fileName = "2.txt")
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                for (int i = 0; i < Size; ++i)
                {
                    double[] row = new double[Size];
                    row[5] = _e[i];
                    row[6] = _d[i];
                    row[Size - i - 1] = _b[i];
                    if (Size - i - 2 >= 0)
                        row[Size - i - 2] = _c[i];
                    if (i > 0 && Size - i > 0)
                        row[Size - i] = _a[i - 1];

                    var builder = new StringBuilder();
                    Array.ForEach(row, x => builder.Append(x + "\t"));
                    var res = builder.ToString();

                    sw.WriteLine(res);
                }

                sw.WriteLine();

                for (int i = 0; i < Size; ++i)
                    sw.WriteLine(_f[i]);
            }
        }

        protected virtual void DevideLine(int rowIndex, double element)
        {
            if (IsBelongsToA(rowIndex))
                _a[rowIndex - 1] /= element;

            _b[rowIndex] /= element;
            _d[rowIndex] /= element;
            _e[rowIndex] /= element;
            _f[rowIndex] /= element;

            if (IsBelongsToC(rowIndex))
                _c[rowIndex] /= element;
        }

        protected virtual void SubCurrentFromNext(int rowIndex)
        {
            if (rowIndex < Size - 1)
            {
                int nextRow = rowIndex + 1;
                _a[rowIndex] -= _b[rowIndex];
                _b[nextRow] -= _c[rowIndex];
                _d[nextRow] -= _d[rowIndex];
                _e[nextRow] -= _e[rowIndex];
                _f[nextRow] -= _f[rowIndex];

                if (nextRow == 4)
                    _c[4] = _d[4];
                else if (nextRow == 5)
                    _c[5] = _e[5];
            }
            else
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "");
        }

        protected virtual void SubPrevFromCurrent(int rowIndex)
        {
            if (rowIndex > 0)
            {
                int prevRow = rowIndex - 1;
                _c[rowIndex - 1] -= _b[rowIndex];
                _b[prevRow] -= _a[prevRow];
                _d[prevRow] -= _d[rowIndex];
                _e[prevRow] -= _e[rowIndex];
                _f[prevRow] -= _f[rowIndex];

                if (prevRow == 7)
                    _a[6] = _e[7];
                else if (prevRow == 6)
                    _a[5] = _d[6];
            }
            else
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "");
        }

        protected virtual bool IsBelongsToC(int rowIndex)
            => rowIndex < Size - 1;

        protected virtual bool IsBelongsToA(int rowIndex)
            => rowIndex > 0;

        protected virtual void FirstPhase()
        {
            for (var rowIndex = 0; rowIndex <= 5; ++rowIndex)
            {
                if (_b[rowIndex] == 0)
                    continue;

                DevideLine(rowIndex, _b[rowIndex]);

                if (_a[rowIndex] == 0)
                    continue;

                DevideLine(rowIndex + 1, _a[rowIndex]);

                SubCurrentFromNext(rowIndex);
            }
        }

        protected virtual void SecondPhase()
        {
            for (var rowIndex = Size - 1; rowIndex > 6; --rowIndex)
            {
                if (_b[rowIndex] == 0)
                    continue;

                DevideLine(rowIndex, _b[rowIndex]);

                if (_c[rowIndex - 1] == 0)
                    continue;

                DevideLine(rowIndex - 1, _c[rowIndex - 1]);

                SubPrevFromCurrent(rowIndex);
            }

            if (_e[6] != 0)
                DevideLine(6, _e[6]);
        }

        protected virtual void ThirdPhase()
        {
            for (var rowIndex = 0; rowIndex < Size; ++rowIndex)
                if (rowIndex != 5 && _d[rowIndex] != 0)
                    DevideLine(rowIndex, _d[rowIndex]);

            for (var rowIndex = 0; rowIndex < Size; ++rowIndex)
            {
                if (rowIndex != 5 && _d[rowIndex] != 0)
                {
                    _d[rowIndex] -= _d[5];
                    _e[rowIndex] -= _e[5];
                    _f[rowIndex] -= _f[5];
                }
            }

            _a[5] = _d[6];
            _a[6] = _e[7];
            _b[5] = _d[5];
            _b[6] = _e[6];
            _c[4] = _d[4];
            _c[5] = _e[5];
        }

        protected virtual void FourthPhase()
        {
            if (_e[6] != 0)
                DevideLine(6, _e[6]);

            for (var rowIndex = 0; rowIndex < Size; ++rowIndex)
            {
                if (rowIndex != 6 && _e[rowIndex] != 0)
                    DevideLine(rowIndex, _e[rowIndex]);
            }

            for (var rowIndex = 0; rowIndex < Size; ++rowIndex)
            {
                if (rowIndex != 6 && _e[rowIndex] != 0)
                {
                    _d[rowIndex] -= _d[6];
                    _e[rowIndex] -= _e[6];
                    _f[rowIndex] -= _f[6];
                }
            }

            _a[5] = _d[6];
            _a[6] = _e[7];
            _b[5] = _d[5];
            _b[6] = _e[6];
            _c[4] = _d[4];
            _c[5] = _e[5];
        }

        protected virtual void FifthPhase()
        {
            for (var rowIndex = 0; rowIndex < Size; ++rowIndex)
                if (_b[rowIndex] != 0)
                    DevideLine(rowIndex, _b[rowIndex]);
        }

        protected virtual void CalculatePhase()
        {
            for (var rowIndex = 4; rowIndex <= 7; ++rowIndex)
                _result[rowIndex] = _f[rowIndex];

            for (var rowIndex = 3; rowIndex >= 0; --rowIndex)
                _result[rowIndex] = _f[rowIndex] - _c[rowIndex] * _result[rowIndex + 1];

            for (var rowIndex = 8; rowIndex < Size; ++rowIndex)
                _result[rowIndex] = _f[rowIndex] - _a[rowIndex - 1] * _result[rowIndex - 1];
        }

        public IReadOnlyList<double> Solve()
        {
            if (!_solved)
            {
                _solved = true;
                FirstPhase();
                SecondPhase();
                ThirdPhase();
                FourthPhase();
                FifthPhase();
                CalculatePhase();
            }

            return _result;
        }
    }
}
