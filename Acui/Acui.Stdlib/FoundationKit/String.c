#include "String.h"
#include "stdio.h"

typedef struct {
    Object obj;
    const char* data;
} StringObject;

Object* foundation_string_new(const char* value)
{
    StringObject* obj = (StringObject*)malloc(sizeof(StringObject));
    obj->obj.objectClass = String;
    obj->obj.refCount = 1;
    obj->data = value;
    return obj;
}

const char* foundation_string_value(Object* self)
{
    return ((StringObject*)self)->data;
}

ClassInitFunction(String)
    ClassMethod(String, 0, ":create", foundation_string_new)
    ClassMethod(String, 1, ":value", foundation_string_value)
EndClassInitFunction