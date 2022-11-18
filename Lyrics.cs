using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;

namespace StorybrewScripts
{
    class Lyrics : StoryboardObjectGenerator
    {
        readonly float FontScale = .3f;

        FontGenerator Font;
        FontGenerator JapFont;

        double BeatDuration;

        protected override void Generate()
        {
            BeatDuration = GetBeatDuration(4298);
            Font = LoadFont("sb/f", new FontDescription
            {
                FontPath = $"{ProjectPath}/assetlibrary/NotoSansJP.otf",
                Color = Color4.White,
                FontSize = 50,
                TrimTransparency = true
            });

            using (var pool = new SpritePools(GetLayer("")))
            {
                Action<string, int, int> MakeLine = (line, startTime, endTime) =>
                {
                    float width = 0;
                    float height = 0;

                    foreach (var letter in line)
                    {
                        var texture = Font.GetTexture(letter.ToString());
                        width += texture.BaseWidth * FontScale;
                        height = Math.Max(height, texture.BaseHeight * FontScale);
                    }

                    var letterX = 320 - width * .5f;
                    var hasBox = false;

                    foreach (var letter in line)
                    {
                        var texture = Font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            Vector2 position = new Vector2(letterX, 420) + texture.OffsetFor(OsbOrigin.Centre) * FontScale;
                            if (!hasBox)
                            {
                                var box = pool.Get(startTime, endTime, "sb/p.png", OsbOrigin.Centre, new Vector2(320, position.Y),
                                    (p, s, e) => p.Color(s, Color4.Black));

                                box.ScaleVec(OsbEasing.OutCirc, startTime, startTime + BeatDuration, 0, height + 5, width + 15, height + 5);
                                box.ScaleVec(OsbEasing.InExpo, endTime - BeatDuration, endTime, width + 15, height + 5, 0, height + 5);

                                hasBox = true;
                            }

                            var sprite = pool.Get(startTime, endTime, texture.Path, OsbOrigin.Centre, new Vector2(0, position.Y),
                                (p, s, e) => p.Scale(s, FontScale));

                            sprite.MoveX(OsbEasing.OutCirc, startTime, startTime + BeatDuration, 320, position.X);
                            sprite.MoveX(OsbEasing.InQuint, endTime - BeatDuration / 2, endTime, position.X, 320);
                            sprite.Fade(startTime, startTime + BeatDuration, 0, 1);
                            sprite.Fade(endTime - BeatDuration / 2, endTime, 1, 0);
                        }
                        letterX += texture.BaseWidth * FontScale;
                    }
                };

                MakeLine("Time is over,", 4298, 5693);
                MakeLine("どうやって make me groove?", 6042, 8484);
                MakeLine("誰の目も気にしないで", 8833, 12496);
                MakeLine("You will get high 宇宙へ", 12670, 14589);
                MakeLine("I'm a dreamer", 15461, 16856);
                MakeLine("妄想じゃない that's true", 17205, 19647);
                MakeLine("終わらない future sound を", 19996, 22961);
                MakeLine("Music 僕らずっと", 23135, 24530);
                MakeLine("So hype", 24879, 25751);

                MakeLine("Time is over-er-er-", 71275, 73193);
                MakeLine("あって make me groove?", 73368, 75461);
                MakeLine("誰の目も気にしないで", 75809, 79472);
                MakeLine("You will get high 宇宙へ", 79647, 81565);
                MakeLine("I'm a dreamer", 82437, 83833);
                MakeLine("妄想じゃない that's true", 84182, 86623);
                MakeLine("終わらない future sound を", 86972, 89937);
                MakeLine("Music 僕らずっと", 90112, 91507);
                MakeLine("So hype", 91856, 92903);

                MakeLine("(Time is over)", 93600, 95344);
                MakeLine("(I'm a dreamer)", 104763, 106507);
                MakeLine("終わらない future sound を", 109298, 112263);
                MakeLine("Music 僕らずっと", 112437, 113920);

                MakeLine("(ひとりじゃないのに", 207321, 209763);
                MakeLine("どうして どうして)", 210112, 212728);
                MakeLine("(ここにはいないような", 218309, 220926);
                MakeLine("どこへ どこへ)", 221275, 223891);

                MakeLine("ひとりじゃないのに", 229647, 232089);
                MakeLine("どうして どうして", 232437, 234879);
                MakeLine("ここにはいないような", 235054, 237670);
                MakeLine("どこへ どこへ", 238019, 240461);
                MakeLine("I still can't forget あの時み", 240809, 243426);
                MakeLine("たいな想像 saw 10 p.m. show", 243600, 246740);
                MakeLine("ユメノセカイヘ", 247089, 250577);

                MakeLine("ひとりじゃないのに", 279879, 282321);
                MakeLine("どうして どうして", 282670, 285112);
                MakeLine("(ここにはいないような", 290868, 293484);
                MakeLine("どこへ どこへ)", 293833, 296449);
            }
        }
    }
}