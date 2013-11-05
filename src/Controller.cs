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
    public class Controller
    {
        public static float Gravity { get; set; }
        public static Input Input { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static ContentManager Content { get; private set; }
        public static Vector2 ScreenSize { get; private set; }
        public static Vector2 LevelBorderSize { get; set; }

        public static LayerController LayerController { get; private set; }
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
        public static Camera CurrentDrawCamera;

        public static SpriteFont FontDefault;

        public static void Initialize(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content, Core core)
        {
            Controller.Gravity = 14;
            Controller.Graphics = graphics;
            Controller.Content = content;
            Controller.Input = new Input();
            Controller.LayerController = new LayerController();
            Controller.FontController = new FontController();

            Controller.ScreenSize = screenSize;
            Controller.Core = core;
            Controller.WorldBounds = Rectangle.Empty;

        }

        /// <summary>
        /// Used to switch between states; old state gets deleted
        /// </summary>
        /// <param name="state"></param>
        public static void SwitchState(Type state)
        {
            Controller.SwitchState(state, StateTransition.IMMEDIATELY, StateTransition.IMMEDIATELY);
        }

        public static void SwitchState(State state)
        {
            Core.SwitchState(state);
        }

        public static void SwitchState(Type state, StateTransition startTransition, StateTransition endTransition)
        {
            Core.SwitchState(state, startTransition, endTransition);
        }

        public static void AddCamera()
        {
            Core.Cameras.Add(new Camera(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y));

            RecalculateCameras();
        }

        public static Camera GetCamera(int index)
        {
            return Core.Cameras.ElementAt(index);
        }

        private static void RecalculateCameras()
        {
            int cameraWidth = (int)ScreenSize.X / Core.Cameras.Count;

            for (int i = 0; i < Core.Cameras.Count; i++)
            {
                Core.Cameras.ElementAt(i).resetViewport(cameraWidth * i, 0, cameraWidth, (int)ScreenSize.Y);
            }
        }

        public int TotalCameras()
        {
            return Core.Cameras.Count;
        }

        public static void Collide(Node node, string collideGroup)
        {
            Core.CollisionSolver.addCollision(node, collideGroup, null, null);
        }

        public static void Collide(Node node, string collideGroup, Action<Node, Node> callback, Func<Node, Node, bool> checker)
        {
            Core.CollisionSolver.addCollision(node, collideGroup, callback, checker);
        }

        public static void Collide(Node node, string collideGroup, Action<Node, Node> callback)
        {
            Core.CollisionSolver.addCollision(node, collideGroup, callback, null);
        }

    }
}
