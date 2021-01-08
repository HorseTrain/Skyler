using SkylerCommon.Globals;

namespace SkylerCommon.Memory
{
    public unsafe class MemoryReader : Stream
    {
        public MemoryReader(void* Location) : base (Location)
        {

        }

        public T ReadStruct<T>() where T: unmanaged
        {
            T Out = *(T*)CurrentLocation;

            Advance((ulong)sizeof(T));

            return Out;
        }

        public T[] ReadStruct<T>(ulong Size) where T: unmanaged
        {
            T[] Out = new T[Size];

            for (ulong i = 0; i < Size; i++)
                Out[i] = ReadStruct<T>();

            return Out;
        }

        //TODO: Change this to "Address"
        public T ReadStructAtOffset<T>(ulong Offset) where T : unmanaged
        {
            Seek(Offset);

            return ReadStruct<T>();
        }

        public string ReadString(ulong size = ulong.MaxValue)
        {
            string Out = "";

            for (ulong i = 0; i < size; i++)
            {
                byte temp = ReadStruct<byte>();

                if (temp == 0)
                    break;

                Out += (char)temp;
            }    

            return Out;
        }

        public string ReadStringAtAddress(ulong Address, ulong size = ulong.MaxValue)
        {
            Seek(Address);

            return ReadString(size);
        }
    }
}
