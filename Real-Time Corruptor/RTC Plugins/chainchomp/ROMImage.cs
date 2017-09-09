using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ChainChomp
{
    public enum MarioAddressMap
    {
        Graphics,
        Music,
    }

    public struct MarioAddressRange
    {
        public MarioAddressRange(int s, int e)
        {
            this.start = s;
            this.end = e;
        }
        public int start;
        public int end;
    }

    public struct ROMSample
    {
        public ROMSample(MarioAddressRange r, IEnumerable<byte> d)
        {
            this.range = r;
            this.data = d;
        }
        public MarioAddressRange range;
        public IEnumerable<byte> data;
    }

    public class ROMImage
    {
        private string _fileName;
        public string fileName
        {
            get
            {
                return _fileName;
            }
        }
        public int length
        {
            get
            {
                return data != null ? data.Length : 0;
            }
        }
        private byte[] data;
        private List<ROMSample> corruptedData = new List<ROMSample>();

        public ROMImage(string path)
        {
            LoadClean(path);
            _fileName = path;

            //add offsets
            //RegisterAddressRange(MarioAddressMap.Graphics, 0x8010, 0xA00F);
            //RegisterAddressRange(MarioAddressMap.Music, 0x79c8, 0x7f1f);

        }

        public void LoadClean(string path)
        {
            data = FileIO.ReadAllBytes(path);
        }

        public bool Dump(string path)
        {
            try
            {
                if (FileIO.WriteAllBytes(path, data)) //dump clean rom
                {
                    using (FileStream fs = File.OpenWrite(path))
                    {
                        //overwrite with modified bytes according to offsets
                        corruptedData.ForEach(n =>
                        {
                            fs.Seek(n.range.start, SeekOrigin.Begin);
                            fs.Write(n.data.ToArray<byte>(), 0, n.range.end - n.range.start);
                        });
                        
                    }

                }
            }
            catch (IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return Dump(path);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public ROMSample GetSample(int startOffset, int endOffset)
        {
            //use safe offsets
			startOffset = Math.Min(data.Count(), Math.Max(0, startOffset));
            endOffset = Math.Min(data.Count(), endOffset);

            //get sample
            ArraySegment<byte> sample = new ArraySegment<byte>(data, startOffset, endOffset-startOffset);

            return new ROMSample(new MarioAddressRange(startOffset, endOffset), sample);
        }

        public void CommitSampleToROM(ROMSample sample)
        {
            corruptedData.Add(sample);
        }

        public void CommitSampleRangeToROM(List<ROMSample> samples)
        {
            corruptedData.AddRange(samples);
        }

        public void ClearCorruptions()
        {
            corruptedData.Clear();
        }
    }
}
