using CureFusion.Contracts.Answer;
using CureFusion.Contracts.Appointment;
using CureFusion.Contracts.Question;
using Mapster;
using RealState.Services;


namespace SurveyBasket.Mapping
{
    public class MappingConfigurations() : IRegister
    {
        private readonly string _filesPath = $"https://curefusion2.runasp.net/Uploads";

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



            TypeAdapterConfig<Drug, DrugResponse>.NewConfig()
    .Map(dest => dest.DrugImage, 
         src => src.DrugImageId != null ? $"{_filesPath}/{src.DrugImage.StoredFileName}" : null);



        }
    }
}
