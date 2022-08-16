// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;

namespace Blockchain;

public class ProofOfWork
{
    const int TargetBytes = 1;
    public Block Block { get; }

    public ProofOfWork(Block block)
    {
        Block = block;
    }

    public static bool HashIsValid(byte[] hash)
    {
        return CountZeros(hash) > TargetBytes;
    }

    public bool Valid()
    {
        var data = ToByteArray(Block.Nonce);
        var hashBytes = Utils.Sha256(data);
        Utils.Log($"Hash: {Utils.ByteArrayToString(hashBytes)}");
        return HashIsValid(hashBytes);
    }

    byte[] ToByteArray(Int64 nonce)
    {
        var result = new List<byte>();
        result.AddRange(Block.Hash);
        result.AddRange(Block.HashTransactions());
        result.AddRange(Utils.Int64ToByteArray(TargetBytes));
        result.AddRange(Utils.Int64ToByteArray(nonce));
        return result.ToArray();
    }

    static int CountZeros(byte[] array)
    {
        int zeros = 0;
        foreach (var b in array)
        {
            if (b != 0)
            {
                return zeros;
            }

            zeros++;
        }

        return zeros;
    }

    public Int64 Run()
    {
        Int64 nonce = 0;
        Utils.Log($"Mining block: {Block}");
        Console.WriteLine("Expecting more than {0} zero bytes", TargetBytes / 8);
        while (nonce < Int64.MaxValue)
        {
            var hashBytes = Utils.Sha256(ToByteArray(nonce));
            if (HashIsValid(hashBytes))
            {
                Utils.Log($"Mined hash: {Utils.ByteArrayToString(hashBytes)} with {nonce} nonce and {CountZeros(hashBytes)} zero bytes");
                return nonce;
            }

            nonce++;
        }

        Debug.Assert(nonce != Int64.MaxValue, "Could not find a valid nonce");
        return nonce;
    }
}