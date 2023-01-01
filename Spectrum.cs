using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Animations;
using System;
using static System.Math;

namespace StorybrewScripts
{
    class Spectrum : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            // If you don't have the beatmap use ImportOsb 
            // CaptureVocals();
            MakeSpectrum(48949, 70577);
            MakeRadial(126740, 145926);
            MakeSpectrum(151507, 173833);
            MakeSpectrum(258251, 279879);
        }
        void MakeSpectrum(int startTime, int endTime)
        {
            var bMap = GetMapsetBitmap("sb/pl.png");
            var BarCount = 20;
            var fftCount = BarCount * 2;
            var scale = new Vector2(15, 150);
            var basePos = new Vector2(-50, 470);

            var keyframes = new KeyframedValue<float>[fftCount];
            for (var i = 0; i < fftCount; i++) keyframes[i] = new KeyframedValue<float>();

            var timeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / 8;
            for (double time = startTime; time < endTime; time += timeStep)
            {
                var fft = GetFft(time, fftCount, null, OsbEasing.InExpo);
                for (var i = 0; i < fftCount; i++)
                {
                    var height = Pow(Log10(1 + fft[i] * 470), 1.6) * scale.Y / bMap.Height;
                    if (height < 1) height = 1;

                    keyframes[i].Add(time, (float)height);
                }
            }

            var width = 780 / BarCount;
            for (var i = 0; i < BarCount; i++)
            {
                var keyframe = keyframes[i];
                keyframe.Simplify1dKeyframes(1.5, f => f);

                var sprite = GetLayer("").CreateSprite("sb/pl.png", OsbOrigin.Centre, new Vector2(basePos.X + i * width, basePos.Y));
                sprite.Fade(startTime, startTime + 500, 0, .5);
                sprite.Fade(endTime - 500, endTime, .5, 0);
                sprite.Additive(startTime);

                var scaleX = (double)scale.X * width / bMap.Width; scaleX = Floor(scaleX * 10) / 10f;

                var hasScale = false;
                keyframe.ForEachPair((start, end) =>
                {
                    hasScale = true;
                    sprite.ScaleVec(start.Time, end.Time, scaleX, start.Value, scaleX, end.Value);
                }, 1);
                if (!hasScale) sprite.ScaleVec(startTime, scaleX, 1);
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
            for (var i = 0; i < fftCount; i++) height[i] = new KeyframedValue<float>();
            
            var timeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / 6;
            for (double t = startTime; t <= endTime + 10; t += timeStep)
            {
                var fft = GetFft(t, fftCount, null, OsbEasing.InExpo);
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

                var bar = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.BottomCentre, new Vector2(
                    (float)(Position.X + radius * Cos(i * 2 * PI / BarCount)), 
                    (float)(Position.Y + radius * Sin(i * 2 * PI / BarCount))));
                
                bar.Additive(startTime);
                bar.Rotate(startTime, i * 2 * PI / BarCount + PI / 2);
                bar.Fade(startTime, .75);
                bar.Fade(endTime, endTime + 200, .75, 0);

                keyframe.ForEachPair((s, e) => bar.ScaleVec(s.Time, e.Time, scale.X, s.Value, scale.X, e.Value), 1);
            }
        }
        [Obsolete("Unused, represents code in ImportOsb script")] void CaptureVocals()
        {
            using (var pool = new SpritePool(GetLayer("ControlledSpec"), "sb/px.png", new Vector2(320, 240), (p, s, e) =>
            {
                p.Additive(s);
                p.Color(s, 1, 0, 0);
            }))
            foreach (var hit in GetBeatmap("hitsound sb").HitObjects)
            {
                var beat = Beatmap.GetTimingPointAt(10000).BeatDuration;
                var sprite = pool.Get(hit.StartTime, hit.EndTime + beat * 4);
                sprite.ScaleVec(OsbEasing.OutQuint, hit.StartTime, hit.EndTime + beat * 2, 0, 50, 854, 50);
                sprite.Rotate(OsbEasing.OutQuint, hit.StartTime, hit.EndTime + beat * 3, 0, Random(-PI / 30, PI / 30));
                sprite.Fade(OsbEasing.Out, hit.StartTime, hit.EndTime + beat * 4, .3, 0);
            }
        }
    }
}