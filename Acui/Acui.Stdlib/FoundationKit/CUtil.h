#pragma once

#define DeclareClassObject(class) Class* class;
#define ClassInitFunction(class) __attribute__((constructor))\
void init_ ## class () {\
    class = (Class*)malloc(sizeof(Class));\
    class->metaObject.isClass = true;\
    class->name = #class;
#define ClassMethod(class, idx, selector, func) Method method ## idx;\
    method ## idx .signature = selector;\
    method ## idx .functionPointer = func;\
    class->methods[idx] = method ## idx;
#define Constructor(class) Object* obj = (Object*)malloc(sizeof(Object));\
    obj->objectClass = class;\
    obj->refCount = 1;\

#define EndClassInitFunction }