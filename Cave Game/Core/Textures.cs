using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cave_Game.Core
{
    public static class Textures
    {
        private static int lastId = int.MinValue;

        public static int LoadTexture(string filePath, TextureMinFilter filter = TextureMinFilter.Nearest)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Texture file not found: {filePath}");

            int id = GL.GenTexture();
            Bind(id);

            using (var image = new Bitmap(filePath))
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); 

                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)filter);
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    level: 0,
                    internalformat: PixelInternalFormat.Rgba,
                    width: data.Width,
                    height: data.Height,
                    border: 0,
                    format: OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    type: PixelType.UnsignedByte,
                    pixels: data.Scan0
                );

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                image.UnlockBits(data);
            }

            return id;
        }

        public static void Bind(int id)
        {
            if (id != lastId)
            {
                GL.BindTexture(TextureTarget.Texture2D, id);
                lastId = id;
            }
        }
    }
}
