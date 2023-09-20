using System.Text;

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] s = File.ReadAllLines("1.txt");
            int size = (s.Length - 1) / 2;
            int[] diagonal_h, diagonal, diagonal_l, k_column, k1_column, answers;
            int k = 0;
            bool correct = false;

            EnterKHandler(size, correct, ref k);
            TextFileToMatrix(s, size, k, out diagonal_h, out diagonal, out diagonal_l, out k_column, out k1_column, out answers);
            MatrixToTextFile(size, k, diagonal_h, diagonal, diagonal_l, k_column, k1_column, answers);
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

        private static void TextFileToMatrix(string[] s, int size, int k, out int[] diagonal_h, out int[] diagonal, out int[] diagonal_l, out int[] k_column, out int[] k1_column, out int[] answers)
        {
            diagonal_h = new int[size - 1];
            diagonal = new int[size];
            diagonal_l = new int[size - 1];
            k_column = new int[size];
            k1_column = new int[size];
            answers = new int[size];
            for (int i = 0; i < size; ++i)
            {
                string[] str = s[i].Split(new char[] { ' ', '\t' });
                diagonal[i] = int.Parse(str[size - i - 1]);
                k_column[i] = int.Parse(str[k]);
                k1_column[i] = int.Parse(str[k + 1]);
                if (size - i - 2 >= 0)
                    diagonal_h[i] = int.Parse(str[size - i - 2]);
                if (i > 0 && size - i > 0)
                    diagonal_l[i - 1] = int.Parse(str[size - i]);
            }

            int j = 0;
            for(int i = size+1; i < s.Length;++i)
                answers[j++] = int.Parse(s[i]);
        }

        private static void MatrixToTextFile(int size, int k, int[] diagonal_h, int[] diagonal, int[] diagonal_l, int[] k_column, int[] k1_column, int[] answers)
        {
            using (StreamWriter sw = new StreamWriter("2.txt", false))
            {
                for (int i = 0; i < size; ++i)
                {
                    int[] row = new int[size];
                    row[k] = k_column[i];
                    row[k + 1] = k1_column[i];
                    row[size - i - 1] = diagonal[i];
                    if (size - i - 2 >= 0)
                        row[size - i - 2] = diagonal_h[i];
                    if (i > 0 && size - i > 0)
                        row[size - i] = diagonal_l[i - 1];

                    var builder = new StringBuilder();
                    Array.ForEach(row, x => builder.Append(x + "\t"));
                    var res = builder.ToString();

                    sw.WriteLine(res);
                }

                sw.WriteLine();

                for (int i = 0; i < size; ++i)
                    sw.WriteLine(answers[i]);
            }
        }
    }
}