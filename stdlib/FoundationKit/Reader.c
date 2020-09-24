#include "Foundation.h"
#include "Reader.h"

#include <stdio.h>

Object* foundation_reader_new() {
    Constructor(Reader)
    return obj;
}

Object* foundation_reader_read(Object* self, const size_t count)
{
    char buffer[count + 1];
    fgets(buffer, count, stdin)
    return foundation_string_new(buffer);
}

ClassInitFunction(Reader)
    ClassMethod(Reader, ":new", foundation_reader_new)
EndClassInitFunction
