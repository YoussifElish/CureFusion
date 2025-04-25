using CureFusion.Contracts.Appointment;
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


        }
    }
}
