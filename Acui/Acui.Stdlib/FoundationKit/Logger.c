#include "Foundation.h"
#include "Logger.h"

#include <stdio.h>

void foundation_logger_print(Object* self, void* data)
{
    printf("%s\n", (const char*)data);
}

Object* foundation_logger_new() {
    Constructor(Logger)
}

ClassInitFunction(Logger)
    ClassMethod(Logger, 0, ":create", foundation_logger_new)
    ClassMethod(Logger, 1, ":print", foundation_logger_print)
EndClassInitFunction
