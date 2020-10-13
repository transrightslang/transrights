#include "stdarg.h"
#include "stdlib.h"
#include "string.h"
#include "stdio.h"

#include "Objects.h"
#include "CUtil.h"

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

Method* lookupMethod(MethodList* methodList, const char* sel, size_t arity)
{
    for (MethodList* list = methodList; list != NULL; list = list->next) {
        Method* item = &list->data;

        if (strcmp(sel, item->signature) == 0 && item->arity == arity) {
            return item;
        }
    }
    return NULL;
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

    Method* foundMethod = lookupMethod(self->isClass ? ((Class*)self)->methods : self->objectClass->methods, sel, maxCount);
    Class* parentClass = self->isClass ? NULL : self->objectClass->inherits;

    while (foundMethod == NULL && parentClass != NULL) {
        foundMethod = lookupMethod(parentClass->methods, sel, maxCount);
        parentClass = parentClass->inherits;
    }

    if (foundMethod == NULL) {
        if (strcmp(sel, ":unhandledMessage") == 0) {
            printf("Fatal Runtime Error: Message unhandled");
            abort();
        } else if (strcmp(sel, ":destruct") == 0) {
            return NULL;
        }
        return acui_sendMessage(self, ":unhandledMessage", 1, sel);
    }

    switch (foundMethod->arity) {
    case  0: return ((void* (*)(Object*))foundMethod->functionPointer)(self);
    case  1: return ((void* (*)(Object*, void*))foundMethod->functionPointer)(self, items[0]);
    case  2: return ((void* (*)(Object*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1]);
    case  3: return ((void* (*)(Object*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2]);
    case  4: return ((void* (*)(Object*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3]);
    case  5: return ((void* (*)(Object*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4]);
    case  6: return ((void* (*)(Object*, void*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4], items[5]);
    case  7: return ((void* (*)(Object*, void*, void*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4], items[5], items[6]);
    case  8: return ((void* (*)(Object*, void*, void*, void*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4], items[5], items[6], items[7]);
    case  9: return ((void* (*)(Object*, void*, void*, void*, void*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4], items[5], items[6], items[7], items[8]);
    case 10: return ((void* (*)(Object*, void*, void*, void*, void*, void*, void*, void*, void*, void*, void*))foundMethod->functionPointer)(self, items[0], items[1], items[2], items[3], items[4], items[5], items[6], items[7], items[8], items[9]);
    }

    return NULL;
}

MethodList* acui_methodListPrepend(MethodList* methodList, Method method)
{
    MethodList* retList = (MethodList*)malloc(sizeof(MethodList));
    retList->data = method;
    retList->next = methodList;
    return retList;
}