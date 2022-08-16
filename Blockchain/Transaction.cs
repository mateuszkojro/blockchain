namespace Blockchain;

public class TxInput
{
    public byte[] TransactionId { get; }
    public int Vout { get; }
    public string scriptSig { get; }
    public TxInput(byte[] transactionId, int vout, string data)
    {
        TransactionId = transactionId;
        Vout = vout;
        scriptSig = data;
    }

    public bool CanUnlockOutputWith(string unlockingData)
    {
        return scriptSig == unlockingData;
    }
}

public class TxOutput
{
    public TxOutput(long amount, string scriptPubKey)
    {
        this.amount = amount;
        this.scriptPubKey = scriptPubKey;
    }

    public bool CanBeUnlockedWith(string unlockingData)
    {
        return scriptPubKey == unlockingData;
    }

    public Int64 amount { get; }
    public string scriptPubKey { get; } // User defined wallet address
}

public class Transaction
{
    public static Transaction CreateCoinbaseTx(string to, string? data)
    {
        if (data is null)
        {
            data = $"Reward to {to}";
        }
        var txin = new TxInput(new byte[] { }, -1, data);
        var txout = new TxOutput(100, to);
        var tx = new Transaction(new[] { txin }, new[] { txout });
        return tx;
    }
    public byte[] id { get; }
    public TxInput[] Vin;
    public TxOutput[] Vout;

    public bool IsCoinbase()
    {
        throw new NotImplementedException(); // Not checked in the original code
        return Vin.Length == 1 && Vin[0].TransactionId.Length == 0 && Vin[0].Vout == -1;
    }

    byte[] GenerateId()
    {
        var data = new List<byte>();
        foreach (var txin in Vin)
        {
            data.AddRange(txin.TransactionId);
            data.AddRange(Utils.Int64ToByteArray(txin.Vout));
            data.AddRange(Utils.StringToByteArray(txin.scriptSig));
        }

        foreach (var txout in Vout)
        {
            data.AddRange(Utils.Int64ToByteArray(txout.amount));
            data.AddRange(Utils.StringToByteArray(txout.scriptPubKey));
        }

        return Utils.Sha256(data.ToArray());
    }

    public Transaction(TxInput[] vin, TxOutput[] vout)
    {
        Vin = vin;
        Vout = vout;
        id = GenerateId();
    }
}