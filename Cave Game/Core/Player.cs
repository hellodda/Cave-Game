using Cave_Game.Core.Extensions;
using Cave_Game.Core.level;
using Cave_Game.Core.physics;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Cave_Game.Core
{
    public class Player
    {
        private readonly Level level;

        public double X, Y, Z;
        public double PrevX, PrevY, PrevZ;
        public double MotionX, MotionY, MotionZ;
        public float XRotation, YRotation;

        private bool onGround;

        public AxisAlignedBoundingBox BoundingBox;

        public Player(Level level)
        {
            this.level = level;

            ResetPosition();
        }

        private void SetPosition(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;

            float width = 0.3F;
            float height = 0.9F;
     
            BoundingBox = new AxisAlignedBoundingBox(x - width, y - height,
                    z - width, x + width,
                    y + height, z + width);
        }
        private void ResetPosition()
        {
            Random rd = new();
            float x = rd.Next(level.Width);    
            float y = rd.Next(level.Depth);
            float z = rd.Next(level.Height);   
            SetPosition(x, y, z);
        }


        public void Turn(float x, float y)
        {
            YRotation += x * 0.15F;
            XRotation -= y * 0.15F;

            XRotation = Math.Max(-90.0F, XRotation);
            XRotation = Math.Min(90.0F, XRotation);
        }

        public void Tick(KeyboardState input)
        {
            PrevX = X;
            PrevY = Y;
            PrevZ = Z;

            float forward = 0.0F;
            float vertical = 0.0F;

            if (input.IsKeyDown(Keys.R))
            {
                ResetPosition();
            }

            if (input.IsKeyDown(Keys.W) || input.IsKeyDown(Keys.Up))
            {
                forward--;
            }
            if (input.IsKeyDown(Keys.S) || input.IsKeyDown(Keys.Down))
            {
                forward++;
            }
            if (input.IsKeyDown(Keys.A) || input.IsKeyDown(Keys.Left))
            {
                vertical--;
            }
            if (input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.Right))
            {
                vertical++;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                if (onGround)
                {
                    MotionY = 0.12F;
                }
            }

            MoveRelative(vertical, forward, onGround ? 0.02F : 0.005F);
            MotionY -= 0.005D;
            Move(MotionX, MotionY, MotionZ);
            MotionX *= 0.91F;
            MotionY *= 0.98F;
            MotionZ *= 0.91F;

            if (onGround)
            {
                MotionX *= 0.8F;
                MotionZ *= 0.8F;
            }
        }
        public void Move(double x, double y, double z)
        {
            double prevX = x;
            double prevY = y;
            double prevZ = z;
            List<AxisAlignedBoundingBox> aABBs = level.GetCubes(BoundingBox.Expand(x, y, z)).ToList();
            foreach (AxisAlignedBoundingBox aABB in aABBs)
            {
                y = aABB.ClipYCollide(BoundingBox, y);
            }
            BoundingBox.Move(0.0F, y, 0.0F);
            foreach (AxisAlignedBoundingBox aABB in aABBs)
            {
                x = aABB.ClipXCollide(BoundingBox, x);
            }
            BoundingBox.Move(x, 0.0F, 0.0F);
            foreach (AxisAlignedBoundingBox aABB in aABBs)
            {
                z = aABB.ClipZCollide(BoundingBox, z);
            }
            BoundingBox.Move(0.0F, 0.0F, z);
            onGround = prevY != y && prevY < 0.0F;

            if (prevX != x) MotionX = 0.0D;
            if (prevY != y) MotionY = 0.0D;
            if (prevZ != z) MotionZ = 0.0D;

            X = (BoundingBox.minX + BoundingBox.maxX) / 2.0D;
            Y = BoundingBox.minY + 1.62D;
            Z = (BoundingBox.minZ + BoundingBox.maxZ) / 2.0D;
       
        }
        private void MoveRelative(float x, float z, float speed)
        {
            float distance = x * x + z * z;

            if (distance < 0.01F)
                return;

            distance = speed / (float)Math.Sqrt(distance);
            x *= distance;
            z *= distance;

            double sin = Math.Sin(MathExtension.DegreesToRadians(YRotation));
            double cos = Math.Cos(MathExtension.DegreesToRadians(YRotation));

            MotionX += x * cos - z * sin;
            MotionZ += z * cos + x * sin;
        }
    }

}
