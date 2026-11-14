using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Common.Errors;

public static class CategoryErrors
{
    public static readonly Error NotFound = new("Category.NotFound","Category was not found.",ErrorType.NotFound);

    public static readonly Error CreationFailed = new("Category.CreationFailed","Category could not be created.",ErrorType.Failure);

    public static readonly Error UpdateFailed = new("Category.UpdateFailed","Category could not be updated.",ErrorType.Failure);

    public static readonly Error DeleteFailed = new("Category.DeleteFailed","Category could not be deleted.",ErrorType.Failure);
}