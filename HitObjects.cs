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
            Trail(126391, 145926, true);
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
            using (var pool = new SpritePool(GetLayer(""), "sb/d.png", true)) foreach (var hit in Beatmap.HitObjects) 
                if (hit.StartTime >= startTime && hit.EndTime <= endTime) for (var i = 0; i < Random(15, 20); i++)
            {
                var angle = Random(PI * 2);
                var radius = Random(40f, 80);

                var startPos = hit.Position + hit.StackOffset;
                var endPos = new Vector2(radius * (float)Cos(angle) + startPos.X, radius * (float)Sin(angle) + startPos.Y);
                var duration = Random(1000, 2000);

                var sprite = pool.Get(hit.StartTime, hit.StartTime + duration);
                sprite.Scale(OsbEasing.In, hit.StartTime, hit.StartTime + duration, radius * 6E-4, 0);
                sprite.Move(OsbEasing.OutExpo, hit.StartTime, hit.StartTime + duration, startPos, endPos);

                if ((Color4)sprite.ColorAt(hit.StartTime) != hit.Color) sprite.Color(hit.StartTime, hit.Color);
            }
        }
        void Trail(int startTime, int endTime, bool collect = false)
        {
            using (var pool = new SpritePool(GetLayer(""), "sb/hl.png", (light, start, end) =>
            {
                light.Additive(start);
                light.Scale(start, 0.1);
            })) 
            foreach (var hit in Beatmap.HitObjects) if (hit.StartTime >= startTime && hit.StartTime <= endTime)
            {
                var pos = hit.Position + hit.StackOffset;
                var sprite = pool.Get(hit.StartTime, hit.StartTime + 1500);
                sprite.Move(hit.StartTime, pos);
                sprite.Fade(hit.StartTime, hit.StartTime + 1500, 0.45, 0);
                if ((Color4)sprite.ColorAt(hit.StartTime) != hit.Color) sprite.Color(hit.StartTime, hit.Color);

                if (hit is OsuSlider)
                {
                    var timestep = Beatmap.GetTimingPointAt((int)hit.StartTime).BeatDuration / 16;
                    var sTime = hit.StartTime + timestep;

                    while (true)
                    {
                        var stepSprite = pool.Get(sTime, sTime + 1500);

                        var slidePos = hit.PositionAtTime(sTime);
                        if (collect)
                        {   
                            stepSprite.Move(OsbEasing.InBack, sTime, sTime + 1500, slidePos, new Vector2(320, 240));
                            stepSprite.Color(OsbEasing.InQuart, sTime, sTime + 1500, hit.Color, Color4.White);
                        }
                        else 
                        {
                            stepSprite.Move(sTime, slidePos);
                            if ((Color4)stepSprite.ColorAt(sTime) != hit.Color) stepSprite.Color(sTime, hit.Color);
                        }
                        stepSprite.Fade(OsbEasing.InQuad, sTime, sTime + 1500, 0.35, 0);

                        if (sTime > hit.EndTime) break;
                        sTime += timestep;
                    }
                }
            }
        }
    }
}