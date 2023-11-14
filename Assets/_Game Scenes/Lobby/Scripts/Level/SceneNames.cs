using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneNames
{
    public static string loadingScene = "LoadingScene";
    public static string mainMenu = "MainMenu";
    public static string lobbyScene = "Lobby";
    public static string meteorShower = "MeteorShower";
    public static string villageSceneName = "VillageScene";
    public static string spaceshipCombatSceneName = "SpaceshipCombat";
    public static string worldTravelSceneName = "WorldTravelScene";
    public static string schoolRoomScene = "SchoolRoom";
    public static string skinChangerScene = "SkinChangerScene";
    public static string environmentRunnerScene = "EnvironmentRunner";
    
    public static List<SceneNameAndCode> sceneNameAndCodes = new()
    {
        new SceneNameAndCode(SceneNames.meteorShower, "mat001"),
        new SceneNameAndCode(SceneNames.villageSceneName, "mat002"),
        new SceneNameAndCode(SceneNames.spaceshipCombatSceneName, "spaceship"),
        new SceneNameAndCode(SceneNames.worldTravelSceneName, "geo001"),
        new SceneNameAndCode(SceneNames.schoolRoomScene, "cie001"),
        new SceneNameAndCode(SceneNames.environmentRunnerScene, "amb001")
    };
}

public class SceneNameAndCode
{
    public string sceneName;
    public string sceneCode;

    public SceneNameAndCode(string name, string code)
    {
        sceneName = name;
        sceneCode = code;
    }
}
