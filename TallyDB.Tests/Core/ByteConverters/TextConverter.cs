using System.Text;

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class TextConverter
  {
    private readonly TallyDB.Core.ByteConverters.TextConverter _textConverter;

    public TextConverter()
    {
      _textConverter = new TallyDB.Core.ByteConverters.TextConverter();
    }

    [TestMethod]
    public void Encode_ShouldConvertTextWithinLimit()
    {
      var input = "Example Text";
      var expected = new byte[32] {
        69, 120, 97, 109, 112, 108, 101, 32, 84, 101, 120, 116, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
      };

      byte[] bytes = _textConverter.Encode(input);
      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod]
    public void Encode_ShouldConvertTextOverLimit()
    {
      var input = "ThisTextGoesOverTheLimitOf32CharactersInItsName";
      var expected = Encoding.ASCII.GetBytes("ThisTextGoesOverTheLimitOf32Char");

      byte[] bytes = _textConverter.Encode(input);

      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod]
    public void Encode_ShouldConvertEmptyText()
    {
      var input = "";
      var expected = new byte[32];

      byte[] bytes = _textConverter.Encode(input);

      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod]
    public void EncodeDecode_ShouldCycleWithinLimitEncoding()
    {
      var input = "test";
      var encoded = _textConverter.Encode(input);
      var output = _textConverter.Decode(encoded);

      Assert.AreEqual(input, output);
    }

    [TestMethod]
    public void EncodeDecode_ShouldCycleOverLimitEncoding()
    {
      var input = "ThisTextGoesOverTheLimitOf32CharactersInItsName";
      var encoded = _textConverter.Encode(input);
      var output = _textConverter.Decode(encoded);
      var expected = "ThisTextGoesOverTheLimitOf32Char";

      Assert.AreEqual(expected, output);
    }
  }
}
