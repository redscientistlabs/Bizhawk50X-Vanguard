using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;

namespace RTC
{
	public static class RTC_Extensions
	{
		public static DialogResult getInputBox(string title, string promptText, ref string value)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}


		#region ARRAY EXTENSIONS
		public static T[] SubArray<T>(this T[] data, long index, long length)
		{
			T[] result = new T[length];

			if (data == null)
				return null;

			Array.Copy(data, index, result, 0, length);
			return result;
		}

		public static T[] FlipWords<T>(this T[] data, int wordSize)
		{
			//2 : 16-bit
			//4 : 32-bit
			//8 : 64-bit

			T[] result = new T[data.Length];

			for (int i = 0; i < data.Length; i++)
			{
				int wordPos = i % wordSize;
				int wordAddress = i - wordPos;
				int newPos = wordAddress + (wordSize - (wordPos + 1));

				result[newPos] = data[i];
			}

			return result;
		}

		#endregion

		#region STRING EXTENSIONS
		public static string ToBase64(this string str)
		{
			var bytes = Encoding.UTF8.GetBytes(str);
			return Convert.ToBase64String(bytes);
		}

		public static string FromBase64(this string base64)
		{
			var data = Convert.FromBase64String(base64);
			return Encoding.UTF8.GetString(data);
		}

		public static byte[] GetBytes(this string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		#endregion

		#region BYTE ARRAY EXTENSIONS

		public static long getNumericMaxValue(byte[] Value)
		{
			switch (Value.Length)
			{
				case 1:
					return byte.MaxValue;
				case 2:
					return UInt16.MaxValue;
				case 4:
					return UInt32.MaxValue;
			}

			return 0;
		}

		public static decimal getDecimalValue(byte[] Value)
		{
			switch (Value.Length)
			{
				case 1:
					return (int)Value[0];
				case 2:
					return BitConverter.ToUInt16(Value, 0);
				case 4:
					return BitConverter.ToUInt32(Value, 0);
			}

			return 0;
		}

		public static byte[] getByteArrayValue(byte[] originalValue, decimal newValue)
		{
			switch (originalValue.Length)
			{
				case 1:
					return new byte[] { (byte)newValue };
				case 2:
					return BitConverter.GetBytes(Convert.ToUInt16(newValue));
				case 4:
					return BitConverter.GetBytes(Convert.ToUInt32(newValue));
			}

			return null;
		}

		public static byte[] getByteArrayValue(int precision, decimal newValue)
		{
			switch (precision)
			{
				case 1:
					return new byte[] { (byte)newValue };
				case 2:
					return BitConverter.GetBytes(Convert.ToUInt16(newValue));
				case 4:
					return BitConverter.GetBytes(Convert.ToUInt32(newValue));
			}

			return null;
		}

		#endregion


		#region COLOR EXTENSIONS

		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(this Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}

		#endregion

		#region CONTROL EXTENSIONS

		public static List<Control> getControlsWithTag(this Control.ControlCollection controls)
		{
			List<Control> allControls = new List<Control>();

			foreach (Control c in controls)
			{
				if (c.Tag != null)
					allControls.Add(c);

				if (c.HasChildren)
					allControls.AddRange(c.Controls.getControlsWithTag()); //Recursively check all children controls as well; ie groupboxes or tabpages
			}

			return allControls;
		}

		#endregion

		#region PATH EXTENSIONS

		public static string getShortFilenameFromPath(string longFilenamePath)
		{
			// >>> Will contain the character \ at the end

			//returns the filename from the full path
			if (longFilenamePath.Contains("\\"))
				return longFilenamePath.Substring(longFilenamePath.LastIndexOf("\\") + 1);
			else
				return longFilenamePath;
		}

		public static string removeFileExtension(string filename)
		{
			// filename.wav -> filename

			if (filename.Contains("."))
				return filename.Substring(0, filename.LastIndexOf("."));
			else
				return filename;
		}

		public static string getLongDirectoryFromPath(string longFilenamePath)
		{
			// >>> Will contain the character \ at the end

			//returns the filename from the full path
			if (longFilenamePath.Contains("\\"))
				return longFilenamePath.Substring(0, longFilenamePath.LastIndexOf("\\") + 1);
			else
				return longFilenamePath;
		}

		#endregion
	}

	// Used code from this https://github.com/wasabii/Cogito/blob/master/Cogito.Core/RandomExtensions.cs
	// MIT Licensed. thank you very much.
	static class RandomExtensions
	{
		public static long RandomLong(this Random rnd)
		{
			byte[] buffer = new byte[8];
			rnd.NextBytes(buffer);
			return BitConverter.ToInt64(buffer, 0);
		}

		public static long RandomLong(this Random rnd, long min, long max)
		{
			EnsureMinLEQMax(ref min, ref max);
			long numbersInRange = unchecked(max - min + 1);
			if (numbersInRange < 0)
				throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

			long randomOffset = RandomLong(rnd);
			if (IsModuloBiased(randomOffset, numbersInRange))
				return RandomLong(rnd, min, max); // Try again
			else
				return min + PositiveModuloOrZero(randomOffset, numbersInRange);
		}

		public static long RandomLong(this Random rnd, long max)
		{
			return rnd.RandomLong(0, max);
		}

		static bool IsModuloBiased(long randomOffset, long numbersInRange)
		{
			long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
			return randomOffset > greatestCompleteRange;
		}

		static long PositiveModuloOrZero(long dividend, long divisor)
		{
			long mod;
			Math.DivRem(dividend, divisor, out mod);
			if (mod < 0)
				mod += divisor;
			return mod;
		}

		static void EnsureMinLEQMax(ref long min, ref long max)
		{
			if (min <= max)
				return;
			long temp = min;
			min = max;
			max = temp;
		}
	}

	/// <summary>
	/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
	/// Provides a method for performing a deep copy of an object.
	/// Binary Serialization is used to perform the copy.
	/// </summary>
	public static class ObjectCopier
	{
		/// <summary>
		/// Perform a deep Copy of the object.
		/// </summary>
		/// <typeparam name="T">The type of object being copied.</typeparam>
		/// <param name="source">The object instance to copy.</param>
		/// <returns>The copied object.</returns>
		public static T Clone<T>(T source)
		{
			if (!typeof(T).IsSerializable)
			{
				throw new ArgumentException("The type must be serializable.", "source");
			}

			// Don't serialize a null object, simply return the default for that object
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
	}


	/// <summary>
	/// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
	/// Custom column type dedicated to the DataGridViewNumericUpDownCell cell type.
	/// </summary>
	public class DataGridViewNumericUpDownColumn : DataGridViewColumn
	{
		/// <summary>
		/// Constructor for the DataGridViewNumericUpDownColumn class.
		/// </summary>
		public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell())
		{
		}

		/// <summary>
		/// Represents the implicit cell that gets cloned when adding rows to the grid.
		/// </summary>
		[
			Browsable(false),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				DataGridViewNumericUpDownCell dataGridViewNumericUpDownCell = value as DataGridViewNumericUpDownCell;
				if (value != null && dataGridViewNumericUpDownCell == null)
				{
					throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewNumericUpDownElements.DataGridViewNumericUpDownCell or derive from it.");
				}
				base.CellTemplate = value;
			}
		}

		/// <summary>
		/// Replicates the DecimalPlaces property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Appearance"),
			DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces),
			Description("Indicates the number of decimal places to display.")
		]
		public int DecimalPlaces
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.DecimalPlaces;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				// Update the template cell so that subsequent cloned cells use the new value.
				this.NumericUpDownCellTemplate.DecimalPlaces = value;
				if (this.DataGridView != null)
				{
					// Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						// Be careful not to unshare rows unnecessarily. 
						// This could have severe performance repercussions.
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							// Call the internal SetDecimalPlaces method instead of the property to avoid invalidation 
							// of each cell. The whole column is invalidated later in a single operation for better performance.
							dataGridViewCell.SetDecimalPlaces(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
				}
			}
		}

		/// <summary>
		/// Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the amount to increment or decrement on each button click.")
		]
		public Decimal Increment
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.Increment;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				this.NumericUpDownCellTemplate.Increment = value;
				if (this.DataGridView != null)
				{
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							dataGridViewCell.SetIncrement(rowIndex, value);
						}
					}
				}
			}
		}

		/// Indicates whether the Increment property should be persisted.
		private bool ShouldSerializeIncrement()
		{
			return !this.Increment.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement);
		}

		/// <summary>
		/// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the maximum value for the numeric up-down cells."),
			RefreshProperties(RefreshProperties.All)
		]
		public Decimal Maximum
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.Maximum;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				this.NumericUpDownCellTemplate.Maximum = value;
				if (this.DataGridView != null)
				{
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							dataGridViewCell.SetMaximum(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: This column and/or grid rows may need to be autosized depending on their
					//       autosize settings. Call the autosizing methods to autosize the column, rows, 
					//       column headers / row headers as needed.
				}
			}
		}

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeMaximum()
		{
			return !this.Maximum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum);
		}

		/// <summary>
		/// Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates the minimum value for the numeric up-down cells."),
			RefreshProperties(RefreshProperties.All)
		]
		public Decimal Minimum
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.Minimum;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				this.NumericUpDownCellTemplate.Minimum = value;
				if (this.DataGridView != null)
				{
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							dataGridViewCell.SetMinimum(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: This column and/or grid rows may need to be autosized depending on their
					//       autosize settings. Call the autosizing methods to autosize the column, rows, 
					//       column headers / row headers as needed.
				}
			}
		}

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeMinimum()
		{
			return !this.Minimum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum);
		}

		/// <summary>
		/// Replicates the ThousandsSeparator property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator),
			Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")
		]
		public bool ThousandsSeparator
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.ThousandsSeparator;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				this.NumericUpDownCellTemplate.ThousandsSeparator = value;
				if (this.DataGridView != null)
				{
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							dataGridViewCell.SetThousandsSeparator(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: This column and/or grid rows may need to be autosized depending on their
					//       autosize settings. Call the autosizing methods to autosize the column, rows, 
					//       column headers / row headers as needed.
				}
			}
		}
		/// <summary>
		/// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
		/// </summary>
		[
			Category("Data"),
			Description("Indicates if it should display as hexadecimal"),
			DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal),
			RefreshProperties(RefreshProperties.All)
		]
		public bool Hexadecimal
		{
			get
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				return this.NumericUpDownCellTemplate.Hexadecimal;
			}
			set
			{
				if (this.NumericUpDownCellTemplate == null)
				{
					throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
				}
				this.NumericUpDownCellTemplate.Hexadecimal = value;
				if (this.DataGridView != null)
				{
					DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
					int rowCount = dataGridViewRows.Count;
					for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
					{
						DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
						DataGridViewNumericUpDownCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
						if (dataGridViewCell != null)
						{
							dataGridViewCell.SetHexadecimal(rowIndex, value);
						}
					}
					this.DataGridView.InvalidateColumn(this.Index);
					// TODO: This column and/or grid rows may need to be autosized depending on their
					//       autosize settings. Call the autosizing methods to autosize the column, rows, 
					//       column headers / row headers as needed.
				}
			}
		}

		/// Indicates whether the Maximum property should be persisted.
		private bool ShouldSerializeHexadecimal()
		{
			return !this.Hexadecimal.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal);
		}

		/// <summary>
		/// Small utility function that returns the template cell as a DataGridViewNumericUpDownCell
		/// </summary>
		private DataGridViewNumericUpDownCell NumericUpDownCellTemplate
		{
			get
			{
				return (DataGridViewNumericUpDownCell)this.CellTemplate;
			}
		}

		/// <summary>
		/// Returns a standard compact string representation of the column.
		/// </summary>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(100);
			sb.Append("DataGridViewNumericUpDownColumn { Name=");
			sb.Append(this.Name);
			sb.Append(", Index=");
			sb.Append(this.Index.ToString(CultureInfo.CurrentCulture));
			sb.Append(" }");
			return sb.ToString();
		}
	}
	/// <summary>
	/// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
	/// Defines a NumericUpDown cell type for the System.Windows.Forms.DataGridView control
	/// </summary>
	public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
	{
		// Used in KeyEntersEditMode function
		[System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern short VkKeyScan(char key);

		// Used in TranslateAlignment function
		private static readonly DataGridViewContentAlignment anyRight = DataGridViewContentAlignment.TopRight |
																		DataGridViewContentAlignment.MiddleRight |
																		DataGridViewContentAlignment.BottomRight;
		private static readonly DataGridViewContentAlignment anyCenter = DataGridViewContentAlignment.TopCenter |
																		 DataGridViewContentAlignment.MiddleCenter |
																		 DataGridViewContentAlignment.BottomCenter;

		// Default dimensions of the static rendering bitmap used for the painting of the non-edited cells
		private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth = 100;
		private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight = 22;

		// Default value of the DecimalPlaces property
		internal const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces = 0;
		// Default value of the Increment property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement = Decimal.One;
		// Default value of the Maximum property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum = (Decimal)100.0;
		// Default value of the Minimum property
		internal const Decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum = Decimal.Zero;
		// Default value of the ThousandsSeparator property
		internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator = false;

		internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal = false;

		// Type of this cell's editing control
		private static Type defaultEditType = typeof(DataGridViewNumericUpDownEditingControl);
		// Type of this cell's value. The formatted value type is string, the same as the base class DataGridViewTextBoxCell
		private static Type defaultValueType = typeof(System.String);

		// The bitmap used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
		[ThreadStatic]
		private static Bitmap renderingBitmap;

		// The NumericUpDown control used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
		[ThreadStatic]
		private NumericUpDown paintingNumericUpDown;

		

		private int decimalPlaces;       // Caches the value of the DecimalPlaces property
		private Decimal increment;       // Caches the value of the Increment property
		private Decimal minimum;         // Caches the value of the Minimum property
		private Decimal maximum;         // Caches the value of the Maximum property
		private bool thousandsSeparator; // Caches the value of the ThousandsSeparator property
		private bool hexadecimal;

		/// <summary>
		/// Constructor for the DataGridViewNumericUpDownCell cell type
		/// </summary>
		public DataGridViewNumericUpDownCell()
		{
			// Create a thread specific bitmap used for the painting of the non-edited cells
			if (renderingBitmap == null)
			{
				renderingBitmap = new Bitmap(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth, DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight);
			}

			// Create a thread specific NumericUpDown control used for the painting of the non-edited cells
			if (paintingNumericUpDown == null)
			{
				paintingNumericUpDown = new NumericUpDown();
				// Some properties only need to be set once for the lifetime of the control:
				paintingNumericUpDown.BorderStyle = BorderStyle.None;
				paintingNumericUpDown.Maximum = Decimal.MaxValue / 10;
				paintingNumericUpDown.Minimum = Decimal.MinValue / 10;
				//paintingNumericUpDown.DoubleBuffered(true);

			}

			// Set the default values of the properties:
			this.decimalPlaces = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces;
			this.increment = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement;
			this.minimum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum;
			this.maximum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum;
			this.thousandsSeparator = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator;
			this.hexadecimal = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal;
		}

		/// <summary>
		/// The DecimalPlaces property replicates the one from the NumericUpDown control
		/// </summary>
		[
			DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces)
		]
		public int DecimalPlaces
		{

			get
			{
				return this.decimalPlaces;
			}

			set
			{
				if (value < 0 || value > 99)
				{
					throw new ArgumentOutOfRangeException("The DecimalPlaces property cannot be smaller than 0 or larger than 99.");
				}
				if (this.decimalPlaces != value)
				{
					SetDecimalPlaces(this.RowIndex, value);
					OnCommonChange();  // Assure that the cell or column gets repainted and autosized if needed
				}
			}
		}
		/// <summary>
		/// Returns the current DataGridView EditingControl as a DataGridViewNumericUpDownEditingControl control
		/// </summary>
		private DataGridViewNumericUpDownEditingControl EditingNumericUpDown
		{
			get
			{
				return this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;
			}
		}

		/// <summary>
		/// Define the type of the cell's editing control
		/// </summary>
		public override Type EditType
		{
			get
			{
				return defaultEditType; // the type is DataGridViewNumericUpDownEditingControl
			}
		}

		/// <summary>
		/// The Increment property replicates the one from the NumericUpDown control
		/// </summary>
		public Decimal Increment
		{

			get
			{
				return this.increment;
			}

			set
			{
				if (value < (Decimal)0.0)
				{
					throw new ArgumentOutOfRangeException("The Increment property cannot be smaller than 0.");
				}
				SetIncrement(this.RowIndex, value);
				// No call to OnCommonChange is needed since the increment value does not affect the rendering of the cell.
			}
		}

		/// <summary>
		/// The Maximum property replicates the one from the NumericUpDown control
		/// </summary>
		public Decimal Maximum
		{

			get
			{
				return this.maximum;
			}

			set
			{
				if (this.maximum != value)
				{
					SetMaximum(this.RowIndex, value);
					OnCommonChange();
				}
			}
		}

		/// <summary>
		/// The Minimum property replicates the one from the NumericUpDown control
		/// </summary>
		public Decimal Minimum
		{

			get
			{
				return this.minimum;
			}

			set
			{
				if (this.minimum != value)
				{
					SetMinimum(this.RowIndex, value);
					OnCommonChange();
				}
			}
		}

		/// <summary>
		/// The ThousandsSeparator property replicates the one from the NumericUpDown control
		/// </summary>
		[
			DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator)
		]
		public bool ThousandsSeparator
		{

			get
			{
				return this.thousandsSeparator;
			}

			set
			{
				if (this.thousandsSeparator != value)
				{
					SetThousandsSeparator(this.RowIndex, value);
					OnCommonChange();
				}
			}
		}
		
		[
		  DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultHexadecimal),
		]
		public bool Hexadecimal
		{

			get
			{
				return this.hexadecimal;
			}

			set
			{
				if (this.hexadecimal != value)
				{ 
					SetHexadecimal(this.RowIndex, value);
					OnCommonChange();
				}
			}
		}
		/// <summary>
		/// Returns the type of the cell's Value property
		/// </summary>
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return defaultValueType;
			}
		}

		/// <summary>
		/// Clones a DataGridViewNumericUpDownCell cell, copies all the custom properties.
		/// </summary>
		public override object Clone()
		{
			DataGridViewNumericUpDownCell dataGridViewCell = base.Clone() as DataGridViewNumericUpDownCell;
			if (dataGridViewCell != null)
			{
				dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
				dataGridViewCell.Increment = this.Increment;
				dataGridViewCell.Maximum = this.Maximum;
				dataGridViewCell.Minimum = this.Minimum;
				dataGridViewCell.ThousandsSeparator = this.ThousandsSeparator;
				dataGridViewCell.Hexadecimal = this.Hexadecimal;
			}
			return dataGridViewCell;
		}

		/// <summary>
		/// Returns the provided value constrained to be within the min and max. 
		/// </summary>
		private Decimal Constrain(Decimal value)
		{
			Debug.Assert(this.minimum <= this.maximum);
			if (value < this.minimum)
			{
				value = this.minimum;
			}
			if (value > this.maximum)
			{
				value = this.maximum;
			}
			return value;
		}

		/// <summary>
		/// DetachEditingControl gets called by the DataGridView control when the editing session is ending
		/// </summary>
		[
			EditorBrowsable(EditorBrowsableState.Advanced)
		]
		public override void DetachEditingControl()
		{
			DataGridView dataGridView = this.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
			}

			NumericUpDown numericUpDown = dataGridView.EditingControl as NumericUpDown;
			if (numericUpDown != null)
			{
				// Editing controls get recycled. Indeed, when a DataGridViewNumericUpDownCell cell gets edited
				// after another DataGridViewNumericUpDownCell cell, the same editing control gets reused for 
				// performance reasons (to avoid an unnecessary control destruction and creation). 
				// Here the undo buffer of the TextBox inside the NumericUpDown control gets cleared to avoid
				// interferences between the editing sessions.
				TextBox textBox = numericUpDown.Controls[1] as TextBox;
				if (textBox != null)
				{
					textBox.ClearUndo();
				}
			}

			base.DetachEditingControl();
		}

		/// <summary>
		/// Adjusts the location and size of the editing control given the alignment characteristics of the cell
		/// </summary>
		private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
		{
			// Add a 1 pixel padding on the left and right of the editing control
			editingControlBounds.X += 1;
			editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);

			// Adjust the vertical location of the editing control:
			int preferredHeight = cellStyle.Font.Height + 3;
			if (preferredHeight < editingControlBounds.Height)
			{
				switch (cellStyle.Alignment)
				{
					case DataGridViewContentAlignment.MiddleLeft:
					case DataGridViewContentAlignment.MiddleCenter:
					case DataGridViewContentAlignment.MiddleRight:
						editingControlBounds.Y += (editingControlBounds.Height - preferredHeight) / 2;
						break;
					case DataGridViewContentAlignment.BottomLeft:
					case DataGridViewContentAlignment.BottomCenter:
					case DataGridViewContentAlignment.BottomRight:
						editingControlBounds.Y += editingControlBounds.Height - preferredHeight;
						break;
				}
			}

			return editingControlBounds;
		}

		/// <summary>
		/// Customized implementation of the GetErrorIconBounds function in order to draw the potential 
		/// error icon next to the up/down buttons and not on top of them.
		/// </summary>
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			const int ButtonsWidth = 16;

			Rectangle errorIconBounds = base.GetErrorIconBounds(graphics, cellStyle, rowIndex);
			if (this.DataGridView.RightToLeft == RightToLeft.Yes)
			{
				errorIconBounds.X = errorIconBounds.Left + ButtonsWidth;
			}
			else
			{
				errorIconBounds.X = errorIconBounds.Left - ButtonsWidth;
			}
			return errorIconBounds;
		}

		/// <summary>
		/// Customized implementation of the GetFormattedValue function in order to include the decimal and thousand separator
		/// characters in the formatted representation of the cell value.
		/// </summary>
		protected override object GetFormattedValue(object value,
													int rowIndex,
													ref DataGridViewCellStyle cellStyle,
													TypeConverter valueTypeConverter,
													TypeConverter formattedValueTypeConverter,
													DataGridViewDataErrorContexts context)
		{
			if (this.Hexadecimal)
			{
				uint valueuint = System.Convert.ToUInt32(value);
				return valueuint.ToString("X");
			}
			else
			{
				// By default, the base implementation converts the Decimal 1234.5 into the string "1234.5"
				object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
				string formattedNumber = formattedValue as string;
				if (!string.IsNullOrEmpty(formattedNumber) && value != null)
				{
					Decimal unformattedDecimal = System.Convert.ToDecimal(value);
					Decimal formattedDecimal = System.Convert.ToDecimal(formattedNumber);
					if (unformattedDecimal == formattedDecimal)
					{
						// The base implementation of GetFormattedValue (which triggers the CellFormatting event) did nothing else than 
						// the typical 1234.5 to "1234.5" conversion. But depending on the values of ThousandsSeparator and DecimalPlaces,
						// this may not be the actual string displayed. The real formatted value may be "1,234.500"
						return formattedDecimal.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Custom implementation of the GetPreferredSize function. This implementation uses the preferred size of the base 
		/// DataGridViewTextBoxCell cell and adds room for the up/down buttons.
		/// </summary>
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (this.DataGridView == null)
			{
				return new Size(-1, -1);
			}

			Size preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
			if (constraintSize.Width == 0)
			{
				const int ButtonsWidth = 16; // Account for the width of the up/down buttons.
				const int ButtonMargin = 8;  // Account for some blank pixels between the text and buttons.
				preferredSize.Width += ButtonsWidth + ButtonMargin;
			}
			return preferredSize;
		}

		/// <summary>
		/// Custom implementation of the InitializeEditingControl function. This function is called by the DataGridView control 
		/// at the beginning of an editing session. It makes sure that the properties of the NumericUpDown editing control are 
		/// set according to the cell properties.
		/// </summary>
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			NumericUpDown numericUpDown = this.DataGridView.EditingControl as NumericUpDown;
			if (numericUpDown != null)
			{
				numericUpDown.BorderStyle = BorderStyle.None;
				numericUpDown.DecimalPlaces = this.DecimalPlaces;
				numericUpDown.Increment = this.Increment;
				numericUpDown.Maximum = this.Maximum;
				numericUpDown.Minimum = this.Minimum;
				numericUpDown.ThousandsSeparator = this.ThousandsSeparator;
				numericUpDown.Hexadecimal = this.Hexadecimal;
				string initialFormattedValueStr = initialFormattedValue as string;
				if (initialFormattedValueStr == null)
				{
					numericUpDown.Text = string.Empty;
				}
				else
				{
					numericUpDown.Text = initialFormattedValueStr;
				}
			}
		}

		/// <summary>
		/// Custom implementation of the KeyEntersEditMode function. This function is called by the DataGridView control
		/// to decide whether a keystroke must start an editing session or not. In this case, a new session is started when
		/// a digit or negative sign key is hit.
		/// </summary>
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
			Keys negativeSignKey = Keys.None;
			string negativeSignStr = numberFormatInfo.NegativeSign;
			if (!string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
			{
				negativeSignKey = (Keys)(VkKeyScan(negativeSignStr[0]));
			}
			if (Hexadecimal && ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)))
			{
				return true;
			}
			if ((char.IsDigit((char)e.KeyCode) ||
				 (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
				 negativeSignKey == e.KeyCode ||
				 Keys.Subtract == e.KeyCode) &&
				!e.Shift && !e.Alt && !e.Control)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Called when a cell characteristic that affects its rendering and/or preferred size has changed.
		/// This implementation only takes care of repainting the cells. The DataGridView's autosizing methods
		/// also need to be called in cases where some grid elements autosize.
		/// </summary>
		private void OnCommonChange()
		{
			if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
			{
				if (this.RowIndex == -1)
				{
					// Invalidate and autosize column
					this.DataGridView.InvalidateColumn(this.ColumnIndex);

					// TODO: Add code to autosize the cell's column, the rows, the column headers 
					// and the row headers depending on their autosize settings.
					// The DataGridView control does not expose a public method that takes care of this.
				}
				else
				{
					// The DataGridView control exposes a public method called UpdateCellValue
					// that invalidates the cell so that it gets repainted and also triggers all
					// the necessary autosizing: the cell's column and/or row, the column headers
					// and the row headers are autosized depending on their autosize settings.
					this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
				}
			}
		}

		/// <summary>
		/// Determines whether this cell, at the given row index, shows the grid's editing control or not.
		/// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
		/// </summary>
		private bool OwnsEditingNumericUpDown(int rowIndex)
		{
			if (rowIndex == -1 || this.DataGridView == null)
			{
				return false;
			}
			DataGridViewNumericUpDownEditingControl numericUpDownEditingControl = this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;
			return numericUpDownEditingControl != null && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
		}

		/// <summary>
		/// Custom paints the cell. The base implementation of the DataGridViewTextBoxCell type is called first,
		/// dropping the icon error and content foreground parts. Those two parts are painted by this custom implementation.
		/// In this sample, the non-edited NumericUpDown control is painted by using a call to Control.DrawToBitmap. This is
		/// an easy solution for painting controls but it's not necessarily the most performant. An alternative would be to paint
		/// the NumericUpDown control piece by piece (text and up/down buttons).
		/// </summary>
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState,
									  object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
									  DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (this.DataGridView == null)
			{
				return;
			}

			// First paint the borders and background of the cell.
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
					   paintParts & ~(DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.ContentForeground));

			Point ptCurrentCell = this.DataGridView.CurrentCellAddress;
			bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
			bool cellEdited = cellCurrent && this.DataGridView.EditingControl != null;

			// If the cell is in editing mode, there is nothing else to paint
			if (!cellEdited)
			{
				if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
				{
					// Paint a NumericUpDown control
					// Take the borders into account
					Rectangle borderWidths = BorderWidths(advancedBorderStyle);
					Rectangle valBounds = cellBounds;
					valBounds.Offset(borderWidths.X, borderWidths.Y);
					valBounds.Width -= borderWidths.Right;
					valBounds.Height -= borderWidths.Bottom;
					// Also take the padding into account
					if (cellStyle.Padding != Padding.Empty)
					{
						if (this.DataGridView.RightToLeft == RightToLeft.Yes)
						{
							valBounds.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
						}
						else
						{
							valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
						}
						valBounds.Width -= cellStyle.Padding.Horizontal;
						valBounds.Height -= cellStyle.Padding.Vertical;
					}
					// Determine the NumericUpDown control location
					valBounds = GetAdjustedEditingControlBounds(valBounds, cellStyle);

					bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;

					if (renderingBitmap.Width < valBounds.Width ||
						renderingBitmap.Height < valBounds.Height)
					{
						// The static bitmap is too small, a bigger one needs to be allocated.
						renderingBitmap.Dispose();
						renderingBitmap = new Bitmap(valBounds.Width, valBounds.Height);
					}
					// Make sure the NumericUpDown control is parented to a visible control
					if (paintingNumericUpDown.Parent == null || !paintingNumericUpDown.Parent.Visible)
					{
						paintingNumericUpDown.Parent = this.DataGridView;
					}
					// Set all the relevant properties
					paintingNumericUpDown.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(cellStyle.Alignment);
					paintingNumericUpDown.DecimalPlaces = this.DecimalPlaces;
					paintingNumericUpDown.Hexadecimal = this.Hexadecimal;
					paintingNumericUpDown.ThousandsSeparator = this.ThousandsSeparator;
					paintingNumericUpDown.Font = cellStyle.Font;
					paintingNumericUpDown.Width = valBounds.Width;
					paintingNumericUpDown.Height = valBounds.Height;
					paintingNumericUpDown.RightToLeft = this.DataGridView.RightToLeft;
					paintingNumericUpDown.Location = new Point(0, -paintingNumericUpDown.Height - 100);
					paintingNumericUpDown.Value = Convert.ToDecimal(value);

					Color foreColor;
					if (PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
					{
						foreColor = cellStyle.SelectionForeColor;
					}
					else
					{
						foreColor = cellStyle.ForeColor;
					}
					if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
					{
						if (foreColor.A < 255)
						{
							// The NumericUpDown control does not support transparent fore colors
							foreColor = Color.FromArgb(255, foreColor);
						}
						paintingNumericUpDown.ForeColor = foreColor;
					}

					Color backColor;
					if (PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected)
					{
						backColor = cellStyle.SelectionBackColor;
					}
					else
					{
						backColor = cellStyle.BackColor;
					}
					if (PartPainted(paintParts, DataGridViewPaintParts.Background))
					{
						if (backColor.A < 255)
						{
							// The NumericUpDown control does not support transparent back colors
							backColor = Color.FromArgb(255, backColor);
						}
						paintingNumericUpDown.BackColor = backColor;
					}
					// Finally paint the NumericUpDown control
					Rectangle srcRect = new Rectangle(0, 0, valBounds.Width, valBounds.Height);
					if (srcRect.Width > 0 && srcRect.Height > 0)
					{
						paintingNumericUpDown.DrawToBitmap(renderingBitmap, srcRect);
						graphics.DrawImage(renderingBitmap, new Rectangle(valBounds.Location, valBounds.Size),
										   srcRect, GraphicsUnit.Pixel);
					}
				}
				if (PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon))
				{
					// Paint the potential error icon on top of the NumericUpDown control
					base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
							   cellStyle, advancedBorderStyle, DataGridViewPaintParts.ErrorIcon);
				}
			}
		}

		/// <summary>
		/// Little utility function called by the Paint function to see if a particular part needs to be painted. 
		/// </summary>
		private static bool PartPainted(DataGridViewPaintParts paintParts, DataGridViewPaintParts paintPart)
		{
			return (paintParts & paintPart) != 0;
		}

		/// <summary>
		/// Custom implementation of the PositionEditingControl method called by the DataGridView control when it
		/// needs to relocate and/or resize the editing control.
		/// </summary>
		public override void PositionEditingControl(bool setLocation,
											bool setSize,
											Rectangle cellBounds,
											Rectangle cellClip,
											DataGridViewCellStyle cellStyle,
											bool singleVerticalBorderAdded,
											bool singleHorizontalBorderAdded,
											bool isFirstDisplayedColumn,
											bool isFirstDisplayedRow)
		{
			Rectangle editingControlBounds = PositionEditingPanel(cellBounds,
														cellClip,
														cellStyle,
														singleVerticalBorderAdded,
														singleHorizontalBorderAdded,
														isFirstDisplayedColumn,
														isFirstDisplayedRow);
			editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
			this.DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
			this.DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
		}

		/// <summary>
		/// Utility function that sets a new value for the DecimalPlaces property of the cell. This function is used by
		/// the cell and column DecimalPlaces property. The column uses this method instead of the DecimalPlaces
		/// property for performance reasons. This way the column can invalidate the entire column at once instead of 
		/// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
		/// this cell may be shared among multiple rows.
		/// </summary>
		internal void SetDecimalPlaces(int rowIndex, int value)
		{
			Debug.Assert(value >= 0 && value <= 99);
			this.decimalPlaces = value;
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.DecimalPlaces = value;
			}
		}

		/// Utility function that sets a new value for the Increment property of the cell. This function is used by
		/// the cell and column Increment property. A row index needs to be provided as a parameter because
		/// this cell may be shared among multiple rows.
		internal void SetIncrement(int rowIndex, Decimal value)
		{
			Debug.Assert(value >= (Decimal)0.0);
			this.increment = value;
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.Increment = value;
			}
		}

		/// Utility function that sets a new value for the Maximum property of the cell. This function is used by
		/// the cell and column Maximum property. The column uses this method instead of the Maximum
		/// property for performance reasons. This way the column can invalidate the entire column at once instead of 
		/// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
		/// this cell may be shared among multiple rows.
		internal void SetMaximum(int rowIndex, Decimal value)
		{
			this.maximum = value;
			if (this.minimum > this.maximum)
			{
				this.minimum = this.maximum;
			}
			object cellValue = GetValue(rowIndex);
			if (cellValue != null)
			{
				Decimal currentValue = System.Convert.ToDecimal(cellValue);
				Decimal constrainedValue = Constrain(currentValue);
				if (constrainedValue != currentValue)
				{
					SetValue(rowIndex, constrainedValue);
				}
			}
			Debug.Assert(this.maximum == value);
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.Maximum = value;
			}
		}
		
		/// Utility function that sets a new value for the Minimum property of the cell. This function is used by
		/// the cell and column Minimum property. The column uses this method instead of the Minimum
		/// property for performance reasons. This way the column can invalidate the entire column at once instead of 
		/// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
		/// this cell may be shared among multiple rows.
		internal void SetMinimum(int rowIndex, Decimal value)
		{
			this.minimum = value;
			if (this.minimum > this.maximum)
			{
				this.maximum = value;
			}
			object cellValue = GetValue(rowIndex);
			if (cellValue != null)
			{
				if (Hexadecimal)
				{
					Decimal currentValue = System.Convert.ToDecimal(cellValue);
					Decimal constrainedValue = Constrain(currentValue);
					if (constrainedValue != currentValue)
					{
						SetValue(rowIndex, constrainedValue);
					}
				}
				else
				{
					Decimal currentValue = System.Convert.ToDecimal(cellValue);
					Decimal constrainedValue = Constrain(currentValue);
					if (constrainedValue != currentValue)
					{
						SetValue(rowIndex, constrainedValue);
					}
				}
			}
			Debug.Assert(this.minimum == value);
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.Minimum = value;
			}
		}

		/// Utility function that sets a new value for the ThousandsSeparator property of the cell. This function is used by
		/// the cell and column ThousandsSeparator property. The column uses this method instead of the ThousandsSeparator
		/// property for performance reasons. This way the column can invalidate the entire column at once instead of 
		/// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
		/// this cell may be shared among multiple rows.
		internal void SetThousandsSeparator(int rowIndex, bool value)
		{
			this.thousandsSeparator = value;
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.ThousandsSeparator = value;
			}
		}

		internal void SetHexadecimal(int rowIndex, bool value)
		{
			this.hexadecimal = value;
			if (OwnsEditingNumericUpDown(rowIndex))
			{
				this.EditingNumericUpDown.Hexadecimal = value;
			}
		}

		/// <summary>
		/// Returns a standard textual representation of the cell.
		/// </summary>
		public override string ToString()
		{
			return "DataGridViewNumericUpDownCell { ColumnIndex=" + ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", RowIndex=" + RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
		}

		/// <summary>
		/// Little utility function used by both the cell and column types to translate a DataGridViewContentAlignment value into
		/// a HorizontalAlignment value.
		/// </summary>
		internal static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
		{
			if ((align & anyRight) != 0)
			{
				return HorizontalAlignment.Right;
			}
			else if ((align & anyCenter) != 0)
			{
				return HorizontalAlignment.Center;
			}
			else
			{
				return HorizontalAlignment.Left;
			}
		}

	}
	/// <summary>
	/// Reference Article https://msdn.microsoft.com/en-us/library/aa730881(v=vs.80).aspx
	/// Defines the editing control for the DataGridViewNumericUpDownCell custom cell type.
	/// </summary>/// <summary>

	class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
	{
		// Needed to forward keyboard messages to the child TextBox control.
		[System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// The grid that owns this editing control
		private DataGridView dataGridView;
		// Stores whether the editing control's value has changed or not
		private bool valueChanged;
		// Stores the row index in which the editing control resides
		private int rowIndex;

		/// <summary>
		/// Constructor of the editing control class
		/// </summary>
		public DataGridViewNumericUpDownEditingControl()
		{
			// The editing control must not be part of the tabbing loop
			this.TabStop = false;
		}

		// Beginning of the IDataGridViewEditingControl interface implementation

		/// <summary>
		/// Property which caches the grid that uses this editing control
		/// </summary>
		public virtual DataGridView EditingControlDataGridView
		{
			get
			{
				return this.dataGridView;
			}
			set
			{
				this.dataGridView = value;
			}
		}

		/// <summary>
		/// Property which represents the current formatted value of the editing control
		/// </summary>
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				this.Text = (string)value;
			}
		}

		/// <summary>
		/// Property which represents the row in which the editing control resides
		/// </summary>
		public virtual int EditingControlRowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				this.rowIndex = value;
			}
		}

		/// <summary>
		/// Property which indicates whether the value of the editing control has changed or not
		/// </summary>
		public virtual bool EditingControlValueChanged
		{
			get
			{
				return this.valueChanged;
			}
			set
			{
				this.valueChanged = value;
			}
		}

		/// <summary>
		/// Property which determines which cursor must be used for the editing panel,
		/// i.e. the parent of the editing control.
		/// </summary>
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		/// <summary>
		/// Property which indicates whether the editing control needs to be repositioned 
		/// when its value changes.
		/// </summary>
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Method called by the grid before the editing control is shown so it can adapt to the 
		/// provided cell style.
		/// </summary>
		public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			this.Font = dataGridViewCellStyle.Font;
			if (dataGridViewCellStyle.BackColor.A < 255)
			{
				// The NumericUpDown control does not support transparent back colors
				Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
				this.BackColor = opaqueBackColor;
				this.dataGridView.EditingPanel.BackColor = opaqueBackColor;
			}
			else
			{
				this.BackColor = dataGridViewCellStyle.BackColor;
			}
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(dataGridViewCellStyle.Alignment);
		}

		/// <summary>
		/// Method called by the grid on keystrokes to determine if the editing control is
		/// interested in the key or not.
		/// </summary>
		public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			switch (keyData & Keys.KeyCode)
			{
				case Keys.Right:
					{
						TextBox textBox = this.Controls[1] as TextBox;
						if (textBox != null)
						{
							// If the end of the selection is at the end of the string,
							// let the DataGridView treat the key message
							if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)) ||
								(this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)))
							{
								return true;
							}
						}
						break;
					}

				case Keys.Left:
					{
						TextBox textBox = this.Controls[1] as TextBox;
						if (textBox != null)
						{
							// If the end of the selection is at the begining of the string
							// or if the entire text is selected and we did not start editing,
							// send this character to the dataGridView, else process the key message
							if ((this.RightToLeft == RightToLeft.No && !(textBox.SelectionLength == 0 && textBox.SelectionStart == 0)) ||
								(this.RightToLeft == RightToLeft.Yes && !(textBox.SelectionLength == 0 && textBox.SelectionStart == textBox.Text.Length)))
							{
								return true;
							}
						}
						break;
					}

				case Keys.Down:
					// If the current value hasn't reached its minimum yet, handle the key. Otherwise let
					// the grid handle it.
					if (this.Value > this.Minimum)
					{
						return true;
					}
					break;

				case Keys.Up:
					// If the current value hasn't reached its maximum yet, handle the key. Otherwise let
					// the grid handle it.
					if (this.Value < this.Maximum)
					{
						return true;
					}
					break;

				case Keys.Home:
				case Keys.End:
					{
						// Let the grid handle the key if the entire text is selected.
						TextBox textBox = this.Controls[1] as TextBox;
						if (textBox != null)
						{
							if (textBox.SelectionLength != textBox.Text.Length)
							{
								return true;
							}
						}
						break;
					}

				case Keys.Delete:
					{
						// Let the grid handle the key if the carret is at the end of the text.
						TextBox textBox = this.Controls[1] as TextBox;
						if (textBox != null)
						{
							if (textBox.SelectionLength > 0 ||
								textBox.SelectionStart < textBox.Text.Length)
							{
								return true;
							}
						}
						break;
					}
			}
			return !dataGridViewWantsInputKey;
		}

		/// <summary>
		/// Returns the current value of the editing control.
		/// </summary>
		public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			bool userEdit = this.UserEdit;
			try
			{
				// Prevent the Value from being set to Maximum or Minimum when the cell is being painted.
				this.UserEdit = (context & DataGridViewDataErrorContexts.Display) == 0;
				return this.Value.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());

			}
			finally
			{
				this.UserEdit = userEdit;
			}
		}

		/// <summary>
		/// Called by the grid to give the editing control a chance to prepare itself for
		/// the editing session.
		/// </summary>
		public virtual void PrepareEditingControlForEdit(bool selectAll)
		{
			TextBox textBox = this.Controls[1] as TextBox;
			if (textBox != null)
			{
				if (selectAll)
				{
					textBox.SelectAll();
				}
				else
				{
					// Do not select all the text, but
					// position the caret at the end of the text
					textBox.SelectionStart = textBox.Text.Length;
				}
			}
		}

		// End of the IDataGridViewEditingControl interface implementation

		/// <summary>
		/// Small utility function that updates the local dirty state and 
		/// notifies the grid of the value change.
		/// </summary>
		private void NotifyDataGridViewOfValueChange()
		{
			if (!this.valueChanged)
			{
				this.valueChanged = true;
				this.dataGridView.NotifyCurrentCellDirty(true);
			}
		}

		/// <summary>
		/// Listen to the KeyPress notification to know when the value changed, and 
		/// notify the grid of the change.
		/// </summary>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			// The value changes when a digit, the decimal separator, the group separator or
			// the negative sign is pressed.
			bool notifyValueChange = false;
			if (char.IsDigit(e.KeyChar))
			{
				notifyValueChange = true;
			}
			else if (((e.KeyChar >= 'a' && e.KeyChar <= 'f') || e.KeyChar >= 'A' && e.KeyChar <= 'F'))
			{
				notifyValueChange = true;
			}
			else
			{
				System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
				string decimalSeparatorStr = numberFormatInfo.NumberDecimalSeparator;
				string groupSeparatorStr = numberFormatInfo.NumberGroupSeparator;
				string negativeSignStr = numberFormatInfo.NegativeSign;
				if (!string.IsNullOrEmpty(decimalSeparatorStr) && decimalSeparatorStr.Length == 1)
				{
					notifyValueChange = decimalSeparatorStr[0] == e.KeyChar;
				}
				if (!notifyValueChange && !string.IsNullOrEmpty(groupSeparatorStr) && groupSeparatorStr.Length == 1)
				{
					notifyValueChange = groupSeparatorStr[0] == e.KeyChar;
				}
				if (!notifyValueChange && !string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
				{
					notifyValueChange = negativeSignStr[0] == e.KeyChar;
				}
			}

			if (notifyValueChange)
			{
				// Let the DataGridView know about the value change
				NotifyDataGridViewOfValueChange();
			}
		}

		protected override void UpdateEditText()
		{
			if (base.UserEdit) HexParseEditText();
			if (!string.IsNullOrEmpty(base.Text))
			{
				base.ChangingText = true;
				base.Text = string.Format("{0:X}", (uint)base.Value);
			}
		}
		protected override void ValidateEditText()
		{
			HexParseEditText();
			UpdateEditText();
		}
		private void HexParseEditText()
		{
			try
			{
				if (!string.IsNullOrEmpty(base.Text))
					this.Value = Convert.ToInt64(base.Text, 16);
			}
			catch { }
			finally
			{
				base.UserEdit = false;
			}
		}

		/// <summary>
		/// Listen to the ValueChanged notification to forward the change to the grid.
		/// </summary>
		protected override void OnValueChanged(EventArgs e)
		{
			base.OnValueChanged(e);
			if (this.Focused)
			{
				// Let the DataGridView know about the value change
				NotifyDataGridViewOfValueChange();
			}
		}

		/// <summary>
		/// A few keyboard messages need to be forwarded to the inner textbox of the
		/// NumericUpDown control so that the first character pressed appears in it.
		/// </summary>
		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			TextBox textBox = this.Controls[1] as TextBox;
			if (textBox != null)
			{
				SendMessage(textBox.Handle, m.Msg, m.WParam, m.LParam);
				return true;
			}
			else
			{
				return base.ProcessKeyEventArgs(ref m);
			}
		}
	}

	//Enables the doublebuffered flag for DGVs
	public static class ExtensionMethods
	{
		public static void DoubleBuffered(this DataGridView dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
			pi.SetValue(dgv, setting, null);
		}
		public static void DoubleBuffered(this NumericUpDown updown, bool setting)
		{
			Type updownType = updown.GetType();
			PropertyInfo pi = updownType.GetProperty("DoubleBuffered",
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty);
			pi.SetValue(updown, setting, null);
		}
	}
}
