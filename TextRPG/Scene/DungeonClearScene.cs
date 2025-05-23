﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public class DungeonClearScene : AScene
    {
        public DungeonClearScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            if (gameContext.curHp > 0)
            {
                dynamicText.Add("축하합니다!!");
                dynamicText.Add($"{gameContext.enteredDungeon!.title}을 클리어 하였습니다");
                dynamicText.Add("\n");
                dynamicText.Add("[탐험 결과]");
                dynamicText.Add($"체력 {gameContext.prevHp} -> {gameContext.curHp}");
                dynamicText.Add($"Gold {gameContext.prevGold}G -> {gameContext.curGold}G");
                gameContext.ch.hp = gameContext.curHp;
                gameContext.ch.gold = gameContext.curGold;
                gameContext.ch.clearCount++;
            }
            //dynamicText.Add($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드:{gameContext.ch.gold})");
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            

            Render();
        }


        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
