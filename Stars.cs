using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Animations;
using System.Threading.Tasks;
using System;

using static OpenTK.MathHelper;

namespace StorybrewScripts
{
    class Stars : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            Action<CommandGenerator> config = g =>
            {
                g.ScaleTolerance = 1;
                g.OpacityTolerance = 1;
                g.PositionDecimals = 6;
                g.ScaleDecimals = 6;
            };
            SceneConstructor(3600, 25926, (s, e, p) =>
            {
                var duration = e - s;

                p.Rotation.Add(s, new Quaternion(DegreesToRadians(25), 0, DegreesToRadians(25)))
                    .Add(s + 2000, Quaternion.Identity, EasingFunctions.BackOut)
                    .Until(e - 5000)
                    .Add(e, new Quaternion(DegreesToRadians(-10), 0, DegreesToRadians(45)), EasingFunctions.QuintIn);

                Parallel.For(0, 1000, i => 
                {
                    var pos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 8);

                    var star = new Sprite3d
                    {
                        SpritePath = "sb/dot.png",
                        UseDistanceFade = true,
                        RotationMode = RotationMode.Fixed
                    };
                    star.ConfigureGenerators(config);

                    star.Opacity.Add(s, Random(.4f, .8f));

                    star.PositionX.Add(s, pos.X);
                    star.PositionY.Add(s, pos.Y);

                    star.PositionZ.Add(s, pos.Z)
                        .Add(s + duration / 2, pos.Z + 2000, EasingFunctions.QuadOut)
                        .Add(e, pos.Z + 8000, EasingFunctions.QuintIn);

                    star.SpriteScale.Add(s, Random(.3f, .6f));

                    lock (p) p.Add(star);
                });
            }, true);
            SceneConstructor(92903, 104065, (s, e, p) =>
            {
                p.Rotation.Add(s, new Quaternion(DegreesToRadians(20), 0, DegreesToRadians(20)))
                    .Add(s + 2000, Quaternion.Identity, EasingFunctions.CubicOut);

                Parallel.For(0, 1000, i => 
                {
                    var pos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 7);

                    var star = new Sprite3d
                    {
                        SpritePath = "sb/dot.png",
                        UseDistanceFade = true,
                        RotationMode = RotationMode.Fixed
                    };
                    star.ConfigureGenerators(config);

                    star.Opacity.Add(s, Random(.5f, 1));

                    star.PositionX.Add(s, pos.X);
                    star.PositionY.Add(s, pos.Y);

                    star.PositionZ.Add(s, pos.Z).Add(e, pos.Z + 4000);

                    star.SpriteScale.Add(s, Random(.4f, .5f));

                    lock (p) p.Add(star);
                });
            });
            SceneConstructor(190577, 229647, (s, e, p) => Parallel.For(0, 1000, i => 
            {
                var pos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(config);

                star.Opacity.Add(s, Random(.4f, .8f));

                star.PositionX.Add(s, pos.X);
                star.PositionY.Add(s, pos.Y);

                star.PositionZ.Add(s, pos.Z)
                    .Add(207321, pos.Z + 800, EasingFunctions.SineOut)
                    .Add(e, pos.Z + 4000, EasingFunctions.SineIn);

                star.SpriteScale.Add(s, Random(.25f, .5f));

                lock (p) p.Add(star);
            }), true);
            SceneConstructor(279879, 302205, (s, e, p) => Parallel.For(0, 1000, i => 
            {
                var pos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(config);

                star.Opacity.Add(s, Random(.4f, .8f))
                    .Until(e - 500)
                    .Add(e, 0);

                star.PositionX.Add(s, pos.X);
                star.PositionY.Add(s, pos.Y);

                star.PositionZ.Add(s, pos.Z).Add(e, pos.Z + 5000);

                star.SpriteScale.Add(s, Random(.25f, .5f));

                lock (p) p.Add(star);
            }), true);

            stopwatch.Stop();
            Log($"Execution: {stopwatch.ElapsedMilliseconds / 1000d} seconds");
        }
        void SceneConstructor(int startTime, int endTime, Action<int, int, Node3d> generate, bool fadeIn = false)
        {
            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, 150).Add(endTime, 200);

            camera.NearClip.Add(startTime, 150);
            camera.FarClip.Add(startTime, 4500);

            camera.NearFade.Add(startTime, 100);
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;
            generate(startTime, endTime, parent);

            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 8);

            if (fadeIn)
            {
                var back = GetLayer("").CreateSprite("sb/px.png", OsbOrigin.TopLeft, new Vector2(-107, 0));
                back.ScaleVec(startTime, 854, 480);
                back.Fade(startTime, startTime + 1000, 1, 0);
                back.Color(startTime, 0, 0, 0);
            }
        }
    }
}