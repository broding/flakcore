using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flakcore.Display;

namespace Flakcore.Utils
{
    public class Pool<T> where T : IPoolable
    {
        private int InitialSize;
        private bool CanResize;
        private Predicate<T> IsItemValid;
        private Func<T> AllocateItem;

        private T[] Items;
        private int NextIndex;

        public Pool(int initialSize, bool canResize, Predicate<T> isItemValid, Func<T> allocateItem)
        {
            if (initialSize < 1)
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");
            if (isItemValid == null)
                throw new ArgumentNullException("validateFunc");
            if (allocateItem == null)
                throw new ArgumentNullException("allocateFunc");

            this.InitialSize = initialSize;
            this.CanResize = canResize;
            this.IsItemValid = isItemValid;
            this.AllocateItem = allocateItem;

            this.Items = new T[initialSize];
            this.NextIndex = 0;

            this.InitializeItems();
        }

        private void InitializeItems()
        {
            for (int i = 0; i < this.InitialSize; i++)
            {
                this.Items[i] = this.AllocateItem();
                this.Items[i].PoolIndex = i;
                this.Items[i].ReportDeadToPool = this.ReportDead;
            }
        }

        public T New()
        {
            for (int i = 0; i < this.Items.Length; i++)
            {
                T item = this.Items[i];

                if (this.IsItemValid(item))
                {
                    this.NextIndex = ++i;
                    return item;
                }
            }

            throw new Exception("Could not find valid items in the pool");
        }

        public void ReportDead(int index)
        {
            if (index < this.NextIndex)
                this.NextIndex = index;
        }
    }
}
