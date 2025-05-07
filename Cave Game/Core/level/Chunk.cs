using Cave_Game.Core.physics;
using OpenTK.Graphics.OpenGL;

namespace Cave_Game.Core.level
{
    public class Chunk
    {
        private static int texture = -1;
        private static readonly Tessellator tessellator = new();

        public static int RebuiltThisFrame { get; set; }
        public static int Updates { get; set; }
        public AxisAlignedBoundingBox BoundingBox {get; set; }
        
        private readonly Level level;
        private readonly int minX, minY, minZ;
        private readonly int maxX, maxY, maxZ;
        private readonly int lists;
        private bool dirty = true;

        public Chunk(Level level, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
        {
            this.level = level;
            this.minX = minX;
            this.minY = minY;
            this.minZ = minZ;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;

            lists = GL.GenLists(2);

            BoundingBox = new AxisAlignedBoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }
        public void Rebuild(int layer)
        {
            if (RebuiltThisFrame == 2) return;

            if (texture == -1)
            {
                texture = Textures.LoadTexture(AppContext.BaseDirectory + "/Resources/terrain.png", OpenTK.Graphics.OpenGL4.TextureMinFilter.Nearest);
            }

            Updates++;
            RebuiltThisFrame++;
            dirty = false;

            GL.NewList(lists + layer, ListMode.Compile);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            tessellator.Init();

            for (int x = minX; x < maxX; x++)
                for (int y = minY; y < maxY; y++)
                    for (int z = minZ; z < maxZ; z++)
                    {
                        if (!level.IsTile(x, y, z)) continue;

                        if (y + 45 > level.Depth - 7)
                            Tile.Grass.Render(tessellator, level, layer, x, y, z);
                        else
                            Tile.Rock.Render(tessellator, level, layer, x, y, z);
                    }

            tessellator.Flush();
            GL.Disable(EnableCap.Texture2D);
            GL.EndList();
        }
        public void Render(int layer)
        {
            if (dirty)
            {
                Rebuild(0);
                Rebuild(1);
            }
            GL.CallList(lists + layer);
        }
        public void SetDirty() => dirty = true;
    }
}
