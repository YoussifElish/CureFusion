namespace CureFusion.Contracts.Hospital;

public record HospitalRespone
(
    int ID,
    string Name,
    string Address,
    string Zone,
    string PhoneNumber,
    double? Rating,
    string Website,
    double? Distance,
    double Latitude,
    double Longitude

    );
