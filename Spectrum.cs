using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Animations;
using System;

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
            var MinimalHeight = 1;
            var Scale = new Vector2(5, 100);
            var LogScale = 15f;
            var Position = new Vector2(-100, 240);
            var Width = 860f;

            var BarCount = 40;
            var fftCount = BarCount * 2;

            var heightKeyframes = new KeyframedValue<float>[fftCount];
            for (var i = 0; i < fftCount; i++) heightKeyframes[i] = new KeyframedValue<float>(null);

            var timeStep = Beatmap.GetTimingPointAt(StartTime).BeatDuration / 8;
            var offset = timeStep * 0.2;
            
            for (var t = (double)StartTime; t <= EndTime; t += timeStep)
            {
                var fft = GetFft(t + offset, fftCount, null, OsbEasing.InExpo);
                for (var i = 0; i < fftCount; i++)
                {
                    var height = (float)Math.Pow(Math.Log10(1 + fft[i] * LogScale), 1.5) * Scale.Y;
                    if (height < MinimalHeight) height = MinimalHeight;

                    heightKeyframes[i].Add(t, height);
                }
            }
            var barWidth = Width / BarCount;
            for (var i = 0; i < BarCount; i++)
            {
                var keyframes = heightKeyframes[i];
                keyframes.Simplify1dKeyframes(3, h => h);

                var bar = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(Position.X + i * barWidth, Position.Y));
                bar.Fade(StartTime, StartTime + 1000, 0, 1);
                bar.Fade(EndTime - 1000, EndTime, 1, 0);

                keyframes.ForEachPair((start, end) => bar.ScaleVec(start.Time, end.Time, Scale.X, start.Value, Scale.X, end.Value),
                    MinimalHeight, s => (int)s);
            }
        }
    }
}