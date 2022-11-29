using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Mapset;
using static System.Math;

namespace StorybrewScripts
{
    class HitObjects : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            Trail(25926, 37001);
            DotBurst(42670, 46856);
		    DotBurst(73368, 92903);
            Trail(48949, 69269);
            Trail(115228, 115228);
            DotBurst(115577, 126304);
            Trail(120461, 120461);
            Trail(126391, 145926);
            DotBurst(148019, 151158);
            Trail(151507, 173746);
            DotBurst(173833, 183513);
            Trail(179414, 184908);
            Trail(240809, 251886);
            DotBurst(247089, 251886);
            Trail(258251, 265839);
            DotBurst(265926, 268368);
            Trail(268716, 278571);
        }
        void DotBurst(int startTime, int endTime)
        {
            using (var pool = new SpritePool(GetLayer(""), "sb/d.png", OsbOrigin.Centre, true)) foreach (var hit in Beatmap.HitObjects)
            {
                if (hit.StartTime < startTime || hit.EndTime > endTime) continue;
                for (var i = 0; i < Random(15, 20); i++)
                {
                    var angle = Random(PI * 2);
                    var radius = Random(40f, 80);

                    var startPos = hit.Position + hit.StackOffset;
                    var endPos = new Vector2(radius * (float)Cos(angle) + startPos.X, radius * (float)Sin(angle) + startPos.Y);
                    var duration = Random(1000, 2000);

                    var sprite = pool.Get(hit.StartTime, hit.StartTime + duration);
                    sprite.Scale(OsbEasing.In, hit.StartTime, hit.StartTime + duration, radius * 6E-4, 0);
                    sprite.Move(OsbEasing.OutQuint, hit.StartTime, hit.StartTime + duration, startPos, endPos);
                }
            }
        }
        void Trail(int startTime, int endTime)
        {
            using (var pool = new SpritePool(GetLayer(""), "sb/hl.png", (light, start, end) =>
            {
                light.Additive(start);
                light.Scale(start, 0.1);
            })) 
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject.StartTime < startTime || hitobject.StartTime > endTime) continue;

                var pos = hitobject.Position + hitobject.StackOffset;
                var sprite = pool.Get(hitobject.StartTime, hitobject.StartTime + 1000);
                sprite.Move(hitobject.StartTime, pos);
                sprite.Fade(hitobject.StartTime, hitobject.StartTime + 1000, 0.5, 0);
                if ((Color4)sprite.ColorAt(hitobject.StartTime) != hitobject.Color) sprite.Color(hitobject.StartTime, hitobject.Color);

                if (hitobject is OsuSlider)
                {
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 12;
                    var sTime = hitobject.StartTime + timestep;

                    while (true)
                    {
                        var stepSprite = pool.Get(sTime - 50, sTime + 1000);
                        stepSprite.Move(sTime - 50, hitobject.PositionAtTime(sTime));
                        stepSprite.Fade(sTime - 50, sTime, 0, 0.4);
                        stepSprite.Fade(sTime, sTime + 1000, 0.4, 0);
                        if ((Color4)stepSprite.ColorAt(sTime) != hitobject.Color) stepSprite.Color(sTime - 50, hitobject.Color);

                        if (sTime > hitobject.EndTime) break;
                        sTime += timestep;
                    }
                }
            }
        }
    }
}