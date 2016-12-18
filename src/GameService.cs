using System;
using System.Collections.Generic;
using System.Text;
using DemolitionRobots.SubLayoutViews;
using Engine;
using Engine.Interfaces;

namespace DemolitionRobots
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