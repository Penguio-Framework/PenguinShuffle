using System;
using Engine;
using Engine.Interfaces;

namespace PenguinShuffle.SubLayoutViews
{
    public class SoundSubLayout : ISubLayoutView
    {
        private readonly BaseClient client;
        public Point SoundToggleButtonPosition;

        public SoundSubLayout(BaseClient client )
        {
            this.client = client;
        }

        

        public void InitLayoutView(ITouchManager touchManager)
        {
            SoundToggleButtonPosition = new Point(126, 1925);

            touchManager.PushClickRect(new TouchRect(SoundToggleButtonPosition.X, SoundToggleButtonPosition.Y, 200, 200, soundToggleTrigger, true));
        }


        public  void TickLayoutView(TimeSpan elapsedGameTime)
        {
        }

        public ITouchManager TouchManager { get; private set; }
        public BaseLayout Layout { get; set; }

        public void Render(ILayer mainLayer)
        {
            mainLayer.Save();
            mainLayer.DrawImage(client.SoundEnabled ? Assets.Images.Layouts.SoundOnButton : Assets.Images.Layouts.SoundOffButton, SoundToggleButtonPosition, true);

            mainLayer.Restore();
        }

        public  void Destroy()
        {
        }

        private bool soundToggleTrigger(TouchType eventtype, TouchRect touchbox, int x, int y, bool collide)
        {
            if (eventtype == TouchType.TouchDown)
            {
                client.SoundEnabled = !client.SoundEnabled;
                client.PlaySoundEffect(Assets.Sounds.Click);
                return false;
            }
            return true;
        }
    }
}