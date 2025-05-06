using Cave_Game.Core.physics;
using OpenTK.Graphics.OpenGL;

namespace Cave_Game.Core.level
{
    public class Frustum
    {
        private enum PlaneSide { Right, Left, Bottom, Top, Back, Front }
        private enum Coords { A, B, C, D }

        private float[][] frustum = new float[6][];

        public Frustum()
        {
            for (int i = 0; i < 6; i++) frustum[i] = new float[4];
        }

        public void Calculate()
        {
            float[] proj = new float[16];
            float[] modl = new float[16];
            GL.GetFloat(GetPName.ProjectionMatrix, proj);
            GL.GetFloat(GetPName.ModelviewMatrix, modl);

            float[] clip = new float[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    clip[i * 4 + j] = modl[i * 4 + 0] * proj[0 * 4 + j] +
                                     modl[i * 4 + 1] * proj[1 * 4 + j] +
                                     modl[i * 4 + 2] * proj[2 * 4 + j] +
                                     modl[i * 4 + 3] * proj[3 * 4 + j];
                }
            }

            // Right plane
            SetPlane(PlaneSide.Right, clip[3] - clip[0], clip[7] - clip[4], clip[11] - clip[8], clip[15] - clip[12]);
            // Left plane
            SetPlane(PlaneSide.Left, clip[3] + clip[0], clip[7] + clip[4], clip[11] + clip[8], clip[15] + clip[12]);
            // Bottom plane
            SetPlane(PlaneSide.Bottom, clip[3] + clip[1], clip[7] + clip[5], clip[11] + clip[9], clip[15] + clip[13]);
            // Top plane
            SetPlane(PlaneSide.Top, clip[3] - clip[1], clip[7] - clip[5], clip[11] - clip[9], clip[15] - clip[13]);
            // Back plane
            SetPlane(PlaneSide.Back, clip[3] - clip[2], clip[7] - clip[6], clip[11] - clip[10], clip[15] - clip[14]);
            // Front plane
            SetPlane(PlaneSide.Front, clip[3] + clip[2], clip[7] + clip[6], clip[11] + clip[10], clip[15] + clip[14]);
        }

        private void SetPlane(PlaneSide side, float a, float b, float c, float d)
        {
            float[] plane = frustum[(int)side];
            // normalize
            float mag = (float)Math.Sqrt(a * a + b * b + c * c);
            plane[(int)Coords.A] = a / mag;
            plane[(int)Coords.B] = b / mag;
            plane[(int)Coords.C] = c / mag;
            plane[(int)Coords.D] = d / mag;
        }

        public static Frustum GetFrustum()
        {
            Frustum frustum = new();
            frustum.Calculate();
            return frustum;
        }

        public bool PointInFrustum(float x, float y, float z)
        {
            for (int i = 0; i < 6; i++)
            {
                var p = frustum[i];
                if (p[(int)Coords.A] * x + p[(int)Coords.B] * y + p[(int)Coords.C] * z + p[(int)Coords.D] <= 0)
                    return false;
            }
            return true;
        }

        public bool SphereInFrustum(float x, float y, float z, float radius)
        {
            for (int i = 0; i < 6; i++)
            {
                var p = frustum[i];
                if (p[(int)Coords.A] * x + p[(int)Coords.B] * y + p[(int)Coords.C] * z + p[(int)Coords.D] <= -radius)
                    return false;
            }
            return true;
        }

        public bool CubeInFrustum(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            for (int i = 0; i < 6; i++)
            {
                var p = frustum[i];
                if (p[(int)Coords.A] * minX + p[(int)Coords.B] * minY + p[(int)Coords.C] * minZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * maxX + p[(int)Coords.B] * minY + p[(int)Coords.C] * minZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * minX + p[(int)Coords.B] * maxY + p[(int)Coords.C] * minZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * maxX + p[(int)Coords.B] * maxY + p[(int)Coords.C] * minZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * minX + p[(int)Coords.B] * minY + p[(int)Coords.C] * maxZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * maxX + p[(int)Coords.B] * minY + p[(int)Coords.C] * maxZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * minX + p[(int)Coords.B] * maxY + p[(int)Coords.C] * maxZ + p[(int)Coords.D] > 0) continue;
                if (p[(int)Coords.A] * maxX + p[(int)Coords.B] * maxY + p[(int)Coords.C] * maxZ + p[(int)Coords.D] > 0) continue;
                return false;
            }
            return true;
        }

        public bool CubeInFrustum(AxisAlignedBoundingBox box)
        {
            return CubeInFrustum((float)box.minX, (float)box.minY, (float)box.minZ,
                                   (float)box.maxX, (float)box.maxY, (float)box.maxZ);
        }
    }
}
