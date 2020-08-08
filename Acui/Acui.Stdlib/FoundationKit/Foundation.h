#include <stddef.h>
#include <stdlib.h>

typedef struct {
    const char* signature;
    void* functionPointer;
} Method;

typedef struct {
    const char* name;
    Method methods[0];
} Class;

typedef struct {
    Class* objectClass;
} Object;

void acui_initRuntime();
void* acui_sendMessage(Object* self, const char* sel, size_t count, ...);

Object* Logger;