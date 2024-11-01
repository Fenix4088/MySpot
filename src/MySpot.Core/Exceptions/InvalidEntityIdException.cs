namespace MySpot.Core.Exceptions;

public sealed class InvalidEntityIdException(Guid value): MySpotException($"Invalid reservation id: {value}.")
{
};