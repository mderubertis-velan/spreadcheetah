using SpreadCheetah.CellValueWriters.Number;
using SpreadCheetah.Styling;
using SpreadCheetah.Styling.Internal;
using System.Buffers.Text;

namespace SpreadCheetah.CellValueWriters.Time;

internal sealed class DateTimeCellValueWriter : NumberCellValueWriterBase
{
    protected override int GetStyleId(StyleId styleId) => styleId.DateTimeId;

    protected override bool TryWriteValue(in DataCell cell, Span<byte> destination, out int bytesWritten)
    {
        return Utf8Formatter.TryFormat(cell.NumberValue.DoubleValue, destination, out bytesWritten);
    }

    public override bool Equals(in CellValue value, in CellValue other) => value.DoubleValue == other.DoubleValue;
    public override int GetHashCodeFor(in CellValue value) => value.DoubleValue.GetHashCode();

    public override bool TryWriteCell(in DataCell cell, DefaultStyling? defaultStyling, SpreadsheetBuffer buffer)
    {
        var defaultStyleId = defaultStyling?.DateTimeStyleId;
        return defaultStyleId is not null
            ? TryWriteCell(cell, defaultStyleId.Value, buffer)
            : TryWriteCell(cell, buffer);
    }

    public override bool TryWriteCell(string formulaText, in DataCell cachedValue, StyleId? styleId, DefaultStyling? defaultStyling, SpreadsheetBuffer buffer)
    {
        var actualStyleId = styleId?.DateTimeId ?? defaultStyling?.DateTimeStyleId;
        return TryWriteCell(formulaText, cachedValue, actualStyleId, buffer);
    }

    public override bool WriteFormulaStartElement(StyleId? styleId, DefaultStyling? defaultStyling, SpreadsheetBuffer buffer)
    {
        var actualStyleId = styleId?.DateTimeId ?? defaultStyling?.DateTimeStyleId;
        return WriteFormulaStartElement(actualStyleId, buffer);
    }
}
