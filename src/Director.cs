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
        public static Input Input { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static ContentManager Content { get; private set; }
        public static Vector2 ScreenSize { get; private set; }

        public static FontController FontController { get; private set; }

        public static int UpdateCalls;

        private static Core Core;
        private static Rectangle _worldBounds;
        public static Rectangle WorldBounds 
        { 
            get { return _worldBounds; }
            set { 
                _worldBounds = value;  
                Core.SetupQuadTree(); 
            } 
        }
        public static Camera Camera;

        public static SpriteFont FontDefault;

        public static void Initialize(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content, Core core)
        {
            Director.Graphics = graphics;
            Director.Content = content;
            Director.Input = new Input();
            Director.FontController = new FontController();

            Director.ScreenSize = screenSize;
            Director.Core = core;
            Director.WorldBounds = Rectangle.Empty;
        }

        /// <summary>
        /// Used to switch between states; old state gets deleted
        /// </summary>
        /// <param name="state"></param>
        public static void SwitchState(Type state)
        {
            Director.SwitchState(state, StateTransition.IMMEDIATELY, StateTransition.IMMEDIATELY);
        }

        public static void SwitchState(State state)
        {
            Core.SwitchState(state);
        }

        public static void SwitchState(Type state, StateTransition startTransition, StateTransition endTransition)
        {
            Core.SwitchState(state, startTransition, endTransition);
        }

		public static void Collide(Node node1, Node node2)
		{
			Core.CollisionSolver.AddCollision(node1, node2, null, null);
		}

    }
}
