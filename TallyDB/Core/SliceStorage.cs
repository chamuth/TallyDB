using TallyDB.Core.ByteConverters;

namespace TallyDB.Core
{
  internal class SliceStorage
  {
    SliceDefinition _definition;
    BinaryWriter _writer;

    public SliceStorage(SliceDefinition definition, BinaryWriter writer)
    {
      _definition = definition;
      _writer = writer;
    }


    /// <summary>
    /// Stores slice definitions into slice file
    /// </summary>
    public void UpdateSliceDefinitions()
    {
      if (_writer.BaseStream.Length > GetSliceHeaderSize())
      {
        // Data frames already available in the slice
      }

      //_writer.Write(,0,)
    }

    public byte[] CreateHeaderBufferForDefinition()
    {
      TextConverter txtConverter = new TextConverter();

      // Name of the slice 
      byte[] name = txtConverter.Convert(_definition.Name);

      // Number of axes
      byte[] axisCount = { (byte)_definition.Axes.Length };

      // Axes information
      List<byte> axesList = new List<byte>();

      foreach (Axis axis in _definition.Axes)
      {
        byte[] axisName = txtConverter.Convert(axis.Name);
        byte dataType = (byte)axis.Type;
        byte function = (byte)axis.Function;
        byte axisProps = (byte)((dataType << 4) | function);

        axesList.AddRange(axisName);
        axesList.Add(axisProps);
      }
      byte[] axes = axesList.ToArray();

      // Frequency
      byte[] frequency = BitConverter.GetBytes(_definition.Frequency);

      return name.Concat(axisCount).Concat(axes).Concat(frequency).ToArray();
    }

    int GetSliceHeaderSize()
    {
      return 41 + 33 * _definition.Axes.Length;
    }
  }
}