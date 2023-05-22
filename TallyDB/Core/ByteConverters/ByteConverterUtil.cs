using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallyDB.Core.ByteConverters
{
    public static class ByteConverterUtil
    {
        /// <summary>
        /// Concat an 2D array into a 1D array
        /// </summary>
        /// <param name="array">2D array</param>
        /// <returns>1D Concatenated array</returns>
        public static T[] ConcatTogether<T>(this T[][] array)
        {
            return array.SelectMany(a => a).ToArray();
        }

        /// <summary>
        /// Decode a slice of a byte array with given converter
        /// </summary>
        /// <typeparam name="T">Type of converter</typeparam>
        /// <param name="bytes">Byte array to slice from</param>
        /// <param name="skip">Reference skip amount</param>
        /// <param name="converter">Converter instance</param>
        /// <returns></returns>
        public static T DecodeByteSlice<T>(this byte[] bytes, ref int skip, IByteConverter<T> converter)
        {
            var value = converter.Decode(bytes.Skip(skip).Take(converter.GetFixedLength()).ToArray());
            skip += converter.GetFixedLength();
            return value;
        }
    }
}
