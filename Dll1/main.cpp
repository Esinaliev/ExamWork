#include "pch.h"

extern "C" _declspec (dllexport) double task1(double num1, double num2, char symbol) {
	double result = 0;
	switch (symbol)
	{
	case '+':
		result = num1 + num2;
		break;

	case '-':
		result = num1 - num2;
		break;

	case '*':
		result = num1 * num2;
		break;

	case '/':
		result = num1 / num2;
		break;

	default:
		break;
	}
	return result;
}