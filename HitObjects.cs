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
            Trail(25926, 37001, true);
            DotBurst(42670, 46856);
            DotBurst(73368, 92903);
            OriginateLaser(48949, 69269);
            Trail(115228, 115228);
            DotBurst(115577, 126304);
            Trail(120461, 120461);
            Trail(126391, 145926, true);
            DotBurst(148019, 151158);
            OriginateLaser(151507, 173746);
            DotBurst(173833, 183513);
            Trail(179414, 184908);
            Trail(240809, 251886);
            DotBurst(247089, 251886);
            OriginateLaser(258251, 265839);
            DotBurst(265926, 268368);
            OriginateLaser(268716, 278571);

            BackHL(93251, 103542);
            BackHL(191972, 227026);
            BackHL(281275, 300984);
        }
        void DotBurst(int startTime, int endTime)
        {
            using (var pool = new SpritePool(GetLayer(""), "sb/dot.png", true)) foreach (var hit in Beatmap.HitObjects) 
                if (hit.StartTime >= startTime && hit.EndTime <= endTime) for (var i = 0; i < Random(20, 25); i++)
            {
                var angle = Random(PI * 2);
                var radius = Random(50f, 100);

                var startPos = hit.Position + hit.StackOffset;
                var endPos = new Vector2((int)(radius * Cos(angle) + startPos.X), (int)(radius * Sin(angle) + startPos.Y));

                var duration = Random(1000, 2000);

                var sprite = pool.Get(hit.StartTime, hit.StartTime + duration);
                sprite.Scale(OsbEasing.InQuad, hit.StartTime, hit.StartTime + duration, radius * 5E-4, 0);
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
                        var stepSprite = pool.Get(sTime, hit.EndTime + 1500);

                        var slidePos = hit.PositionAtTime(sTime);
                        if (collect)
                        {   
                            stepSprite.Move(OsbEasing.InExpo, sTime, hit.EndTime + 1500, slidePos, new Vector2(320, 240));
                            stepSprite.Color(OsbEasing.InQuart, sTime, hit.EndTime + 1500, hit.Color, Color4.White);
                        }
                        else 
                        {
                            stepSprite.Move(sTime, slidePos);
                            if ((Color4)stepSprite.ColorAt(sTime) != hit.Color) stepSprite.Color(sTime, hit.Color);
                        }
                        stepSprite.Fade(OsbEasing.InQuad, sTime, hit.EndTime + 1500, 0.35, 0);

                        if (sTime > hit.EndTime) break;
                        sTime += timestep;
                    }
                }
            }
        }
        void OriginateLaser(int startTime, int endTime)
        {
            var i = 0;

            using (var pool = new SpritePools(GetLayer("")))
            foreach (var hit in Beatmap.HitObjects) if (hit.StartTime >= startTime && hit.StartTime <= endTime)
                for (var j = 0; j < 2; j++)
            {
                if (i > 1) i = 0;

                var sprite = pool.Get(hit.StartTime, hit.EndTime + 1000, "sb/px.png", true);
                var angle = .17 + i * PI / 2;
                if (sprite.RotationAt(hit.StartTime) != angle) sprite.Rotate(hit.StartTime, angle);

                sprite.Move(hit.StartTime, hit.Position + hit.StackOffset);
                sprite.ScaleVec(OsbEasing.OutQuint, hit.StartTime, hit.EndTime + 1000, 2.5, 0, 1, 1400);

                if ((Color4)sprite.ColorAt(hit.StartTime) != hit.Color) sprite.Color(hit.StartTime, hit.Color);
                sprite.Fade(hit.StartTime, hit.EndTime + 1000, 1, 0);

                i++;
            }
        }
        void BackHL(int startTime, int endTime)
        {
            using (var pool = new SpritePool(GetLayer("BG"), "sb/hl.png", new Vector2(320, 530), (flash, start, end) =>
            {
                flash.Additive(start);
                flash.Scale(start, 10);
            }))
            foreach (var hit in Beatmap.HitObjects) if (hit.StartTime >= startTime - 5 && hit.StartTime <= endTime + 5)
            {
                var flash = pool.Get(hit.StartTime, hit.EndTime + 500);
                flash.Fade(hit.StartTime, hit.EndTime + 500, .125, 0);
            }
        }
    }
}