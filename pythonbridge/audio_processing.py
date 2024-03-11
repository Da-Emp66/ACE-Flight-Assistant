import ctypes
import numpy as np

# Load the shared library
audio_lib = ctypes.CDLL('./audio_processing.so')  # Use '.dll' extension on Windows

# Define argument types for the function
audio_lib.processAudio.argtypes = [ctypes.POINTER(ctypes.c_float), ctypes.c_int, ctypes.c_char_p]
audio_lib.processAudio.restype = None

def process_audio(audio_data, sample_rate, filename):
    # Convert audio data to ctypes array
    audio_data_ptr = (ctypes.c_float * len(audio_data))(*audio_data)
    # Call the C++ function
    audio_lib.processAudio(audio_data_ptr, sample_rate, filename)

# Example usage
audio_data = np.random.rand(44100)  # Generate random audio data
sample_rate = 44100
filename = 'output.wav'
process_audio(audio_data, sample_rate, filename)
