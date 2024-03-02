from typing import Optional
from mimic3_tts import Mimic3TextToSpeechSystem, Mimic3Settings

class TextToSpeech:
    def __init__(self):
        """Initialization"""
        self.configuration = Mimic3Settings()
        self.speech_synthesizer = Mimic3TextToSpeechSystem(self.configuration)

    def __call__(self, speech: str, out: Optional[str] = './tests/spoken-audio.wav', voice_key: Optional[str] = None, speaker_id: Optional[int] = None):
        """Inference"""
        if voice_key:
            self.speech_synthesizer.voice = voice_key
            self.speech_synthesizer.preload_voice(voice_key)
        if speaker_id:
            self.speech_synthesizer.settings.speaker = speaker_id
        wav_bytes = self.speech_synthesizer.text_to_wav(speech)
        with open(out, 'wb') as wav_file:
            wav_file.write(wav_bytes)
            wav_file.close()

    def available_voices(self):
        """Returns list of (voice_key, speaker_id_range)"""
        return list(
            filter(
                lambda x: x != None,
                [
                    (voice.key, f"0-{len(voice.speakers) - 1}")
                    if voice.speakers is not None and 'en_' in voice.language and len(voice.speakers) else None
                    for voice in self.speech_synthesizer.get_voices()
                ]
            )
        )


def main():
    tts = TextToSpeech()
    print("Voices:")
    print(tts.available_voices())
    tts("Hooray! Text-to-Speech is working in Mimic!", voice_key='en_US/vctk_low', speaker_id=2, out="./tests/spoken-audio.wav")


if __name__ == "__main__":
    main()
