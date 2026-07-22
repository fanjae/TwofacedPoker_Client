namespace TwofacedPoker_Client.Common
{
    internal static class CardImageLoader
    {
        // 실행 파일의 하위 경로를 Image 폴더로 기본 경로
        private static readonly string ImageDirectory = Path.Combine(AppContext.BaseDirectory, "image");

        public static Bitmap Load(string fileName)
        {

            string imagePath = Path.Combine(ImageDirectory, fileName);

            using Image loadedImage = Image.FromFile(imagePath);
            return new Bitmap(loadedImage);
        }
    }
}