using Engine.Interfaces;

namespace PenguinShuffle
{
    public class GameService
    {

        public GameService(AssetManager assetManager)
        {
            AssetManager = assetManager;
        }

        public AssetManager AssetManager { get; set; }
    }
     
}