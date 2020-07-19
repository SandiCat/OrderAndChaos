using System;
using System.Collections.Generic;

namespace OrderAndChaos {
	public interface IExplorable {
		IEnumerable<IExplorable> Explore();
		bool IsTerminal();
		double HeuresticValue(int playCount);
	}
}

