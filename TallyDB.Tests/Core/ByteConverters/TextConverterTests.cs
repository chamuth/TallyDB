using System.Text;
using TallyDB.Core.ByteConverters;

namespace TallyDB.Tests.Core.ByteConverters
{
  [TestClass]
  public class TextConverterTests
  {
    private readonly TextConverter _textConverter;

    public TextConverterTests()
    {
      _textConverter = new TextConverter();
    }

    [TestMethod("Should convert text within limits")]
    public void Encode_ShouldConvertTextWithinLimit()
    {
      var input = "Example Text";
      var expected = new byte[32] {
        69, 120, 97, 109, 112, 108, 101, 32, 84, 101, 120, 116, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
      };

      byte[] bytes = _textConverter.Encode(input);
      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod("Should convert text over the limit")]
    public void Encode_ShouldConvertTextOverLimit()
    {
      var input = "ThisTextGoesOverTheLimitOf32CharactersInItsName";
      var expected = Encoding.ASCII.GetBytes("ThisTextGoesOverTheLimitOf32Char");

      byte[] bytes = _textConverter.Encode(input);

      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod("Should convert empty text")]
    public void Encode_ShouldConvertEmptyText()
    {
      var input = "";
      var expected = new byte[32];

      byte[] bytes = _textConverter.Encode(input);

      CollectionAssert.AreEqual(expected, bytes);
    }

    [TestMethod("Should encode/decode cycle within limit")]
    public void EncodeDecode_ShouldCycleWithinLimitEncoding()
    {
      var input = "test";
      var encoded = _textConverter.Encode(input);
      var output = _textConverter.Decode(encoded);

      Assert.AreEqual(input, output);
    }

    [TestMethod("Should encode/decode cycle over limit")]
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
