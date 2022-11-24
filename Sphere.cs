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
        /// <summary>
        /// Custom build: <see href="http://github.com/nolife99/storybrew"/>
        /// </summary>
        protected override void Generate()
        {
            MakeSphere(73368, 92903, 140, 18, 34, 18);
            MakeSphere(126740, 145926, 140, 18, 34, 15);
        }
        void MakeSphere(int startTime, int endTime, double size, int rings, int ringDots, double durationMul)
        {
            Func<Vector3d, Vector3d, Vector3d> Rotate = (v, r) => Vector3d.Transform(v, new Quaterniond(r));
            Func<double, double> DegToRad = val => val * Math.PI / 180;
            Func<double, int> Ceiling = val => (int)Math.Ceiling(val);
            Func<Vector2d[], Vector2d> GetGreatestValue = values =>
            {
                var maxVal = values.Max(v => v.Y);
                var finalVal = new Vector2d();

                foreach (var val in values) if (maxVal == val.Y) finalVal = val; 
                return finalVal;
            };

            var beat = GetBeatDuration(startTime);
            var spinDur = beat * durationMul;
            
            var i = 0;
            for (var r = .0; r < rings; r++)
            {
                i++;
                for (var c = 1.0; c < ringDots; c++)
                {
                    if (i > 5 && i < 10) break;
                    else if (i == 10) i = 1;
                    if (c == ringDots / 2) continue;

                    var rad = size * Math.Sin(c / (double)ringDots * Math.PI * 2);
                    var basePos = new Vector3d(
                        rad * Math.Cos(r / (double)rings * Math.PI),
                        size * Math.Cos(c / (double)ringDots * Math.PI * 2),
                        rad * Math.Sin(r / (double)rings * Math.PI));
                            
                    var rotFunc = new Vector3d(DegToRad(-50), 0, DegToRad(-35));
                    var pos = Rotate(basePos, rotFunc);

                    var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                    sprite.Fade(startTime + (c - 1) * 40, startTime + (c - 1) * 40 + 800, 0, 1);
                    sprite.Fade(endTime - r * 30, endTime - r * 30 + 800, 1, 0);
                    
                    var values = new Vector2d[800];
                    for (var f = .0; f <= 360; f += .45)
                    {
                        pos = Rotate(basePos, new Vector3d(rotFunc.X, DegToRad(f), rotFunc.Z));
                        values[(int)(1 / .45 * f)] = new Vector2d(spinDur / 360 * f, pos.X);
                    }
                    var maxSVal = GetGreatestValue(values);

                    var sTime = startTime + maxSVal.X - spinDur;
                    sprite.StartLoopGroup(sTime, Ceiling((endTime + 1000 - sTime) / spinDur));
                    sprite.MoveX(OsbEasing.InOutSine, 0, spinDur / 2, 320 + maxSVal.Y, 320 - maxSVal.Y);
                    sprite.MoveX(OsbEasing.InOutSine, spinDur / 2, spinDur, 320 - maxSVal.Y, 320 + maxSVal.Y);
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
