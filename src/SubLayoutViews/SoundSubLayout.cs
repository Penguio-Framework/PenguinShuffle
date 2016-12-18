using System;
using Engine;
using Engine.Interfaces;

namespace DemolitionRobots.SubLayoutViews
{
    public class SoundSubLayout : ISubLayoutView
    {
        private readonly Game game;
        public Point SoundToggleButtonPosition;

        public SoundSubLayout(AssetManager assetManager, Game game)
        {
            this.game = game;
            AssetManager = assetManager;
        }

        public AssetManager AssetManager { get; set; }

        public void InitLayoutView(ITouchManager touchManager)
        {
            SoundToggleButtonPosition = new Point(126, 1925);

            touchManager.PushClickRect(new TouchRect(SoundToggleButtonPosition.X, SoundToggleButtonPosition.Y, 200, 200, soundToggleTrigger, true));
        }


        public void TickLayoutView(TimeSpan elapsedGameTime)
        {
        }

        public ITouchManager TouchManager { get; private set; }
        public ILayout Layout { get; set; }

        public void Render(ILayer mainLayer)
        {
            mainLayer.Save();
//            mainLayer.DrawImage(AssetManager.GetImage(game.Client.SoundEnabled ? Images.Layouts.SoundOn : Images.Layouts.SoundOff), SoundToggleButtonPosition, true);

            mainLayer.Restore();
        }

        public void Destroy()
        {
        }

        private bool soundToggleTrigger(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                game.Client.SoundEnabled = !game.Client.SoundEnabled;
//                game.Client.PlaySoundEffect(SoundEffects.Click);
                return false;
            }
            return true;
        }
    }
}