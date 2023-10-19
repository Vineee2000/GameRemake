using System.Diagnostics;

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
        private List<GameObject> gameObjectsList = new List<GameObject>();
        internal void RunGame()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                foreach (var gameObject in gameObjectsList.ToList())
                {
                    gameObject.Update();
                    if (gameObject.dead)
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
    internal class GameObject
    {
        public bool dead { get; set; } = false;
        public void Update()
        {

        }
    }
}