namespace Project.Api.Presentation.Endpoints;

public static class OrderEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapOrderEndpoints()
        {
            var group = route.MapGroup("api/v1/orders");


        }
    }
}
