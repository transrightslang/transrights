#pragma once

#include <stddef.h>
#include <stdlib.h>
#include <stdbool.h>

typedef struct Method {
    const char* signature;
    void* functionPointer;
} Method;

typedef struct MethodList MethodList;

typedef struct MethodList {
    Method method;
    MethodList* next;
} MethodList;

typedef struct Class Class;

typedef struct {
    Class* objectClass;
    bool isClass;
    int refCount;
} Object;

typedef struct Class {
    Object metaObject;
    const char* name;
    MethodList* methods;
} Class;

void* acui_sendMessage(Object* self, const char* sel, size_t count, ...);
void acui_refUp(Object* obj);
void acui_refDown(Object** obj);
MethodList* acui_methodListPrepend(MethodList* methodList, Method method);