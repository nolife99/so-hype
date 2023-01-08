using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace StorybrewScripts
{
    class Lyrics : StoryboardObjectGenerator
    {
        FontGenerator font;
        double beat;

        protected override void Generate()
        {
            var scale = .3f;
            beat = Beatmap.GetTimingPointAt(4298).BeatDuration;
            
            font = LoadFont("sb/f", new FontDescription
            {
                FontPath = $"{ProjectPath}/assetlibrary/NotoSansJP.otf",
                Color = Color4.White,
                FontSize = 50,
                TrimTransparency = true
            });

            MakePixelLine("誰の目も気にしないで", 28542, 31507);
            MakePixelLine("終わらない future sound を", 34123, 37089);
            MakePixelLine("終わらない future sound を", 56449, 59414);
            MakePixelLine("Time is over...", 71275, 73368);
            MakePixelLine("Time is over...", 93600, 95693);
            MakePixelLine("I'm a dreamer...", 104763, 106856, 20);
            MakePixelLine("終わらない future sound を", 159705, 161972);
            MakePixelLine("終わらない future sound を", 170868, 173833);
            MakePixelLine("終わらない future sound を", 265751, 268368);

            using (var pool = new SpritePools(GetLayer("")))
            {
                Action<string, int, int> MakeLine = (line, startTime, endTime) =>
                {
                    var width = 0f;
                    var height = 0f;

                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        width += texture.BaseWidth * scale;
                        height = Math.Max(height, texture.BaseHeight * scale);
                    }

                    var letterX = 320 - width * .5f;
                    var hasBox = false;

                    foreach (var letter in line)
                    {
                        var texture = font.GetTexture(letter.ToString());
                        if (!texture.IsEmpty)
                        {
                            var position = new Vector2(letterX, 420) + texture.OffsetFor(OsbOrigin.Centre) * scale;
                            if (!hasBox)
                            {
                                var box = pool.Get(startTime, endTime, "sb/px.png", OsbOrigin.Centre, new Vector2(320, position.Y),
                                    (p, s, e) => p.Color(s, 0, 0, 0));

                                box.ScaleVec(OsbEasing.OutQuart, startTime, startTime + beat, 0, height + 5, width + 15, height + 5);
                                box.ScaleVec(OsbEasing.InQuint, endTime - beat, endTime, width + 15, height + 5, 0, height + 5);

                                hasBox = true;
                            }

                            var sprite = pool.Get(startTime, endTime, texture.Path, OsbOrigin.Centre, new Vector2(0, position.Y),
                                (p, s, e) => p.Scale(s, scale));

                            sprite.MoveX(OsbEasing.OutQuart, startTime, startTime + beat, 320, position.X);
                            sprite.MoveX(OsbEasing.InQuint, endTime - beat / 2, endTime, position.X, 320);
                            sprite.Fade(startTime, startTime + beat, 0, 1);
                            sprite.Fade(endTime - beat / 2, endTime, 1, 0);
                        }
                        letterX += texture.BaseWidth * scale;
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

                MakeLine("あって make me groove?", 73368, 75461);
                MakeLine("誰の目も気にしないで", 75809, 79472);
                MakeLine("You will get high 宇宙へ", 79647, 81565);
                MakeLine("I'm a dreamer", 82437, 83833);
                MakeLine("妄想じゃない that's true", 84182, 86623);
                MakeLine("終わらない future sound を", 86972, 89937);
                MakeLine("Music 僕らずっと", 90112, 91507);
                MakeLine("So hype", 91856, 92903);

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
        void MakePixelLine(string line, int startTime, int endTime, float fontY = 197)
        {
            Func<string, List<Vector2>> PixelArray = path =>
            {
                var bitmapOri = new Bitmap(MapsetPath + "/" + path);
                var bitmap = new Bitmap(bitmapOri, new Size(bitmapOri.Width, bitmapOri.Height));

                var sprites = new List<Vector2>();
                for (var y = 0; y < bitmap.Height; y += Random(7, 8) / 2) for (var x = 0; x < bitmap.Width; x += Random(7, 8) / 2) 
                {
                    var pixel = bitmap.GetPixel(x, y);
                    if (pixel.R <= 0 || pixel.A <= 0) continue;

                    sprites.Add(new Vector2(x, y) * .8f);
                }

                bitmap.Dispose();
                bitmapOri.Dispose();
                
                return sprites;
            };

            var width = 0f;
            var height = 0f;

            foreach (var letter in line)
            {
                var texture = font.GetTexture(letter.ToString());
                width += texture.BaseWidth * .8f;
                height = Math.Max(height, texture.BaseHeight * .8f);
            }

            var letterX = 320 - width * .5f;

            foreach (var letter in line)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, fontY) + texture.OffsetFor(OsbOrigin.TopLeft) * .8f;
                    var pixels = PixelArray(texture.Path);
                    
                    foreach (var pixel in pixels)
                    {
                        var sprite = GetLayer("Pixel").CreateSprite("sb/px.png");
                        sprite.Move(OsbEasing.OutExpo, startTime, startTime + beat * 2, 
                            pixel + position + new Vector2(Random(-20f, 20), Random(-20f, 20)), pixel + position);
                        sprite.Move(OsbEasing.OutCubic, endTime, endTime + beat * 2, 
                            pixel + position, pixel + position + new Vector2(Random(-20f, 20), Random(-20f, 20)));

                        sprite.Scale(OsbEasing.OutElastic, startTime, endTime - beat, 0, Random(1f, 3));
                        sprite.Fade(endTime, endTime + beat * 2, .8, 0);
                        sprite.ColorHsb(startTime, endTime - beat, 0, 0, 1, 0, 0, .5);
                        sprite.Additive(startTime);
                    }
                }
                letterX += texture.BaseWidth * .8f;
            }
        }
    }
}