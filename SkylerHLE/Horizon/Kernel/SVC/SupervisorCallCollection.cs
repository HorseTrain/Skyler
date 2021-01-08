using SkylerCommon.Debugging;
using SkylerCommon.Utilities.Tools;
using SkylerCPU;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SkylerHLE.Horizon.Kernel.SVC
{
    public static class SupervisorCallCollection
    {
        public static void Call(int ID, ObjectIndexer<ulong> Registers)
        {
            if (Calls[ID] != null)
            {
                Calls[ID](Registers);
            }
            else
            {
                Debug.LogError($"Unknwon SVC Call. 0x{StringTools.FillStringFront(ID.ToString("X"),'0',2)}",true);
            }
        }

        static Dictionary<int, SupervisorCall> Calls { get; set; } = new Dictionary<int, SupervisorCall>()
        {
            { 0x00, null },
            { 0x01, SvcMemory.SetHeapSize },
            { 0x02, null },
            { 0x03, SvcMemory.SetMemoryAttribute  },
            { 0x04, null },
            { 0x05, null },
            { 0x06, SvcMemory.QueryMemory },
            { 0x07, null },
            { 0x08, null },
            { 0x09, null },
            { 0x0A, null },
            { 0x0B, SvcIO.SleepThread },
            { 0x0C, null },
            { 0x0D, null },
            { 0x0E, null },
            { 0x0F, null },
            { 0x10, null },
            { 0x11, null },
            { 0x12, null },
            { 0x13, SvcMemory.MapSharedMemory },
            { 0x14, null },
            { 0x15, SvcMemory.CreateTransferMemory },
            { 0x16, SvcIO.CloseHandle },
            { 0x17, null },
            { 0x18, null },
            { 0x19, null },
            { 0x1A, null },
            { 0x1B, null },
            { 0x1C, null },
            { 0x1D, null },
            { 0x1E, SvcIO.GetSystemTick },
            { 0x1F, SvcIPC.ConnectToNamedPort },
            { 0x20, null },
            { 0x21, SvcIPC.SendSyncRequest },
            { 0x22, null },
            { 0x23, null },
            { 0x24, null },
            { 0x25, null },
            { 0x26, null },
            { 0x27, SvcIO.OutputDebugString },
            { 0x28, null },
            { 0x29, SvcIO.GetInfo },
            { 0x2A, null },
            { 0x2B, null },
            { 0x2C, null },
            { 0x2D, null },
            { 0x2E, null },
            { 0x2F, null },
            { 0x30, null },
            { 0x31, null },
            { 0x32, null },
            { 0x33, null },
            { 0x34, null },
            { 0x35, null },
            { 0x36, null },
            { 0x37, null },
            { 0x38, null },
            { 0x39, null },
            { 0x3A, null },
            { 0x3B, null },
            { 0x3C, null },
            { 0x3D, null },
            { 0x3E, null },
            { 0x3F, null },
            { 0x40, null },
            { 0x41, null },
            { 0x42, null },
            { 0x43, null },
            { 0x44, null },
            { 0x45, null },
            { 0x46, null },
            { 0x47, null },
            { 0x48, null },
            { 0x49, null },
            { 0x4A, null },
            { 0x4B, null },
            { 0x4C, null },
            { 0x4D, null },
            { 0x4E, null },
            { 0x4F, null },
            { 0x50, null },
            { 0x51, null },
            { 0x52, null },
            { 0x53, null },
            { 0x54, null },
            { 0x55, null },
            { 0x56, null },
            { 0x57, null },
            { 0x58, null },
            { 0x59, null },
            { 0x5A, null },
            { 0x5B, null },
            { 0x5C, null },
            { 0x5D, null },
            { 0x5E, null },
            { 0x5F, null },
            { 0x60, null },
            { 0x61, null },
            { 0x62, null },
            { 0x63, null },
            { 0x64, null },
            { 0x65, null },
            { 0x66, null },
            { 0x67, null },
            { 0x68, null },
            { 0x69, null },
            { 0x6A, null },
            { 0x6B, null },
            { 0x6C, null },
            { 0x6D, null },
            { 0x6E, null },
            { 0x6F, null },
            { 0x70, null },
            { 0x71, null },
            { 0x72, null },
            { 0x73, null },
            { 0x74, null },
            { 0x75, null },
            { 0x76, null },
            { 0x77, null },
            { 0x78, null },
            { 0x79, null },
            { 0x7A, null },
            { 0x7B, null },
            { 0x7C, null },
            { 0x7D, null },
            { 0x7E, null },
        };

        //TODO: Maybe place this somewhere else?
        public static void SvcLog(string message) => Debug.Log($"SVC: {message}");
    }
}
