using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Common.Errors;

public static class ProductErrors
{
    public static readonly Error NotFound = new(
        "Product.NotFound",
        "Product was not found.",
        ErrorType.NotFound);

    public static readonly Error CategoryNotFound = new(
        "Product.CategoryNotFound",
        "The specified category was not found.",
        ErrorType.NotFound);

    public static readonly Error CreationFailed = new(
        "Product.CreationFailed",
        "Product could not be created.",
        ErrorType.Failure);

    public static readonly Error UpdateFailed = new(
        "Product.UpdateFailed",
        "Product could not be updated.",
        ErrorType.Failure);

    public static readonly Error DeleteFailed = new(
        "Product.DeleteFailed",
        "Product could not be deleted.",
        ErrorType.Failure);
}