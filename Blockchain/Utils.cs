// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;

namespace Blockchain;

public static class Utils
{
    public static byte[] Int64ToByteArray(Int64 value)
    {
        unsafe
        {
            byte[] buffer = new byte[8];
            fixed (byte* numRef = buffer)
            {
                *((long*)numRef) = value;
            }

            return buffer;
        }
    }

    public static byte[] StringToByteArray(string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }

    public static string ByteArrayToString(byte[] array)
    {
        return BitConverter.ToString(array);
    }
    public static byte[] Sha256(byte[] input)
    {
        var hasher = SHA256.Create();
        var hash = hasher.ComputeHash(input);
        hasher.Dispose();
        return hash;
    }

    public static string Align(string text, int chars)
    {
        int len = text.Length;
        if (len == chars)
        {
            return text;
        }

        if (len > chars)
        {
            return text.Substring(0, chars - 3) + "...";
        }

        return text.PadRight(chars);
    }

    public static void Log(string msg)
    {
        Console.WriteLine("{0}", msg);
    }
}