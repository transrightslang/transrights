#include "Foundation.h"
#include "Logger.h"

#include <stdio.h>

void foundation_logger_print(Object* self, Object* data)
{
    printf("%s\n", (const char*)(acui_sendMessage(data, ":value", 0)));
}

void foundation_logger_print_eol(Object* self, Object* data, Object* data2)
{
    printf("%s%s", (const char*)(acui_sendMessage(data, ":value", 0)), (const char*)(acui_sendMessage(data2, ":value", 0)));
}

Object* foundation_logger_new() {
    Constructor(Logger)
    printf("constructing logger %p\n", obj);
    return obj;
}

ClassInitFunction(Logger)
    ClassMethod(Logger, 0, ":create", foundation_logger_new)
    ClassMethod(Logger, 1, ":print", foundation_logger_print)
    ClassMethod(Logger, 2, ":print:withEndOfLine", foundation_logger_print)
EndClassInitFunction
