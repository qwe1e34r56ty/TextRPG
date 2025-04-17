using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public static class SceneID
    {
        public const string Main = "Main";
        public const string Status = "Status";
        public const string Inventory = "Inventory";
        public const string Wear = "Wear";
        public const string Shop = "Shop";
        public const string Nothing = "None";
        public const string Buy = "Buy";
        public const string Rest = "Rest";
        public const string Sell = "Sell";
        public const string DungeonSelect = "DungeonSelect";
        public const string DungeonClear = "DungeonClear";
        public const string DungeonFail = "DungeonFail";
    }
    internal abstract class AScene
    {
        protected GameContext gameContext { get; set; }
        protected Dictionary<string, AView> viewMap { get; set; }
        protected SceneText sceneText { get; set; }
        protected SceneNext sceneNext { get; set; }
        public AScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext)
        {
            this.gameContext = gameContext;
            this.viewMap = viewMap;
            this.sceneText = sceneText;
            this.sceneNext = sceneNext;
        }
        public virtual void DrawScene()
        {
            ((ScriptView)viewMap[ViewID.Script]).SetText(sceneText.scriptText!);
            ((ChoiceView)viewMap[ViewID.Choice]).SetText(sceneText.choiceText!);
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(System.Array.Empty<string>());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }
            ((InputView)viewMap[ViewID.Input]).SetCursor();
        }
        public abstract string respond(int i);
    }
}
