#include <iostream>
#include <vector>
#include <functional>

std::vector<double> generate_equidistant_nodes(double a, double b, int n) {
    std::vector<double> nodes;
    double h = (b - a) / (n - 1);
    for (int i = 0; i < n; ++i) {
        nodes.push_back(a + i * h);
    }
    return nodes;
}

std::vector<double> compute_function_values(const std::vector<double>& xi, std::function<double(double)> func) {
    std::vector<double> yi;
    for (double x : xi) {
        yi.push_back(func(x));
    }
    return yi;
}

std::vector<std::vector<double>> get_coefficients(int pl, std::vector<double>& xi) {
    int n = xi.size();
    std::vector<std::vector<double>> coefficients(n, std::vector<double>(2, 0.0));
    for (int i = 0; i < n; ++i) {
        if (i == pl) {
            coefficients[i][0] = INFINITY;
            coefficients[i][1] = INFINITY;
        }
        else {
            coefficients[i][0] = 1 / (xi[pl] - xi[i]);
            coefficients[i][1] = -xi[i] / (xi[pl] - xi[i]);
        }
    }
    std::vector<std::vector<double>> filtered_coefficients;
    for (int i = 0; i < n; ++i) {
        if (coefficients[i][0] != INFINITY) {
            filtered_coefficients.push_back({ coefficients[i][1], coefficients[i][0] });
        }
    }
    return filtered_coefficients;
}

std::vector<std::vector<double>> get_polynomial_l(std::vector<double>& xi) {
    int n = xi.size();
    std::vector<std::vector<double>> pli(n, std::vector<double>(n, 0.0));
    for (int pl = 0; pl < n; ++pl) {
        auto coefficients = get_coefficients(pl, xi);
        for (int i = 1; i < n - 1; ++i) {
            if (i == 1) {
                pli[pl][0] = coefficients[i - 1][0] * coefficients[i][0];
                pli[pl][1] = coefficients[i - 1][1] * coefficients[i][0] + coefficients[i][1] * coefficients[i - 1][0];
                pli[pl][2] = coefficients[i - 1][1] * coefficients[i][1];
            }
            else {
                std::vector<double> clone_pli = pli[pl];
                std::vector<double> zeros_pli(n, 0.0);
                for (int j = 0; j < n - 1; ++j) {
                    double product_1 = clone_pli[j] * coefficients[i][0];
                    double product_2 = clone_pli[j] * coefficients[i][1];
                    zeros_pli[j] += product_1;
                    zeros_pli[j + 1] += product_2;
                }
                pli[pl] = zeros_pli;
            }
        }
    }
    return pli;
}

std::vector<double> get_polynomial(std::vector<double>& xi, std::vector<double>& yi) {
    int n = xi.size();
    auto polynomial_l = get_polynomial_l(xi);
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            polynomial_l[i][j] *= yi[i];
        }
    }
    std::vector<double> L(n, 0.0);
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            L[i] += polynomial_l[j][i];
        }
    }
    return L;
}

double evaluate_lagrange_polynomial_at_point(const std::vector<double>& coefficients, double x) {
    double result = 0.0;
    int degree = coefficients.size() - 1;
    for (int i = 0; i <= degree; ++i) {
        result += coefficients[i] * std::pow(x, i);
    }
    return result;
}

std::vector<double> evaluate_lagrange_polynomial_at_points(const std::vector<double>& coefficients, const std::vector<double>& points) {
    std::vector<double> results;
    for (double x : points) {
        results.push_back(evaluate_lagrange_polynomial_at_point(coefficients, x));
    }
    return results;
}

std::vector<double> condense_arguments(const std::vector<double>& xi) {
    int new_size = xi.size() * 2 - 1;
    std::vector<double> results(new_size);
    double delta = (xi[1] - xi[0]) / 2;
    for (int i = 0; i < new_size; i += 2)
        results[i] = xi[i / 2];
    for (int i = 1; i < new_size; i += 2)
        results[i] = results[i - 1] + delta;

    return results;
}

int main() {
    std::vector<double> xi = generate_equidistant_nodes(-2, 3, 6);
    std::vector<double> yi = compute_function_values(xi, [](double x) {return -x+2.3; }); //{ -14.1014, -0.931596, 0, 0.931596, 14.1014 };
    std::vector<double> polynomial = get_polynomial(xi, yi);
    std::cout << "Interpolating Polynomial Coefficients:" << std::endl;
    for (double coef : polynomial) {
        std::cout << coef << " ";
    }

    std::cout << "\n----------------\n";

    std::cout << "Function values:" << std::endl;
    std::cout << "x: ";
    for (double x : xi) {
        std::cout << x << " ";
    }
    std::cout << std::endl;
    std::cout << "y: ";
    for (double y : yi) {
        std::cout << y << " ";
    }

    std::cout << std::endl;

    std::cout << "Function values computed with Lagrange polynom:" << std::endl;
   // xi = generate_equidistant_nodes(-1.5, 1.5, 10);
    xi = condense_arguments(xi);
    std::vector<double> y_polynomial = evaluate_lagrange_polynomial_at_points(polynomial, xi);
    std::cout << "x: ";
    for (double x : xi) {
        std::cout << x << " ";
    }
    std::cout << std::endl;
    std::cout << "y: ";
    for (double coef : y_polynomial) {
        std::cout << coef << " ";
    }

    std::cout << std::endl;
    return 0;
}
