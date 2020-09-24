#include "stdarg.h"
#include "stdlib.h"
#include "string.h"
#include "stdio.h"

#include "Objects.h"

void acui_refUp(Object* obj)
{
    if (obj == NULL) {
        return;
    }
    obj->refCount++;
}

void acui_refDown(Object** obj)
{
    if (*obj == NULL) {
        return;
    }

    (*obj)->refCount--;
    if ((*obj)->refCount <= 0) {
        acui_sendMessage((*obj), ":destruct", 0);
        free((*obj));
    }
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

    for (MethodList* methodList = self->isClass ? ((Class*)self)->methods : self->objectClass->methods; methodList != NULL; methodList = methodList->next) {
        Method* method = &methodList->method;
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

MethodList* acui_methodListPrepend(MethodList* methodList, Method method)
{
    MethodList* retList = (MethodList*)malloc(sizeof(MethodList));
    retList->method = method;
    retList->next = methodList;
    return retList;
}