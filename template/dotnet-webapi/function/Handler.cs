namespace Function
{

    [Route("/")]
    public class Handler
        : ControllerBase
    {

        //The following lines of code demonstrates how to inject services into the controller. See: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-6.0
        //public Handler(ILogger logger)
        //{
        //    Logger = logger;
        //}
        
        //ILogger Logger { get; }

        [HttpPost(Name = "handle")] //The name is used to generate the Open API operation's id. You can customize the path to bind to parameters, as described here: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-6.0#conventional-routing
        public async Task<IActionResult> Handle(/* You can bind arguments here, like so: [FromBody] MyRequestParams params. See https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0 */)
        {
            //Do something
            return Ok();
        }

    }

}
