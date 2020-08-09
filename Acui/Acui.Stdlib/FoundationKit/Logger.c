#include "Foundation.h"
#include "Logger.h"

#include <stdio.h>

void foundation_logger_print(Object* self, void* data)
{
    printf("%s\n", (const char*)data);
}

void foundation_logger_print_eol(Object* self, void* data, void* data2)
{
    printf("%s%s", (const char*)data, (const char*)data2);
}

Object* foundation_logger_new() {
    Constructor(Logger)
}

ClassInitFunction(Logger)
    ClassMethod(Logger, 0, ":create", foundation_logger_new)
    ClassMethod(Logger, 1, ":print", foundation_logger_print)
    ClassMethod(Logger, 2, ":print:withEndOfLine", foundation_logger_print)
EndClassInitFunction
