using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Text.Json;
using TextRPG.View;
using TextRPG.Scene;
using static System.Formats.Asn1.AsnWriter;
using TextRPG.Context;


namespace TextRPG
{
    internal class MainCtrl
    {
        delegate AScene SceneMaker(GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap);
        static void Main(string[] args)
        {
            Console.SetWindowSize(183, 45);
            Console.SetBufferSize(183, 45);
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;


            string? name = "";
            string? useSaveStr = "";
            bool? useSave = null;
            while (true)
            {
                if (File.Exists(JsonPath.saveDataJsonPath))
                {
                    Console.Write("이미 존재하는 세이브 데이터를 사용합니까?(Y/N) : ");
                    while (useSaveStr?.Length <= 0)
                    {
                        useSaveStr = Console.ReadLine();
                    }
                    switch (useSaveStr?[0])
                    {
                        case 'Y': useSave = true; break;
                        case 'N': useSave = false; break;
                        default: Console.Clear(); Console.SetCursorPosition(0, 1); Console.Write("잘못된 응답입니다."); Console.SetCursorPosition(0, 0); break;
                    }
                    useSaveStr = "";
                    if (useSave != null)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            string saveDataJson;
            SaveData saveData;
            if (useSave == true && File.Exists(JsonPath.saveDataJsonPath))
            {
                saveDataJson = File.ReadAllText(JsonPath.saveDataJsonPath);
                saveData = JsonSerializer.Deserialize<SaveData>(saveDataJson)!;
            }
            else
            {
                Console.Clear();
                Console.Write("사용할 이름을 입력하세요 : ");
                name = Console.ReadLine();
                Console.Clear();
                saveDataJson = File.ReadAllText(JsonPath.defaultDataJsonPath);
                saveData = JsonSerializer.Deserialize<SaveData>(saveDataJson)!;
                saveData.name = name;
            }

            Dictionary<string, AView> viewMap = new();
            initViewMap(viewMap);
            Dictionary<string, SceneText> sceneTextMap = new();
            initSceneTextMap(sceneTextMap);
            Dictionary<string, SceneNext> sceneNextMap = new();
            initSceneNextMap(sceneNextMap);
            Dictionary<string, AScene> sceneMap = new();
            Dictionary<string, SceneMaker> sceneFactoryMap = new();
            initSceneFactoryMap(sceneFactoryMap);

            var dungeonDataJson = File.ReadAllText(JsonPath.dungeonDataJsonPath);
            var dungeonData = JsonSerializer.Deserialize<List<DungeonData>>(dungeonDataJson);
            GameContext gameContext = new(saveData!, dungeonData!);
            AScene startScene = sceneFactoryMap[SceneID.Main](gameContext, viewMap, sceneTextMap, sceneMap, sceneNextMap);

            run(gameContext, startScene, viewMap, sceneTextMap, sceneMap, sceneFactoryMap, sceneNextMap);
        }

        static void run(GameContext gameContext, AScene startScene, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneMaker> sceneFactoryMap, Dictionary<string, SceneNext> sceneNextMap)
        {
            AScene curScene = startScene;
            string response = "";
            curScene.DrawScene();
            ((InputView)viewMap[ViewID.Input]).SetCursor();
            string? str = "";
            while (true)
            {
                str = Console.ReadLine();
                if(str?.Length <= 0)
                {
                    curScene.DrawScene();
                }
                else if (str?[0] == 'r')
                {
                    foreach (var pair in viewMap)
                    {
                        pair.Value.Update();
                        pair.Value.Render();
                    }
                }
                else if (str?[0] == 'q')
                {
                    SaveData saveData = new SaveData(gameContext);
                    string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(JsonPath.saveDataJsonPath, json);
                    return;
                }
                else
                {
                    if (!(str![0] >= '0' && str![0] <= '9'))
                    {
                        continue;
                    }
                    int i = str![0] - '0';
                    response = curScene.respond(i);
                    if (response == SceneID.Nothing)
                    {
                        curScene.DrawScene();
                    }
                    else
                    {
                        curScene = sceneFactoryMap[response](gameContext, viewMap, sceneTextMap, sceneMap, sceneNextMap);
                        curScene.DrawScene();
                    }
                    // todo 다른 화면 넘어가기? or 현재 화면 처리하기
                }
                str = "";
                ((InputView)viewMap[ViewID.Input]).Render();
                ((InputView)viewMap[ViewID.Input]).SetCursor();
            }
        }

        static void initViewMap(Dictionary<string, AView> viewMap)
        {
            viewMap.Add(ViewID.Script, new ScriptView());
            viewMap.Add(ViewID.Dynamic, new DynamicView());
            viewMap.Add(ViewID.Sprite, new SpriteView());
            viewMap.Add(ViewID.Log, new LogView());
            viewMap.Add(ViewID.Choice, new ChoiceView());
            viewMap.Add(ViewID.Input, new InputView());
            var viewTranJson = File.ReadAllText(JsonPath.viewTranJsonPath);
            var viewDefs = JsonSerializer.Deserialize<List<ViewTransform>>(viewTranJson);
            foreach (var def in viewDefs!)
            {
                viewMap[def.key].setTransform(def);
                viewMap[def.key].Update();
                viewMap[def.key].Render();
            }
        }

        static void initSceneTextMap(Dictionary<string, SceneText> sceneTextMap)
        {
            var sceneTextJson = File.ReadAllText(JsonPath.sceneTextJsonPath);
            var sceneTexts = JsonSerializer.Deserialize<List<SceneText>>(sceneTextJson);
            foreach (var sceneText in sceneTexts!)
            {
                sceneTextMap[sceneText.key] = sceneText;
            }
        }

        static void initSceneNextMap(Dictionary<string, SceneNext> sceneNextMap)
        {
            var sceneNextJson = File.ReadAllText(JsonPath.sceneNextJsonPath);
            var sceneNexts = JsonSerializer.Deserialize<List<SceneNext>>(sceneNextJson);
            foreach (var sceneNext in sceneNexts!)
            {
                sceneNextMap[sceneNext.key] = sceneNext;
            }
        }

        delegate void AddPair<T>(string sceneKey, Dictionary<string, SceneMaker> sceneFactoryMap);

        static void initSceneFactoryMap(Dictionary<string, SceneMaker> sceneFactoryMap)
        {
            sceneFactoryMap[SceneID.Main] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Main))
                {
                    sceneMap[SceneID.Main] = new MainScene(gameContext, viewMap, sceneTextMap[SceneID.Main], sceneNextMap[SceneID.Main]);
                }
                return sceneMap[SceneID.Main];
            };

            sceneFactoryMap[SceneID.Wear] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Wear))
                {
                    sceneMap[SceneID.Wear] = new WearScene(gameContext, viewMap, sceneTextMap[SceneID.Wear], sceneNextMap[SceneID.Wear]);
                }
                return sceneMap[SceneID.Wear];
            };

            sceneFactoryMap[SceneID.Inventory] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Inventory))
                {
                    sceneMap[SceneID.Inventory] = new InventoryScene(gameContext, viewMap, sceneTextMap[SceneID.Inventory], sceneNextMap[SceneID.Inventory]);
                }
                return sceneMap[SceneID.Inventory];
            };

            sceneFactoryMap[SceneID.Status] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Status))
                {
                    sceneMap[SceneID.Status] = new StatusScene(gameContext, viewMap, sceneTextMap[SceneID.Status], sceneNextMap[SceneID.Status]);
                }
                return sceneMap[SceneID.Status];
            };

            sceneFactoryMap[SceneID.Shop] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Shop))
                {
                    sceneMap[SceneID.Shop] = new ShopScene(gameContext, viewMap, sceneTextMap[SceneID.Shop], sceneNextMap[SceneID.Shop]);
                }
                return sceneMap[SceneID.Shop];
            };

            sceneFactoryMap[SceneID.Buy] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Buy))
                {
                    sceneMap[SceneID.Buy] = new BuyScene(gameContext, viewMap, sceneTextMap[SceneID.Buy], sceneNextMap[SceneID.Buy]);
                }
                return sceneMap[SceneID.Buy];
            };

            sceneFactoryMap[SceneID.Rest] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Rest))
                {
                    sceneMap[SceneID.Rest] = new RestScene(gameContext, viewMap, sceneTextMap[SceneID.Rest], sceneNextMap[SceneID.Rest]);
                }
                return sceneMap[SceneID.Rest];
            };

            sceneFactoryMap[SceneID.Sell] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.Sell))
                {
                    sceneMap[SceneID.Sell] = new SellScene(gameContext, viewMap, sceneTextMap[SceneID.Sell], sceneNextMap[SceneID.Sell]);
                }
                return sceneMap[SceneID.Sell];
            };
            sceneFactoryMap[SceneID.DungeonSelect] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.DungeonSelect))
                {
                    sceneMap[SceneID.DungeonSelect] = new DungeonSelectScene(gameContext, viewMap, sceneTextMap[SceneID.DungeonSelect], sceneNextMap[SceneID.DungeonSelect]);
                }
                return sceneMap[SceneID.DungeonSelect];
            };
            sceneFactoryMap[SceneID.DungeonClear] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.DungeonClear))
                {
                    sceneMap[SceneID.DungeonClear] = new DungeonClearScene(gameContext, viewMap, sceneTextMap[SceneID.DungeonClear], sceneNextMap[SceneID.DungeonClear]);
                }
                return sceneMap[SceneID.DungeonClear];
            };
            sceneFactoryMap[SceneID.DungeonFail] = (GameContext gameContext, Dictionary<string, AView> viewMap, Dictionary<string, SceneText> sceneTextMap, Dictionary<string, AScene> sceneMap, Dictionary<string, SceneNext> sceneNextMap) =>
            {
                if (!sceneMap.ContainsKey(SceneID.DungeonFail))
                {
                    sceneMap[SceneID.DungeonFail] = new DungeonFailScene(gameContext, viewMap, sceneTextMap[SceneID.DungeonFail], sceneNextMap[SceneID.DungeonFail]);
                }
                return sceneMap[SceneID.DungeonFail];
            };
        }
    }
}

