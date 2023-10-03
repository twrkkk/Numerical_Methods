public class MatrixFromTextFile
{
    private double[,] _matrix;
    private double[] _rightSide;

    public double[,] Matrix => _matrix;
    public double[] RightSide => _rightSide;
    public int Count => _matrix.GetLength(0);   
    public MatrixFromTextFile(string fileName = "1.txt")
    {
        string[] s = File.ReadAllLines(fileName);
        var size = (s.Length - 1) / 2;
        _matrix = new double[size, size];
        _rightSide = new double[size];

        for (int i = 0; i < size; i++)
        {
            var arr = s[i]
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => double.Parse(x));

            int k = 0;
            foreach (var elem in arr)
                _matrix[i, k++] = elem;
        }

        int j = 0;
        for (int i = size + 1; i < s.Length; i++)
        {
            var right = s[i]
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => double.Parse(x));

            foreach (var elem in right)
                _rightSide[j++] = elem;
        }
    }
}
