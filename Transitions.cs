using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Transitions : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            BarStatic(25926, 26623, 30, true, true);
            BarTrans(45461, 46856, finalize: p => p.Color(47554, 0, 0, 0));
            BarTrans(64996, 65780, .75, finalize: p => p.Color(66042, 0, 0, 0));

            Action<int> SequenceTransition = start =>
            {
                BarTrans(start, 784 + start, .75, finalize: p => p.Color(1046 + start, 0, 0, 0));
                BarTrans(697 + start, 1046 + start, .5, true, p => p.Color(2790 + start, 1, 1, 1));
                BarTrans(1046 + start, 1831 + start, .75, finalize: p => p.Color(2790 + start, 0, 0, 0));
                BarStatic(1744 + start, 2093 + start, 4, true, finalize: p => p.Color(3139 + start, 1, 1, 1));
                BarTrans(2093 + start, 3139 + start, 1, true, finalize: p => p.Color(3837 + start, 0, 0, 0));
                BarStatic(3488 + start, 4186 + start, 8, finalize: p => p.Color(5581 + start, 1, 1, 1));
                BarStatic(4186 + start, 5581 + start, 16, finalize: p => p.Color(4186 + start, 0, 0, 0));
            };
            SequenceTransition(64996);

            BarStatic(104065, 105461, 10, true, true);
            BarTrans(124996, 126391, side: true, finalize: p => p.Color(126740, 1, 1, 1));

            BarTrans(172437, 173833, .5);

            BarStatic(229647, 231042, 10, true, true);

            BarTrans(249182, 250577, finalize: p => p.Color(251972, 0, 0, 0));
            BarTrans(250577, 251972, side: true);
            BarTrans(257030, 257554, finalize: p => p.Color(258251, 1, 1, 1));

            SequenceTransition(274298);
        }
        void BarStatic(int startTime, int endTime, int amount, bool side = false, bool _out = false, Action<OsbSprite> finalize = null)
        {
            var bDur = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var width = (side ? 480d : 854d) / amount;
            var offset = (side ? 0 : -107) + width * .5;

            for (var i = 0; i < amount; i++)
            {
                if (side)
                {
                    var sprite = GetLayer("").CreateSprite("sb/px.png",
                        i % 2 == 0 ? OsbOrigin.CentreLeft : OsbOrigin.CentreRight,
                        new Vector2(i % 2 == 0 ? -107 : 747, (float)offset));
                    sprite.ScaleVec(OsbEasing.OutExpo, startTime, endTime, _out ? 854 : 0, width, _out ? 0 : 854, width);
                    if (finalize != null) finalize(sprite);
                }
                else
                {
                    var sprite = GetLayer("").CreateSprite("sb/px.png",
                        i % 2 == 0 ? OsbOrigin.TopCentre : OsbOrigin.BottomCentre,
                        new Vector2((float)offset, i % 2 == 0 ? 0 : 480));
                    sprite.ScaleVec(OsbEasing.OutExpo, startTime, endTime, width, _out ? 480 : 0, width, _out ? 0 : 480);
                    if (finalize != null) finalize(sprite);
                }
                
                offset += width;
            }
        }
        void BarTrans(int startTime, int endTime, double beatMult = .25, bool side = false, Action<OsbSprite> finalize = null)
        {
            var bDur = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var spriteAmount = (int)Math.Round((endTime - startTime) / (bDur * beatMult));
            var width = (side ? 480d : 854d) / spriteAmount;
            var offset = (side ? 0 : -107) + width * .5;
            var delay = 0d;

            for (var i = 0; i < spriteAmount; i++)
            {
                if (side)
                {
                    var sprite = GetLayer("").CreateSprite("sb/px.png",
                        i % 2 == 0 ? OsbOrigin.CentreLeft : OsbOrigin.CentreRight,
                        new Vector2(i % 2 == 0 ? -107 : 747, (float)offset));
                    sprite.ScaleVec(OsbEasing.OutCubic, startTime + delay, endTime, 0, width, 854, width);
                    if (finalize != null) finalize(sprite);
                }
                else
                {
                    var sprite = GetLayer("").CreateSprite("sb/px.png",
                        i % 2 == 0 ? OsbOrigin.TopCentre : OsbOrigin.BottomCentre,
                        new Vector2((float)offset, i % 2 == 0 ? 0 : 480));
                    sprite.ScaleVec(OsbEasing.OutCubic, startTime + delay, endTime, width, 0, width, 480);
                    if (finalize != null) finalize(sprite);
                }
                
                offset += width;
                delay += bDur * beatMult;
            }
        }
    }
}