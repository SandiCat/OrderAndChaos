using System;

namespace OrderAndChaos {

	public static class AlphaBeta {
		private static readonly double inf = double.PositiveInfinity;

		private static double recursion(IExplorable node, int depth, double alpha, double beta, bool maximizing, int playCount) {
			if (depth == 0 || node.IsTerminal()) {
				return node.HeuresticValue(playCount);
			}
			if (maximizing) {
				double v = -inf;
				foreach (var child in node.Explore()) {
					v = Math.Max(v, AlphaBeta.recursion(child, depth - 1, alpha, beta, false, playCount + 1));
					alpha = Math.Max(alpha, v);
					if (beta <= alpha)
						break;
				}
				return v;
			} else {
				double v = inf;
				foreach (var child in node.Explore()) {
					v = Math.Min(v, AlphaBeta.recursion(child, depth - 1, alpha, beta, true, playCount + 1));
					beta = Math.Min(beta, v);
					if (beta <= alpha)
						break;
				}
				return v;
			}
		}

		public static IExplorable BestMove(IExplorable node, int depth, bool maximizing) {
			IExplorable best_child = null;
			double alpha = -inf, beta = inf;

			if (maximizing) {
				double v = -inf;
				foreach (var child in node.Explore()) {
					var v2 = AlphaBeta.recursion(child, depth - 1, alpha, beta, false, 1);
					if (v2 > v) {
						v = v2;
						best_child = child;
					}
					alpha = Math.Max(alpha, v);
					if (beta <= alpha)
						break;
				}
				return best_child;
			} else {
				double v = inf;
				foreach (var child in node.Explore()) {
					var v2 = AlphaBeta.recursion(child, depth - 1, alpha, beta, true, 1);
					if (v2 < v) {
						v = v2;
						best_child = child;
					}
					beta = Math.Min(beta, v);
					if (beta <= alpha)
						break;
				}
				return best_child;
			}
		}
	}
}