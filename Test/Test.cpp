#define EXPORT extern "C" __declspec(dllexport)

#include <cstdlib>
#include <cstdio>
#include <locale.h>

EXPORT void Init() {
	system("chcp 65001 > nul");
}

EXPORT const char* PrintString(const char* Msg) {
	printf("%s\n", Msg);

	return Msg;
}