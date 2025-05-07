namespace Cave_Game.Core.level
{
    public class Tile
    {
        public static Tile Grass { get; set; } = new Tile(0);
        public static Tile Rock { get; set; } = new Tile(1);

        private readonly int textureId;

        public Tile(int textureId)
        {
            this.textureId = textureId;
        }

        public void Render(Tessellator tessellator, Level level, int layer, int x, int y, int z)
        {
            float minU = textureId / 16.0F;
            float maxU = minU + 16 / 256F;
            float minV = 0.0F;
            float maxV = minV + 16 / 256F;

            float shadeX = 0.6f;
            float shadeY = 1.0f;
            float shadeZ = 0.8f;

            float minX = x + 0.0F;
            float maxX = x + 1.0F;
            float minY = y + 0.0F;
            float maxY = y + 1.0F;
            float minZ = z + 0.0F;
            float maxZ = z + 1.0F;

            if (!level.IsSolidTile(x, y - 1, z))
            {
                float brightness = level.GetBrightness(x, y - 1, z) * shadeY;

                if (layer == 1 ^ brightness == shadeY)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(minX, minY, maxZ);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(minX, minY, minZ);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(maxX, minY, minZ);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(maxX, minY, maxZ);
                }
            }

            if (!level.IsSolidTile(x, y + 1, z))
            {
                float brightness = level.GetBrightness(x, y + 1, z) * shadeY;

                if (layer == 1 ^ brightness == shadeY)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(maxX, maxY, maxZ);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(maxX, maxY, minZ);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(minX, maxY, minZ);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(minX, maxY, maxZ);
                }
            }

            if (!level.IsSolidTile(x, y, z - 1))
            {
                float brightness = level.GetBrightness(x, y, z - 1) * shadeZ;

                if (layer == 1 ^ brightness == shadeZ)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(minX, maxY, minZ);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(maxX, maxY, minZ);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(maxX, minY, minZ);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(minX, minY, minZ);
                }
            }
            if (!level.IsSolidTile(x, y, z + 1))
            {
                float brightness = level.GetBrightness(x, y, z + 1) * shadeZ;

                if (layer == 1 ^ brightness == shadeZ)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(minX, maxY, maxZ);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(minX, minY, maxZ);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(maxX, minY, maxZ);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(maxX, maxY, maxZ);
                }
            }

            if (!level.IsSolidTile(x - 1, y, z))
            {
                float brightness = level.GetBrightness(x - 1, y, z) * shadeX;

                if (layer == 1 ^ brightness == shadeX)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(minX, maxY, maxZ);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(minX, maxY, minZ);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(minX, minY, minZ);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(minX, minY, maxZ);
                }
            }
            if (!level.IsSolidTile(x + 1, y, z))
            {
                float brightness = level.GetBrightness(x + 1, y, z) * shadeX;

                if (layer == 1 ^ brightness == shadeX)
                {
                    tessellator.Color(brightness, brightness, brightness);
                    tessellator.Texture(minU, maxV);
                    tessellator.Vertex(maxX, minY, maxZ);
                    tessellator.Texture(maxU, maxV);
                    tessellator.Vertex(maxX, minY, minZ);
                    tessellator.Texture(maxU, minV);
                    tessellator.Vertex(maxX, maxY, minZ);
                    tessellator.Texture(minU, minV);
                    tessellator.Vertex(maxX, maxY, maxZ);
                }
            }
        }
    }

}
