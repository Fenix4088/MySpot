namespace MySpot.Api.Exceptions;

public sealed class EmptyLicensePlateException(string message = "License plate is empty!"): MySpotException(message)
{
    
}