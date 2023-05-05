using System.ComponentModel;
using TallyDB.Core.ByteConverters.Util;

namespace TallyDB.Core.ByteConverters
{
  /// <summary>
  /// Contains Slice Storage helper functions taking Slice Definitions
  /// </summary>
  public class SliceHeaderConverter: IByteConverter<SliceDefinition>
  {
    /// <summary>
    /// Reads a byte buffer and returns a SliceDefinition
    /// </summary>
    /// <param name="bytes">Array of bytes</param>
    /// <returns>SliceDefinition</returns>
    public SliceDefinition Decode(byte[] bytes)
    {
      var skip = 0;
      // load slice name
      var textConverter = new TextConverter();
      var name = bytes.DecodeByteSlice(ref skip, textConverter);
      // load axis count
      int axisCount = bytes[++skip - 1];

      // Load each axis
      List<Axis> axes = new List<Axis>();
      for (var i = 0; i < axisCount; i++)
      {
        var axisName = bytes.DecodeByteSlice(ref skip, textConverter);
        var axisProps = bytes[++skip - 1];

        DataType dataType = (DataType)(axisProps >> 4);
        AggregateFunction function = (AggregateFunction)(axisProps & 0x0F);
        axes.Add(new Axis(axisName, dataType, function));
      }

      double frequency = BitConverter.ToDouble(bytes.Skip(skip).Take(8).ToArray());

      return new SliceDefinition(name, axes.ToArray(), frequency);
    }

    /// <summary>
    /// Gets byte buffer of slice header based on definition
    /// </summary>
    /// <returns>Byte array</returns>
    public byte[] Encode(SliceDefinition definition)
    {
      TextConverter txtConverter = new TextConverter();

      // Name of the slice 
      byte[] name = txtConverter.Encode(definition.Name);

      // Number of axes
      byte[] axisCount = { (byte)definition.Axes.Length };

      // Axes information
      List<byte> axesList = new List<byte>();

      foreach (Axis axis in definition.Axes)
      {
        byte[] axisName = txtConverter.Encode(axis.Name);
        byte dataType = (byte)axis.Type;
        byte function = (byte)axis.Function;
        byte axisProps = (byte)((dataType << 4) | function);

        axesList.AddRange(axisName);
        axesList.Add(axisProps);
      }
      byte[] axes = axesList.ToArray();

      // Frequency
      byte[] frequency = BitConverter.GetBytes(definition.Frequency);

      // Final byte buffer
      return (new byte[][] { name, axisCount, axes, frequency }).ConcatTogether();
    }

    public int GetFixedLength()
    {
      return 0;
    }
  }
}