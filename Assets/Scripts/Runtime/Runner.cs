using System;
using System.Collections.Generic;
using Enemy;
using EnemySpawn;
using Field;
using TurretSpawn;
using UnityEngine;

namespace Runtime
{
    public class Runner : MonoBehaviour
    {
        private List<IController> m_Controllers;
        private bool m_IsRunnung = false;

        public void Update()
        {
            if (!m_IsRunnung)
            {
                return;
            }
            TickControllers();
        }

        public void StartRunning()
        {
            CreateAllControllers();
            OnStartControllers();
            m_IsRunnung = true;
        }

        public void StopRunning()
        {
            m_IsRunnung = false;
            OnStopControllers();
        }

        private void CreateAllControllers()
        {
            m_Controllers = new List<IController>();
            m_Controllers.Add(new GridPointerController(Game.Player.GridHolder));
            m_Controllers.Add(new EnemySpawnController(Game.CurrentLevelAsset.spawnWavesAsset, Game.Player.Grid));
            m_Controllers.Add(new TurretSpawnController(Game.Player.Grid, Game.Player.TurretMarket));
            m_Controllers.Add(new MovementController());
            
        }

        private void OnStartControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.OnStart();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
        }
        
        private void OnStopControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.OnStop();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
        }
        private void TickControllers()
        {
            foreach (IController controller in m_Controllers)
            {
                try
                {
                    controller.Tick();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                
            }
        }
    }
}