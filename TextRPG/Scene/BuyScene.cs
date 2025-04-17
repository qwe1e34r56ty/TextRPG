using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class BuyScene : AScene
    {
        public BuyScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
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
            dynamicText.Add("[보유 골드]");
            dynamicText.Add($"{gameContext.ch.gold} G");
            dynamicText.Add("");
            dynamicText.Add("[아이템 목록]");
            for (int i = 0; i < gameContext.shop?.items?.Count; i++)
            {
                Item tmp = gameContext.shop.items[i];
                dynamicText.Add($"- {i + 1} {tmp.name} \t | {(tmp.attack > 0 ? "공격력" : "방어력")} + {(tmp.attack > 0 ? tmp.attack : tmp.guard)} \t | {tmp.description} \t | {(tmp.bought ? "구매완료" : tmp.price + "G")}");
            }
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
            if (i > 0 && i < gameContext.shop?.items?.Count + 1)
            {
                if (gameContext.shop!.items![i - 1].bought == false)
                {
                    if (gameContext.ch.gold >= gameContext.shop!.items![i - 1].price)
                    {
                        gameContext.ch.gold -= gameContext.shop!.items![i - 1].price;
                        gameContext.shop!.items![i - 1].bought = true;
                        gameContext.ch.inventory.items.Add(gameContext.shop!.items![i - 1]);
                        ((LogView)viewMap[ViewID.Log]).AddLog($"{gameContext.shop!.items![i - 1].name} 을 구매했습니다!");
                    }
                    else
                    {
                        ((LogView)viewMap[ViewID.Log]).AddLog("Gold가 부족합니다!");
                    }
                }
                else
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog("이미 구매한 아이템입니다!");
                }
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
