using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

// 새 Scene을 만들때 복붙
namespace TextRPG.Scene
{
    public class EmptyScene : AScene
    {
        public EmptyScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
