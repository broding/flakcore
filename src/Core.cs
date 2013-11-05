using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Display;
using Microsoft.Xna.Framework.Content;
using Flakcore.Utils;
using Flakcore.Physics;
using Flakcore.Display.Level;
using System.Diagnostics;
using System.Threading;

namespace Flakcore
{
    public class Core
    {
        public List<Camera> Cameras { get; private set; }
        public CollisionSolver CollisionSolver { get; private set; }

        private QuadTree CollisionQuad;
        private Stopwatch Stopwatch;

        private State CurrentState;

        public Core(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content)
        {
            Controller.Initialize(screenSize, graphics, content, this);

            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            this.Cameras = new List<Camera>();
            Camera camera = new Camera(0,0,(int)screenSize.X, (int)screenSize.Y);
            this.Cameras.Add(camera);
            Controller.CurrentDrawCamera = camera;

            Controller.WorldBounds = new Rectangle(0, 0, 2500,2000);

            Controller.LayerController.AddLayer("base");
            SetupQuadTree();

            this.Stopwatch = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            if (this.CurrentState == null)
                return;

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            ResetCollisionQuadTree();
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Reset Collision Quad", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            for(int i = 0; i < Controller.LayerController.Layers.Count; i++)
            {
                Controller.LayerController.Layers[i].Update(gameTime);
            }
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Update", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            this.CollisionSolver.resolveCollisions(gameTime);
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Resolve Collisions", this.Stopwatch.ElapsedMilliseconds + " ms");

            this.Stopwatch.Reset();
            this.Stopwatch.Start();

            foreach (Layer layer in Controller.LayerController.Layers)
            {
                layer.PostUpdate(gameTime);
            }
            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Post Update", this.Stopwatch.ElapsedMilliseconds + " ms");

            DebugInfo.AddDebugItem("Update calls", Controller.UpdateCalls + " times");
            DebugInfo.AddDebugItem("Allocated memory", System.GC.GetTotalMemory(false) / 131072 + " mb");
            Controller.UpdateCalls = 0;

            Controller.Input.Update(gameTime);

            foreach (Camera camera in Cameras)
                camera.update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (this.CurrentState == null)
                return;

            this.Stopwatch.Reset();
            this.Stopwatch.Start();
            Controller.Graphics.GraphicsDevice.Clear(CurrentState.BackgroundColor);

            Controller.LayerController.SortLayersByDepth();

            foreach (Camera camera in Cameras)
            {
                Controller.CurrentDrawCamera = camera;
                Controller.Graphics.GraphicsDevice.Viewport = camera.Viewport;

                foreach (Layer layer in Controller.LayerController.Layers)
                {
                    if (layer.Parent != null)
                        continue;

                    Controller.Graphics.GraphicsDevice.SetRenderTarget(layer.RenderTarget);
                    Controller.Graphics.GraphicsDevice.Clear(Color.Transparent);

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformMatrix());
                    layer.DrawCall(spriteBatch);
                    spriteBatch.End();
                }

                Controller.Graphics.GraphicsDevice.SetRenderTarget(null);

                foreach (Layer layer in Controller.LayerController.Layers)
                {
                    if (layer.Parent != null)
                        continue;

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

                    if (layer.PostEffectAction != null)
                        layer.PostEffectAction(layer, gameTime);

                    spriteBatch.Draw(layer.RenderTarget, Vector2.Zero, Color.White);
                    spriteBatch.End();
                }

#if(DEBUG)  
                this.DrawDebug(spriteBatch, camera, gameTime);    
#endif

                Node.ResetDrawDepth();

            }
        }

        private void DrawDebug(SpriteBatch spriteBatch, Camera camera, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, camera.GetTransformMatrix());
            DrawCollisionQuad(spriteBatch);
            spriteBatch.End();

            this.Stopwatch.Stop();
            DebugInfo.AddDebugItem("Draw", this.Stopwatch.ElapsedMilliseconds + " ms");
            DebugInfo.AddDebugItem("FPS", "" + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds));

            spriteBatch.Begin();
            DebugInfo.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void SwitchState(State state)
        {
            if (this.CurrentState != null)
            {
                Controller.LayerController.GetLayer("base").RemoveAllChildren();
            }

            this.CurrentState = null;
            this.CurrentState = state;
            Controller.LayerController.GetLayer("base").AddChild(this.CurrentState);
        }

        public void SwitchState(Type state, StateTransition startTransition, StateTransition endTransition)
        {
            this.CurrentState = null;
            this.CurrentState = (State)Activator.CreateInstance(state);
        }

        private void ResetCollisionQuadTree()
        {
            this.CollisionQuad.clear();

            List<Node> children = this.CurrentState.GetAllCollidableChildren(new List<Node>());

            foreach (Node child in children)
            {
                this.CollisionQuad.insert(child);
            }
#if(DEBUG)
            DebugInfo.AddDebugItem("Collidable Children", children.Count + " children");
#endif
        }

        private void DrawCollisionQuad(SpriteBatch spriteBatch)
        {
            Texture2D blank = new Texture2D(Controller.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[]{Color.White});

            List<BoundingRectangle> quads = CollisionQuad.getAllQuads(new List<BoundingRectangle>());

            foreach (BoundingRectangle quad in quads)
            {
                // left
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y), new Vector2(quad.X, quad.Y + quad.Height));
                // right
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X + quad.Width, quad.Y), new Vector2(quad.X + quad.Width, quad.Y + quad.Height));
                // top
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y), new Vector2(quad.X + quad.Width, quad.Y));
                // bottom
                DebugInfo.DrawLine(spriteBatch, blank, 1, Color.White, new Vector2(quad.X, quad.Y + quad.Height), new Vector2(quad.X + quad.Width, quad.Y + quad.Height));
            }
        }

        public void SetupQuadTree()
        {
            CollisionQuad = new QuadTree(0, new BoundingRectangle(0, 0, Controller.WorldBounds.Width, Controller.WorldBounds.Height));
            CollisionSolver = new CollisionSolver(CollisionQuad);
        }
    }
}
