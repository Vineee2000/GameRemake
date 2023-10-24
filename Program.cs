using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace GameLogic
{
    internal class Startup
    {
        static void Main(string[] args)
        {
            GameLoop gameLoop = new GameLoop();
            while (true)
            {
                gameLoop.RunGame();
            }
        }
    }

    internal class GameLoop
    {
        private static int tickTime = 15;
        private List<IGameObject> gameObjectsList = new List<IGameObject>();
        internal void RunGame()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                foreach (var gameObject in gameObjectsList.ToList())
                {
                    gameObject.Update();
                    if (gameObject.Dead)
                    {
                        gameObjectsList.Remove(gameObject);
                    }
                }
                Console.WriteLine("Tick");
                if (sw.Elapsed < TimeSpan.FromMilliseconds(tickTime))
                {
                    Thread.Sleep((int)(tickTime - sw.ElapsedMilliseconds));
                }
                sw.Restart();
            }
        }
    }
    public interface IGameObject
    {
        public bool Dead { get; set; }
        public Point Position { get; set; }

        public void Update() { }
    }
    internal interface IMovableGameObject : IGameObject
    {
        public Vector2 Velocity { get; set; }

        public Point MoveBy(int x, int y);
        public Vector2 ChangeVelocity(int x, int y);
    }
}

namespace Graphics
{

}