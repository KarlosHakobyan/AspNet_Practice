using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Common.Errors;

public static class TagErrors
{
    public static readonly Error NotFound = new("Tag.NotFound","Tag was not found.",ErrorType.NotFound);

    public static readonly Error IsInUse = new("Tag.IsInUse","Tag cannot be deleted because it is assigned to products.",ErrorType.Conflict);

    public static readonly Error CreationFailed = new("Tag.CreationFailed","Tag could not be created.",ErrorType.Failure);

    public static readonly Error UpdateFailed = new("Tag.UpdateFailed","Tag could not be updated.",ErrorType.Failure);

    public static readonly Error DeleteFailed = new("Tag.DeleteFailed","Tag could not be deleted.",ErrorType.Failure);
}