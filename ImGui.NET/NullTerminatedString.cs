using System.Text;

namespace ImGuiNET
{
    public unsafe struct NullTerminatedString
    {
        public readonly byte* Data;

        public NullTerminatedString(byte* data)
        {
            Data = data;
        }

        public static implicit operator string(NullTerminatedString nts) => nts.ToString();
    }
}
