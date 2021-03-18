using System;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Runtime
{
    public static class Game
    {
        private static Player s_Player;
        private static AssetRoot s_AssetRoot;
        private static LevelAsset s_CurrentLevelAsset;

        private static Runner s_Runner;

        public static Player Player => s_Player;

        public static AssetRoot AssetRoot => s_AssetRoot;

        public static LevelAsset CurrentLevelAsset => s_CurrentLevelAsset;

        public static void SetAssetRoot(AssetRoot assetRoot)
        {
            s_AssetRoot = assetRoot;
        }

        public static void StartLevel(LevelAsset levelAsset)
        {
            s_CurrentLevelAsset = levelAsset;
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelAsset.SceneAsset.name);
            operation.completed += StartPlayer;
            //StartPlayer(); // после загрузки сцены
        }

        private static void StartPlayer(AsyncOperation operation)
        {
            if (!operation.isDone)
            {
                throw new Exception("Can't load scene");
            }
            s_Player = new Player();
            s_Runner = Object.FindObjectOfType<Runner>();
            s_Runner.StartRunning();
        }

        private static void StopPlayer()
        {
            s_Runner.StopRunning();
        }
    }
}