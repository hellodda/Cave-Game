using Cave_Game.Core.physics;
using System.IO.Compression;

namespace Cave_Game.Core.level
{
    public class Level
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int Depth;

        private readonly byte[] blocks;
        private readonly int[] lightDepths;

        public Level(int width, int height, int depth)
        {
            Random rnd = new();
            Width = width;
            Height = height;
            Depth = depth;

            blocks = new byte[width * height * depth];
            lightDepths = new int[width * height];

            bool exists = File.Exists("level.dat");
            if (exists)
            {
                Load();
            }
            else
            { 
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < depth; y++)
                        for (int z = 0; z < height; z++)
                            blocks[(y * height + z) * width + x] = 1;

                
                for (int i = 0; i < 10000; i++)
                {
                    int caveSize = rnd.Next(1, 8);       
                    int caveX = rnd.Next(width);
                    int caveY = rnd.Next(depth);
                    int caveZ = rnd.Next(height);

                    for (int radius = 0; radius < caveSize; radius++)
                        for (int s = 0; s < 1000; s++)
                        {
                            int offX = rnd.Next(-radius, radius + 1);
                            int offY = rnd.Next(-radius, radius + 1);
                            int offZ = rnd.Next(-radius, radius + 1);
                            if (offX * offX + offY * offY + offZ * offZ > radius * radius) continue;

                            int tx = caveX + offX, ty = caveY + offY, tz = caveZ + offZ;
                            if (tx > 0 && ty > 0 && tz > 0 && tx < width - 1 && ty < depth && tz < height - 1)
                                blocks[(ty * height + tz) * width + tx] = 0;
                        }
                }

                calcLightDepths(0, 0, width, height);

                Save();
            }
        }
        public void Load()
        {
            try
            {
                string path = "level.dat";
                if (!File.Exists(path))
                    return;

                using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
                {
                    int offset = 0;
                    while (offset < blocks.Length)
                    {
                        int read = gz.Read(blocks, offset, blocks.Length - offset);
                        if (read <= 0) break;
                        offset += read;
                    }
                }
                calcLightDepths(0, 0, Width, Height);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading level from 'level.dat': {e.Message}");
            }
        }

        public void Save()
        {
            try
            {
                string path = "level.dat";

                using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
                {
                    gz.Write(blocks, 0, blocks.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving level to 'level.dat': {e.Message}");
            }
        }
        private void calcLightDepths(int minX, int minZ, int maxX, int maxZ)
        {
            for (int x = minX; x < minX + maxX; x++)
            {
                for (int z = minZ; z < minZ + maxZ; z++)
                {
                    int prevDepth = lightDepths[x + z * Width];
                    int depth = Depth - 1;
                    while (depth > 0 && !IsLightBlocker(x, depth, z))
                    {
                        depth--;
                    }
                    lightDepths[x + z * Width] = depth;
                }
            }
        }
        public bool IsTile(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Depth || z >= Height)
            {
                return false;
            }

            int index = (y * Height + z) * Width + x;

            return blocks[index] != 0;
        }
        public bool IsSolidTile(int x, int y, int z)
        {
            return IsTile(x, y, z);
        }
        public bool IsLightBlocker(int x, int y, int z)
        {
            return IsSolidTile(x, y, z);
        }
        public float GetBrightness(int x, int y, int z)
        {
            float dark = 0.8F;
            float light = 1.0F;

            if (x < 0 || y < 0 || z < 0 || x >= Width || y >= Depth || z >= Height)
            {
                return light;
            }

            if (y < lightDepths[x + z * Width])
            {
                return dark;
            }

            return light;
        }
        public IEnumerable<AxisAlignedBoundingBox> GetCubes(AxisAlignedBoundingBox boundingBox)
        {
            List<AxisAlignedBoundingBox> boundingBoxList = new();

            int minX = (int)(Math.Floor(boundingBox.minX) - 1);
            int maxX = (int)(Math.Ceiling(boundingBox.maxX) + 1);
            int minY = (int)(Math.Floor(boundingBox.minY) - 1);
            int maxY = (int)(Math.Ceiling(boundingBox.maxY) + 1);
            int minZ = (int)(Math.Floor(boundingBox.minZ) - 1);
            int maxZ = (int)(Math.Ceiling(boundingBox.maxZ) + 1);

            minX = Math.Max(0, minX);
            minY = Math.Max(0, minY);
            minZ = Math.Max(0, minZ);

            maxX = Math.Min(Width, maxX);
            maxY = Math.Min(Depth, maxY);
            maxZ = Math.Min(Height, maxZ);

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int z = minZ; z < maxZ; z++)
                    {
                        if (IsSolidTile(x, y, z))
                        {
                            boundingBoxList.Add(new AxisAlignedBoundingBox(x, y, z, x + 1, y + 1, z + 1));
                        }
                    }
                }
            }
            return boundingBoxList;
        }
    }

}
