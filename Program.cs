using GameLogic;
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
            GameLoop gameLoop = new GameLoop();
            GraphicsStartup.gameLoop = gameLoop;
            gameLoop.InitialiseGame();
            Thread graphicsThread = new Thread(new ThreadStart(GraphicsStartup.DisplayWindow));
            graphicsThread.Start();
            gameLoop.RunGame();
        }
    }

    internal class GameLoop
    {
        public bool running = true;

        private static int tickTime = 15;
        internal Size GameGridSize { get;} = new Size(1000, 1000);
        internal List<IGameObject> gameObjectsList = new List<IGameObject>();
        internal void InitialiseGame()
        {
            this.gameObjectsList.Add(new Turret(new Point(200, 900)));
            this.gameObjectsList.Add(new Turret(new Point(400, 900)));
            this.gameObjectsList.Add(new Turret(new Point(600, 900)));
            this.gameObjectsList.Add(new Turret(new Point(800, 900)));
        }
        internal void RunGame()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (running)
            {
                foreach (var gameObject in gameObjectsList.ToList())
                {
                    gameObject.Update();
                    if (gameObject.ToBeRemoved)
                    {
                        gameObjectsList.Remove(gameObject);
                    }
                }
                if (sw.Elapsed < TimeSpan.FromMilliseconds(tickTime))
                {
                    Thread.Sleep((int)(tickTime - sw.ElapsedMilliseconds));
                }
                sw.Restart();
            }
        }
    }
}

namespace Graphics
{
    public static class GraphicsStartup
    {
        internal static GameLoop gameLoop;
        internal static void DisplayWindow()
        {
            GameWindow gameWindow = new GameWindow(gameLoop);
            Thread thread = new Thread(() => { Application.Run(gameWindow); });
            thread.Start();
            gameWindow.GameDrawLoop(TimeSpan.FromMilliseconds(1000 / 60));
        }
    }
    public class GameWindow : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private GameLoop gameLoop;
        private double[] gridConversionFactor;

        internal GameWindow(GameLoop gameLoop)
        {
            this.gameLoop = gameLoop;
            InitializeComponent();

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1280, 720);
            this.gridConversionFactor = new double[2] { (double)this.ClientSize.Width / this.gameLoop.GameGridSize.Width, (double)this.ClientSize.Height / this.gameLoop.GameGridSize.Height };
            this.Text = "GameWindow";

            this.FormClosed += EndGameLogicThread;
        }

        private void EndGameLogicThread(object sender, EventArgs e)
        {
            this.gameLoop.running = false;
        }

        internal void GameDrawLoop(TimeSpan frameDuration)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (this.gameLoop.running)
            {
                try
                {
                    this.Invoke(() => Refresh());
                }
                catch (ObjectDisposedException) { }
                if (sw.Elapsed < frameDuration)
                {
                    Thread.Sleep((int)(frameDuration.TotalMilliseconds - sw.ElapsedMilliseconds));
                }
                sw.Restart();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Graphics g = e.Graphics;
            foreach (IFormsDrawableGameObject gameObject in this.gameLoop.gameObjectsList)
            {
                gameObject.Draw(this.gridConversionFactor, this, g);
            }
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
            
        }

        #endregion
    }
}