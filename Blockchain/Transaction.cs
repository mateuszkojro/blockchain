namespace Blockchain;

class TxInput
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
}

class TxOutput
{
    public TxOutput(long amount, string scriptPubKey)
    {
        this.amount = amount;
        this.scriptPubKey = scriptPubKey;
    }

    public Int64 amount { get; }
    public string scriptPubKey { get; } // User defined wallet address
}

class Transaction
{
    static Transaction CreateCoinbaseTx(string to, string? data)
    {
        if(data is null) {
            data = $"Reward to {to}";
        }
        var txin = new TxInput(new byte[] { }, -1, data);
        var txout = new TxOutput(100, to);
        var tx = new Transaction(new[] { txin }, new[] { txout });
        return tx;
    }
    byte[] id;
    TxInput[] Vin;
    TxOutput[] Vout;

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