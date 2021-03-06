using System;
using Xunit;

namespace Server.Tests
{
    [Collection("Sequential Tests")]
    public class TimerTests : IClassFixture<ServerFixture>
    {
        [Theory]
        [InlineData(0L, 8L)]
        [InlineData(80L, 80L)]
        [InlineData(65L, 72L)]
        [InlineData(32767L, 32768L)]
        [InlineData(32768L, 32768L)]
        [InlineData(32833L, 32840L)]
        [InlineData(134217729L, 134217736L)]
        public void TestVariousTimes(long ticks, long expectedTicks)
        {
            var timerTicks = new TimerTicks();
            void action()
            {
                Assert.Equal(expectedTicks, timerTicks.Ticks);
                timerTicks.ExecutedCount++;
            }

            Timer.Initialize(timerTicks.Ticks);

            var timer = Timer.DelayCall(TimeSpan.FromMilliseconds(ticks), action);
            timer.Start();

            var tickCount = expectedTicks / 8;

            for (int i = 1; i <= tickCount; i++)
            {
                timerTicks.Ticks = i * 8;

                Timer.Slice(timerTicks.Ticks);
            }

            Assert.Equal(1, timerTicks.ExecutedCount);
        }

        [Theory]
        [InlineData(1000L, 1000L, 30000L, 30000L, 2)]
        public void TestIntervals(long delay, long expectedDelayTicks, long interval, long expectedIntervalTicks, int count)
        {
            var timerTicks = new TimerTicks();
            void action()
            {
                timerTicks.ExpectedTicks += timerTicks.ExecutedCount++ == 0 ? expectedDelayTicks : expectedIntervalTicks;
                Assert.Equal(timerTicks.ExpectedTicks, timerTicks.Ticks);
            }

            Timer.Initialize(timerTicks.Ticks);

            var timer = Timer.DelayCall(TimeSpan.FromMilliseconds(delay), TimeSpan.FromMilliseconds(interval), count, action);
            timer.Start();

            var tickCount = (expectedDelayTicks + (expectedIntervalTicks * count - 1)) / 8;

            for (int i = 1; i <= tickCount; i++)
            {
                timerTicks.Ticks = i * 8;

                Timer.Slice(timerTicks.Ticks);
            }

            Assert.Equal(count, timerTicks.ExecutedCount);
        }

        private class TimerTicks
        {
            public long ExpectedTicks;
            public long Ticks;
            public int ExecutedCount;
        }
    }
}
