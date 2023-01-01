using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Animations;
using System;

using static OpenTK.MathHelper;

namespace StorybrewScripts
{
    class Stars : StoryboardObjectGenerator
    {
        readonly Action<CommandGenerator> config = g =>
        {
            g.ScaleTolerance = 1;
            g.OpacityTolerance = 1;
            g.PositionDecimals = 6;
            g.ScaleDecimals = 6;
        };

        protected override void Generate()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            Scene1(3600, 25926);
            Scene2(92903, 104065);
            Scene3(190577, 229647);
            stopwatch.Stop();
            Log($"Execution: {stopwatch.ElapsedMilliseconds / 1000d} seconds");
        }
        void Scene1(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, 150).Add(endTime, 200);

            camera.NearClip.Add(startTime, 45);
            camera.FarClip.Add(startTime, 4500);

            camera.NearFade.Add(startTime, 10);
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            parent.Rotation.Add(startTime, new Quaternion(DegreesToRadians(25), 0, DegreesToRadians(25)))
                .Add(startTime + 2000, new Quaternion(Vector3.Zero), EasingFunctions.CubicOut)
                .Until(endTime - 5000)
                .Add(endTime, new Quaternion(DegreesToRadians(-45), 0, DegreesToRadians(10)), EasingFunctions.QuintIn);

            for (var i = 0; i < 1000; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(config);

                star.Opacity.Add(startTime, Random(.25f, .8f));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(startTime + duration / 2, RandEndPos.Z + 2000, EasingFunctions.QuadOut)
                    .Add(endTime, RandEndPos.Z + 8000, EasingFunctions.QuintIn);

                star.SpriteScale.Add(startTime, Random(.3f, .6f));

                parent.Add(star);
            }
            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 8);

            var fadeIn = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            fadeIn.ScaleVec(startTime, 854, 480);
            fadeIn.Fade(startTime, startTime + 1000, 1, 0);
            fadeIn.Color(startTime, 0, 0, 0);
        }
        void Scene2(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, 150).Add(endTime, 200);

            camera.NearClip.Add(startTime, 45);
            camera.FarClip.Add(startTime, 4500);

            camera.NearFade.Add(startTime, 10);
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            parent.Rotation.Add(startTime, new Quaternion(DegreesToRadians(20), 0, DegreesToRadians(20)))
                .Add(startTime + 2000, new Quaternion(0, 0, 0), EasingFunctions.CubicOut);

            for (var i = 0; i < 1000; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 7);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(config);

                star.Opacity.Add(startTime, Random(.5f, 1));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z).Add(endTime, RandEndPos.Z + 4000);

                star.SpriteScale.Add(startTime, Random(.25f, .5f));

                parent.Add(star);
            }
            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 6);
        }
        void Scene3(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, 150).Add(endTime, 200);

            camera.NearClip.Add(startTime, 45);
            camera.FarClip.Add(startTime, 4500);

            camera.NearFade.Add(startTime, 10);
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            for (var i = 0; i < 1000; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(config);

                star.Opacity.Add(startTime, Random(.25f, .8f));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(207321, RandEndPos.Z + 800, EasingFunctions.SineOut)
                    .Add(endTime, RandEndPos.Z + 4000, EasingFunctions.SineIn);

                star.SpriteScale.Add(startTime, Random(.25f, .5f));

                parent.Add(star);
            }
            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 4);

            var fadeIn = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
            fadeIn.ScaleVec(startTime, 854, 480);
            fadeIn.Fade(startTime, startTime + 1000, 1, 0);
            fadeIn.Color(startTime, 0, 0, 0);
        }
    }
}