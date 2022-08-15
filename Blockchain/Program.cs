// See https://aka.ms/new-console-template for more information

namespace Blockchain;

static class Program
{
    static void Main(string[] args)
    {
        var genesisBlock = new Block(Utils.StringToByteArray("Hello chain"), new byte[] { 1, 2, 3 });
        var blockchain = new Blockchain(genesisBlock);
        blockchain.AddBlock(Utils.StringToByteArray("Buy something"));
        blockchain.AddBlock(Utils.StringToByteArray("Sell something"));

        Console.WriteLine("Blockchain: {0}", blockchain);

        foreach (var block in blockchain.Blocks)
        {
            Console.WriteLine("Block: {0} : {1}", Utils.Align(block.ToString(), 80), new ProofOfWork(block).Valid() ? "valid" : "invalid");
        }
        
        
        Console.WriteLine("Is SHA256 repetable");
        var array = new byte[] { 1, 2, 3 };
        Console.WriteLine(Utils.ByteArrayToString(Utils.Sha256(array)));
        Console.WriteLine(Utils.ByteArrayToString(Utils.Sha256(array)));
    }
}