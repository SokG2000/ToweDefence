using Runtime;

namespace Enemy
{
    public class MovementController : IController
    {
        public void OnStart()
        {
            
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            foreach (EnemyData enemyData in Game.Player.EnemyDatas)
            {
                enemyData.MView.MMovementAgent.TickMovement();
            }
        }
    }
}