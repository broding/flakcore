using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace Flakcore
{
    public class LayerController
    {
        public List<Layer> Layers { get; private set; }

        public LayerController()
        {
            this.Layers = new List<Layer>();
        }

        public Layer AddLayer(Layer layer)
        {
            this.Layers.Add(layer);

            return layer;
        }

        public Layer AddLayer(string layerName)
        {
            Layer layer = new Layer();
            layer.Name = layerName;

            return this.AddLayer(layer);
        }

        public Layer GetLayer(string name)
        {
            foreach (Layer layer in this.Layers)
            {
                if (layer.Name == name)
                    return layer;
            }

            throw new Exception("Could not find layer");
        }

        internal void SortLayersByDepth()
        {
            this.Layers = this.Layers.OrderBy(o => o.Depth).ToList();
        }
    }
}
