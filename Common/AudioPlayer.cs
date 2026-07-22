using NAudio.Wave;

namespace TwofacedPoker_Client.Common
{
    public class AudioPlayer : IDisposable
    {
        private IWavePlayer? waveOutDevice;
        private AudioFileReader? audioFileReader;

        public void PlaySound(string path)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string soundFilePath = Path.Combine(currentDir, "sound", path);

            try
            {
                // 새로운 사운드를 재성하기 전 기존 리소스를 초기화
                DisposeWave();

                waveOutDevice = new WaveOutEvent();
                audioFileReader = new AudioFileReader(soundFilePath);

                // 재생이 끝났을 때 자원을 해제하도록 이벤트 구독
                waveOutDevice.PlaybackStopped += OnPlaybackStopped;

                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
            }
            catch
            {
                DisposeWave();
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            // 이벤트 구독 해제 후 자원 정리
            if (waveOutDevice != null)
            {
                waveOutDevice.PlaybackStopped -= OnPlaybackStopped;
            }


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

        public void Dispose()
        {
            DisposeWave();
            GC.SuppressFinalize(this);
        }
    }
}
