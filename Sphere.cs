using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System.Linq;
using System;

namespace StorybrewScripts
{
    class Sphere : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            MakeSphere(73368, 92903, 140, new Vector2i(18, 34), 18);
            MakeSphere(126740, 145926, 140, new Vector2i(18, 34), 15);
        }
        void MakeSphere(int startTime, int endTime, double size, Vector2i dots, double durationMul)
        {
            Func<Keyframe<double>[], Keyframe<double>> GetGreatestValue = values =>
            {
                var maxVal = values.Max(v => v.Value);
                var finalVal = new Keyframe<double>();

                foreach (var val in values) if (maxVal == val.Value) finalVal = val; 
                return finalVal;
            };

            var beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var spinDur = beat * durationMul;
            
            var i = 0;
            for (var r = .0; r < dots.X; r++)
            {
                i++;
                for (var c = 1.0; c < dots.Y; c++)
                {
                    if (i > 5 && i < 10) break;
                    else if (i == 10) i = 1;
                    if (c == ringDots / 2) continue;

                    var rad = size * Math.Sin(c / (double)dots.Y * Math.PI * 2);
                    var basePos = new Vector3d(
                        rad * Math.Cos(r / (double)dots.X * Math.PI),
                        size * Math.Cos(c / (double)dots.Y * Math.PI * 2),
                        rad * Math.Sin(r / (double)dots.X * Math.PI));
                            
                    var rotFunc = new Vector3d(MathHelper.DegreesToRadians(-50), 0, MathHelper.DegreesToRadians(-35));
                    var pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc));

                    var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                    sprite.Fade(startTime + (c - 1) * 40, startTime + (c - 1) * 40 + 800, 0, 1);
                    sprite.Fade(endTime - r * 30, endTime - r * 30 + 800, 1, 0);
                    
                    var values = new Keyframe<double>[1000];
                    for (var f = .0; f < 360; f += .36)
                    {
                        pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc.X, MathHelper.DegreesToRadians(f), rotFunc.Z));
                        values.Add(spinDur / 360 * f, pos.X);
                    }
                    var maxVal = GetGreatestValue(values);

                    var sTime = startTime + maxVal.Time - spinDur;
                    sprite.StartLoopGroup(sTime, (int)Math.Ceiling((endTime + 1000 - sTime) / spinDur));
                    sprite.MoveX(OsbEasing.InOutSine, 0, spinDur / 2, 320 + maxVal.Value, 320 - maxVal.Value);
                    sprite.MoveX(OsbEasing.InOutSine, spinDur / 2, spinDur, 320 - maxVal.Value, 320 + maxVal.Value);
                    sprite.EndGroup();

                    sprite.Scale(startTime, .03);
                    if (i == 1 || i == 5)
                    {
                        sprite.Color(startTime, Color4.DeepSkyBlue);
                        sprite.Additive(startTime);

                        sprite.StartTriggerGroup("HitSoundClap", startTime, endTime);
                        sprite.Scale(0, beat / 2, .06, .03);
                        sprite.EndGroup();
                    }
                }
            }
        }
    }
}
