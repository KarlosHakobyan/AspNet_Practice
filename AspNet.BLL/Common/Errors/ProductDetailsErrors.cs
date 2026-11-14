using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Common.Errors;

public static class ProductDetailsErrors
{
    public static readonly Error NotFound = new("ProductDetails.NotFound","Product details were not found.",ErrorType.NotFound);

    public static readonly Error ProductNotFound = new("ProductDetails.ProductNotFound","The specified product was not found.",ErrorType.NotFound);

    public static readonly Error AlreadyExists = new("ProductDetails.AlreadyExists","Details already exist for this product.",ErrorType.Conflict);

    public static readonly Error CreationFailed = new("ProductDetails.CreationFailed","Product details could not be created.",ErrorType.Failure);

    public static readonly Error UpdateFailed = new("ProductDetails.UpdateFailed","Product details could not be updated.",ErrorType.Failure);

    public static readonly Error DeleteFailed = new("ProductDetails.DeleteFailed","Product details could not be deleted.",ErrorType.Failure);
}