#include <iostream>
#include <vector>
#include <cmath>
#include <cstdlib>
#include <ctime>
#include <algorithm>
#include<tuple>
#include <iomanip>
#include <string>
#include <Windows.h>

using namespace std;

std::vector<double> generateRandomVector(int N) {
	std::vector<double> randomVector(N, 0.0);
	double norm = 0.0;

	for (int i = 0; i < N; ++i) {
		randomVector[i] = rand() % 10 + 1; // Генерация случайных чисел от 1 до 10
		norm += randomVector[i] * randomVector[i];
	}

	norm = sqrt(norm);
	for (int i = 0; i < N; ++i) {
		randomVector[i] /= norm;
	}

	return randomVector;
}

std::vector<std::vector<double>> multiplyMatrices(const std::vector<std::vector<double>>& matrix1,
	const std::vector<std::vector<double>>& matrix2) {

	if (matrix1[0].size() != matrix2.size()) {
		std::cerr << "Невозможно умножить матрицы. Количество столбцов первой матрицы не равно количеству строк второй матрицы." << std::endl;
		return std::vector<std::vector<double>>();
	}

	std::vector<std::vector<double>> result(matrix1.size(), std::vector<double>(matrix2[0].size(), 0.0));

	for (size_t i = 0; i < matrix1.size(); ++i) {
		for (size_t j = 0; j < matrix2[0].size(); ++j) {
			for (size_t k = 0; k < matrix1[0].size(); ++k) {
				result[i][j] += matrix1[i][k] * matrix2[k][j];
			}
		}
	}

	return result;
}

std::vector<std::vector<double>> transposeMatrix(const std::vector<std::vector<double>>& matrix) {
	if (matrix.empty() || matrix[0].empty()) {
		std::cerr << "Ошибка: Пустая матрица." << std::endl;
		return {};
	}

	size_t rows = matrix.size();
	size_t cols = matrix[0].size();

	std::vector<std::vector<double>> result(cols, std::vector<double>(rows, 0.0));

	for (size_t i = 0; i < rows; ++i) {
		for (size_t j = 0; j < cols; ++j) {
			result[j][i] = matrix[i][j];
		}
	}

	return result;
}

std::vector<std::vector<double>> generateMatrixWithColumn(vector<double> column, int n, int c)
{
	std::vector<std::vector<double>> result(n);
	for (int i = 0; i < n; i++)
		result[i].resize(n);

	for (int i = 0; i < n; i++)
		result[i][c] = column[i];

	return result;
}

std::vector<std::vector<double>> E(int n)
{
	std::vector<std::vector<double>> result(n);
	for (int i = 0; i < n; ++i)
		result[i].resize(n);

	for (int i = 0; i < n; ++i)
		result[i][i] = 1;

	return result;
}

std::vector<std::vector<double>> generateMatrixWithDiagonal(vector<double> diagonal, int n)
{
	std::vector<std::vector<double>> result(n);
	for (int i = 0; i < n; i++)
		result[i].resize(n);

	for (int i = 0; i < n; i++)
		result[i][i] = diagonal[i];

	return result;
}

std::vector<std::vector<double>> subtractMatrices(const std::vector<std::vector<double>>& A,
	const std::vector<std::vector<double>>& B) {
	if (A.size() != B.size() || A[0].size() != B[0].size()) {
		std::cerr << "Error: Matrices must have the same dimensions for subtraction.\n";
		return A;  // Возвращаем исходную матрицу, так как операция не выполнима
	}

	std::vector<std::vector<double>> result(A.size(), std::vector<double>(A[0].size(), 0.0));

	for (size_t i = 0; i < A.size(); ++i) {
		for (size_t j = 0; j < A[0].size(); ++j) {
			result[i][j] = A[i][j] - B[i][j];
		}
	}

	return result;
}

std::vector<std::vector<double>> multiply(std::vector<std::vector<double>>& matrix, double value)
{
	int size = matrix.size();
	for (int i = 0; i < size; ++i)
		for (int j = 0; j < size; ++j)
			matrix[i][j] *= value;

	return matrix;
}

std::vector<double> multiplyMatrixVector(const std::vector<std::vector<double>>& matrix, const std::vector<double>& vector) {
	if (matrix[0].size() != vector.size()) {
		std::cerr << "Некорректные размеры матрицы и вектора для умножения." << std::endl;
		exit(EXIT_FAILURE);
	}

	size_t rows = matrix.size();
	size_t cols = matrix[0].size();

	std::vector<double> result(rows, 0.0);

	for (size_t i = 0; i < rows; ++i) {
		for (size_t j = 0; j < cols; ++j) {
			result[i] += matrix[i][j] * vector[j];
		}
	}

	return result;
}

double calculateVectorNorma(const std::vector<double>& vec) {
	double result = 0.0;

	for (const auto& element : vec) {
		result = max(result, abs(element));
	}

	return result;
}

std::vector<double> subtractVectors(std::vector<double> vector1, std::vector<double> vector2) {
	if (vector1.size() != vector2.size()) {
		std::cerr << "Некорректные размеры векторов для вычитания." << std::endl;
		exit(EXIT_FAILURE);
	}

	size_t size = vector1.size();

	std::vector<double> result(size);

	for (size_t i = 0; i < size; ++i) {
		result[i] = vector1[i] - vector2[i];
	}

	return result;
}

void printMatrix(const std::vector<std::vector<double>>& matrix) {
	for (const auto& row : matrix) {
		for (double element : row) {
			std::cout << element << " ";
		}
		std::cout << std::endl;
	}
}

std::vector<std::vector<double>> generateMatrixWithLambda(vector<double> l, double& norma)
{
	int N = l.size();
	auto w = generateRandomVector(N);
	auto e = E(N);
	auto matrix_w = generateMatrixWithColumn(w, N, 0);
	auto w_trasposed = transposeMatrix(matrix_w);
	auto right = multiplyMatrices(multiply(matrix_w, 2), w_trasposed);
	auto hausholder = subtractMatrices(e, right);

	cout << '\n' << " Матрица Хаусхолдера " << '\n';
	printMatrix(hausholder);
	cout << '\n';

	auto lambda = generateMatrixWithDiagonal(l, N);
	auto symmetricMatrix = multiplyMatrices(multiplyMatrices(hausholder, lambda), transposeMatrix(hausholder));

	auto left = multiplyMatrixVector(symmetricMatrix, hausholder[0]);
	auto r = multiplyMatrixVector(lambda, hausholder[0]);

	auto vector = subtractVectors(left, r);
	norma = calculateVectorNorma(vector);

	return symmetricMatrix;
}

vector<double> jacobi_rotation(std::vector<std::vector<double>>& A, int n, double eps, int& m, double& max_aij)
{
	int k = 0;
	vector<double> diag(n);
	for (int i = 0; i < n; ++i)
		diag[i] = A[i][i];

	while (true) {
		double max_off_diag = 0.0;
		int p, q;
		for (int i = 0; i < n; ++i) {
			for (int j = i + 1; j < n; ++j) {
				if (std::abs(A[i][j]) > max_off_diag) {
					max_off_diag = std::abs(A[i][j]);
					max_aij = max(max_aij, max_off_diag);
					p = i;
					q = j;
				}
			}
		}

		if (max_off_diag < eps || k >= m)
		{
			m = k;
			return diag;
		}

		double x = (diag[q] - diag[p]) / (2.0 * A[p][q]);
		double t = (x >= 0.0) ? 1.0 / (x + sqrt(1.0 + x * x)) : 1.0 / (x - sqrt(1.0 + x * x));
		double c = 1.0 / sqrt(1.0 + t * t);
		double s = t * c;
		double tau = s / (1.0 + c);

		double a_pq = A[p][q];
		double a_pp = diag[p];
		double a_qq = diag[q];

		diag[p] -= t * a_pq;
		diag[q] += t * a_pq;

		A[q][p] = A[p][q] = (c * c - s * s) * a_pq + c * s * (a_pp - a_qq);

		for (int i = 0; i < n; ++i) {
			if (i != p && i != q) {
				double Aip = A[i][p];
				double Aiq = A[i][q];
				A[i][p] = A[p][i] = Aip - s * (Aiq + tau * Aip);
				A[i][q] = A[q][i] = Aiq + s * (Aip - tau * Aiq);
			}
		}
		++k;
	}
}

vector<tuple<double, double>> test(int n, int l_min, int l_max, double eps, int& max_iteration, double& max_aij, double& norma_sum)
{
	vector<double> expected_lambda(n);
	for (int i = 0; i < n; ++i)
		expected_lambda[i] = l_min + static_cast<double>(rand()) / RAND_MAX * (l_max - l_min);

	double norma = 0;
	auto symmetricMatrix = generateMatrixWithLambda(expected_lambda, norma);
	norma_sum += norma;
	cout << " Сгенерированная матрица " << '\n';
	printMatrix(symmetricMatrix);
	cout << '\n';

	auto real_lambda = jacobi_rotation(symmetricMatrix, n, eps, max_iteration, max_aij);

	sort(expected_lambda.begin(), expected_lambda.end());
	sort(real_lambda.begin(), real_lambda.end());

	cout << " Ожидаемые собственные значения " << '\n';
	for (int i = 0; i < n; ++i)
		cout << expected_lambda[i] << ' ';
	cout << '\n';
	cout << " Полученные собственные значения " << '\n';
	for (int i = 0; i < n; ++i)
		cout << real_lambda[i] << ' ';
	cout << '\n';

	vector<tuple<double, double>> result;
	for (int i = 0; i < n; ++i)
		result.push_back(make_tuple(expected_lambda[i], real_lambda[i]));

	return result;
}

int main() {
	srand(static_cast<unsigned int>(time(nullptr)));

	int iteration = 1;

	vector<tuple<int, int, int, double, int>> test_cases
	{
		make_tuple(3, -2, 2, 1e-5, 2000),
		/*make_tuple(10, -2, 2, 1e-7, 2000),
		make_tuple(10, -2, 2, 1e-9, 2000),
		make_tuple(10, -50, 50, 1e-5, 2000),
		make_tuple(10, -50, 50, 1e-7, 2000),
		make_tuple(10, -50, 50, 1e-9, 2000),

		make_tuple(30, -2, 2, 1e-5, 2000),
		make_tuple(30, -2, 2, 1e-7, 2000),
		make_tuple(30, -2, 2, 1e-9, 2000),
		make_tuple(30, -50, 50, 1e-5, 2000),
		make_tuple(30, -50, 50, 1e-7, 2000),
		make_tuple(30, -50, 50, 1e-9, 2000),*/
	};

	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	int test_count = test_cases.size();

	std::cout << std::left << std::setw(25) << "Размерность системы"
		<< std::setw(25) << "Диапазон значений /\\"
		<< std::setw(25) << "Максимальное значение A_ij"
		<< std::setw(25) << "Среднее число итераций"
		<< std::setw(25) << "Средняя оценка точности /\\"
		<< std::setw(25) << "Средняя мера точности"
		<< '\n';

	for (int c = 0; c < test_count; ++c)
	{
		int n = get<0>(test_cases[c]);
		int l_min = get<1>(test_cases[c]);
		int l_max = get<2>(test_cases[c]);
		double eps = get<3>(test_cases[c]);
		int max_iteration = get<4>(test_cases[c]);
		double max_aij = -1e20;

		double sum_of_iteration = 0;
		double sum_of_delta = 0;

		double norma_sum = 0;
		for (int i = 0; i < iteration; ++i)
		{
			double max_delta = -1e20;
			max_aij = -1e20;
			max_iteration = get<4>(test_cases[c]);
			auto result = test(n, l_min, l_max, eps, max_iteration, max_aij, norma_sum);

			sum_of_iteration += max_iteration;

			for (int j = 0; j < result.size(); ++j)
			{
				auto expected_lambda = get<0>(result[j]);
				auto real_lambda = get<1>(result[j]);

				max_delta = max(max_delta, abs(expected_lambda - real_lambda));
			}

			sum_of_delta += max_delta;
		}

		std::cout << std::left << std::setw(25) << n
			<< std::setw(35) << " от " + to_string(l_min) + " до " + to_string(l_max)
			<< std::setw(25) << max_aij
			<< std::setw(25) << sum_of_iteration / iteration
			<< std::setw(25) << sum_of_delta / iteration
			<< std::setw(25) << norma_sum / iteration
			<< "\n\n";
	}

}