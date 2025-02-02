using SpreadCheetah.SourceGeneration;

namespace SpreadCheetah.SourceGenerator.Test.Models.Contexts;

[WorksheetRow(typeof(ClassWithNoProperties))]
[WorksheetRow(typeof(RecordClassWithNoProperties))]
[WorksheetRow(typeof(StructWithNoProperties))]
[WorksheetRow(typeof(RecordStructWithNoProperties))]
[WorksheetRow(typeof(ReadOnlyStructWithNoProperties))]
[WorksheetRow(typeof(ReadOnlyRecordStructWithNoProperties))]
[WorksheetRowGenerationOptions(SuppressWarnings = true)]
public partial class NoPropertiesContext : WorksheetRowContext
{
}
