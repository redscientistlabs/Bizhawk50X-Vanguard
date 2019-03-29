using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTCV.NetCore
{
	public static class SyncObjectSingleton
	{
		public static Form SyncObject;

		public static void FormExecute(Action<object, EventArgs> a, object[] args = null)
		{
			if (a == null)
			{
				Console.WriteLine("Syncobject was null!");
					return;
			}

			if (SyncObject.InvokeRequired)
				SyncObject.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
			else
				a.Invoke(null, null);
		}

		public static void SyncObjectExecute(Form sync, Action<object, EventArgs> a, object[] args = null)
		{
			if (sync == null)
			{
				Console.WriteLine("Syncobject was null!");
				return;
			}

			if (sync.InvokeRequired)
				sync.Invoke(new MethodInvoker(() => { a.Invoke(null, null); }));
			else
				a.Invoke(null, null);
		}
	}
}
