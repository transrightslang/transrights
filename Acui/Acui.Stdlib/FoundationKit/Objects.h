#pragma once

#include <stddef.h>
#include <stdlib.h>
#include <stdbool.h>

typedef struct {
    const char* signature;
    void* functionPointer;
} Method;

typedef struct Class Class;

typedef struct {
    Class* objectClass;
    bool isClass;
} Object;

typedef struct Class {
    Object metaObject;
    const char* name;
    Method methods[5];
} Class;

void* acui_sendMessage(Object* self, const char* sel, size_t count, ...);
