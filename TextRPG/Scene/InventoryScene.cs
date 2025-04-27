using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public class InventoryScene : AScene
    {
        public InventoryScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add("[보유 골드]");
            dynamicText.Add($"{gameContext.ch.gold} G");
            dynamicText.Add("");
            dynamicText.Add("[아이템 목록]");
            for(int i = 0; i < gameContext.ch.inventory.items.Count; i++)
            {
                Item tmp = gameContext.ch.inventory.items[i];
                dynamicText.Add($"- {i + 1} {(tmp.equiped ? "[E]" : "")} {tmp.name} \t | {(tmp.attack > 0 ? "공격력" : "방어력" )} + {(tmp.attack > 0 ? tmp.attack : tmp.guard)} \t | {tmp.description}");
            }
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            

            Render();
        }
        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
