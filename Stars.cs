using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Animations;
using static OpenTK.MathHelper;

namespace StorybrewScripts
{
    class Stars : StoryboardObjectGenerator
    {
        protected override void Generate()
        {
            Scene1(3600, 25926);
            Scene2(92903, 104065);
            Scene3(190577, 229647);
        }
        void Scene1(int startTime, int endTime)
        {
            var duration = endTime - startTime;

            var scene = new Scene3d();
            var camera = new PerspectiveCamera();

            camera.PositionX.Add(startTime, 0).Add(endTime, 0);
            camera.PositionY.Add(startTime, 0).Add(endTime, 0);
            camera.PositionZ.Add(startTime, 150).Add(endTime, 150);
            
            camera.NearClip.Add(startTime, 50);
            camera.FarClip.Add(startTime, 2000);

            camera.NearFade.Add(startTime, 100); 
            camera.FarFade.Add(startTime, 3000);

            var parent = scene.Root;

            parent.Rotation.Add(startTime, new Quaternion(DegreesToRadians(45), 0, DegreesToRadians(35)))
                .Add(startTime + 2000, new Quaternion(Vector3.Zero), EasingFunctions.CubicOut)
                .Until(endTime - 5000)
                .Add(endTime - 5000, new Quaternion(Vector3.Zero))
                .Add(endTime, new Quaternion(DegreesToRadians(-45), 0, .25f), EasingFunctions.QuintIn);

            for (var i = 0; i < 800; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 10);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, Random(.25f, .8f));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(startTime + duration / 2, RandEndPos.Z + 2000, EasingFunctions.CubicOut)
                    .Add(endTime, RandEndPos.Z + 8000, EasingFunctions.QuintIn);

                star.SpriteScale.Add(startTime, new Vector2(.5f, .5f));
           
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
            camera.PositionZ.Add(startTime, 150).Add(endTime, 150);
            
            camera.NearClip.Add(startTime, 30);
            camera.FarClip.Add(startTime, 3000);

            camera.NearFade.Add(startTime, 10); 
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            parent.Rotation.Add(startTime, new Quaternion(DegreesToRadians(20), 0, DegreesToRadians(20)))
                .Add(startTime + 2000, new Quaternion(Vector3.Zero), EasingFunctions.CubicOut);

            for (var i = 0; i < 800; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 8);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, Random(.5f, 1));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z).Add(endTime, RandEndPos.Z + 2500);

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
            camera.PositionZ.Add(startTime, 120).Add(endTime, 120);
            
            camera.NearClip.Add(startTime, 30);
            camera.FarClip.Add(startTime, 3000);

            camera.NearFade.Add(startTime, 10); 
            camera.FarFade.Add(startTime, 1000);

            var parent = scene.Root;

            for (var i = 0; i < 800; i++)
            {
                var RandEndPos = new Vector3(Random(-5024, 5024), Random(-3600, 3600), -i * 5);

                var star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    RotationMode = RotationMode.Fixed
                };
                star.ConfigureGenerators(g =>
                {
                    g.PositionTolerance = 1;
                    g.ScaleTolerance = 3;
                    g.OpacityTolerance = 3;
                });
                
                star.Opacity.Add(startTime, Random(.25f, .8f));

                star.PositionX.Add(startTime, RandEndPos.X);
                star.PositionY.Add(startTime, RandEndPos.Y);

                star.PositionZ.Add(startTime, RandEndPos.Z)
                    .Add(207321, RandEndPos.Z + 800, EasingFunctions.SineOut)
                    .Add(endTime, RandEndPos.Z + 2400, EasingFunctions.SineIn);

                star.SpriteScale.Add(startTime, new Vector2(.5f, .5f));
           
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