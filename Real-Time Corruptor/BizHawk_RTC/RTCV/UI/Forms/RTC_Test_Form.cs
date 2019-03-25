using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ceras;
using RTCV.CorruptCore;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_Test_Form : Form, IAutoColorize
	{
		public RTC_Test_Form()
		{
			InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			var serializer = new CerasSerializer();
			var proto = new TestClass();
			proto.ListLongArr.Add(new long[] { 5, 10 });
			proto.ListLongArr.Add(new long[] { 2, 20 });

			var serialized = serializer.Serialize(proto);
			var deserialized = serializer.Deserialize<VmdPrototype>(serialized);

			foreach (var x in deserialized.AddRanges)
			{
				Console.WriteLine($"{x[0]} {x[1]}");
			}
		}
	}

	[Ceras.MemberConfig(TargetMember.All)]
	public class TestClass
	{
		public List<long[]> ListLongArr { get; set; } = new List<long[]>();
		public TestClass()
		{

		}
	}
}
