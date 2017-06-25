using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace WindowsGlitchHarvester
{
    public class ProcessHijacker
    {

        //C# Signature for the FindWindow() API 
        [DllImport("USER32.DLL")] // Let the computer know where the below function will be imported from (user32.dll) 
        public static extern IntPtr FindWindow( // Create a function "template" 
            string lpClassName, // The function has two arguments, lpClassName (=Window class name) and lpWindowName (=Window name) 
            string lpWindowName // Example of FindWindow() call using the arguments -> FindWindow("WindowClass" <- this is the lpClassName type string, "WindowName" <- this is the lpWindowName type string); 
        );

        //C# Signature for the WriteProcessMemory() API 
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        //C# Signature for the OpenProcess() API 
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            UInt32 dwProcessId
        );


        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);



        //C# Signature for the GetWindowThreadProcessId() API 
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(
            IntPtr hWnd,
            out uint lpdwProcessId
        );

        //C# Signature for the ReadProcessMemory() API 
        [DllImport("kernel32.dll", SetLastError = true)]
        static unsafe extern bool ReadProcessMemory(
         IntPtr hProcess,
         IntPtr lpBaseAddress,
         void* lpBuffer,
         int dwSize,
         out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);


        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);


        // REQUIRED STRUCTS

        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }



        // REQUIRED CONSTS

        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        // Global variables (containers) 
        UInt32 ProcID;
        IntPtr WindowHandle;
        IntPtr ProcessReadHandle;
        IntPtr ProcessWriteHandle;
        IntPtr bytesout; // If using ReadProcessMemory(), dump the successfully read bytes here. 

        public ushort processorArchitecture;
        ushort reserved;
        public uint pageSize;
        public IntPtr minimumApplicationAddress;
        public IntPtr maximumApplicationAddress;
        public IntPtr activeProcessorMask;
        public uint numberOfProcessors;
        public uint processorType;
        public uint allocationGranularity;
        public ushort processorLevel;
        public ushort processorRevision;

        public long processSize;

		public bool Hooked;


        //Memory indexing
        long[] allMemoryChunkSizes;
		long[] allMemoryChunkAddresses;
		private string processName;


		// thx these for info
		// http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory
		// http://www.codeproject.com/Articles/670373/Csharp-Read-Write-another-Process-Memory


		public ProcessHijacker(string processName)
		{
			Hooked = HookToProcess(processName);
		}

        public void WriteBytes(byte[] byteArray, long address)
        {
			long _address = GetRealAddressFromVirtual(address);


			for (long i = _address; i < byteArray.Length + _address; i++)
                WriteProcessMemory(ProcessWriteHandle, (IntPtr)(_address + i) /*Target address*/, new byte[] { 0 } /*Byte array call*/, (UIntPtr)1 /*Byte array size*/, out bytesout);
        }

        public void WriteByte(byte byteValue, long address)
        {
			long _address = GetRealAddressFromVirtual(address);

			WriteProcessMemory(ProcessWriteHandle, (IntPtr)_address /*Target address*/, new byte[] { byteValue } /*Byte array call*/, (UIntPtr)1 /*Byte array size*/, out bytesout);
        }

        public long GetRealAddressFromVirtual(long virtualAddress)
        {
            long address = 0;
            long blockVirtualAddress = 0;

            for(int i = 0; i < allMemoryChunkAddresses.Length; i++)
            {

                if (virtualAddress < blockVirtualAddress + allMemoryChunkSizes[i])
                {
                    address = (virtualAddress - blockVirtualAddress) + allMemoryChunkAddresses[i];
                    break;
                }

                blockVirtualAddress += allMemoryChunkSizes[i];

            }

            return address;
        }

        public long? GetProcessSizeAndIndexChunks()
        {
            //returns memory size


            // getting minimum & maximum address

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            int fullMemSize = 0;

            var _allMemoryChunkSizes = new List<long>();
            var _allMemoryChunkAddresses = new List<long>();

            while (proc_min_address_l < proc_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(ProcessReadHandle, proc_min_address, out mem_basic_info, 28);

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
					_allMemoryChunkSizes.Add(mem_basic_info.RegionSize);
					_allMemoryChunkAddresses.Add(proc_min_address_l);

                    fullMemSize += mem_basic_info.RegionSize;
                }

                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }

			if(fullMemSize == 0)
			{
				MessageBox.Show("The process refused being hooked");
				return null;
			}

			allMemoryChunkSizes = _allMemoryChunkSizes.ToArray();
			allMemoryChunkAddresses = _allMemoryChunkAddresses.ToArray();

			return fullMemSize;

        }

		public byte ReadByte(long address)
		{
			int bytesRead = 0;
			byte[] buffer = new byte[1];

			ReadProcessMemory((int)ProcessReadHandle, (int)address, buffer, buffer.Length, ref bytesRead);

			return buffer[0];
		}


		public byte[] ReadBytes(long address, long amount)
		{
			int bytesRead = 0;
			byte[] buffer = new byte[amount];

			ReadProcessMemory((int)ProcessReadHandle, (int)address, buffer, buffer.Length, ref bytesRead);

			return buffer;
		}



		/*
		public byte ReadByte(long address)
		{

			long _address = GetRealAddressFromVirtual(address);


			// getting minimum & maximum address

			SYSTEM_INFO sys_info = new SYSTEM_INFO();
			GetSystemInfo(out sys_info);

			IntPtr proc_min_address = sys_info.minimumApplicationAddress;
			IntPtr proc_max_address = sys_info.maximumApplicationAddress;

			// saving the values as long ints so I won't have to do a lot of casts later
			long proc_min_address_l = _address;
			long proc_max_address_l = _address + 1;


			// notepad better be runnin'
			//Process process = Process.GetProcessesByName("notepad")[0];

			// opening the process with desired access level
			//IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

			//StreamWriter sw = new StreamWriter("dump.txt");

			// this will store any information we get from VirtualQueryEx()
			MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

			int bytesRead = 0;  // number of bytes read with ReadProcessMemory

			List<byte> outputBytes = new List<byte>();

			while (proc_min_address_l < proc_max_address_l)
			{
				// 28 = sizeof(MEMORY_BASIC_INFORMATION)
				VirtualQueryEx(ProcessReadHandle, proc_min_address, out mem_basic_info, 28);

				// if this memory chunk is accessible
				if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
				{

					byte[] buffer = new byte[mem_basic_info.RegionSize];

					// read everything in the buffer above
					ReadProcessMemory((int)ProcessReadHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

					// then output this in the file
					//for (int i = 0; i < mem_basic_info.RegionSize; i++)
					//sw.WriteLine("0x{0} : {1}", (mem_basic_info.BaseAddress + i).ToString("X"), (char)buffer[i]);

					outputBytes.AddRange(buffer);
				}

				// move to the next memory chunk
				proc_min_address_l += mem_basic_info.RegionSize;
				proc_min_address = new IntPtr(proc_min_address_l);
			}

			return outputBytes[0];

		}

		public byte[] ReadBytes(long address, long amount)
		{

			long _address = GetRealAddressFromVirtual(address);

			// getting minimum & maximum address

			SYSTEM_INFO sys_info = new SYSTEM_INFO();
			GetSystemInfo(out sys_info);

			IntPtr proc_min_address = sys_info.minimumApplicationAddress;
			IntPtr proc_max_address = sys_info.maximumApplicationAddress;

			// saving the values as long ints so I won't have to do a lot of casts later
			long proc_min_address_l = _address;
			long proc_max_address_l = _address + amount;


			// notepad better be runnin'
			//Process process = Process.GetProcessesByName("notepad")[0];

			// opening the process with desired access level
			//IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

			//StreamWriter sw = new StreamWriter("dump.txt");

			// this will store any information we get from VirtualQueryEx()
			MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

			int bytesRead = 0;  // number of bytes read with ReadProcessMemory

			List<byte> outputBytes = new List<byte>();

			while (proc_min_address_l < proc_max_address_l)
			{
				// 28 = sizeof(MEMORY_BASIC_INFORMATION)
				VirtualQueryEx(ProcessReadHandle, proc_min_address, out mem_basic_info, 28);

				// if this memory chunk is accessible
				if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
				{

					byte[] buffer = new byte[mem_basic_info.RegionSize];

					// read everything in the buffer above
					ReadProcessMemory((int)ProcessReadHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

					// then output this in the file
					//for (int i = 0; i < mem_basic_info.RegionSize; i++)
					//sw.WriteLine("0x{0} : {1}", (mem_basic_info.BaseAddress + i).ToString("X"), (char)buffer[i]);

					outputBytes.AddRange(buffer);
				}

				// move to the next memory chunk
				proc_min_address_l += mem_basic_info.RegionSize;
				proc_min_address = new IntPtr(proc_min_address_l);
			}

			return outputBytes.ToArray();

		}

	*/

		public byte[] ReadAllData()
        {

            // getting minimum & maximum address

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;


            // notepad better be runnin'
            //Process process = Process.GetProcessesByName("notepad")[0];

            // opening the process with desired access level
            //IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

            //StreamWriter sw = new StreamWriter("dump.txt");

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

			int bytesRead = 0;  // number of bytes read with ReadProcessMemory

            List<byte> outputBytes = new List<byte>();

            while (proc_min_address_l < proc_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(ProcessReadHandle, proc_min_address, out mem_basic_info, 28);



                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {

                    byte[] buffer = new byte[mem_basic_info.RegionSize];

                    // read everything in the buffer above
                    ReadProcessMemory((int)ProcessReadHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

                    // then output this in the file
                    //for (int i = 0; i < mem_basic_info.RegionSize; i++)
                    //sw.WriteLine("0x{0} : {1}", (mem_basic_info.BaseAddress + i).ToString("X"), (char)buffer[i]);

                    outputBytes.AddRange(buffer);
                }

                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }

            return outputBytes.ToArray();

        }

        public bool HookToWindow(string windowClass, string windowTitle)
        {
            //WindowHandle = FindWindow("WindowClass", "WindowTitle"); // Establish a Window Handle (hWnd) 
            WindowHandle = FindWindow(windowClass, windowTitle); // Establish a Window Handle (hWnd) 

            GetWindowThreadProcessId(WindowHandle, out ProcID); // Get the Process ID (PID) from the targeted window/window class 
            ProcessReadHandle = OpenProcess(PROCESS_ALL_ACCESS, 1, ProcID); // Gain access to the process memory with OpenProcess() using Process ID as an entry

			var pSize = GetProcessSizeAndIndexChunks();

			if (pSize == null)
				return false;
			else
			{
				processSize = (long)pSize;
				return true;
			}
        }

        public bool HookToProcess(string _processName = null)
        {
			if(_processName!= null)
				processName = _processName;



			string[] processData = processName.Split(':');

			Process process = Process.GetProcessById(Convert.ToInt32(processData[1]));

            // opening the process with desired access level
            ProcessReadHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
            ProcessWriteHandle = OpenProcess(0x1F0FFF, false, process.Id);
			//ProcessHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_ALL_ACCESS, true, process.Id);

			var pSize = GetProcessSizeAndIndexChunks();

			if(pSize == null)
				return false;
			else
			{
				processSize = (long)pSize;
				return true;
			}
        }

		public void refreshProcessSize()
		{
			HookToProcess();
		}
	}
}