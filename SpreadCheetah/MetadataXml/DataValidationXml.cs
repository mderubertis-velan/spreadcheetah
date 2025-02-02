using SpreadCheetah.Validations;
using System.Net;
using System.Text;

namespace SpreadCheetah.MetadataXml;

internal static class DataValidationXml
{
    public static async ValueTask WriteAsync(
        Stream stream,
        SpreadsheetBuffer buffer,
        Dictionary<CellReference, DataValidation> validations,
        CancellationToken token)
    {
        var sb = new StringBuilder("<dataValidations count=\"");
        sb.Append(validations.Count);
        sb.Append("\">");

        foreach (var keyValue in validations)
        {
            var validation = keyValue.Value;
            sb.Append("<dataValidation ");
            sb.AppendType(validation.Type);
            sb.AppendErrorType(validation.ErrorType);
            sb.AppendOperator(validation.Operator);

            if (validation.IgnoreBlank)
                sb.Append("allowBlank=\"1\" ");

            if (!validation.ShowDropdown)
                sb.Append("showDropDown=\"1\" ");

            if (validation.ShowInputMessage)
                sb.Append("showInputMessage=\"1\" ");

            if (validation.ShowErrorAlert)
                sb.Append("showErrorMessage=\"1\" ");

            if (!string.IsNullOrEmpty(validation.InputTitle))
                sb.AppendTextAttribute("promptTitle", validation.InputTitle!);

            if (!string.IsNullOrEmpty(validation.InputMessage))
                sb.AppendTextAttribute("prompt", validation.InputMessage!);

            if (!string.IsNullOrEmpty(validation.ErrorTitle))
                sb.AppendTextAttribute("errorTitle", validation.ErrorTitle!);

            if (!string.IsNullOrEmpty(validation.ErrorMessage))
                sb.AppendTextAttribute("error", validation.ErrorMessage!);

            sb.AppendTextAttribute("sqref", keyValue.Key.Reference);

            if (validation.Value1 is null)
            {
                sb.Append("/>");
                continue;
            }

            sb.Append("><formula1>").Append(validation.Value1).Append("</formula1>");

            if (validation.Value2 is not null)
                sb.Append("<formula2>").Append(validation.Value2).Append("</formula2>");

            sb.Append("</dataValidation>");
        }

        sb.Append("</dataValidations>");
        await buffer.WriteStringAsync(sb, stream, token).ConfigureAwait(false);
    }

    private static void AppendTextAttribute(this StringBuilder sb, string attribute, string value)
    {
        sb.Append(attribute);
        sb.Append("=\"");
        sb.Append(WebUtility.HtmlEncode(value));
        sb.Append("\" ");
    }

    private static StringBuilder AppendType(this StringBuilder sb, ValidationType type) => type switch
    {
        ValidationType.Decimal => sb.Append("type=\"decimal\" "),
        ValidationType.Integer => sb.Append("type=\"whole\" "),
        ValidationType.List => sb.Append("type=\"list\" "),
        ValidationType.TextLength => sb.Append("type=\"textLength\" "),
        _ => sb
    };

    private static StringBuilder AppendErrorType(this StringBuilder sb, ValidationErrorType type) => type switch
    {
        ValidationErrorType.Warning => sb.Append("errorStyle=\"warning\" "),
        ValidationErrorType.Information => sb.Append("errorStyle=\"information\" "),
        _ => sb
    };

    private static StringBuilder AppendOperator(this StringBuilder sb, ValidationOperator op) => op switch
    {
        ValidationOperator.NotBetween => sb.Append("operator=\"notBetween\" "),
        ValidationOperator.EqualTo => sb.Append("operator=\"equal\" "),
        ValidationOperator.NotEqualTo => sb.Append("operator=\"notEqual\" "),
        ValidationOperator.GreaterThan => sb.Append("operator=\"greaterThan\" "),
        ValidationOperator.LessThan => sb.Append("operator=\"lessThan\" "),
        ValidationOperator.GreaterThanOrEqualTo => sb.Append("operator=\"greaterThanOrEqual\" "),
        ValidationOperator.LessThanOrEqualTo => sb.Append("operator=\"lessThanOrEqual\" "),
        _ => sb
    };
}
