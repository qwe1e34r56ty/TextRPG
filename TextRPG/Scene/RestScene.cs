using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public class RestScene : AScene
    {
        public RestScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드:{gameContext.ch.gold})");
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            

            Render();
        }

        public override string respond(int i)
        {
            Character ch = gameContext.ch;
            if(i == 1)
            {
                if (ch.gold >= 500)
                {
                    ch.gold -= 500;
                    ch.hp = 100;
                    ((LogView)viewMap[ViewID.Log]).AddLog("휴식을 완료했습니다.");
                }
                else
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog("Gold가 부족합니다.");
                }
            }
            return sceneNext.next![i];
        }
    }
}