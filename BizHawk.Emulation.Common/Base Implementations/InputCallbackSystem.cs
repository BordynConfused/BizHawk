﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BizHawk.Emulation.Common
{
	public class InputCallbackSystem : List<Action>, IInputCallbackSystem
	{
		public void Call()
		{
			foreach (var action in this)
			{
				action();
			}
		}

		public void RemoveAll(IEnumerable<Action> actions)
		{
			var hadAny = this.Any();

			foreach (var action in actions)
			{
				this.Remove(action);
			}

			var hasAny = this.Any();

			Changes(hadAny, hasAny);
		}

		public new void Add(Action item)
		{
			var hadAny = this.Any();
			base.Add(item);
			var hasAny = this.Any();

			Changes(hadAny, hasAny);
		}

		public new bool Remove(Action item)
		{
			var hadAny = this.Any();
			var result = base.Remove(item);
			var hasAny = this.Any();

			Changes(hadAny, hasAny);

			return result;
		}

		// TODO: these just happen to be all the add/remove methods the client uses, to be thorough the others should be overriden as well

		public delegate void ActiveChangedEventHandler();
		public event ActiveChangedEventHandler ActiveChanged;

		private void Changes(bool hadAny, bool hasAny)
		{
			if ((hadAny && !hasAny) || (!hadAny && hasAny))
			{
				if (ActiveChanged != null)
				{
					ActiveChanged();
				}
			}
		}
	}
}
