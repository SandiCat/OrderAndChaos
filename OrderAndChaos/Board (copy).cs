using System;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrderAndChaos {
	public enum SquareT {
		Empty, X, O
	}

	public class Board {
		public static readonly int SideLen = 6;

		private BitVector32[] bitArray = Enumerable.Repeat(new BitVector32(), SideLen).ToArray();

		public Board() {}

		public Board(string[] template) : this() {
			for (int x = 0; x < SideLen; x++) {
				for (int y = 0; y < SideLen; y++) {
					var symb = template[x][y];
					if (symb == 'O')
						Set(x, y, SquareT.O);
					else if (symb == 'X')
						Set(x, y, SquareT.X);
				}
			}
		}

		public SquareT Get(int x, int y) {
			if (x >= SideLen || y >= SideLen || x < 0 || y < 0)
				throw new IndexOutOfRangeException();
			var a = getBit(x, 2 * y);
			var b = getBit(x, 2 * y + 1);
			if (a && b) {
				return SquareT.O;
			} else if (a || b) {
				return SquareT.X;
			} else {
				return SquareT.Empty;
			}
		}

		public void Set(int x, int y, SquareT move) {
			if (x >= SideLen || y >= SideLen || x < 0 || y < 0)
				throw new IndexOutOfRangeException();
			setBit(x, 2*y, move != SquareT.Empty);
			setBit(x, 2 * y + 1, move == SquareT.O);
		}

		public Board Clone() {
			var b = new Board();
			b.bitArray = bitArray.Clone() as BitVector32[];
			return b;
		}

		public override string ToString() {
			var ret = "";
			for (int x = 0; x < Board.SideLen; x++) {
				for (int y = 0; y < Board.SideLen; y++) {
					ret += Get(x, y) == SquareT.Empty ? "." : Get(x, y).ToString();
				}
				ret += '\n';
			}
			return ret;
		}

		private bool getBit(int x, int y) {
			return bitArray[x][1 << y];
		}
		private void setBit(int x, int y, bool bit) {
			bitArray[x][1 << y] = bit;
		}
	}
}

