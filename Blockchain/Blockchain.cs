// See https://aka.ms/new-console-template for more information

namespace Blockchain;

public class Blockchain
{
    public List<Block> Blocks { get; }

    public Blockchain(Block genesisBlock)
    {
        Blocks = new List<Block> { genesisBlock };
    }

    // public Block AddBlock(byte[] data)
    // {
    //     var previousBlockHash = Blocks.Last().Hash;

    //     Blocks.Add(new Block(data, previousBlockHash));
    //     return Blocks.Last();
    // }

    Transaction[] FindUnspendTransactions(string address)
    {
        var spentTransactions = new Dictionary<string, int[]>();
        var unspentTx = new List<Transaction>();
        foreach (var block in Blocks)
        {
            foreach (var transaction in block.Transactions)
            {
                string txId = Utils.ByteArrayToString(transaction.id);

            outputs:
                for (int outIdx = 0; outIdx < transaction.Vout.Length; outIdx++)
                {
                    if (spentTransactions[txId] != null)
                    {
                        foreach (var spentOutput in spentTransactions[txId])
                        {
                            if (spentOutput == outIdx)
                            {
                                goto outputs;
                            }
                        }
                    }

                    if (transaction.Vout[outIdx].CanBeUnlockedWith(address))
                    {
                        unspentTx.Add(transaction);
                    }
                }

                if (transaction.IsCoinbase() == false)
                {
                    foreach (var input in transaction.Vin)
                    {
                        if (input.CanUnlockOutputWith(address))
                        {
                            string inTxId = Utils.ByteArrayToString(input.TransactionId);
                            if (spentTransactions[inTxId] == null)
                            {
                                spentTransactions[inTxId] = new int[] { input.Vout };
                            }
                            else
                            {
                                // var tmp = spentTransactions[inTxId];
                                // Array.Resize(ref tmp, tmp.Length + 1);
                                // tmp[tmp.Length - 1] = input.Vout;
                                spentTransactions[inTxId].Append(input.Vout);
                            }
                        }
                    }
                }
            }
            if (block.PreviousBlockHash.Length == 0)
            {
                break;
            }
        }

        return unspentTx.ToArray();
    }

    public override string ToString()
    {
        string result = "Blockchain {\n";
        foreach (var block in Blocks)
        {
            result += "\t" + block.ToString() + "\n";
        }
        result += "}";
        return result;
    }
}