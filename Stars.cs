using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding3d; // Debug assembly
using StorybrewCommon.Animations;

namespace StorybrewScripts
{
    class Stars : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            Scene1(3600, 25926);
            Scene2(92903, 104065);
            Scene3(190577, 229647);
            timer.Stop();
            Log($"Execution: {timer.ElapsedMilliseconds / 1000f} seconds");
        }
        void Scene1(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, -20).Add(endTime, -20);
            
            camera.NearClip.Add(startTime, 30);
            camera.FarClip.Add(startTime, 3000);

            camera.NearFade.Add(startTime, 10); 
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            for (var i = 0; i < 750; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), i * 7);

                var star = new Sprite3d
                {
                    SpritePath = "sb/d.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1.5;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, 0)
                    .Add(startTime + 1000, 1)
                    .Add(endTime - 500, 1)
                    .Add(endTime, 0);

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(startTime + duration / 2, RandEndPos.Z - 2000, EasingFunctions.SineOut)
                    .Add(endTime, RandEndPos.Z - 4000, EasingFunctions.QuintIn);

                star.SpriteScale.Add(startTime, new Vector2(.5f, .5f));
           
                parent.Add(star);
            }
            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 8);
        }
        void Scene2(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, -20).Add(endTime, -20);
            
            camera.NearClip.Add(startTime, 30);
            camera.FarClip.Add(startTime, 3000);

            camera.NearFade.Add(startTime, 10); 
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            for (var i = 0; i < 750; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), i * 5);

                var star = new Sprite3d
                {
                    SpritePath = "sb/d.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1.5;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, 0)
                    .Add(startTime + 1000, 1)
                    .Add(endTime - 500, 1)
                    .Add(endTime, 0);

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(endTime, RandEndPos.Z - 2000);

                star.SpriteScale.Add(startTime, new Vector2(.5f, .5f));
           
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
            camera.PositionZ.Add(startTime, -20).Add(endTime, -20);
            
            camera.NearClip.Add(startTime, 30);
            camera.FarClip.Add(startTime, 3000);

            camera.NearFade.Add(startTime, 10); 
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            for (var i = 0; i < 700; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), i * 5);

                var star = new Sprite3d
                {
                    SpritePath = "sb/d.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1.5;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, 0)
                    .Add(startTime + 1000, 1)
                    .Add(endTime - 500, 1)
                    .Add(endTime, 0);

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(207321, RandEndPos.Z - 800, EasingFunctions.SineOut)
                    .Add(endTime, RandEndPos.Z - 1800, EasingFunctions.SineIn);

                star.SpriteScale.Add(startTime, new Vector2(.5f, .5f));
           
                parent.Add(star);
            }
            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 4);
        }
    }
}