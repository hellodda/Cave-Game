namespace Cave_Game.Core.physics
{

    public class AxisAlignedBoundingBox
    {
        private readonly double epsilon = 0.0F;

        public double minX;
        public double minY;
        public double minZ;
        public double maxX;
        public double maxY;
        public double maxZ;

        public AxisAlignedBoundingBox(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            this.minX = minX;
            this.minY = minY;
            this.minZ = minZ;
            this.maxX = maxX;
            this.maxY = maxY;
            this.maxZ = maxZ;
        }
        public AxisAlignedBoundingBox Clone()
        {
            return new AxisAlignedBoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }
        public AxisAlignedBoundingBox Expand(double x, double y, double z)
        {
            double minX = this.minX;
            double minY = this.minY;
            double minZ = this.minZ;
            double maxX = this.maxX;
            double maxY = this.maxY;
            double maxZ = this.maxZ;

            if (x < 0.0F)
            {
                minX += x;
            }
            else
            {
                maxX += x;
            }
            if (y < 0.0F)
            {
                minY += y;
            }
            else
            {
                maxY += y;
            }
            if (z < 0.0F)
            {
                minZ += z;
            }
            else
            {
                maxZ += z;
            }
            return new AxisAlignedBoundingBox(minX, minY, minZ, maxX, maxY, maxZ);
        }
        public AxisAlignedBoundingBox Grow(double x, double y, double z)
        {
            return new AxisAlignedBoundingBox(minX - x, minY - y,
                    minZ - z, maxX + x,
                    maxY + y, maxZ + z);
        }
        public double ClipXCollide(AxisAlignedBoundingBox otherBoundingBox, double x)
        {
            if (otherBoundingBox.maxY <= minY || otherBoundingBox.minY >= maxY)
            {
                return x;
            }
            if (otherBoundingBox.maxZ <= minZ || otherBoundingBox.minZ >= maxZ)
            {
                return x;
            }
            if (x > 0.0F && otherBoundingBox.maxX <= minX)
            {
                double max = minX - otherBoundingBox.maxX - epsilon;
                if (max < x)
                {
                    x = max;
                }
            }
            if (x < 0.0F && otherBoundingBox.minX >= maxX)
            {
                double max = maxX - otherBoundingBox.minX + epsilon;
                if (max > x)
                {
                    x = max;
                }
            }

            return x;
        }
        public double ClipYCollide(AxisAlignedBoundingBox otherBoundingBox, double y)
        {
            if (otherBoundingBox.maxX <= minX || otherBoundingBox.minX >= maxX)
            {
                return y;
            }
            if (otherBoundingBox.maxZ <= minZ || otherBoundingBox.minZ >= maxZ)
            {
                return y;
            }
            if (y > 0.0F && otherBoundingBox.maxY <= minY)
            {
                double max = minY - otherBoundingBox.maxY - epsilon;
                if (max < y)
                {
                    y = max;
                }
            }
            if (y < 0.0F && otherBoundingBox.minY >= maxY)
            {
                double max = maxY - otherBoundingBox.minY + epsilon;
                if (max > y)
                {
                    y = max;
                }
            }

            return y;
        }
        public double ClipZCollide(AxisAlignedBoundingBox otherBoundingBox, double z)
        {
            if (otherBoundingBox.maxX <= minX || otherBoundingBox.minX >= maxX)
            {
                return z;
            }
            if (otherBoundingBox.maxY <= minY || otherBoundingBox.minY >= maxY)
            {
                return z;
            }
            if (z > 0.0F && otherBoundingBox.maxZ <= minZ)
            {
                double max = minZ - otherBoundingBox.maxZ - epsilon;
                if (max < z)
                {
                    z = max;
                }
            }
            if (z < 0.0F && otherBoundingBox.minZ >= maxZ)
            {
                double max = maxZ - otherBoundingBox.minZ + epsilon;
                if (max > z)
                {
                    z = max;
                }
            }

            return z;
        }
        public bool Intersects(AxisAlignedBoundingBox otherBoundingBox)
        {
         
            if (otherBoundingBox.maxX <= minX || otherBoundingBox.minX >= maxX)
            {
                return false;
            }
            if (otherBoundingBox.maxY <= minY || otherBoundingBox.minY >= maxY)
            {
                return false;
            }
            return !(otherBoundingBox.maxZ <= minZ) && !(otherBoundingBox.minZ >= maxZ);
        }
        public void Move(double x, double y, double z)
        {
            minX += x;
            minY += y;
            minZ += z;
            maxX += x;
            maxY += y;
            maxZ += z;
        }
        public AxisAlignedBoundingBox Offset(double x, double y, double z)
        {
            return new AxisAlignedBoundingBox(minX + x, minY + y, minZ + z, maxX + x, maxY + y, maxZ + z);
        }
    }
}
