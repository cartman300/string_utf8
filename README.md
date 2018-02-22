# string_utf8

StringUTF8, StringASCII classes and StringPtr struct mainly for use with unmanaged code interop.
Unmanaged functions which take a string should instead take a StringPtr argument, and all supplied strings will properly
implicitly map or convert to the proper encoding before being passed on to the function.

![alt text](https://raw.githubusercontent.com/cartman300/string_utf8/master/screenshots/a.png "A")