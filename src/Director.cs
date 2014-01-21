using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Flakcore.Display;
using Flakcore.Physics;

namespace Flakcore
{
    public class Director
    {
		public static String ContentPath = "../../../Content/";

		public static bool DrawDebug;
        public static Input Input { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static ContentManager Content { get; private set; }
        public static Vector2 ScreenSize { get; private set; }

        public static int UpdateCalls;

		private static Core _core;
        private static Rectangle _worldBounds;
        public static Rectangle WorldBounds 
        { 
            get { return _worldBounds; }
            set { 
                _worldBounds = value;  
            } 
        }
        public static Camera Camera;

        public static SpriteFont FontDefault;

        public static void Initialize(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content, Core core)
        {
            Director.Graphics = graphics;
            Director.Content = content;
            Director.Input = new Input();
			Director.DrawDebug = false;
            Director.ScreenSize = screenSize;
			Director._core = core;
            Director.WorldBounds = Rectangle.Empty;
			Director.ContentPath = Content.RootDirectory;
        }

        /// <summary>
        /// Used to switch between states; old state gets deleted
        /// </summary>
        /// <param name="state"></param>
        public static void SwitchState(Type state)
        {
			Director.SwitchState ((State)Activator.CreateInstance(state));
        }

        public static void SwitchState(State state)
        {
			_core.SwitchState(state);
        }

		public static void Collide(Node node1, Node node2)
		{
			_core.CollisionSolver.AddCollision(node1, node2, null, null);
		}

    }
}
