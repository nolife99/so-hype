using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using System;

namespace StorybrewScripts
{
    class Background : StoryboardObjectGenerator
    {
        int bitmapH, beat;
        const string bg = "63888719_p0.jpg", blur = "sb/b/blur.png";

        protected override void Generate()
        {
            bitmapH = GetMapsetBitmap(blur).Height;
            beat = (int)Beatmap.GetTimingPointAt(48949).BeatDuration;

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
            Flash(179414, 180809, .5);
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

            Section1();
            Kiai1();
            Section2();
            Kiai2();
            Kiai3();
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
            var sprite = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            sprite.Scale(startFade, 480f / GetMapsetBitmap(blur).Height);
            if (startTime != startFade) sprite.Fade(OsbEasing.Out, startFade, startTime, 0, 1);
            sprite.Fade(OsbEasing.In, endTime, endFade, 1, 0);
        }
        void MainBG(int startFade, int startTime, int endTime, int endFade)
        {
            var sprite = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
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
            var clap = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            clap.Scale(0, 0);
            clap.StartTriggerGroup("HitSoundClap", 25926, 37089);
            clap.Fade(0, 1000, .75, 0);
            clap.Scale(OsbEasing.OutQuad, 0, 1000, 475f / bitmapH, 480f / bitmapH);
            clap.EndGroup();

            var finish = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            finish.Scale(0, 0);
            finish.StartTriggerGroup("HitSoundFinish", 25926, 37089);
            finish.Fade(0, 1000, .9, 0);
            finish.Scale(OsbEasing.OutQuad, 0, 1000, 475f / bitmapH, 480f / bitmapH);
            finish.EndGroup();
        }
        void Kiai1()
        {
            var snare = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            snare.StartLoopGroup(48949, (64996 - 48949) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();

            var clap = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            clap.StartLoopGroup(49298, (57234 - 49298) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();

            clap.StartLoopGroup(59763, (65344 - 59763) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();
        }
        void Section2()
        {
            var snare = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            snare.StartLoopGroup(73368, (92903 - 73368) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();

            var clap = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            clap.StartLoopGroup(73716, (93251 - 73716) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();

            snare.StartLoopGroup(104065, (112437 - 104065) / beat);
            snare.Scale(OsbEasing.OutQuad, 0, beat, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat, .8, 0);
            snare.EndGroup();
            
            snare.StartLoopGroup(112437, (113833 - 112437) / (beat / 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat / 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat / 2, .8, 0);
            snare.EndGroup();

            snare.StartLoopGroup(115577, (126565 - 115577) / beat);
            snare.Scale(OsbEasing.OutQuad, 0, beat, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat, .8, 0);
            snare.EndGroup();
        }
        void Kiai2()
        {
            var clap = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            clap.StartLoopGroup(126740, (146275 - 126740) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();

            var snare = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            snare.StartLoopGroup(127089, (145926 - 127089) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();
        }
        void Kiai3()
        {
            var clap = GetLayer("BG").CreateSprite(bg, OsbOrigin.Centre, new Vector2(320, 240));
            clap.StartLoopGroup(149065, (151158 - 149065) / (int)(beat * 1.5));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 1.5, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 1.5, .9, 0);
            clap.EndGroup();

            clap.StartLoopGroup(151856, (162321 - 151856) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();

            clap.StartLoopGroup(163019, (172786 - 163019) / (beat * 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat * 2, .9, 0);
            clap.EndGroup();

            clap.StartLoopGroup(173833, (182205 - 173833) / beat);
            clap.Scale(OsbEasing.OutQuad, 0, beat, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat, .9, 0);
            clap.EndGroup();

            clap.StartLoopGroup(182205, (183600 - 182205) / (beat / 2));
            clap.Scale(OsbEasing.OutQuad, 0, beat / 2, 475f / bitmapH, 480f / bitmapH);
            clap.Fade(0, beat / 2, .9, 0);
            clap.EndGroup();

            var snare = GetLayer("BG").CreateSprite(blur, OsbOrigin.Centre, new Vector2(320, 240));
            snare.StartLoopGroup(151507, (157089 - 151507) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();

            snare.StartLoopGroup(157786, (161972 - 157786) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();

            snare.StartLoopGroup(162670, (172437 - 162670) / (beat * 2));
            snare.Scale(OsbEasing.OutQuad, 0, beat * 2, 475f / bitmapH, 480f / bitmapH);
            snare.Fade(0, beat * 2, .8, 0);
            snare.EndGroup();
        }

        #endregion
    }
}