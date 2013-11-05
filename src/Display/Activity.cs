﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public abstract class Activity
    {
        public Action Callback;
        public Node Node;

        protected int Duration;
        protected int Time;

        private bool Started;

        public Activity(Node node, int duration)
        {
            this.Node = node;
            this.Duration = duration;
            this.Time = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!this.Started)
                return;

            this.Time += gameTime.ElapsedGameTime.Milliseconds;

            if (this.Time > this.Duration)
                this.Done();
        }

        public void Start()
        {
            this.Started = true;
        }

        protected virtual void Done()
        {
            if (this.Callback != null)
                this.Callback.Invoke();

            this.Node.RemoveActivity(this);

            return;
        }
    }
}
