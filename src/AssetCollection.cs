using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace DemolitionRobots
{
    public class AssetCollection
    {
        public AssetCollection(AssetManager assetManager)
        {
            Images.Layouts.Logo = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/logo");
            Images.Layouts.Foob = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/foob");

            /*
                        Images.Cards.Big.Cards = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/cards/big/cards-{0}");
                        Images.Cards.Character.Cards = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/cards/character/cards-{0}");
                        Images.Cards.Small.Cards = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/cards/small/cards-{0}");
                        Images.Character.Arrow.PlaceCharacterArrow = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/character/arrow/place-character-arrow-{0}");
                        Images.Character.Banners.Big.LongBanner = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", "4", "5", "6", }, "images/character/banners/big/{0}-long-banner");
                        Images.Character.Banners.Small.ShortBanner = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", "4", "5", "6", }, "images/character/banners/small/{0}-short-banner");
                        Images.Character.Box.CharacterBox = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", "4", "5", "6", }, "images/character/box/character.{0}.box");
                        Images.Character.LabelBox.CharacterBox = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", "4", "5", "6", }, "images/character/labelBox/character.{0}.box");
                        Images.Character.Placement.CharacterPlacement = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", "4", "5", "6", }, "images/character/placement/{0}-character-placement");
                        Images.Character.Sliding.SlidingCharacters = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/character/sliding/sliding-characters-{0}");
                        Images.Character.Stationary.Big.StationaryCharacters = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/character/stationary/big/stationary-characters-{0}");
                        Images.Character.Stationary.Small.StationaryCharacters = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/character/stationary/small/stationary-characters-{0}");
                        Images.Layouts.Cloud = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", "3", }, "images/layouts/cloud{0}");
                        Images.Layouts.Tutorial = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "1", "2", }, "images/layouts/tutorial{0}");
                        Images.Tiles.Big.FishTiles = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/tiles/big/fish-tiles-{0}");
                        Images.Tiles.Small.FishTiles = assetManager.CreateImage(Guid.NewGuid().ToString(), new[] { "01", "02", "03", "04", "05", "06", }, "images/tiles/small/fish-tiles-{0}");
            //            Fonts.BabyDoll.BabyDollPt = assetManager.CreateFont(Guid.NewGuid().ToString(), new[] { "100", "120", "130", "240", "36", "48", "60", "72", "90", }, "fonts/BabyDoll/BabyDoll-{0}pt");
                        Images.About.AboutBubble = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/about/about-bubble");
                        Images.About.AboutContact = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/about/about-contact");
                        Images.About.AboutRate = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/about/about-rate");
                        Images.About.MainPenguin = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/about/main-penguin");
                        Images.Board.Box = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/board/box");
                        Images.Cards.CardsAll = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/cards/cards-all");
                        Images.Cards.CardsBack = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/cards/cards-back");
                        Images.Cards.CardsExtra = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/cards/cards-extra");
                        Images.Character.CharacterShadow = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/character/character-shadow");
                        Images.Layouts.AboutButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/about-button");
                        Images.Layouts.Arrow = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/arrow");
                        Images.Layouts.BackButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/back-button");
                        Images.Layouts.BlurBox = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/blurBox");
                        Images.Layouts.CloudlessMainBg = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/cloudless-main-bg");
                        Images.Layouts.GameBackground = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/gameBackground");
                        Images.Layouts.ModeButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/mode-button");
                        Images.Layouts.NumberSelectBar = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/numberSelectBar");
                        Images.Layouts.PenguinLogo = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/penguin-logo");
                        Images.Layouts.PlayButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/play-button");
                        Images.Layouts.PlayerSelected = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/player-selected");
                        Images.Layouts.PlayerUnselected = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/player-unselected");
                        Images.Layouts.ShuffleLogo = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/shuffle-logo");
                        Images.Layouts.SoundOffButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/sound-off-button");
                        Images.Layouts.SoundOnButton = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/sound-on-button");
                        Images.Layouts.StagingArea = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/staging-area");
                        Images.Layouts.TextBoard = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/layouts/text-board");
                        Images.Tiles.FishTilesExtra = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/tiles/fish-tiles-extra");
                        Images.Tiles.WallHorizontal = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/tiles/wall-horizontal");
                        Images.Tiles.WallSolid = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/tiles/wall-solid");
                        Images.Tiles.WallVertical = assetManager.CreateImage(Guid.NewGuid().ToString(), "images/tiles/wall-vertical");
            */
            //            Fonts.MyriadPro.MyriadPro100Pt = assetManager.CreateFont(Guid.NewGuid().ToString(), "fonts/Myriad Pro/Myriad Pro-100pt");
            //            SoundEffects.Slide = assetManager.CreateSoundEffect(Guid.NewGuid().ToString(), "sounds/slide");
            //            Songs.MenuMusic = assetManager.CreateSong(Guid.NewGuid().ToString(), "songs/menu-music");
            //            Songs.OceanBackground = assetManager.CreateSong(Guid.NewGuid().ToString(), "songs/ocean-background");
        }
    }
    public static class Images
    {
        public class Layouts
        {
            public static IImage Logo { get; set; }
            public static IImage Foob { get; set; }
        }
    }
    public static class Fonts
    {
        public static class BabyDoll
        {
            public static Dictionary<object, IFont> BabyDollPt { get; set; }
            public static class BabyDollPtOptions { public const string _100 = "100"; public const string _120 = "120"; public const string _130 = "130"; public const string _240 = "240"; public const string _36 = "36"; public const string _48 = "48"; public const string _60 = "60"; public const string _72 = "72"; public const string _90 = "90"; }

        }
        public static class MyriadPro
        {
            public static IFont MyriadPro100Pt { get; set; }

        }

    }
    public static class SoundEffects
    {
        public static ISoundEffect Slide { get; set; }

    }
    public static class Songs
    {
        public static ISong MenuMusic { get; set; }
        public static ISong OceanBackground { get; set; }

    }
}