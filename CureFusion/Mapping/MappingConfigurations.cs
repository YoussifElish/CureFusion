using CureFusion.Contracts.Appointment;
using RealState.Services;

public class MappingConfigurations : IRegister
{
    private readonly IFileService _fileService;
    private readonly string _filesPath = $"https://curefusion2.runasp.net/Uploads"; //check it again

    public MappingConfigurations()
    {
    }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CureFusion.Contracts.Auth.RegisterRequest, ApplicationUser>()
              .Map(dest => dest.UserName, src => src.Email)
              .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<RegisterAsDoctorRequest, ApplicationUser>()
              .Map(dest => dest.UserName, src => src.Email)
              .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<PatientAppointmentRequest, PatientAppointment>()
              .Ignore(dest => dest.UserId);

        TypeAdapterConfig<Question, QuestionResponse>.NewConfig()
            .Map(dest => dest.UserName, src => src.User.FirstName + " " + src.User.LastName);

        TypeAdapterConfig<AnswerRequest, Answer>.NewConfig()
            .Map(dest => dest.CreatedIn, src => DateTime.UtcNow);

        TypeAdapterConfig<Answer, AnswerResponse>.NewConfig()
            .Map(dest => dest.UserName, src => src.User.FirstName + " " + src.User.LastName);

        TypeAdapterConfig<Drug, DrugResponse>.NewConfig()
            .Map(dest => dest.DrugImage,
                 src => src.DrugImageId != null ? $"{_filesPath}/{src.DrugImage.StoredFileName}" : null);

        TypeAdapterConfig<(ApplicationUser user, IList<string> roles), UserResponse>.NewConfig()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

        config.NewConfig<UserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.NormalizedUserName, src => src.Email);
    }
}
