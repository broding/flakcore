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
using System.Diagnostics;
using System.Threading;
using FlakCore;

namespace Flakcore
{
    public class Core
    {
        public CollisionSolver CollisionSolver { get; private set; }

		private Camera _camera;
		private Renderer _renderer;
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

            _camera = new Camera(0,0,(int)screenSize.X, (int)screenSize.Y);
			_renderer = new Renderer ();

			Director.Camera = _camera;
            Director.WorldBounds = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);

			CollisionSolver = new CollisionSolver();

            this.stopwatch = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            if (this.currentState == null)
                return;

            this.stopwatch.Reset();
            this.stopwatch.Start();

			currentState.Update (gameTime);

            this.stopwatch.Stop();
            DebugInfo.AddDebugItem("Update", this.stopwatch.ElapsedMilliseconds + " ms");


            this.stopwatch.Reset();
            this.stopwatch.Start();

			currentState.Update (gameTime);
			currentState.PostUpdate (gameTime);

            this.stopwatch.Stop();

			this.stopwatch.Reset();
			this.stopwatch.Start();

			CollisionSolver.Resolve (gameTime);

			this.stopwatch.Stop();

			DebugInfo.AddDebugItem("Resolve Collisions", this.stopwatch.ElapsedMilliseconds + " ms");

            DebugInfo.AddDebugItem("Post Update", this.stopwatch.ElapsedMilliseconds + " ms");
            DebugInfo.AddDebugItem("Update calls", Director.UpdateCalls + " times");
            DebugInfo.AddDebugItem("Allocated memory", System.GC.GetTotalMemory(false) / 131072 + " mb");

            Director.UpdateCalls = 0;
			CollisionSolver.Reset ();

            Director.Input.Update(gameTime);

			_camera.update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
			this.graphics.GraphicsDevice.Clear (Color.Black);

            if (this.currentState == null)
                return;

			_renderer.Draw (currentState, spriteBatch);

			#if(DEBUG)  
			if(Director.DrawDebug)
				this.DrawDebug(spriteBatch, _camera, gameTime);    
			#endif

			Node.ResetDrawDepth();
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

		public void SwitchState(Type state)
		{
			SwitchState ((State)Activator.CreateInstance(state));
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
    }
}
