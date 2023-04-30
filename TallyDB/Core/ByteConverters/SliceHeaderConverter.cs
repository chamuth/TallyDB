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
      throw new NotImplementedException();
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

      return name.Concat(axisCount).Concat(axes).Concat(frequency).ToArray();
    }

    public int GetHeaderLength(SliceDefinition definition)
    {
      return 41 + 33 * _definition.Axes.Length;
    }
  }
}