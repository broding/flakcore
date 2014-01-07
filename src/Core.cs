﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Flakcore.Display;
using Microsoft.Xna.Framework.Content;
using Flakcore.Utils;
using Flakcore.Physics;
using System.Diagnostics;
using System.Threading;

namespace Flakcore
{
    public class Core
    {
        public List<Camera> Cameras { get; private set; }
        public CollisionSolver CollisionSolver { get; private set; }
		
		private State currentState;
		private GraphicsDeviceManager graphics;
        private QuadTree collisionQuadTree;
        private Stopwatch stopwatch;

        public Core(Vector2 screenSize, GraphicsDeviceManager graphics, ContentManager content)
        {
            Director.Initialize(screenSize, graphics, content, this);

			this.graphics = graphics;
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            this.Cameras = new List<Camera>();
            Camera camera = new Camera(0,0,(int)screenSize.X, (int)screenSize.Y);
            this.Cameras.Add(camera);

            Director.CurrentDrawCamera = camera;
            Director.WorldBounds = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);

            SetupQuadTree();

            this.stopwatch = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            if (this.currentState == null)
                return;

            this.stopwatch.Reset();
            this.stopwatch.Start();
            ResetCollisionQuadTree();

			CollisionSolver.Reset ();

            this.stopwatch.Stop();
            DebugInfo.AddDebugItem("Reset Collision Quad", this.stopwatch.ElapsedMilliseconds + " ms");

            this.stopwatch.Reset();
            this.stopwatch.Start();

			currentState.Update (gameTime);

            this.stopwatch.Stop();
            DebugInfo.AddDebugItem("Update", this.stopwatch.ElapsedMilliseconds + " ms");

            this.stopwatch.Reset();
            this.stopwatch.Start();

			CollisionSolver.Resolve (gameTime);

            this.stopwatch.Stop();

            DebugInfo.AddDebugItem("Resolve Collisions", this.stopwatch.ElapsedMilliseconds + " ms");

            this.stopwatch.Reset();
            this.stopwatch.Start();

			currentState.Update (gameTime);
			currentState.PostUpdate (gameTime);

            this.stopwatch.Stop();

            DebugInfo.AddDebugItem("Post Update", this.stopwatch.ElapsedMilliseconds + " ms");
            DebugInfo.AddDebugItem("Update calls", Director.UpdateCalls + " times");
            DebugInfo.AddDebugItem("Allocated memory", System.GC.GetTotalMemory(false) / 131072 + " mb");

            Director.UpdateCalls = 0;

            Director.Input.Update(gameTime);

            foreach (Camera camera in Cameras)
                camera.update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
			this.graphics.GraphicsDevice.Clear (Color.Aquamarine);

            if (this.currentState == null)
                return;

            this.stopwatch.Reset();
            this.stopwatch.Start();
            Director.Graphics.GraphicsDevice.Clear(currentState.BackgroundColor);

            foreach (Camera camera in Cameras)
            {
                Director.CurrentDrawCamera = camera;
                Director.Graphics.GraphicsDevice.Viewport = camera.Viewport;

				currentState.DrawCall (spriteBatch);

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

            this.stopwatch.Stop();
            DebugInfo.AddDebugItem("Draw", this.stopwatch.ElapsedMilliseconds + " ms");
            DebugInfo.AddDebugItem("FPS", "" + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds));

            spriteBatch.Begin();
            //DebugInfo.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void SwitchState(State state)
        {
            if (this.currentState != null)
            {
				currentState.Dispose ();
            }

            this.currentState = null;
            this.currentState = state;
        }

        public void SwitchState(Type state, StateTransition startTransition, StateTransition endTransition)
        {
            this.currentState = null;
            this.currentState = (State)Activator.CreateInstance(state);
        }

        private void ResetCollisionQuadTree()
        {
            this.collisionQuadTree.clear();

            List<Node> children = this.currentState.GetAllCollidableChildren(new List<Node>());

            foreach (Node child in children)
                this.collisionQuadTree.insert(child);

#if(DEBUG)
            DebugInfo.AddDebugItem("Collidable Children", children.Count + " children");
#endif
        }

        private void DrawCollisionQuad(SpriteBatch spriteBatch)
        {
            Texture2D blank = new Texture2D(Director.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[]{Color.White});

            List<BoundingRectangle> quads = collisionQuadTree.getAllQuads(new List<BoundingRectangle>());

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
            collisionQuadTree = new QuadTree(0, new BoundingRectangle(0, 0, Director.WorldBounds.Width, Director.WorldBounds.Height));
            CollisionSolver = new CollisionSolver(collisionQuadTree);
        }
    }
}
