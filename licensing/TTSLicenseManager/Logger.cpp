#include "Logger.h"

void Logger::log(string state) {
	WriteFile(hOut, state.c_str(), state.size(), &dwSize, NULL);  
}

Logger::Logger(string task) {
	this->task = task;
	DWORD dwSize;
	hOut = CreateFile(task.c_str(), GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
}

Logger::~Logger() {
	CloseHandle(hOut);
}