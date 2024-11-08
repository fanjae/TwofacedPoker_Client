using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TwofacedPoker_Client
{
    public class AudioPlayer
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;

        public void PlaySound(string path)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string soundFilePath = Path.Combine(currentDir, "sound", path);

            try
            {

                DisposeWave();

                waveOutDevice = new WaveOutEvent();
                audioFileReader = new AudioFileReader(soundFilePath);


                waveOutDevice.PlaybackStopped += OnPlaybackStopped;

                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
            }
            catch (Exception ex)
            {

                MessageBox.Show("오디오 재생 중 오류 발생: " + ex.Message);
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {

            waveOutDevice.PlaybackStopped -= OnPlaybackStopped;


            DisposeWave();
        }

        private void DisposeWave()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
        }
    }
}
