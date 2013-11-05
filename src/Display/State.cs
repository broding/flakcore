using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Flakcore.Display
{
    public class State : Node
    {
        public Color BackgroundColor { get; protected set; }

        public State()
        {
            this.BackgroundColor = Color.DarkSlateGray;
        }

        public virtual void Load()
        {
        }

        public override List<Node> GetAllChildren(List<Node> nodes)
        {
            if (Children.Count == 0)
                return nodes;
            else
            {
                foreach (Node child in Children)
                {
                    child.GetAllChildren(nodes);
                }

                return nodes;
            }
        }
    }

    public enum StateTransition
    {
        IMMEDIATELY,
        FADE
    }
}
