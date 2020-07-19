using System;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrderAndChaos {
	public class LineEnumerable : Indexable<SquareT> {
		private Board board;
		private readonly int x0, y0, dx, dy;

		public LineEnumerable(int x0, int y0, int dx, int dy, int len, Board board) : base(len) {
			this.x0 = x0;
			this.y0 = y0;
			this.dx = dx;
			this.dy = dy;
			this.board = board;
		}

		public void ChangeBoard(Board board) {
			this.board = board;
		}

		protected override SquareT Get(int i) {
			if (i < 0 || i >= Length)
				throw new IndexOutOfRangeException();
			return board.Get(x0 + i * dx, y0 + i * dy);
		}
		protected override void Set(int i, SquareT t) {
			if (i < 0 || i >= Length)
				throw new IndexOutOfRangeException();
			board.Set(x0 + i * dx, y0 + i * dy, t);
		}
	}

	public class BoardEnumeration {
		public readonly LineEnumerable[] Horizontal;
		public readonly LineEnumerable[] Vertical;
		public readonly LineEnumerable[] MainDiagonal;
		public readonly LineEnumerable[] AntiDiagonal;
		public readonly IEnumerable<LineEnumerable> AllDirections;

		private readonly int[] zeros = Enumerable.Repeat(0, Board.SideLen).ToArray();
		private readonly int[] ascend = Enumerable.Range(0, Board.SideLen).ToArray();
		private readonly int[] mainDiagonal_x0 = new int[]{0, 0, 1};
		private readonly int[] mainDiagonal_y0 = new int[]{0, 1, 0};
		private readonly int[] antiDiagonal_x0 = new int[]{5, 4, 5};
		private readonly int[] antiDiagonal_y0 = new int[]{0, 0, 1};
		private readonly int[] uniformLen = Enumerable.Repeat(Board.SideLen, Board.SideLen).ToArray();
		private readonly int[] diagonalLen = new int[]{6, 5, 5};

		public BoardEnumeration(Board board) {
			Horizontal = CreateLines(board, zeros, ascend, 1, 0, uniformLen);
			Vertical = CreateLines(board, ascend, zeros, 0, 1, uniformLen);
			MainDiagonal = CreateLines(board, mainDiagonal_x0, mainDiagonal_y0, 1, 1, diagonalLen);
			AntiDiagonal = CreateLines(board, antiDiagonal_x0, antiDiagonal_y0, -1, 1, diagonalLen);
			AllDirections = new ChainedEnumerable<LineEnumerable>
				(Horizontal, Vertical, MainDiagonal, AntiDiagonal);
		}

		public void ChangeBoard(Board board) {
			foreach (var line in AllDirections) {
				line.ChangeBoard(board);
			}
		}

		public LineEnumerable MainDiagonalFromPoint(int x, int y) {
			for (int i = 0; i < mainDiagonal_x0.Length; i++) {
				if (- x + mainDiagonal_x0[i] + mainDiagonal_y0[i] == y) {
					return MainDiagonal[i];
				}
			}
			return null;
		}

		public LineEnumerable AntiDiagonalFromPoint(int x, int y) {
			for (int i = 0; i < antiDiagonal_x0.Length; i++) {
				if (x - antiDiagonal_x0[i] + antiDiagonal_y0[i] == y) {
					return AntiDiagonal[i];
				}
			}
			return null;
		}

		private LineEnumerable[] CreateLines(Board board, int[] x0, int[] y0, int dx, int dy, int[] len) {
			return (from i in Enumerable.Range(0, x0.Length)
			        select new LineEnumerable(x0[i], y0[i], dx, dy, len[i], board)).ToArray();
		}
	}
}


