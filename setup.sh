# Whisper setup
sudo apt update && sudo apt install ffmpeg
pip3 install -U openai-whisper
pip3 install setuptools-rust
# Test install
whisper ./tests/stt-audio.wav --model medium

# Mimic3 setup
sudo apt-get update && sudo apt-get install libespeak-ng1
pip3 install --upgrade pip
pip3 install mycroft-mimic3-tts[all]
# Test install
mimic3 --voice en_US/vctk_low#p236 "Hooray! The text-to-speech model is speaking" > tests/tts-audio.wav

# Copy the MACE plugin into a directory MACE will recognize
cp ./mace/SDTest.cs "/mnt/c/Users/Public/Public Documents/MACE/CodeScripts/SDTest.cs"