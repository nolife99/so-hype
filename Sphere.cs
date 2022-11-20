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
            Generate(73368, 92903, 140, 18, 34, 18);
            Generate(126740, 145926, 140, 18, 34, 15);
        }
        void Generate(int startTime, int endTime, double size, int rings, int ringDots, double durationMult)
        {
            #region ExtensionMethods

            Func<Vector3d, Vector3d, Vector3d> Rotate = (v, r) => Vector3d.Transform(v, new Quaterniond(r.X, r.Y, r.Z));
            Func<double, double> DegToRad = val => val * Math.PI / 180;
            Func<double, int> Ceiling = val => (int)Math.Ceiling(val);
            Func<StoredValue[], StoredValue> GetGreatestValue = values =>
            {
                var maxVal = values.Max(t => t.Value);
                var finalVal = new StoredValue();

                foreach (var value in values) if (maxVal == value.Value) finalVal = value; 
                return finalVal;
            };

            #endregion

            var beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var spinDuration = beat * durationMult;
            
            var i = 0;
            for (double r = 0; r < rings; r++)
            {
                i++;
                for (double c = 1; c < ringDots; c++)
                {
                    if (i > 5 && i < 10) break;
                    else if (i == 10) i = 1;
                    if (c == ringDots / 2) continue;

                    var radii = size * Math.Sin(c / (double)ringDots * Math.PI * 2);
                    var basePos = new Vector3d(
                        radii * Math.Cos(r / (double)rings * Math.PI),
                        size * Math.Cos(c / (double)ringDots * Math.PI * 2),
                        radii * Math.Sin(r / (double)rings * Math.PI));
                            
                    var rotFunc = new Vector3d(DegToRad(-50), 0, DegToRad(-35));
                    var pos = Rotate(basePos, rotFunc);

                    var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                    sprite.Fade(startTime + (c - 1) * 40, startTime + (c - 1) * 40 + 800, 0, 1);
                    sprite.Fade(endTime - r * 30, endTime - r * 30 + 800, 1, 0);
                    
                    var values = new StoredValue[450];
                    for (double f = 0; f <= 360; f += .8)
                    {
                        pos = Rotate(basePos, new Vector3d(rotFunc.X, DegToRad(f), rotFunc.Z));
                        values[(int)(f * 1.25)] = new StoredValue(spinDuration / 360 * f, pos.X);
                    }
                    var maxSV = GetGreatestValue(values);

                    var sTime = startTime + maxSV.Time - spinDuration;
                    sprite.StartLoopGroup(sTime, Ceiling((endTime + 1000 - sTime) / spinDuration));
                    sprite.MoveX(OsbEasing.InOutSine, 0, spinDuration / 2, 320 + maxSV.Value, 320 - maxSV.Value);
                    sprite.MoveX(OsbEasing.InOutSine, spinDuration / 2, spinDuration, 320 - maxSV.Value, 320 + maxSV.Value);
                    sprite.EndGroup();

                    if (i == 1 || i == 5)
                    {
                        sprite.Scale(startTime, .03);
                        sprite.Color(startTime, Color4.DeepSkyBlue);
                        sprite.Additive(startTime);

                        Action<double, double> Trigger = (MaxScale, AmpScale) =>
                        {
                            sprite.StartTriggerGroup("HitSoundClap", startTime, endTime);
                            sprite.Scale(0, beat / 2, AmpScale, .03);
                            sprite.EndGroup();
                        };
                        Trigger(.06, .065);
                    }
                    else sprite.Scale(startTime, .03);
                }
            }
        }
        struct StoredValue
        {
            internal double Time, Value;
            internal StoredValue(double time, double value)
            {
                Time = time;
                Value = value;
            }
        }
    }
}