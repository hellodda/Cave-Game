namespace Cave_Game.Core.level
{
    public class LevelRenderer
    {
        private static readonly int CHUNK_SIZE = 8;

        private readonly Chunk[] chunks;

        private readonly int chunkAmountX;
        private readonly int chunkAmountY;
        private readonly int chunkAmountZ;

        public LevelRenderer(Level level)
        {
          
            chunkAmountX = level.Width / CHUNK_SIZE;
            chunkAmountY = level.Depth / CHUNK_SIZE;
            chunkAmountZ = level.Height / CHUNK_SIZE;

            chunks = new Chunk[chunkAmountX * chunkAmountY * chunkAmountZ];


            for (int x = 0; x < chunkAmountX; x++)
            {
                for (int y = 0; y < chunkAmountY; y++)
                {
                    for (int z = 0; z < chunkAmountZ; z++)
                    {
                        int minChunkX = x * CHUNK_SIZE;
                        int minChunkY = y * CHUNK_SIZE;
                        int minChunkZ = z * CHUNK_SIZE;

                        int maxChunkX = (x + 1) * CHUNK_SIZE;
                        int maxChunkY = (y + 1) * CHUNK_SIZE;
                        int maxChunkZ = (z + 1) * CHUNK_SIZE;

                        maxChunkX = Math.Min(level.Width, maxChunkX);
                        maxChunkY = Math.Min(level.Depth, maxChunkY);
                        maxChunkZ = Math.Min(level.Height, maxChunkZ);

                        Chunk chunk = new Chunk(level, minChunkX, minChunkY, minChunkZ, maxChunkX, maxChunkY, maxChunkZ);
                        chunks[(x + y * chunkAmountX) * chunkAmountZ + z] = chunk;
                    }
                }
            }
        }
        public void Render(int layer)
        {
            Frustum frustum = Frustum.GetFrustum();
            Chunk.RebuiltThisFrame = 0;

            foreach (Chunk chunk in chunks)
            {
                if (frustum.CubeInFrustum(chunk.BoundingBox))
                {
                    chunk.Render(layer);
                }
            }
        }

        public void SetDirty(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            minX /= CHUNK_SIZE;
            minY /= CHUNK_SIZE;
            minZ /= CHUNK_SIZE;
            maxX /= CHUNK_SIZE;
            maxY /= CHUNK_SIZE;
            maxZ /= CHUNK_SIZE;

            minX = Math.Max(minX, 0);
            minY = Math.Max(minY, 0);
            minZ = Math.Max(minZ, 0);

            maxX = Math.Min(maxX, chunkAmountX - 1);
            maxY = Math.Min(maxY, chunkAmountY - 1);
            maxZ = Math.Min(maxZ, chunkAmountZ - 1);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        Chunk chunk = chunks[(x + y * chunkAmountX) * chunkAmountZ + z];
                        chunk.SetDirty();
                    }
                }
            }
        }
    }

}
