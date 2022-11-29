using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Sphere : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            MakeSphere(73368, 92903, 140, 5, new Vector2(36, 18), 18);
            MakeSphere(126740, 145926, 140, 5, new Vector2(36, 18), 15);
            timer.Stop();
            Log($"Execution: {timer.ElapsedMilliseconds / 1000f} seconds");
        }
        void MakeSphere(int startTime, int endTime, double size, int split, Vector2 dots, double durationMul, int accuracyLevel = 1000)
        {
            var beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var spinDur = beat * durationMul;
            
            var i = 1;
            for (var r = 0; r < dots.X; r++, i++) for (var c = 1; c < dots.Y; c++)
            {
                if (i > split && i < split * 2) break;
                else if (i == split * 2) i = 1;

                var rad = size * Math.Sin(c / dots.Y * Math.PI);
                var basePos = new Vector3d(
                    rad * Math.Cos(r / dots.X * Math.PI * 2),
                    size * Math.Cos(c / dots.Y * Math.PI),
                    rad * Math.Sin(r / dots.X * Math.PI * 2));
                        
                var rotFunc = new Vector3d(MathHelper.DegreesToRadians(47.5), 0, MathHelper.DegreesToRadians(30));
                var pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc));

                var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                sprite.Fade(startTime + r * 40, startTime + r * 40 + 800, 0, 1);
                sprite.Fade(endTime - r * 30, endTime - r * 30 + 800, 1, 0);
                
                var maxVal = new Vector2d();
                for (var f = .0; f < 360; f += 360 / (double)accuracyLevel)
                {
                    pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc.X, MathHelper.DegreesToRadians(f), rotFunc.Z));
                    if (pos.X < maxVal.Y) continue;
                    maxVal = new Vector2d(spinDur / 360 * f, pos.X);
                }

                var sTime = startTime + maxVal.X - spinDur;
                sprite.StartLoopGroup(sTime, (int)Math.Ceiling((endTime + 1000 - sTime) / spinDur));
                sprite.MoveX(OsbEasing.InOutSine, 0, spinDur / 2, 320 + maxVal.Y, 320 - maxVal.Y);
                sprite.MoveX(OsbEasing.InOutSine, spinDur / 2, spinDur, 320 - maxVal.Y, 320 + maxVal.Y);
                sprite.EndGroup();

                sprite.Scale(startTime, .03);
                if (i == 1 || i == split)
                {
                    sprite.Color(startTime, 0, .75, 1);
                    sprite.Additive(startTime);

                    sprite.StartTriggerGroup("HitSoundClap", startTime, endTime);
                    sprite.Scale(0, beat / 2, .06, .03);
                    sprite.EndGroup();
                }
            }
        }
    }
}