using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallyDB.Core.ByteConverters.Util
{
  public static class ByteConverterUtil
  {
    public static byte[] ConcatTogether(this byte[][] array)
    {
      return array.SelectMany(a => a).ToArray();
    }

    public static T DecodeByteSlice<T>(this byte[] bytes, ref int skip, IByteConverter<T> converter)
    {
      var value =  converter.Decode(bytes.Skip(skip).Take(converter.GetFixedLength()).ToArray());
      skip += converter.GetFixedLength();
      return value;
    }
  }
}
