using Assets;
using Enemy;
using Runtime;
using UnityEngine;
using Grid = Field.Grid;

namespace EnemySpawn
{
    public class EnemySpawnController : IController
    {
        private SpawnWavesAsset m_SpawnWavesAsset;
        private Grid m_Grid;
        private float m_SpawnStartTime;
        private float m_PassedTimeAtPreviousFrame = -1f;

        public EnemySpawnController(SpawnWavesAsset mSpawnWavesAsset, Grid mGrid)
        {
            m_SpawnWavesAsset = mSpawnWavesAsset;
            m_Grid = mGrid;
        }

        public void OnStart()
        {
            m_SpawnStartTime = Time.time; // время с начала игры
        }

        public void OnStop()
        {
            
        }

        public void Tick()
        {
            float passedTime = Time.time - m_SpawnStartTime;
            // spawn if need
            float timeToSpawn = 0f;
            foreach (SpawnWave wave in m_SpawnWavesAsset.SpawnWaves)
            {
                timeToSpawn += wave.TimeBeforeSpawnWave;
                for (int i = 0; i < wave.Count; ++i)
                {
                    if (passedTime >= timeToSpawn && m_PassedTimeAtPreviousFrame < timeToSpawn)
                    {
                        SpawnEnemy(wave.EnemyAsset);
                    }

                    timeToSpawn += wave.TimeBetweenSpawns;
                }
            }

            m_PassedTimeAtPreviousFrame = passedTime;
        }

        private void SpawnEnemy(EnemyAsset asset)
        {
            EnemyView view = Object.Instantiate(asset.ViewPrefab);
            Vector3 position = m_Grid.GetStartNode().Position;
            position.y = view.transform.position.y;
            view.transform.position = position;
            EnemyData data = new EnemyData(asset);
            data.AttachView(view);
            view.CreateMovementAgent(m_Grid);
            
            Game.Player.EnemySpawned(data);
        }
    }
}