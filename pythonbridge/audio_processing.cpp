#include <iostream>
#include <fstream>
#include <vector>

void processAudio(const std::vector<float>& audioData, int sampleRate, const char* filename) {
    // Write audio data to a .wav file
    std::ofstream outfile(filename, std::ios::binary);
    if (!outfile) {
        std::cerr << "Error: Failed to open output file." << std::endl;
        return;
    }

    // Write WAV file header
    // Assuming 16-bit PCM format
    int numChannels = 1; // Mono
    int bitsPerSample = 16;
    int byteRate = sampleRate * numChannels * bitsPerSample / 8;
    int blockAlign = numChannels * bitsPerSample / 8;
    int dataSize = audioData.size() * bitsPerSample / 8;
    int fileSize = dataSize + 36;

    outfile << "RIFF" << static_cast<char>(fileSize & 0xFF) << static_cast<char>((fileSize >> 8) & 0xFF)
            << static_cast<char>((fileSize >> 16) & 0xFF) << static_cast<char>((fileSize >> 24) & 0xFF)
            << "WAVEfmt " << static_cast<char>(16) << static_cast<char>(0) << static_cast<char>(0) << static_cast<char>(0)
            << static_cast<char>(1) << static_cast<char>(0) << static_cast<char>(numChannels) << static_cast<char>(0)
            << static_cast<char>(sampleRate & 0xFF) << static_cast<char>((sampleRate >> 8) & 0xFF)
            << static_cast<char>((sampleRate >> 16) & 0xFF) << static_cast<char>((sampleRate >> 24) & 0xFF)
            << static_cast<char>(byteRate & 0xFF) << static_cast<char>((byteRate >> 8) & 0xFF)
            << static_cast<char>((byteRate >> 16) & 0xFF) << static_cast<char>((byteRate >> 24) & 0xFF)
            << static_cast<char>(blockAlign) << static_cast<char>(0) << static_cast<char>(bitsPerSample) << static_cast<char>(0)
            << "data" << static_cast<char>(dataSize & 0xFF) << static_cast<char>((dataSize >> 8) & 0xFF)
            << static_cast<char>((dataSize >> 16) & 0xFF) << static_cast<char>((dataSize >> 24) & 0xFF);

    // Write audio data
    for (float sample : audioData) {
        int16_t pcmSample = static_cast<int16_t>(sample * 32767); // Convert to signed 16-bit PCM
        outfile.write(reinterpret_cast<const char*>(&pcmSample), sizeof(int16_t));
    }
}
