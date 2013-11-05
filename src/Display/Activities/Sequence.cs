using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Flakcore.Display.Activities
{
    public class Sequence : Activity
    {
        private List<Activity> Activities;

        public Sequence(Node node) : base(node, 10000)
        {
            this.Activities = new List<Activity>(30);
        }

        public void AddActivity(Activity activity)
        {
            this.Activities.Add(activity);

            if (this.Activities.Count == 1)
                this.Activities[0].Callback = this.ActivityDone;
                
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.Activities.Count <= 0)
                this.Callback();
            else
                this.Activities[0].Update(gameTime);
        }

        private void ActivityDone()
        {
            this.Activities.RemoveRange(0, 1);
        }
    }
}
