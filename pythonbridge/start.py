import ctypes

add_lib = ctypes.CDLL('./add.so') 


add_lib.add.argtypes = [ctypes.c_int, ctypes.c_int]
add_lib.add.restype = ctypes.c_int


result = add_lib.add(10, 20)
print("Result from C++:", result)
