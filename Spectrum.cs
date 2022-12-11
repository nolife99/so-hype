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
            MakeLinear(126740, 145926);
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
                keyframe.Simplify1dKeyframes(4, h => h);

                var bar = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(basePos.X + i * barWidth, basePos.Y));
                bar.Fade(StartTime, StartTime + 500, 0, .6);
                bar.Fade(EndTime - 500, EndTime, .6, 0);
                bar.Additive(StartTime);

                keyframe.ForEachPair((s, e) => bar.ScaleVec(s.Time, e.Time, scale.X, s.Value, scale.X, e.Value),
                    1, s => (int)s);
            }
        }
    }
}