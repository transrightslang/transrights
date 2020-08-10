foundation_kit_src = [
    'FoundationKit/Objects.c',
    'FoundationKit/Logger.c',
    'FoundationKit/String.c',
]
foundation_kit_h = [
    'FoundationKit/Foundation.h',
    'FoundationKit/Objects.h',
    'FoundationKit/Logger.h',
    'FoundationKit/CUtil.h',
    'FoundationKit/String.h',
]
library = shared_library('AcuiFoundationKit',
                         foundation_kit_src,
                         foundation_kit_h,
                         include_directories: 'FoundationKit',
                         install: true)

install_headers(foundation_kit_h, subdir: 'Acui/FoundationKit')
