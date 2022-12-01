using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Animations;
using static OpenTK.MathHelper;
using static System.Math;

namespace StorybrewScripts
{
    class Sphere : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            MakeSphere(73368, 92903, 130, 5, new Vector2(36, 20), 18);
            MakeSphere(126740, 145926, 130, 5, new Vector2(36, 20), 15);
        }

        ///<summary> Generates a transformed sphere </summary>
        ///<param name="startTime"> Start time of the object </param>
        ///<param name="endTime"> End time of the object </param>
        ///<param name="size"> Scale multiplier of the object </param>
        ///<param name="split"> Amount of dots between equal splits <para> Should be a positive integer <para/></param>
        ///<param name="dots"> X and Y subdivisions of the object <para> Should be positive integers <para/></param>
        ///<param name="durationMul"> Spin duration multiplier of the object </param>
        ///<param name="polling"> Translation accuracy of the object <para> Higher for better looks, lower for speed </para></param>
        void MakeSphere(int startTime, int endTime, double size, int split, Vector2 dots, double spinMult, double polling = 95)
        {
            var beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var spinDur = beat * spinMult;

            var back = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(323, 257));
            back.Scale(OsbEasing.OutBack, startTime, startTime + beat * 4, 0, 5.1);
            back.Color(startTime, 0, 0, 0);
            back.Fade(OsbEasing.In, endTime, endTime + beat * 4, .85, 0);
            
            var i = 1;
            for (var r = 0; r < dots.X; r++, i++) for (var c = 1; c < dots.Y; c++)
            {
                if (i > split && i < split * 2) break;
                else if (i == split * 2) i = 1;

                var rad = size * Sin(c / dots.Y * PI);
                var basePos = new Vector3d(rad * Cos(r / dots.X * TwoPi), size * Cos(c / dots.Y * PI), rad * Sin(r / dots.X * TwoPi));
                
                var rotFunc = new Vector3d(DegreesToRadians(47.5), 0, DegreesToRadians(30));
                var pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc));

                var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                sprite.Fade(startTime + r * 30, startTime + r * 30 + beat * 2, 0, 1);
                sprite.Fade(endTime - c * 30, endTime - c * 30 + beat * 4, 1, 0);
                
                var maxVal = new Keyframe<double>();
                for (var f = 0d; f < 360; f += 36 / polling)
                {
                    pos = Vector3d.Transform(basePos, new Quaterniond(rotFunc.X, DegreesToRadians(f), rotFunc.Z));
                    if (pos.X <= maxVal.Value) continue;
                    maxVal = new Keyframe<double>(spinDur / 360 * f, pos.X);
                }

                var sTime = startTime + maxVal.Time - spinDur;
                sprite.StartLoopGroup(sTime, (int)Ceiling((endTime + 1000 - sTime) / spinDur));
                sprite.MoveX(OsbEasing.InOutSine, 0, spinDur / 2, 320 + maxVal.Value, 320 - maxVal.Value);
                sprite.MoveX(OsbEasing.InOutSine, spinDur / 2, spinDur, 320 - maxVal.Value, 320 + maxVal.Value);
                sprite.EndGroup();

                sprite.Scale(startTime, .03);
                if (split > 0 && i == 1 || i == split)
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
