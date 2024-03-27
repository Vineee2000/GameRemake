using Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    internal interface IGameObject
    {
        public bool ToBeRemoved { get; set; }
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
    internal interface IFormsDrawableGameObject : IGameObject
    {
        public void Draw(double[] gridConversionFactor, GameWindow gameWindow, System.Drawing.Graphics g);
    }
    internal interface IRotatableGameObject : IGameObject
    {
        public Vector2 HeadingVector 
        { 
            get => HeadingVector; 
            set => Vector2.Normalize(value); 
        }
        public void SetHeadingAngle(double headingAngle);
        public void ChangeHeadingAngle(double angleChange);
    }

    internal class Turret : IGameObject, IFormsDrawableGameObject, IRotatableGameObject
    {
        public bool ToBeRemoved {  set; get; }
        public Point Position { get; set; }
        internal Turret(Point position)
        {
            Position = position;
        }

        public void SetHeadingAngle(double headingAngle)
        {
            throw new NotImplementedException();
        }

        public void ChangeHeadingAngle(double angleChange)
        {
            throw new NotImplementedException();
        }

        public void Draw(double[] gridConversionFactor, GameWindow gameWindow, System.Drawing.Graphics g)
        {
            SolidBrush myBrush = new SolidBrush(Color.Red);
            g.FillRectangle(myBrush, new Rectangle(new Point(Convert.ToInt32(Position.X * gridConversionFactor[0]), Convert.ToInt32(Position.Y * gridConversionFactor[1])), new Size(20, 20)));
        }
    }
}
