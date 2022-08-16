// See https://aka.ms/new-console-template for more information

namespace Blockchain;

public class Block
{
    readonly Int64 _timestamp;
    Transaction[] _transactions;
    readonly byte[] _data;

    readonly byte[] _previousBlockHash;
    public byte[] Hash { get; }
    public Int64 Nonce { get; }

    public byte[] HashTransactions() {
        var result = new List<byte>();
        foreach (var transaction in _transactions)
        {
            result.AddRange(transaction.id);
        }

        return Utils.Sha256(result.ToArray());
    }

    static byte[] CalcHash(Int64 timestamp, byte[] data, byte[] previousBlockHash)
    {
        var result = new List<byte>();

        result.AddRange(Utils.Int64ToByteArray(timestamp));
        result.AddRange(data);
        result.AddRange(previousBlockHash);

        return Utils.Sha256(result.ToArray());
    }

    private Block(byte[] data, byte[] previousBlockHash, Int64 nonce, Transaction[] transactions)
    {
        _transactions = transactions;
        _timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        _data = data;
        _previousBlockHash = previousBlockHash;
        Hash = CalcHash(_timestamp, data, previousBlockHash);
        Nonce = nonce;
    }

    public Block(byte[] data, byte[] previousBlockHash, Transaction[] transactions)
    {
        // Create the proof of work
        var proofOfWork = new ProofOfWork(new Block(data, previousBlockHash, 0, transactions));
        Nonce = proofOfWork.Run();

        // Copy the block with the proof to `this`
        var readyBlock = proofOfWork.Block;
        _timestamp = readyBlock._timestamp;
        _data = readyBlock._data;
        Hash = readyBlock.Hash;
        _previousBlockHash = readyBlock._previousBlockHash;
        _transactions = readyBlock._transactions;
    }

    public override string ToString()
    {
        return $"Block(hash={BitConverter.ToString(Hash)}, data={BitConverter.ToString(_data)})";
    }
}