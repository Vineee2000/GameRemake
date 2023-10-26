using GameRemake;
using Graphics;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace GameLogic
{
    internal class Startup
    {
        static void Main(string[] args)
        {
            Thread graphicsThread = new Thread(new ThreadStart(Graphics.GraphicsStartup.DisplayWindow));
            graphicsThread.Start();
            GameLoop gameLoop = new GameLoop();
            gameLoop.RunGame();
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

        public void MoveBy(int x, int y);
        public void MoveBy(Vector2 movement);
        public void ChangeVelocity(int x, int y);
        public void ChangeVelocity(Vector2 velocity);
    }
}

namespace Graphics
{
    public class GraphicsStartup
    {
        public static void DisplayWindow()
        {
            GameWindow gameWindow = new GameWindow();
            Application.Run(gameWindow);
        }
    }
    public class GameWindow : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public GameWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "GameWindow";
        }

        #endregion
    }
}