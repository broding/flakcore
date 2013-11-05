using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace Flakcore.Utils
{
    public class Pool<T> where T : IPoolable
    {
        private int initialSize;
        private Predicate<T> isItemValid;
        private Func<T> allocateItem;
		private bool canResize;

        private T[] items;
        private int nextIndex;

        public Pool(int initialSize, bool canResize, Predicate<T> isItemValid, Func<T> allocateItem)
        {
            if (initialSize < 1)
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");
            if (isItemValid == null)
                throw new ArgumentNullException("validateFunc");
            if (allocateItem == null)
                throw new ArgumentNullException("allocateFunc");

            this.initialSize = initialSize;
            this.canResize = canResize;
            this.isItemValid = isItemValid;
            this.allocateItem = allocateItem;

            this.items = new T[initialSize];
            this.nextIndex = 0;

            this.InitializeItems();
        }

        private void InitializeItems()
        {
            for (int i = 0; i < this.initialSize; i++)
            {
                this.items[i] = this.allocateItem();
                this.items[i].PoolIndex = i;
                this.items[i].ReportDeadToPool = this.ReportDead;
            }
        }

        public T New()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                T item = this.items[i];

                if (this.isItemValid(item))
                {
                    this.nextIndex = ++i;
                    return item;
                }
            }

            throw new Exception("Could not find valid items in the pool");
        }

        public void ReportDead(int index)
        {
            if (index < this.nextIndex)
                this.nextIndex = index;
        }
    }
}
