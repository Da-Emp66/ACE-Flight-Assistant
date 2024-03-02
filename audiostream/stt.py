import whisper

model = whisper.load_model("small.en")
result = model.transcribe("./tests/stt-audio.wav")
print(result["text"])