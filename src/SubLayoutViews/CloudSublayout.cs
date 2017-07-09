using System;
using Engine;
using Engine.Animation;
using Engine.Interfaces;
using PenguinShuffle.Utils;

namespace PenguinShuffle.SubLayoutViews
{
    public class CloudSubLayout : ISubLayoutView
    {
        private readonly CloudPath Cloud1Path;
        private readonly CloudPath Cloud2Path;
        private readonly CloudPath Cloud3Path;

        public CloudSubLayout()
        {

            Cloud1Path = new CloudPath((mainLayer, x, y) => mainLayer.DrawImage(Assets.Images.Layouts.Cloud1, x, y));
            Cloud2Path = new CloudPath((mainLayer, x, y) => mainLayer.DrawImage(Assets.Images.Layouts.Cloud2, x, y));
            Cloud3Path = new CloudPath((mainLayer, x, y) => mainLayer.DrawImage(Assets.Images.Layouts.Cloud3, x, y));

            Cloud1Path.Start();
            Cloud2Path.Start();
            Cloud3Path.Start();
            BgSlidingState = BgSlidingState.Left;
        }

        

        public MotionManager BackgroundAnimation { get; set; }

        public BgSlidingState BgSlidingState { get; set; }

        public void InitLayoutView(ITouchManager touchManager)
        {
        }


        public  void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Cloud1Path.Animation.Tick(elapsedGameTime);
            Cloud2Path.Animation.Tick(elapsedGameTime);
            Cloud3Path.Animation.Tick(elapsedGameTime);
            if (BackgroundAnimation != null) BackgroundAnimation.Tick(elapsedGameTime);
        }

        public ITouchManager TouchManager { get; private set; }
        public BaseLayout Layout { get; set; }

        public void Render(ILayer mainLayer)
        {
            mainLayer.Save();

            if (BackgroundAnimation != null)
            {
                BackgroundAnimation.Render(mainLayer);
            }
            else
            {
                switch (BgSlidingState)
                {
                    case BgSlidingState.Left:
                        mainLayer.Translate(0, 0);
                        mainLayer.DrawImage(Assets.Images.Layouts.CloudlessMainBg, 0, 0);
                        break;
                    case BgSlidingState.Right:
                        mainLayer.Translate(-384, 0);
                        mainLayer.DrawImage(Assets.Images.Layouts.CloudlessMainBg, 0, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }


            Cloud1Path.Animation.Render(mainLayer);
            Cloud2Path.Animation.Render(mainLayer);
            Cloud3Path.Animation.Render(mainLayer);

            mainLayer.Restore();
        }

        public  void Destroy()
        {
        }

        public void SlideRight()
        {
            if (BgSlidingState != BgSlidingState.Left) return;

            BgSlidingState = BgSlidingState.SlidingRight;
            BackgroundAnimation = MotionManager.StartMotion(0, 0)
                .Motion(new AnimationMotion(-384, 0, 800, AnimationEasing.SineEaseIn))
                .OnRender((layer, x, y) =>
                {
                    layer.Translate(x, 0);
                    layer.DrawImage(Assets.Images.Layouts.CloudlessMainBg, 0, 0);
                })
                .OnComplete(() =>
                {
                    BackgroundAnimation = null;
                    BgSlidingState = BgSlidingState.Right;
                });
        }

        public void SlideLeft()
        {
            if (BgSlidingState != BgSlidingState.Right) return;


            BgSlidingState = BgSlidingState.SlidingRight;
            BackgroundAnimation = MotionManager.StartMotion(-384, 0)
                .Motion(new AnimationMotion(0, 0, 700, AnimationEasing.SineEaseIn))
                .OnRender((layer, x, y) =>
                {
                    layer.Translate(x, 0);
                    layer.DrawImage(Assets.Images.Layouts.CloudlessMainBg, 0, 0);
                })
                .OnComplete(() =>
                {
                    BackgroundAnimation = null;
                    BgSlidingState = BgSlidingState.Left;
                });
        }

        private class CloudPath
        {
            private readonly OnRender render;

            public CloudPath(OnRender render)
            {
                this.render = render;
            }


            public MotionManager Animation { get; set; }

            public MotionManager Motion { get; set; }

            public int StartY { get; set; }
            public int EndY { get; set; }
            public bool MovingLeft { get; set; }

            public void Start()
            {
                MovingLeft = RandomUtil.RandomBool();


                StartY = RandomUtil.RandomInt(0, 800);
                EndY = StartY + RandomUtil.RandomInt(-100, 100);
                var startPosition = new Point(MovingLeft ? 1900 + RandomUtil.RandomInt(0, 1200) : -500 - RandomUtil.RandomInt(0, 1200), StartY);
                var endPosition = new Point(!MovingLeft ? 1900 + RandomUtil.RandomInt(0, 1200) : -500 - RandomUtil.RandomInt(0, 1200), EndY);

                int msDuration = RandomUtil.RandomInt(18000, 25000);
                Animation = MotionManager.StartMotion(startPosition).Motion(new AnimationMotion(endPosition, msDuration, AnimationEasing.Linear)).OnRender((mainLayer, x, y) => render(mainLayer, x, y)).OnComplete(() => Start());
            }
        }
    }

    public enum BgSlidingState
    {
        Left,
        SlidingRight,
        Right,
        SlidingLeft
    }
}