using System.Net.NetworkInformation;

namespace Pulpa.TrainerSafety.Api.AppStart
{
    public class Registrations
    {
        public static void MapEndpoints(IEndpointRouteBuilder app)
        {
            Feature.RegisterUser.MapEndpoint(app);
            Feature.LoginUser.MapEndpoint(app);
            /*Pulpa.TrainerSafety.Api.Feature.GetUserProfile.MapEndpoint(app);
            Pulpa.TrainerSafety.Api.Feature.UpdateUserProfile.MapEndpoint(app);
            Pulpa.TrainerSafety.Api.Feature.ChangeUserPassword.MapEndpoint(app);
            Pulpa.TrainerSafety.Api.Feature.DeleteUserAccount.MapEndpoint(app);
            Pulpa.TrainerSafety.Api.Feature.ListAllUsers.MapEndpoint(app);*/
        }
    }
}
