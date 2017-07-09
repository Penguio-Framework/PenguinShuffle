//THIS FILE IS AUTO GENERATED
using Engine.Interfaces;
using System.Collections.Generic;

namespace PenguinShuffle
{
    public class Assets 
    {
        public static void LoadAssets(IRenderer renderer, AssetManager assetManager)
        {
Assets.Sounds.Click = assetManager.CreateSoundEffect("sounds/click");
Assets.Sounds.Slide = assetManager.CreateSoundEffect("sounds/slide");
Assets.Sounds.TimeDing = assetManager.CreateSoundEffect("sounds/time-ding");
Assets.Sounds.TimeTick = assetManager.CreateSoundEffect("sounds/time-tick");

Assets.Images.About.AboutBubble = assetManager.CreateImage("images/about/about-bubble");
Assets.Images.About.AboutContact = assetManager.CreateImage("images/about/about-contact");
Assets.Images.About.AboutRate = assetManager.CreateImage("images/about/about-rate");
Assets.Images.About.MainPenguin = assetManager.CreateImage("images/about/main-penguin");
Assets.Images.Board.Box = assetManager.CreateImage("images/board/box");
Assets.Images.Cards.CardsAll = assetManager.CreateImage("images/cards/cards-all");
Assets.Images.Cards.CardsBack = assetManager.CreateImage("images/cards/cards-back");
Assets.Images.Cards.CardsExtra = assetManager.CreateImage("images/cards/cards-extra");
Assets.Images.Cards.Big.Cards = assetManager.CreateImage("images/cards/big/cards-{0}",new []{1,2,3,4,5,6});
Assets.Images.Cards.Character.Cards = assetManager.CreateImage("images/cards/character/cards-{0}",new []{1,2,3,4,5,6});
Assets.Images.Cards.Small.Cards = assetManager.CreateImage("images/cards/small/cards-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.CharacterShadow = assetManager.CreateImage("images/character/character-shadow");
Assets.Images.Character.Animations.CharactersAnimated = assetManager.CreateImage("images/character/animations/{0}/characters-animated-{1}",new []{1,2,3,4,5,6},new []{1,2,3,4,5,6,7,8});
Assets.Images.Character.Arrow.PlaceCharacterArrow = assetManager.CreateImage("images/character/arrow/place-character-arrow-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Banners.Big.LongBanner = assetManager.CreateImage("images/character/banners/big/long-banner-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Banners.Small.ShortBanner = assetManager.CreateImage("images/character/banners/small/short-banner-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Box.CharacterBox = assetManager.CreateImage("images/character/box/character.{0}.box",new []{1,2,3,4,5,6});
Assets.Images.Character.LabelBox.CharacterBox = assetManager.CreateImage("images/character/labelBox/character.{0}.box",new []{1,2,3,4,5,6});
Assets.Images.Character.Placement.CharacterPlacement = assetManager.CreateImage("images/character/placement/character-placement-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Sliding.SlidingCharacters = assetManager.CreateImage("images/character/sliding/sliding-characters-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Stationary.Big.StationaryCharacters = assetManager.CreateImage("images/character/stationary/big/stationary-characters-{0}",new []{1,2,3,4,5,6});
Assets.Images.Character.Stationary.Small.StationaryCharacters = assetManager.CreateImage("images/character/stationary/small/stationary-characters-{0}",new []{1,2,3,4,5,6});
Assets.Images.Layouts.AboutButton = assetManager.CreateImage("images/layouts/about-button");
Assets.Images.Layouts.Arrow = assetManager.CreateImage("images/layouts/arrow");
Assets.Images.Layouts.BackButton = assetManager.CreateImage("images/layouts/back-button");
Assets.Images.Layouts.BlurBox = assetManager.CreateImage("images/layouts/blurBox");
Assets.Images.Layouts.Cloud1 = assetManager.CreateImage("images/layouts/cloud1");
Assets.Images.Layouts.Cloud2 = assetManager.CreateImage("images/layouts/cloud2");
Assets.Images.Layouts.Cloud3 = assetManager.CreateImage("images/layouts/cloud3");
Assets.Images.Layouts.CloudlessMainBg = assetManager.CreateImage("images/layouts/cloudless-main-bg");
Assets.Images.Layouts.GameBackground = assetManager.CreateImage("images/layouts/gameBackground");
Assets.Images.Layouts.GrayBg = assetManager.CreateImage("images/layouts/gray-bg");
Assets.Images.Layouts.HelperImage1 = assetManager.CreateImage("images/layouts/helper-image-1");
Assets.Images.Layouts.HelperImage2 = assetManager.CreateImage("images/layouts/helper-image-2");
Assets.Images.Layouts.Logo = assetManager.CreateImage("images/layouts/logo");
Assets.Images.Layouts.ModeButton = assetManager.CreateImage("images/layouts/mode-button");
Assets.Images.Layouts.NumberSelectBar = assetManager.CreateImage("images/layouts/numberSelectBar");
Assets.Images.Layouts.PenguinLogo = assetManager.CreateImage("images/layouts/penguin-logo");
Assets.Images.Layouts.PlayButton = assetManager.CreateImage("images/layouts/play-button");
Assets.Images.Layouts.PlayerLocked = assetManager.CreateImage("images/layouts/player-locked");
Assets.Images.Layouts.PlayerSelected = assetManager.CreateImage("images/layouts/player-selected");
Assets.Images.Layouts.PlayerUnselected = assetManager.CreateImage("images/layouts/player-unselected");
Assets.Images.Layouts.ShuffleLogo = assetManager.CreateImage("images/layouts/shuffle-logo");
Assets.Images.Layouts.SoundOffButton = assetManager.CreateImage("images/layouts/sound-off-button");
Assets.Images.Layouts.SoundOnButton = assetManager.CreateImage("images/layouts/sound-on-button");
Assets.Images.Layouts.StagingArea = assetManager.CreateImage("images/layouts/staging-area");
Assets.Images.Layouts.TextBoard = assetManager.CreateImage("images/layouts/text-board");
Assets.Images.Layouts.Tutorials.Tutorial = assetManager.CreateImage("images/layouts/tutorials/tutorial{0}",new []{1,2});
Assets.Images.Tiles.FishTilesExtra = assetManager.CreateImage("images/tiles/fish-tiles-extra");
Assets.Images.Tiles.WallHorizontal = assetManager.CreateImage("images/tiles/wall-horizontal");
Assets.Images.Tiles.WallSolid = assetManager.CreateImage("images/tiles/wall-solid");
Assets.Images.Tiles.WallVertical = assetManager.CreateImage("images/tiles/wall-vertical");
Assets.Images.Tiles.Big.FishTiles = assetManager.CreateImage("images/tiles/big/fish-tiles-{0}",new []{1,2,3,4,5,6});
Assets.Images.Tiles.Small.FishTiles = assetManager.CreateImage("images/tiles/small/fish-tiles-{0}",new []{1,2,3,4,5,6});

Assets.Songs.MenuMusic = assetManager.CreateSong("songs/menu-music");
Assets.Songs.OceanBackground = assetManager.CreateSong("songs/ocean-background");

Assets.Fonts.BabyDoll._36 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-36pt");
Assets.Fonts.BabyDoll._48 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-48pt");
Assets.Fonts.BabyDoll._60 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-60pt");
Assets.Fonts.BabyDoll._72 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-72pt");
Assets.Fonts.BabyDoll._90 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-90pt");
Assets.Fonts.BabyDoll._100 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-100pt");
Assets.Fonts.BabyDoll._120 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-120pt");
Assets.Fonts.BabyDoll._130 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-130pt");
Assets.Fonts.BabyDoll._240 = assetManager.CreateFont("fonts/BabyDoll/BabyDoll-240pt");
Assets.Fonts.MyriadPro._100 = assetManager.CreateFont("fonts/Myriad Pro/Myriad Pro-100pt");


        }
    
public static class Fonts {
public static class BabyDoll {
public static IFont _36 {get;set;}
public static IFont _48 {get;set;}
public static IFont _60 {get;set;}
public static IFont _72 {get;set;}
public static IFont _90 {get;set;}
public static IFont _100 {get;set;}
public static IFont _120 {get;set;}
public static IFont _130 {get;set;}
public static IFont _240 {get;set;}
}
public static class MyriadPro {
public static IFont _100 {get;set;}
}
}


public static class Songs {
public static ISong MenuMusic {get;set;}
public static ISong OceanBackground {get;set;}
}


public static class Sounds {
public static ISoundEffect Click {get;set;}
public static ISoundEffect Slide {get;set;}
public static ISoundEffect TimeDing {get;set;}
public static ISoundEffect TimeTick {get;set;}
}


public static class Images {
public static class About {
public static IImage AboutBubble {get;set;}
public static IImage AboutContact {get;set;}
public static IImage AboutRate {get;set;}
public static IImage MainPenguin {get;set;}
}
public static class Board {
public static IImage Box {get;set;}
}
public static class Cards {
public static IImage CardsAll {get;set;}
public static IImage CardsBack {get;set;}
public static IImage CardsExtra {get;set;}
public static class Big {
public static Dictionary<int,IImage> Cards {get;set;}
}
public static class Character {
public static Dictionary<int,IImage> Cards {get;set;}
}
public static class Small {
public static Dictionary<int,IImage> Cards {get;set;}
}
}
public static class Character {
public static IImage CharacterShadow {get;set;}
public static class Animations {
public static Dictionary<int,Dictionary<int,IImage>> CharactersAnimated {get;set;}
}
public static class Arrow {
public static Dictionary<int,IImage> PlaceCharacterArrow {get;set;}
}
public static class Banners {
public static class Big {
public static Dictionary<int,IImage> LongBanner {get;set;}
}
public static class Small {
public static Dictionary<int,IImage> ShortBanner {get;set;}
}
}
public static class Box {
public static Dictionary<int,IImage> CharacterBox {get;set;}
}
public static class LabelBox {
public static Dictionary<int,IImage> CharacterBox {get;set;}
}
public static class Placement {
public static Dictionary<int,IImage> CharacterPlacement {get;set;}
}
public static class Sliding {
public static Dictionary<int,IImage> SlidingCharacters {get;set;}
}
public static class Stationary {
public static class Big {
public static Dictionary<int,IImage> StationaryCharacters {get;set;}
}
public static class Small {
public static Dictionary<int,IImage> StationaryCharacters {get;set;}
}
}
}
public static class Layouts {
public static IImage AboutButton {get;set;}
public static IImage Arrow {get;set;}
public static IImage BackButton {get;set;}
public static IImage BlurBox {get;set;}
public static IImage Cloud1 {get;set;}
public static IImage Cloud2 {get;set;}
public static IImage Cloud3 {get;set;}
public static IImage CloudlessMainBg {get;set;}
public static IImage GameBackground {get;set;}
public static IImage GrayBg {get;set;}
public static IImage HelperImage1 {get;set;}
public static IImage HelperImage2 {get;set;}
public static IImage Logo {get;set;}
public static IImage ModeButton {get;set;}
public static IImage NumberSelectBar {get;set;}
public static IImage PenguinLogo {get;set;}
public static IImage PlayButton {get;set;}
public static IImage PlayerLocked {get;set;}
public static IImage PlayerSelected {get;set;}
public static IImage PlayerUnselected {get;set;}
public static IImage ShuffleLogo {get;set;}
public static IImage SoundOffButton {get;set;}
public static IImage SoundOnButton {get;set;}
public static IImage StagingArea {get;set;}
public static IImage TextBoard {get;set;}
public static class Tutorials {
public static Dictionary<int,IImage> Tutorial {get;set;}
}
}
public static class Tiles {
public static IImage FishTilesExtra {get;set;}
public static IImage WallHorizontal {get;set;}
public static IImage WallSolid {get;set;}
public static IImage WallVertical {get;set;}
public static class Big {
public static Dictionary<int,IImage> FishTiles {get;set;}
}
public static class Small {
public static Dictionary<int,IImage> FishTiles {get;set;}
}
}
}

 
    }
}