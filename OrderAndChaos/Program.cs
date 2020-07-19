using System;
using System.Collections.Generic;

namespace OrderAndChaos {
	class MainClass {
		public static void Main(string[] args) {
			Console.WriteLine("Potez je oblika [x/o][a-f][1-6]");
			Console.WriteLine("Npr. xd4\n");

			var n = new GameTreeNode(new Board());

			for (int i = 0; !n.IsTerminal(); i++) {
				int depth;
				if (i == 0)
					depth = 3;
				else if (i < 4)
					depth = 4;
				else if (i < 15)
					depth = 5;
				else
					depth = 6;
				n = AlphaBeta.BestMove(n, depth, true) as GameTreeNode;
				Console.Write(n);

				bool success = true;
				do {
					try {
						var move = Console.ReadLine();
						var t = move[0] == 'x' ? SquareT.X : SquareT.O;
						int x = move[1] - 'a';
						int y = move[2] - '1';
						Console.WriteLine();
						n.PlayMove(x, y, t);
						success = true;
					} catch {
						success = false;
						Console.WriteLine("Error. Try again.");
					}
				} while (!success);

				Console.Write(n);
			}
		}
	}
}
