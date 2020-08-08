#include <stdarg.h>
#include <string.h>
#include <stdio.h>

#include "Foundation.h"

void acui_logger(Object* self, void* data)
{
    printf((const char*)data);
}

void acui_initRuntime()
{
    Logger = (Object*)malloc(sizeof(Object));

    Class* loggerClass = (Class*)malloc(sizeof(Class));
    loggerClass->name = "Logger";

    Method loggerPrint;
    loggerPrint.signature = ":print";
    loggerPrint.functionPointer = acui_logger;

    loggerClass->methods[0] = loggerPrint;

    Logger->objectClass = loggerClass;
}

void* acui_sendMessage(Object* self, const char* sel, size_t count, ...)
{
    void* items[count];
    size_t maxCount = count;

    va_list args;
    va_start(args, count);
    while (count > 0) {
        void* arg = va_arg(args, void*);
        items[maxCount - count] = arg;
        --count;
    }
    va_end(args);

    for (Method* method = self->objectClass->methods; method != NULL; method++) {
        if (strcmp(sel, method->signature) == 0) {
            switch (maxCount) {
            case 0: return ((void* (*)(Object*))method->functionPointer)(self);
            case 1: return ((void* (*)(Object*, void*))method->functionPointer)(self, items[0]);
            case 2: return ((void* (*)(Object*, void*, void*))method->functionPointer)(self, items[0], items[1]);
            case 3: return ((void* (*)(Object*, void*, void*, void*))method->functionPointer)(self, items[0], items[1], items[2]);
            }
        }
    }

    return NULL;
}