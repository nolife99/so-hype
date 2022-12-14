using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Animations;

using static StorybrewCommon.OpenTKUtil.MathHelper;

namespace StorybrewScripts
{
    class ParticleManager : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            RingRise(25926, 42670);
            RingRise(73368, 92903);
            RingRise(104065, 113833);
            BeatShapes(48949, 64996);
            BeatShapes(115577, 145926);
            BeatShapes(151507, 172437, true);
            BeatShapes(258251, 265926);
            BeatShapes(268716, 274298);
            PulsingSquare(104065, 113833);

            RotatingLines();
            SpectrumParticles();
        }
        void RotatingLines()
        {
            var startTime = 73368;
            var endTime = 92903;
            var amount = 20;
            double angle = 90;
            double radius = 150;

            for (var i = 0; i < amount; i++)
            {
                var ConnectionAngle = Pi / amount * 1.5;

                var position = new Vector2(
                    (float)(320 + Cos(angle) * radius),
                    (float)(240 + Sin(angle) * radius));

                var lines = GetLayer("RotLines").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(0, 0));

                lines.ScaleVec(startTime, 1.5, 20);
                lines.Fade(endTime, endTime + 1000, 1, 0);

                var keyframe = new KeyframedValue<Vector2>();

                var timeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / 4;
                for (var time = (double)startTime; time < endTime + timeStep; time += timeStep)
                {
                    angle -= 0.06;

                    var nPosition = new Vector2(
                        (float)(320 + Cos(angle) * radius),
                        (float)(240 + Sin(angle) * radius));

                    keyframe.Add(time, position);

                    position = nPosition;
                }
                angle += ConnectionAngle / (amount / 3);

                keyframe.Simplify2dKeyframes(.9, v => v);
                keyframe.ForEachPair((start, end) => lines.Move(start.Time, end.Time, start.Value, end.Value));

                var pos = new Vector2(
                    (float)(320 + Cos(angle) * radius),
                    (float)(240 + Sin(angle) * radius));

                var Rotation = Atan2(position.Y - pos.Y, position.X - pos.X) - Pi;

                lines.Rotate(startTime, endTime, Rotation, Rotation - Pi * 2);
            }
        }
        void RingRise(int start, int end)
        {
            for (var i = 0; i < 70; i++)
            {
                var sprite = GetLayer("").CreateSprite("sb/ringw.png");

                var fade = Random(.5, 1);
                var fadeTime = Random(200, 500);
                var duration = Random(2500, 5000);
                var startX = Random(50, 590);
                var middleX = startX + Random(-100, 100);
                var endX = middleX + Random(-50, 50);

                sprite.ColorHsb(start, 0, 0, Random(1f));
                sprite.StartLoopGroup(start + i * 70, (end - (start + i * 35)) / duration);
                sprite.Additive(0, duration);
                sprite.Fade(0, fadeTime, 0, fade);
                sprite.Fade(duration - fadeTime, duration, fade, 0);
                sprite.Scale(0, duration, Random(.025, .05), Random(.025, .05));
                sprite.MoveY(0, duration, Random(400, 500), Random(-20, 0));

                var shift = Random(50, 150);
                if (startX >= 320)
                {
                    sprite.MoveX(OsbEasing.InOutSine, 0, duration / 2, startX + shift, middleX + shift);
                    sprite.MoveX(OsbEasing.InOutSine, duration / 2, duration, middleX + shift, endX + shift);
                }
                else if (startX < 320)
                {
                    sprite.MoveX(OsbEasing.InOutSine, 0, duration / 2, startX - shift, middleX - shift);
                    sprite.MoveX(OsbEasing.InOutSine, duration / 2, duration, middleX - shift, endX - shift);
                }
                sprite.EndGroup();
            }
        }
        void BeatShapes(int start, int end, bool circ = false)
        {
            for (int i = 0; i < (circ ? 20 : 30); i++)
            {
                var timeStep = Beatmap.GetTimingPointAt(start).BeatDuration;
                var realStart = start + timeStep * (i / 2);
                var fade = Random(.3, .6);

                var square = GetLayer("beat").CreateSprite(circ ? "sb/c.png" : "sb/p.png", 
                    OsbOrigin.Centre, new Vector2(Random(-107, 747), 0));

                square.Fade(realStart, realStart + 150, 0, fade);
                square.Scale(realStart, circ ? Random(.08, .16) : Random(10, 50));
                square.Fade(end, end + 250, fade, 0);
                square.Additive(realStart);

                var posY = 500;
                for (double s = realStart; s < end; s += timeStep)
                {
                    var endY = posY - Random(27, 77);
                    square.MoveY(OsbEasing.OutQuart, s, s + timeStep, posY, endY);

                    if (endY < -20)
                    {
                        posY = 510;
                        square.MoveX(s + timeStep, Random(-107, 747));
                    }
                    else posY = endY;
                }

                if (circ) continue;
                for (double r = realStart; r < end; r += timeStep * 2)
                {
                    square.Rotate(OsbEasing.OutQuart, r, r + timeStep, 0, PiOver4);
                    square.Rotate(OsbEasing.OutQuart, r + timeStep, r + timeStep * 2, PiOver4, PiOver2);
                }
            }
        }
        void PulsingSquare(int startTime, int endTime)
        {
            var easing = OsbEasing.OutQuad;
            var timeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration;

            var Pix = GetLayer("BeatScale").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = startTime; i < endTime - 1; i += timeStep)
            {
                Pix.Scale(easing, i, i + timeStep, 125, 85);
                Pix.Fade(easing, i, i + timeStep, 0.5, 1);
            }
            Pix.Rotate(startTime, MathHelper.DegreesToRadians(45));
            Pix.Scale(easing, endTime, endTime + timeStep, 140, 0);

            double angle = 0;
            var scaleStart = 85;
            var scaleEnd = 150;
            var pos = new Vector2(320, 240);

            for (int i = 0; i < 4; i++)
            {
                var startPos = new Vector2(
                    (float)(pos.X + Cos(angle) * scaleStart),
                    (float)(pos.Y + Sin(angle) * scaleStart));

                var endPos = new Vector2(
                    (float)(pos.X + Cos(angle) * scaleEnd),
                    (float)(pos.Y + Sin(angle) * scaleEnd));

                double startScale = Sqrt(scaleStart * scaleStart + scaleStart * scaleStart);
                double endScale = Sqrt(scaleEnd * scaleEnd + scaleEnd * scaleEnd);

                var border = GetLayer("BeatScale").CreateSprite("sb/p.png", OsbOrigin.BottomCentre);
                border.Rotate(startTime, angle - Pi / 4);
                for (double s = startTime; s < endTime - 1; s += timeStep)
                {
                    border.ScaleVec(easing, s, s + timeStep, 1.23, startScale + 0.5, 0.6, endScale);
                    border.Move(easing, s, s + timeStep, startPos, endPos);
                    border.Fade(OsbEasing.In, s, s + timeStep, 0.8, 0);
                }
                angle += Pi / 2;
            }
        }
        void SpectrumParticles()
        {
            using (var pool = new SpritePool(GetLayer("specPart"), "sb/p.png", (sprite, start, end) =>
            {
                sprite.Scale(start, Random(5, 20));
                sprite.Rotate(start, Pi / 4 + (Random(-.1, .1)));
                sprite.Additive(start);
            }))
            {
                System.Action<int, int> SpectrumParticle = (start, end) =>
                {
                    for (var i = start; i < end - 800; i += 15)
                    {
                        var duration = Random(1000, 2200);
                        var top = Random(0, 2) % 2 == 0;
                        var fade = Random(.25, .5);
                        var endPos = top ? new Vector2(320, 0) : new Vector2(320, 480);
                        var sprite = pool.Get(i, i + duration);

                        sprite.Fade(i, i + 300, 0, fade);
                        sprite.Move(i, i + duration, new Vector2(Random(-107, 747), 240), endPos);
                        if (i + duration < end) sprite.Fade(i + duration - 300, i + duration, fade, 0);
                        else if (i + duration > end && sprite.OpacityAt(end - 500) >= fade && 
                        sprite.PositionAt(end - 500).Y != 480 || sprite.PositionAt(end - 500).Y != 0) 
                        sprite.Fade(end - 500, end, fade, 0);
                    }
                };
                SpectrumParticle(151507, 172437);
                SpectrumParticle(48949, 69182);
            }
        }
    }
}
