using System;
using System.Collections.Generic;
using System.Collections;

namespace OrderAndChaos {
	public abstract class Indexable<T> : IEnumerable<T> {
		public readonly int Length;

		public Indexable(int length) {
			this.Length = length;
		}

		public T this[int i] {
			get { return Get(i); }
			set { Set(i, value); }
		}

		public IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < Length; i++) {
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		protected abstract T Get(int i);
		protected abstract void Set(int i, T t);
	}
}

