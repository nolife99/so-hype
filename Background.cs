using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Background : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
		    ScreenFillHighlight(25228, 25926, 25926, 26275);
            ScreenFillHighlight(102670, 103368, 104065, 104414);
            ScreenFillHighlight(226856, 228949, 229647, 229996);

            Flash(28542, 29414, .6);
            Flash(29937, 30809, .5);
            Flash(30635, 31507, .5);
            Flash(30984, 31507, .5);
            Flash(31246, 31507, .5);
            Flash(31507, 31856, .6);
            Flash(32728, 33600, .6);
            Flash(34123, 34996, .6);
            Flash(35519, 37089, .5);
            Flash(36042, 37089, .5);
            Flash(36391, 37089, .5);
            Flash(36740, 37089, .5);
            Flash(37089, 38484, .6);
            Flash(42670, 44065, .5);
            Flash(48949, 51042);
            Flash(53833, 55228, .6);
            Flash(59414, 60810, .8);

            Action<int> FlashSequence = start =>
            {
                Flash(start, 66042 - 64996 + start, .5);
                Flash(65257 - 64996 + start, 66042 - 64996 + start, .4);
                Flash(65519 - 64996 + start, 66042 - 64996 + start, .3);
                Flash(66042 - 64996 + start, 67089 - 64996 + start, .5);
                Flash(66304 - 64996 + start, 67089 - 64996 + start, .4);
                Flash(66565 - 64996 + start, 67089 - 64996 + start, .3);
                Flash(67089 - 64996 + start, 67786 - 64996 + start, .5);
                Flash(67437 - 64996 + start, 67786 - 64996 + start, .5);
                Flash(67786 - 64996 + start, 69182 - 64996 + start, .5);
                Flash(68484 - 64996 + start, 69182 - 64996 + start, .5);
                Flash(69182 - 64996 + start, 69879 - 64996 + start, .5);
            };
            FlashSequence(64996);

            Flash(73368, 74763, .6);
            Flash(81740, 83135, .6);
            Flash(92903, 94298, .6);
            Flash(109647, 110344, .6);
            Flash(115577, 116623, .5);
            Flash(126740, 129182);
            Flash(137554, 138949, .8);
            Flash(145926, 147670, .6);
            Flash(148716, 149414, .5);
            Flash(151507, 154298);
            Flash(157437, 158484, .8);
            Flash(162670, 164065, .8);
            Flash(168251, 169647, .8);
            Flash(173833, 175228, .8);
            Flash(184996, 189182, .5);
            Flash(240809, 241507, .6);
            Flash(251972, 254763, .7);
            Flash(258251, 260344);
            Flash(263135, 264182, .6);
            Flash(265926, 267321, .6);
            Flash(268716, 271507);
            FlashSequence(274298);

            BlurBG(25926, 25926, 46856, 48251);
            MainBG(48949, 48949, 69879, 70577);
            BlurBG(71972, 73368, 92903, 93251);
            BlurBG(104065, 104065, 126391, 126740);
            MainBG(126391, 126740, 145926, 147321);
            BlurBG(145926, 147321, 151158, 151158);
            MainBG(151507, 151507, 172437, 173833);
            BlurBG(172437, 173833, 184996, 187786);
            BlurBG(229647, 229647, 240809, 241158);
            MainBG(240809, 241158, 251972, 252147);
            BlurBG(251972, 252147, 256158, 257554);
            MainBG(258251, 258251, 279879, 281275);

            Vignette(25228, 25926, 92903, 93251);
            Vignette(102670, 104065, 184996, 187786);
            Vignette(226856, 229647, 279879, 281275);
        }
        void ScreenFillHighlight(int startScale, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("Flash").CreateSprite("sb/hl.png", OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(OsbEasing.InQuint, startScale, startTime, 0, 5);
            sprite.Fade(endTime, endFade, 1, 0);
        }
        void Flash(int startTime, int endTime, double fade = 1)
        {
            var sprite = GetLayer("Flash").CreateSprite("sb/p.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            sprite.ScaleVec(startTime, 854, 480);
            sprite.Fade(startTime, endTime, fade, 0);
            sprite.Additive(startTime);
        }
        void BlurBG(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("BG").CreateSprite("sb/b/blur.png", OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap("sb/b/blur.png").Height);
            if (startTime != startFade) sprite.Fade(OsbEasing.Out, startFade, startTime, 0, 1);
            sprite.Fade(OsbEasing.In, endTime, endFade, 1, 0);
        }
        void MainBG(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("BG").CreateSprite("63888719_p0.jpg", OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap("63888719_p0.jpg").Height);
            if (startTime != startFade) sprite.Fade(OsbEasing.Out, startFade, startTime, 0, 1);
            sprite.Fade(OsbEasing.In, endTime, endFade, 1, 0);
        }
        void Vignette(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("Vignette").CreateSprite("sb/vig.png", OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap("sb/vig.png").Height);
            if (startTime != startFade) sprite.Fade(OsbEasing.Out, startFade, startTime, 0, .8);
            sprite.Fade(OsbEasing.In, endTime, endFade, .8, 0);
        }
    }
}