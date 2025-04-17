using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class StatusScene : AScene
    {
        public StatusScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ((ScriptView)viewMap[ViewID.Script]).SetText(sceneText.scriptText!);
            ((ChoiceView)viewMap[ViewID.Choice]).SetText(sceneText.choiceText!);
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(System.Array.Empty<string>());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(System.Array.Empty<string>());
            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }

            List<string> dynamicText = new();

            Character ch = gameContext.ch;

            dynamicText.Add($"Lv.{ch.getLevel()}");
            dynamicText.Add($"{ch.name} ({ch.job})");
            dynamicText.Add($"공격력 : {ch.getTotalAttack() } {(ch.getPlusAttack() > 0 ? "(+" + ch.getPlusAttack() + ")" : "")}");
            dynamicText.Add($"방어력 : {ch.getTotalGuard()} {(ch.getPlusGuard() > 0 ? "(+" + ch.getPlusGuard() + ")" : "")}");
            dynamicText.Add($"체 력 : {ch.hp}");
            dynamicText.Add($"Gold : {ch.gold}G");
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);

            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }
            ((InputView)viewMap[ViewID.Input]).SetCursor();
        }

        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
