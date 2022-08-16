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