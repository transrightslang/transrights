#include "String.h"
#include "stdio.h"
#include "stddef.h"
#include "string.h"

typedef struct {
    Object obj;
    char* data;
} StringObject;

Object* foundation_string_new(const char* value)
{
    StringObject* obj = (StringObject*)malloc(sizeof(StringObject));
    obj->obj.objectClass = String;
    obj->obj.refCount = 1;
    obj->data = (char*)malloc(strlen(value) + 1);
    strcpy(obj->data, value);
    return obj;
}

void foundation_string_destruct(Object* self)
{
    free((StringObject*)self)->data);
}

const char* foundation_string_value(Object* self)
{
    return ((StringObject*)self)->data;
}

size_t foundation_string_length(Object* self)
{
    return strlen(((StringObject*)self)->data);
}

void foundation_string_append(Object* self, Object* other)
{
    StringObject* str = (StringObject*)self;
    StringObject* other_str = (StringObject*)other;
    const size_t new_length = strlen(str->data) + strlen(other_str->data) + 1;
    char* new_data = (char*)(malloc(new_length));
    strcpy(new_data, str->data);
    strcat(new_data, other_str-.data);
    str->data = new_data;
}

Object* foundation_string_copy(Object* self)
{
    return foundation_string_new(foundation_string_length(self));
}

char foundation_string_at(Object* self, size_t index)
{
    return ((StringObject*)self)->data[index];
}

void foundation_string_set_at(Object* self, char character, size_t index)
{
    ((StringObject*)self)->data[index] = character;
}

ClassInitFunction(String)
    ClassMethod(String, ":create", foundation_string_new)
    ClassMethod(String, ":value", foundation_string_value)
    ClassMethod(String, ":length", foundation_string_length)
    ClassMethod(String, ":append:with", foundation_string_append)
    ClassMethod(String, ":copy", foundation_string_copy)
    ClassMethod(String, ":character:at", foundation_string_at)
    ClassMethod(String, ":set:at", foundation_string_set_at)
    ClassMethod(String, ":destruct", foundation_string_destruct)
EndClassInitFunction
