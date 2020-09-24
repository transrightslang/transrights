#include "Foundation.h"
#include "Logger.h"

#include <stdio.h>

void foundation_logger_print(Object* self, Object* data)
{
    printf("%s\n", (const char*)(acui_sendMessage(data, ":value", 0)));
    acui_refDown(&data);
}

void foundation_logger_print_eol(Object* self, Object* data, Object* data2)
{
    printf("%s%s", (const char*)(acui_sendMessage(data, ":value", 0)), (const char*)(acui_sendMessage(data2, ":value", 0)));
    acui_refDown(&data);
    acui_refDown(&data2);
}

Object* foundation_logger_new() {
    Constructor(Logger)
    return obj;
}

ClassInitFunction(Logger)
    ClassMethod(Logger, ":new", foundation_logger_new)
    ClassMethod(Logger, ":print", foundation_logger_print)
    ClassMethod(Logger, ":print:withEndOfLine", foundation_logger_print)
EndClassInitFunction
