using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Background : StoryboardObjectGenerator
    {
        int bitmapH, beat;
        double beatD;
        const string bg = "63888719_p0.jpg", blur = "sb/blur.png";

        /*
            Why did I use for loops instead of OsbLoops?
            Due to osu!'s use of integers instead of doubles for times, loops can get quite inaccurate over time.
            In for loops, incrementing is accurate, although it is more command-heavy.
            This song in particular has a precise beat duration, so this approach was better than OsbLoops.
        */
        protected override void Generate()
        {
            bitmapH = GetMapsetBitmap(blur).Height;
            beatD = Beatmap.GetTimingPointAt(48949).BeatDuration;
            beat = (int)beatD;

            ScreenFillHighlight(25228, 25926, 25926, 26275);
            ScreenFillHighlight(102670, 103891, 104065, 104414);
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
            Flash(109647, 110344, .3);
            Flash(115577, 116623, .5);
            Flash(126740, 129182);
            Flash(137554, 138949, .8);
            Flash(145926, 147670, .6);
            Flash(148716, 149414, .5);
            Flash(151507, 154298);
            Flash(157437, 158484);
            Flash(162670, 164065);
            Flash(168251, 169647, .8);
            Flash(173833, 175228, .8);
            Flash(179414, 180809, .5);
            Flash(184996, 189182);
            Flash(240809, 241507, .6);
            Flash(251972, 254763);
            Flash(258251, 260344);
            Flash(263135, 264182, .6);
            Flash(265926, 267321, .6);
            Flash(268716, 271507);
            FlashSequence(274298);

            MainBG(48949, 48949, 69879, 70577);
            MainBG(126391, 126740, 145926, 147321);
            BlurBG(145926, 147321, 151158, 151158);
            MainBG(151507, 151507, 172437, 173833);
            BlurBG(172437, 173833, 184996, 187786);
            MainBG(240809, 241158, 251972, 252147);
            BlurBG(251972, 252147, 256158, 257554);
            MainBG(258251, 258251, 279879, 281275);

            Vignette(25228, 25926, 92903, 93251);
            Vignette(102670, 104065, 184996, 187786);
            Vignette(226856, 229647, 279879, 281275);

            Section1();
            Kiai1();
            Section2();
            Kiai2();
            Kiai3();
            AfterCalm1();
            LastKiai();
        }
        void ScreenFillHighlight(int startScale, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("Flash").CreateSprite("sb/hl.png", OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(OsbEasing.InQuint, startScale, startTime, 0, 5);
            sprite.Fade(endTime, endFade, 1, 0);
        }
        void Flash(int startTime, int endTime, double fade = 1)
        {
            var sprite = GetLayer("Flash").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            sprite.ScaleVec(startTime, 854, 480);
            sprite.Fade(startTime, endTime, fade, 0);
            sprite.Additive(startTime);
        }
        void BlurBG(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap(blur).Height);
            if (startTime != startFade) sprite.Fade(OsbEasing.Out, startFade, startTime, 0, 1);
            sprite.Fade(OsbEasing.In, endTime, endFade, 1, 0);
        }
        void MainBG(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap(bg).Height);
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

        #region Sections

        void Section1()
        {
            var back = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            back.ScaleVec(25926, 854, 480);
            back.ColorHsb(25926, 30, .5, .5);
            back.Fade(37089, 38484, .7, 0);

            var back2 = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            back2.ColorHsb(37089, 190, .15, 1);
            back2.ScaleVec(37089, 854, 480);
            back2.Fade(37089, 38484, 0, .7);
            back2.Fade(46856, 48600, .7, 0);

            var diamond = GetLayer("diam").CreateSprite("sb/px.png", OsbOrigin.Centre, new Vector2(320, 240));
            diamond.Scale(OsbEasing.OutExpo, 37089, 37786, 854, 130);
            diamond.Fade(37089, 37437, 0, 1);
            diamond.Rotate(OsbEasing.OutQuad, 37089, 37437, -Math.PI / 4, Math.PI / 4);
            diamond.Rotate(OsbEasing.InOutSine, 37437, 47554, Math.PI / 4, Math.PI * 20);
            diamond.Scale(OsbEasing.InOutSine, 45461, 47554, 130, 854);
            diamond.Fade(48251, 48949, 1, 0);
        }
        void Kiai1()
        {
            var snare = GetLayer("").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 48949; i < 64298; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }

            var clap = GetLayer("").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 49298; i < 64647; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }
        }
        void Section2()
        {
            var back = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            back.ColorHsb(71972, 190, .15, 1);
            back.ScaleVec(71972, 854, 480);
            back.Fade(71972, 73368, 0, .7);
            
            for (double i = 73716; i < 92554; i += beatD * 2)
                back.Fade(OsbEasing.In, i, i + beatD * 2, .9, .7);

            back.Fade(93251, 94298, .7, 0);
            back.Fade(104065, .7);
            back.ColorHsb(113833, 115577, 190, .15, 1, 20, .15, 1);
            back.Fade(126391, 126740, .7, 0);
        }
        void Kiai2()
        {
            var clap = GetLayer("").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 126740; i < 145926; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }

            var snare = GetLayer("").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 127089; i < 145926; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }
        }
        void Kiai3()
        {
            var clap = GetLayer("").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 149065; i < 151071; i += beatD * 1.5)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 1.5, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 1.5, .8, 0);
            }
            for (double i = 151856; i < 156827; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 157437; i < 161711; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 163019; i < 172176; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 173833; i < 182118; i += beatD)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD, .8, 0);
            }
            for (double i = 182205; i < 183513; i += beatD / 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD / 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD / 2, .8, 0);
            }

            var snare = GetLayer("").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 151507; i < 156478; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 157786; i < 161362; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 162670; i < 171827; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }

            var diamond = GetLayer("???").CreateSprite("sb/px.png", OsbOrigin.Centre, new Vector2(320, 240));
            diamond.Scale(OsbEasing.OutCubic, 156740, 157263, 0, 150);
            diamond.Rotate(OsbEasing.OutBack, 156740, 157263, 0, Math.PI / 4 * 5);
            diamond.Scale(OsbEasing.InQuad, 157263, 157437, 150, 944);
            diamond.Scale(157437, 0);

            diamond.Scale(OsbEasing.OutBack, 161972, 162234, 0, 50);
            diamond.Scale(OsbEasing.OutBack, 162234, 162408, 50, 150);
            diamond.Scale(OsbEasing.InBack, 162408, 162670, 150, 944);
            diamond.Scale(162670, 0);

            diamond.Scale(OsbEasing.OutBack, 183600, 183949, 0, 50);
            diamond.Scale(OsbEasing.OutBack, 183949, 184298, 50, 100);
            diamond.Scale(OsbEasing.OutBack, 184298, 184647, 100, 150);
            diamond.Scale(OsbEasing.InQuart, 184647, 184996, 150, 944);
        }
        void AfterCalm1()
        {
            var back = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            back.ScaleVec(229647, 854, 480);
            back.ColorHsb(229647, 190, .15, 1);
            back.Fade(240809, 240984, .7, 0);

            var diamond = GetLayer("???").CreateSprite("sb/px.png", OsbOrigin.Centre, new Vector2(320, 240));
            diamond.Scale(OsbEasing.InExpo, 249182, 251972, 50, 854);
            diamond.Rotate(OsbEasing.In, 249182, 251972, Math.PI / 4, Math.PI / 4 * 10);
        }
        void LastKiai()
        {
            var clap = GetLayer("").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 269065; i < 274036; i += beatD * 2)
            {
                clap.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                clap.Fade(i, i + beatD * 2, .8, 0);
            }

            var snare = GetLayer("").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            for (double i = 258251; i < 265664; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }
            for (double i = 266275; i < 268106; i += beatD * 1.5)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 1.5, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 1.5, .8, 0);
            }
            for (double i = 268716; i < 273687; i += beatD * 2)
            {
                snare.Scale(OsbEasing.OutQuad, i, i + beatD * 2, 475f / bitmapH, 480f / bitmapH);
                snare.Fade(i, i + beatD * 2, .8, 0);
            }
        }

        #endregion
    }
}