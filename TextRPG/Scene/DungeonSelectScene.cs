using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class DungeonSelectScene : AScene
    {
        public DungeonSelectScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
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
            List<DungeonData> dungeonList = gameContext.dungeonList;
            for (int i = 0; i < dungeonList.Count; i++) {
                dynamicText.Add($"{i + 1}.{dungeonList[i].title} \t| 방어력 {dungeonList[i].recommandArmor} 이상 권장");
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
            List<DungeonData> dungeonList = gameContext.dungeonList;
            Character ch = gameContext.ch;
            if (i > 0 && i < dungeonList.Count + 1)
            {
                gameContext.enteredDungeon = dungeonList[i - 1];
                gameContext.prevHp = ch.hp;
                gameContext.prevGold = ch.gold;
                bool success = Success(i - 1);
                if (success)
                {
                    gameContext.curHp = gameContext.prevHp - calculReduceHp(i - 1);
                    gameContext.curGold = gameContext.prevGold + calculReward(i - 1);
                    if (gameContext.curHp > 0)
                    {
                        return SceneID.DungeonClear;
                    }
                    else
                    {
                        return SceneID.DungeonFail;
                    }
                }
                else
                {
                    gameContext.curHp = (int)(gameContext.prevHp * 0.5f);
                    gameContext.curGold = gameContext.prevGold;
                    return SceneID.DungeonFail;
                }
            }
            return sceneNext.next![i];
        }

        public int calculReduceHp(int i)
        {
            List<DungeonData> dungeonList = gameContext.dungeonList;
            Character ch = gameContext.ch;
            float changeDamage = dungeonList[i].recommandArmor - ch.getTotalGuard();
            float minDamage = Math.Max(20 + changeDamage, 0);
            float maxDamage = Math.Max(35 + changeDamage, 0);
            Random rand = new Random();
            return (int)(rand.NextDouble() * (maxDamage - minDamage) + minDamage);
        }

        public int calculReward(int i)
        {
            List<DungeonData> dungeonList = gameContext.dungeonList;
            Character ch = gameContext.ch;
            float changeDamage = dungeonList[i].recommandArmor - ch.getTotalGuard();
            float minExtraReward = 0.01f * ch.getTotalAttack();
            float maxExtraReward = 0.02f * ch.getTotalAttack();
            Random rand = new Random();
            return (int)(dungeonList[i].reward + dungeonList[i].reward * 
                (rand.NextDouble() * (maxExtraReward - minExtraReward) + minExtraReward));
        }
        public bool Success(int i)
        {
            List<DungeonData> dungeonList = gameContext.dungeonList;
            Character ch = gameContext.ch;
            if (dungeonList[i].recommandArmor > ch.getTotalGuard())
            {
                Random rand = new Random();
                if(rand.NextDouble() > 0.4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
