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

		private SquareT[,] array = new SquareT[SideLen, SideLen];

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
			return array[x, y];
		}

		public void Set(int x, int y, SquareT move) {
			array[x, y] = move;
		}

		public Board Clone() {
			var b = new Board();
			b.array = array.Clone() as SquareT[,];
			return b;
		}

		public override string ToString() {
			string ret = "";
			for (int y = 0; y < Board.SideLen; y++) {
				ret += (y + 1).ToString();
				for (int x = 0; x < Board.SideLen; x++) {
					ret += ' ';
					ret += Get(x, y) == SquareT.Empty ? "·" : Get(x, y).ToString();
				}
				ret += '\n';
			}
			ret += ' ';
			for (int x = 0; x < Board.SideLen; x++) {
				ret += ' ';
				ret += (char)('a' + x);
			}
			ret += "\n\n";
			return ret;
		}

		public override int GetHashCode() {
			int hash = 0;
			for (int x = 0; x < SideLen; x++) {
				for (int y = 0; y < SideLen; y++) {
					hash += (int)Get(x, y) * quickPow(3, x * SideLen + y);
				}
			}
			return hash;
		}

		private int quickPow(int b, int exp) {
			if (exp == 1) {
				return b;
			} else if (exp == 0) {
				return 1;
			} else if (exp % 2 == 0) {
				int v = quickPow(b, exp / 2);
				return v * v;
			} else {
				return quickPow(b, exp - 1);
			}
		}
	}
}

