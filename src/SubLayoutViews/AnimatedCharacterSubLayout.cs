using System;
using Engine.Animation;
using Engine.Interfaces;
using PenguinShuffle.Utils;

namespace PenguinShuffle.SubLayoutViews
{
    public class AnimatedCharacterSubLayout : ISubLayoutView
    {
        public AssetManager AssetManager { get; set; }
        public Game Game { get; set; }
        public int CharacterIndex { get; set; }

        public bool Selected { get; set; }

        public AnimatedCharacterSubLayout(AssetManager assetManager, Game game, int characterIndex)
        {
            AssetManager = assetManager;
            Game = game;
            CharacterIndex = characterIndex;
        }

        public void InitLayoutView(ITouchManager touchManager)
        {
            createAnimation();
        }

        private void createAnimation()
        {

            var timeBetween = 45;
            var msDuration = RandomUtil.RandomInt(1000, 5500);

            if (RandomUtil.RandomInt(0, 100) < 25)
            {
                Motion = MotionManager.StartMotion(0, 0)
                    .Motion(new WaitMotion(msDuration))//frame 1
                    .Motion(new WaitMotion(timeBetween))//frame 1
                    .Motion(new WaitMotion(timeBetween))//frame 2
                    .Motion(new WaitMotion(timeBetween))//frame 3
                    .Motion(new WaitMotion(timeBetween))//frame 4
                    .Motion(new WaitMotion(timeBetween))//frame 5
                    .Motion(new WaitMotion(timeBetween))//frame 6
                    .Motion(new WaitMotion(timeBetween))//frame 7
                    .Motion(new WaitMotion(timeBetween * 6));//frame 8 (blink)
                Motion.OnRender(render);

            }
            else
            {
                Motion = MotionManager.StartMotion(0, 0)
                    .Motion(new WaitMotion(msDuration))//frame 1
                    .Motion(new WaitMotion(timeBetween * 3));//frame 8 (blink)
                Motion.OnRender((layer, posx, posy, animationIndex, percent) => render(layer, posx, posy, animationIndex != 0 ? 8 : 0, 0));
            }


            Game.Client.Timeout(createAnimation, msDuration + RandomUtil.RandomInt(2000, 5500));
        }


        private void render(ILayer layer, double posx, double posy, int animationindex, double percent)
        {
            IImage frame;
            switch (animationindex)
            {
                case 0:
                case 1:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated1, CharacterIndex);
                    break;
                case 2:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated2, CharacterIndex);
                    break;
                case 3:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated3, CharacterIndex);
                    break;
                case 4:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated4, CharacterIndex);
                    break;
                case 5:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated5, CharacterIndex);
                    break;
                case 6:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated6, CharacterIndex);
                    break;
                case 7:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated7, CharacterIndex);
                    break;
                case 8:
                    frame = AssetManager.GetImage(Images.Characters.StandingAnimated8, CharacterIndex);
                    break;
                default:
                    throw new Exception();
            }


            if (Selected)
            {
                layer.DrawImage(frame, 0, 0, (int)(frame.Width * 1.2), (int)(frame.Height * 1.2), true);
            }
            else
            {
                layer.DrawImage(frame, 0, 0, true);
            }
        }

        public MotionManager Motion { get; set; }

        public void TickLayoutView(TimeSpan elapsedGameTime)
        {
            Motion.Tick(elapsedGameTime);
        }

        public ITouchManager TouchManager { get; private set; }
        public ILayout Layout { get; set; }

        public void Render(ILayer mainLayer)
        {
            if (Motion.Completed)
            {
                render(mainLayer, 0, 0, 0, 0);
            }
            else
            {
                Motion.Render(mainLayer);
            }
        }

        public void Destroy()
        {
            Motion = null;
        }
    }
}