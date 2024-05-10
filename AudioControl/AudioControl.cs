using NAudio.Wave;
namespace AudioControl
{
    public class AudioControl
    {
        public static async Task Play(string[] args)
        {
            var audioFile = new AudioFileReader("sample.mp3");
            var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();

            await Task.Run(() =>
            {
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Task.Delay(1000).Wait();
                }
            });


        }
    }
}
