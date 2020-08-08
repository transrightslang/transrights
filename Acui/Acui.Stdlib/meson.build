foundation_kit_src = [
    'FoundationKit/Foundation.c',
]
foundation_kit_h = [
    'FoundationKit/Foundation.h',
]
library = shared_library('AcuiFoundationKit',
                         foundation_kit_src,
                         include_directories: 'FoundationKit',
                         install: true)

install_headers(foundation_kit_h, subdir: 'Acui/FoundationKit')
