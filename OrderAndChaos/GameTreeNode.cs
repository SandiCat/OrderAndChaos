using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderAndChaos {
	public class GameTreeNode : IExplorable {
		private static readonly List<SquareT> moves = new List<SquareT>{SquareT.X, SquareT.O};
		private static readonly double inf = double.PositiveInfinity;
		private static readonly BoardEnumeration enumerate = new BoardEnumeration(null);

		private Board board = new Board();

		public GameTreeNode() {}
		public GameTreeNode(Board board) {
			this.board = board;
			enumerate.ChangeBoard(board);
		}

		public override string ToString() {
			return board.ToString();
		}

		public override int GetHashCode() {
			return board.GetHashCode();
		}

		public void PlayMove(int x, int y, SquareT move) {
			board.Set(x, y, move);
		}
			
		public IEnumerable<IExplorable> Explore() {
			for (int x = 0; x < Board.SideLen; x++) {
				for (int y = 0; y < Board.SideLen; y++) {
					if (board.Get(x, y) == SquareT.Empty) {
						foreach (var move in moves) {
							var newBoard = board.Clone();
							newBoard.Set(x, y, move);
							yield return (new GameTreeNode(newBoard)) as IExplorable;
						}
					}
				}
			}
		}

		public bool IsTerminal() {
			foreach (var line in enumerate.AllDirections) {
				var last = SquareT.Empty;
				int run = 1;
				foreach (var item in line) {
					if (item == last) {
						run++;
						if (run >= 5 && last != SquareT.Empty) {
							return true;
						}
					} else {
						last = item;
						run = 1;
					}
				}
			}
			return false;
		}

		public double HeuresticValue(int playCount) {
			/* + otvoreni kvadrat koji je sjeciste dvaju nezatvorenih linija
			 *   u kojima se grade suprotni runovi, puta duljina obaju runova, puta
			 *   vrsta - jesu li na rubovima ili dalje?
			 * + run dulji od 2 u nezatvorenoj liniji, puta duljina
			 * + otvorena linija
			 * - "mrtvi" kvadrat, gdje se moze igrati tako da ne nadopunjava nista
			 * - zatvorena linija, puta koliko ima zarobljenih simbola
			 */

			if (playCount > 10 && IsTerminal())
				return inf;
			
			int _ret = 0;
			foreach (var line in enumerate.AllDirections) {
				if (IsOpen(line))
					_ret += RunLen(line);
			}

			return _ret;

			/*
			double ret = 0;

			for (int x = 0; x < Board.SideLen; x++) {
				for (int y = 0; y < Board.SideLen; y++) {
					var lines = new List<LineEnumerable> {
						enumerate.Horizontal[y],
						enumerate.Vertical[x],
						enumerate.MainDiagonalFromPoint(x, y),
						enumerate.AntiDiagonalFromPoint(x, y)};
					lines = lines.Where(ln => ln != null &&  IsOpen(ln)).ToList();

					for (int i = 0; i < lines.Count; i++) {
						for (int j = i + 1; j < lines.Count; j++) {
							if (MainRun(lines[i]) != MainRun(lines[i])) {
								ret += 100 * (RunLen(lines[i]) + RunLen(lines[j]));
							}
						}
					}
				}
			}

			foreach (var line in enumerate.AllDirections) {
				var runLen = RunLen(line);
				if (IsOpen(line)) {
					ret += 50 * runLen + 30;
				} else {
					ret -= 50 * runLen;
				}
			}
			
			return ret;
			*/
		}

		private static int RunLen(LineEnumerable line) {
			var t = MainRun(line);
			int ret = 0;
			foreach (var item in line) {
				if (item == t)
					ret++;
			}
			return ret;
		}

		private static SquareT MainRun(LineEnumerable line) {
			int x = 0, o = 0;
			foreach (var item in line) {
				if (item == SquareT.X)
					x++;
				else if (item == SquareT.O)
					o++;
			}
			return x > o ? SquareT.X : SquareT.O;
		}

		private static bool IsOpen(LineEnumerable line) {
			bool[] runs = new bool[]{true, true};
			for (int run = 0; run < 2; run++) {
				int i = 0;
				SquareT match = SquareT.Empty;
				foreach (var item in line) {
					if (run == 0 ? i == 0 : i == Board.SideLen - 1) {
						i++;
						continue;
					}

					if (item != SquareT.Empty && match == SquareT.Empty)
						match = item;

					if (item != SquareT.Empty && item != match) {
						runs[run] = false;
						break;
					}

					i++;
				}
			}
			return runs[0] || runs[1];
		}
	}
}
