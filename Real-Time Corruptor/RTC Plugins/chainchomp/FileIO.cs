using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChainChomp
{
    public static class FileIO
    {
        //BYTE METHODS

        public static byte[] ReadAllBytes(string path)
        {
            byte[] result;
            try
            {
                result = File.ReadAllBytes(path);
            }
            catch (IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return ReadAllBytes(path);
                }
                else
                {
                    result = new byte[0];
                }
            }

            return result;
        }

        public static bool WriteAllBytes(string path, byte[] data)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.WriteAllBytes(path, data);
            }
            catch (IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return WriteAllBytes(path, data);
                }
                else
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return WriteAllBytes(path, data);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        // JSON METHODS

        public static object Read(string path, Type desiredType)
        {
            
            object obj;
            try
            {
                MemoryStream str = new MemoryStream();
                DataContractSerializer ser = new DataContractSerializer(desiredType);
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.CopyTo(str);
                }

                str.Position = 0;
                obj = ser.ReadObject(str);
            }
            catch (IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return Read(path, desiredType);
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidDataContractException e)
            {
                MessageBox.Show("Invalid file type or corrupted file.", "Error", MessageBoxButtons.OK);
                return null;
            }

            return obj;
        }

        public static bool Write(string path, object data, Type dataType)
        {
            try
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }

                MemoryStream str = new MemoryStream();
                DataContractSerializer ser = new DataContractSerializer(dataType);
                ser.WriteObject(str, data);

                using (FileStream fs = File.OpenWrite(path))
                {
                    str.WriteTo(fs);
                }

            }
            catch (IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return Write(path, data, dataType);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidDataContractException e)
            {
                MessageBox.Show("Error #00 - Serialisation Failed", "Error", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }


        //ZIP METHODS

        public static bool ExtractZip(string source, string destination)
        {
            try
            {
                ZipFile.ExtractToDirectory(source, destination);
            }
            catch(IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return ExtractZip(source, destination);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidDataException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return ExtractZip(source, destination);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ZipFromDir(string source, string destination)
        {
            try
            {
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                ZipFile.CreateFromDirectory(source, destination);
            }
            catch(IOException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return ZipFromDir(source, destination);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidDataException e)
            {
                if (MessageBox.Show(e.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                {
                    return ExtractZip(source, destination);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
