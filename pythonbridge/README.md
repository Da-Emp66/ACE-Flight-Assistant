# Terminal command to compile and run

g++ -shared -o add.so -fPIC start.cpp

g++  : compiler

-shared : calls the shared library needed to compile python and c++ code

-o add.so : name of the sharedlibrary file that will be outputted after compilation

-fPIC : "position independent code" flag. Needed for compiling shared libraries

start.cpp : file that is being compiled

then...

python3 start.py
