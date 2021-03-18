namespace Runtime
{
    public interface IController
    {
        void OnStart(); // создание
        void OnStop(); // остановка
        void Tick(); // каждый такт
    }
}