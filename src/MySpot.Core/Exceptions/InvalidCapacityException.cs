namespace MySpot.Core.Exceptions;

public sealed class InvalidCapacityException(int capacity): MySpotException($"Capacity {capacity} is invalid.")
{
    
}