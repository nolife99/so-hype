using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.OpenTKUtil;
using System;

using static StorybrewCommon.OpenTKUtil.MathHelper;

namespace StorybrewScripts
{
    class Sphere : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            MakeSphere(25926, 37089, 110, 5, new Vector2i(36, 20), 20, (sp, s, e) =>
            {
                var beat = Beatmap.GetTimingPointAt(s).BeatDuration;
                sp.ColorHsb(s, 30, .8, 1);
                sp.Additive(s);

                sp.StartTriggerGroup("HitSoundWhistle", s, e);
                sp.Scale(0, beat / 2, .06, .03);
                sp.EndGroup();
            });
            MakeSphere(73368, 92903, 110, 5, new Vector2i(36, 20), 20, (sp, s, e) =>
            {
                var beat = Beatmap.GetTimingPointAt(s).BeatDuration;
                sp.Color(s, 0, .75, 1);
                sp.Additive(s);

                sp.StartTriggerGroup("HitSoundClap", s, e);
                sp.Scale(0, beat / 2, .06, .03);
                sp.EndGroup();
            });
            MakeSphere(126740, 145926, 110, 5, new Vector2i(36, 20), 17.5, (sp, s, e) =>
            {
                var beat = Beatmap.GetTimingPointAt(s).BeatDuration;
                sp.ColorHsb(s, 30, .8, 1);
                sp.Additive(s);

                sp.StartTriggerGroup("HitSoundClap", s, e);
                sp.Scale(0, beat / 2, .06, .03);
                sp.EndGroup();
            });
        }
        void MakeSphere(int start, int end, double size, uint split, Vector2i dots, double spinMult, 
            Action<OsbSprite, int, int> finalize = null)
        {
            // Constants
            var beat = Beatmap.GetTimingPointAt(start).BeatDuration;
            var spinDur = beat * spinMult;
            var rotFunc = new Quaterniond(DegreesToRadians(42.5), 0, DegreesToRadians(25));

            var back = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(317, 238));
            back.Scale(OsbEasing.OutBack, start, start + beat * 4, 0, 4.5);
            back.Color(start, 0, 0, 0);
            back.Fade(OsbEasing.In, end, end + beat * 4, .9, 0);

            var i = 1;
            for (double r = 0; r < dots.X; r++, i++) for (double c = 1; c < dots.Y; c++)
            {
                // Sphere splitting
                if (i > split && i < split * 2) break;
                else if (i == split * 2) i = 1;

                var rad = size * Sin(c / dots.Y * Pi);
                var basePos = new Vector3d(rad * Cos(r / dots.X * TwoPi), size * Cos(c / dots.Y * Pi), rad * Sin(r / dots.X * TwoPi));
                var pos = Vector3d.Transform(basePos, rotFunc);

                var sprite = GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(0, (float)pos.Y + 240));
                var delay = Abs(c - dots.Y / 2) * 100;
                sprite.Fade(start + delay, start + delay + beat * 2, 0, 1);
                sprite.Fade(end - delay, end - delay + beat * 4, 1, 0);

                var sTime = start - spinDur * Atan2(pos.Z, pos.X) / TwoPi;
                if (sTime > start + delay) sTime -= spinDur;
                sprite.StartLoopGroup(sTime, (int)Ceiling((end - delay + beat * 4 - sTime) / spinDur));

                var maxRad = Sqrt(pos.X * pos.X + pos.Z * pos.Z);
                sprite.MoveX(OsbEasing.InOutSine, 0, spinDur / 2, 320 + maxRad, 320 - maxRad);
                sprite.MoveX(OsbEasing.InOutSine, spinDur / 2, spinDur, 320 - maxRad, 320 + maxRad);
                sprite.EndGroup();

                sprite.Scale(start, .03);
                if (split > 0 && finalize != null && i == 1 || i == split) finalize(sprite, start, end);
            }
        }
    }
}