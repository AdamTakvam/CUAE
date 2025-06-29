using System;
using System.Net;
using System.Net.Sockets;

namespace Metreos.SccpStack
{
	public delegate void SelectActionDelegate(Socket socket);

	/// <summary>
	/// Summary description for SelectAction.
	/// </summary>
	public abstract class SelectAction
	{
		public SelectAction(SelectActionDelegate action)
		{
			this.action = action;
		}

		protected SelectActionDelegate action;

		public abstract void Perform(Socket socket);
	}

	public class ReceiveSelectAction : SelectAction
	{
		public ReceiveSelectAction(SelectActionDelegate action) : base(action)
		{
		}

		public override void Perform(Socket socket)
		{
			// May perform action-specific logic in future
			action(socket);
		}
	}

	public class AcceptSelectAction : SelectAction
	{
		public AcceptSelectAction(SelectActionDelegate action) : base(action)
		{
		}

		public override void Perform(Socket socket)
		{
			// May perform action-specific logic in future
			action(socket);
		}
	}

	public class SendSelectAction : SelectAction
	{
		public SendSelectAction(SelectActionDelegate action) : base(action)
		{
		}

		public override void Perform(Socket socket)
		{
			// May perform action-specific logic in future
			action(socket);
		}
	}

	public class ConnectSelectAction : SelectAction
	{
		public ConnectSelectAction(SelectActionDelegate action) : base(action)
		{
		}

		public override void Perform(Socket socket)
		{
			// May perform action-specific logic in future
			action(socket);
		}
	}
}
