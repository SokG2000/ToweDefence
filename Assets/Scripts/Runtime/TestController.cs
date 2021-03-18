using UnityEngine;

namespace Runtime
{
    public class TestController : IController
    {
        public void OnStart()
        {
            Debug.Log("Stast");
        }

        public void OnStop()
        {
            Debug.Log("Stop");
        }

        public void Tick()
        {
            Debug.Log("Tick");
        }
    }
}