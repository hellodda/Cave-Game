using System.Diagnostics;

namespace Cave_Game.Core
{
    public class Timer
    {
        private const double NS_PER_SECOND = 1e9;
        private const double MAX_NS_PER_UPDATE = 1e9;
        private const int MAX_TICKS_PER_UPDATE = 100;
        private readonly float ticksPerSecond;
        private long lastTime;
        public float TimeScale = 1.0F;
        public float Fps = 0.0F;
        public float PassedTime = 0.0F;
        public int Ticks;
        public float PartialTicks;

        public Timer(float ticksPerSecond)
        {
            this.ticksPerSecond = ticksPerSecond;
            lastTime = Stopwatch.GetTimestamp();
        }
        public void AdvanceTime()
        {
            long now = Stopwatch.GetTimestamp();
            long deltaTicks = now - lastTime;
            lastTime = now;
            double passedNs = deltaTicks * NS_PER_SECOND / Stopwatch.Frequency;
            passedNs = Math.Max(0, passedNs);
            passedNs = Math.Min(MAX_NS_PER_UPDATE, passedNs);

            Fps = (float)(NS_PER_SECOND / passedNs);
   
            PassedTime += (float)(passedNs * TimeScale * ticksPerSecond / NS_PER_SECOND);

            Ticks = (int)PassedTime;
            Ticks = Math.Min(MAX_TICKS_PER_UPDATE, Ticks);

            PassedTime -= Ticks;
            PartialTicks = PassedTime;
        }
    }
}
