namespace MySpot.Api.Exceptions;


public sealed class InvalidLicensePlateException(string licensePlate): MySpotException($"License plate: {licensePlate} is invalid.")
{
    public string LicensePlate { get; } = licensePlate;
}
