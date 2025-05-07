using OpenTK.Graphics.OpenGL;

namespace Cave_Game.Core.level
{
    public class Tessellator
    {
        private const int MAX_VERTICES = 100000;

        private readonly FloatBuffer vertexBuffer = new FloatBuffer(MAX_VERTICES * 3);
        private readonly FloatBuffer textureBuffer = new FloatBuffer(MAX_VERTICES * 2);
        private readonly FloatBuffer colorBuffer = new FloatBuffer(MAX_VERTICES * 3);

        private int vertexCount = 0;

        private bool hasTexture = false;
        private float textureU, textureV;

        private bool hasColor = false;
        private float red, green, blue;

        public void Init()
        {
            Clear();
        }

        public void Vertex(float x, float y, float z)
        {
            vertexBuffer.Put(x).Put(y).Put(z);

            if (hasTexture)
                textureBuffer.Put(textureU).Put(textureV);

            if (hasColor)
                colorBuffer.Put(red).Put(green).Put(blue);

            vertexCount++;
            if (vertexCount >= MAX_VERTICES)
                Flush();
        }

        public void Texture(float u, float v)
        {
            hasTexture = true;
            textureU = u;
            textureV = v;
        }

        public void Color(float r, float g, float b)
        {
            hasColor = true;
            red = r;
            green = g;
            blue = b;
        }

        public void Flush()
        {
            vertexBuffer.Flip();
            textureBuffer.Flip();
            colorBuffer.Flip();

            var verts = vertexBuffer.ToArray();
            var texs = textureBuffer.ToArray();
            var cols = colorBuffer.ToArray();

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, verts);

            if (hasTexture)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, texs);
            }
            if (hasColor)
            {
                GL.EnableClientState(ArrayCap.ColorArray);
                GL.ColorPointer(3, ColorPointerType.Float, 0, cols);
            }

            GL.DrawArrays(PrimitiveType.Quads, 0, vertexCount);

            GL.DisableClientState(ArrayCap.VertexArray);
            if (hasTexture) GL.DisableClientState(ArrayCap.TextureCoordArray);
            if (hasColor) GL.DisableClientState(ArrayCap.ColorArray);

            Clear();
        }

        private void Clear()
        {
            vertexBuffer.Clear();
            textureBuffer.Clear();
            colorBuffer.Clear();
            vertexCount = 0;
            hasTexture = false;
            hasColor = false;
        }
    }
}