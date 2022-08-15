using NUnit.Framework;

using Blockchain;

namespace Blockchain.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ProofOfWorkOfBasicBlock()
    {
        var block = new Block(Utils.StringToByteArray("Hello chain"), new byte[] { 1, 2, 3 });
        var proofOfWork = new ProofOfWork(block);
        Assert.IsTrue(proofOfWork.Valid());
    }
    [Test]
    public void CheckWholeChainIsValid()
    {
        var blockchain = new Blockchain(new Block(Utils.StringToByteArray("Hello chain"), new byte[] { 1, 2, 3 }));
        blockchain.AddBlock(Utils.StringToByteArray("Buy something"));
        blockchain.AddBlock(Utils.StringToByteArray("Sell something"));
        blockchain.AddBlock(Utils.StringToByteArray("Buy something again"));
        foreach (var block in blockchain.Blocks)
        {
            Assert.IsTrue(new ProofOfWork(block).Valid());
        }
    }

    [Test]
    public void ProofOfWorkIsRepetable()
    {
        var block = new Block(Utils.StringToByteArray("Hello chain"), new byte[] { 1, 2, 3 });
        var proofOfWork = new ProofOfWork(block);
        var nonceFirst = proofOfWork.Run();
        var nonceSecond = proofOfWork.Run();
        Assert.AreEqual(nonceFirst, nonceSecond);
    }
    [Test]
    public void ProofOfWorkIsRepetableAdvanced()
    {
        var block = new Block(Utils.StringToByteArray("Hello chain"), new byte[] { 1, 2, 3 });
        var proofOfWorkOne = new ProofOfWork(block);
        var proofOfWorkTwo = new ProofOfWork(block);
        var nonceFirst = proofOfWorkOne.Run();
        var nonceSecond = proofOfWorkTwo.Run();
        Assert.AreEqual(nonceFirst, nonceSecond);
    }
}