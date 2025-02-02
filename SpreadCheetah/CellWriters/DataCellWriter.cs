using SpreadCheetah.Styling.Internal;

namespace SpreadCheetah.CellWriters;

internal sealed class DataCellWriter : BaseCellWriter<DataCell>
{
    public DataCellWriter(SpreadsheetBuffer buffer, DefaultStyling? defaultStyling)
        : base(buffer, defaultStyling)
    {
    }

    protected override bool TryWriteCell(in DataCell cell)
    {
        return cell.Writer.TryWriteCell(cell, DefaultStyling, Buffer);
    }

    protected override bool WriteStartElement(in DataCell cell)
    {
        return cell.Writer.WriteStartElement(Buffer);
    }

    protected override bool TryWriteEndElement(in DataCell cell)
    {
        return cell.Writer.TryWriteEndElement(Buffer);
    }

    protected override bool FinishWritingCellValue(in DataCell cell, ref int cellValueIndex)
    {
        return Buffer.WriteLongString(cell.StringValue, ref cellValueIndex);
    }
}
