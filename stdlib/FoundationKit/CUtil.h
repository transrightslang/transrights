#pragma once

#define DeclareClassObject(class) Class* class;
#define ClassInitFunction(class) __attribute__((constructor))\
void init_ ## class () {\
    class = (Class*)malloc(sizeof(Class));\
    class->metaObject.isClass = true;\
    class->name = #class;\
    class->methods = NULL;

#define ClassMethod(class, selector, func) { Method method = {\
        .signature = selector,\
        .functionPointer = func\
    }; class->methods = acui_methodListPrepend(class->methods, method); }
#define Constructor(class) Object* obj = (Object*)malloc(sizeof(Object));\
    obj->objectClass = class;\
    obj->refCount = 1;

#define concatTwo(a, b) a b
#define concatThree(a, b, c) a ## b ## c

#define CObject(var) Object* var __attribute__((__cleanup__(acui_refDown)))

#define PObject(param) CObject(concatThree(obj, param, __LINE__)) = param; (void)concatThree(obj, param, __LINE__);

#define EndClassInitFunction }