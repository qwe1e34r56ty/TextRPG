using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public class WearScene : AScene
    {
        public WearScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add("[아이템 목록]");
            for (int i = 0; i < gameContext.ch.inventory.items.Count; i++)
            {
                Item tmp = gameContext.ch.inventory.items[i];
                dynamicText.Add($"- {i + 1} {(tmp.equiped ? "[E]" : "")} {tmp.name} \t | {(tmp.attack > 0 ? "공격력" : "방어력")} + {(tmp.attack > 0 ? tmp.attack : tmp.guard)} \t | {tmp.description}");
            }
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            

            Render();
        }
        public override string respond(int i)
        {
            bool weaponEquiped = false;
            bool armorEquiped = false;
            Character ch = gameContext.ch;
            if (i > 0 && i < gameContext.ch.inventory?.items?.Count + 1)
            {
                foreach(var item in ch.inventory.items!)
                {
                    if (item.weapon && item.equiped)
                    {
                        weaponEquiped = true;
                    }
                    if (item.armor && item.equiped)
                    {
                        armorEquiped = true;
                    }
                }
                if (gameContext.ch.inventory!.items![i - 1].weapon && gameContext.ch.inventory!.items![i - 1].equiped == false && weaponEquiped)
                {
                    for(int j = 0; j < ch.inventory.items.Count; j++)
                    {
                        if (ch.inventory.items[j].weapon && ch.inventory.items[j].equiped)
                        {
                            ch.inventory.items[j].equiped = false;
                        }
                    }
                }
                if (gameContext.ch.inventory!.items![i - 1].armor && gameContext.ch.inventory!.items![i - 1].equiped == false && armorEquiped)
                {
                    for (int j = 0; j < ch.inventory.items.Count; j++)
                    {
                        if (ch.inventory.items[j].armor && ch.inventory.items[j].equiped)
                        {
                            ch.inventory.items[j].equiped = false;
                        }
                    }
                }
                gameContext.ch.inventory!.items![i - 1].equiped = !gameContext.ch.inventory!.items![i - 1].equiped;
            }
            else if (i != 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("잘못된 입력입니다!");
            }
            else if (i == 0)
            {
                //((LogView)viewMap[ViewID.Log]).AddLog("메인 화면으로 돌아갑니다!");
            }
            return sceneNext.next![i];
        }
    }
}
