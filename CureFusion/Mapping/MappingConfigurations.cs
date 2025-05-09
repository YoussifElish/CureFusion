using CureFusion.Contracts.Answer;
using CureFusion.Contracts.Appointment;
using CureFusion.Contracts.Question;
using Mapster;


namespace SurveyBasket.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CureFusion.Contracts.Auth.RegisterRequest, ApplicationUser>().Map(dest => dest.UserName, src => src.Email).Map(dest => dest.EmailConfirmed, src => true);
            config.NewConfig<RegisterAsDoctorRequest, ApplicationUser>().Map(dest => dest.UserName, src => src.Email).Map(dest => dest.EmailConfirmed, src => true);

            config.NewConfig<PatientAppointmentRequest, PatientAppointment>()
    .Ignore(dest => dest.UserId);

            TypeAdapterConfig<Question, QuestionResponse>.NewConfig()
    .Map(dest => dest.UserName, src => src.User.FirstName + " " + src.User.LastName);


            TypeAdapterConfig<AnswerRequest, Answer>.NewConfig()
                     .Map(dest => dest.CreatedIn, src => DateTime.UtcNow); ; // For setting CreatedIn automatically

        TypeAdapterConfig<Answer, AnswerResponse>.NewConfig()
            .Map(dest => dest.UserName, src => src.User.FirstName + " " + src.User.LastName); // For concatenating user name


<<<<<<< Updated upstream
=======

            TypeAdapterConfig<Drug, DrugResponse>.NewConfig()
    .Map(dest => dest.DrugImage, 
         src => src.DrugImageId != null ? $"{_filesPath}/{src.DrugImage.StoredFileName}" : null);



          TypeAdapterConfig<(ApplicationUser user, IList<string> roles),UserResponse>.NewConfig()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);

            config.NewConfig<UserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.EmailConfirmed, src => true);

            config.NewConfig<UpdateUserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
               .Map(dest => dest.NormalizedUserName, src => src.Email);
               



>>>>>>> Stashed changes
        }
    }
}
