using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Animations;
using static System.Math;

namespace StorybrewScripts
{
    class Spectrum : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            MakeLinear(48949, 70577);
            MakeRadial(126740, 145926);
            MakeLinear(151507, 173833);
            MakeLinear(258251, 279879);
        }
        void MakeLinear(int StartTime, int EndTime)
        {
            var scale = new Vector2(17.5f, 100);
            var basePos = new Vector2(-96, 240);

            var barCount = 35;
            var fftCount = barCount * 2;

            var fftKeyframes = new KeyframedValue<float>[fftCount];
            for (var i = 0; i < fftCount; i++) fftKeyframes[i] = new KeyframedValue<float>(null);

            var timeStep = Beatmap.GetTimingPointAt(StartTime).BeatDuration / 8;
            var offset = timeStep * .2;
            for (double t = StartTime; t <= EndTime + 10; t += timeStep)
            {
                var fft = GetFft(t + offset, fftCount, null, OsbEasing.InExpo);
                for (var i = 0; i < fftCount; i++)
                {
                    var height = Pow(Log10(1 + fft[i] * 15), 1.5) * scale.Y;
                    if (height < 1) height = 1;

                    fftKeyframes[i].Add(t, (float)height);
                }
            }

            var barWidth = 856f / barCount;
            for (var i = 0; i < barCount; i++)
            {
                var keyframe = fftKeyframes[i];
                keyframe.Simplify1dKeyframes(5, h => h);

                var bar = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(basePos.X + i * barWidth, basePos.Y));
                bar.Fade(StartTime, StartTime + 500, 0, .6);
                bar.Fade(EndTime - 500, EndTime, .6, 0);
                bar.Additive(StartTime);

                keyframe.ForEachPair((s, e) => bar.ScaleVec(s.Time, e.Time, scale.X, s.Value, scale.X, e.Value), 1, s => (int)s);
            }
        }
        void MakeRadial(int startTime, int endTime)
        {
            var BarCount = 28;
            var fftCount = (int)(BarCount * 1.5);
            var scale = new Vector2(7.5f, 200);
            var Position = new Vector2(320, 240);
            var radius = 125;

            var height = new KeyframedValue<float>[fftCount];
            for (var i = 0; i < fftCount; i++) height[i] = new KeyframedValue<float>(null);
            
            var timeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / 6;
            var offset = timeStep * .2;
            for (double t = startTime; t <= endTime + 10; t += timeStep)
            {
                var fft = GetFft(t + offset, fftCount, null, OsbEasing.InExpo);
                for (var i = 0; i < fftCount; i++)
                {
                    var val = Log10(1 + fft[i] * 5) * scale.Y;
                    if (val < 1) val = 1;

                    height[i].Add(t, (float)val);
                }
            }

            for (var i = 0; i < BarCount; i++)
            {
                var keyframe = height[i];
                keyframe.Simplify1dKeyframes(5, h => h);

                var bar = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(
                    (float)(Position.X + radius * Cos(i * 2 * PI / BarCount)), 
                    (float)(Position.Y + radius * Sin(i * 2 * PI / BarCount))));
                
                bar.Additive(startTime);
                bar.Rotate(startTime, i * 2 * PI / BarCount + PI / 2);
                bar.Fade(startTime, .75);
                bar.Fade(endTime, endTime + 200, .75, 0);

                keyframe.ForEachPair((s, e) => bar.ScaleVec(s.Time, e.Time, scale.X, s.Value, scale.X, e.Value), 1, s => (int)s);
            }
        }
    }
}