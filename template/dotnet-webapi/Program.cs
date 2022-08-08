using Neuroglia.Eventing;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddYamlFile("configuration.yaml", true, true);
builder.Configuration.AddJsonFile("configuration.json", true, true);

var brokerUri = Environment.GetEnvironmentVariable("CLOUDEVENTS_BROKER_URI");
if (!string.IsNullOrEmpty(brokerUri))
    builder.Services.AddCloudEventBus(bus =>
    {
        bus.WithBrokerUri(new(brokerUri));
    });

builder.Services.AddLogging(logger =>
{
    logger.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.UseUtcTimestamp = true;
        options.TimestampFormat = "[HH:mm:ss] ";
    });
});
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var pathPrefix = Environment.GetEnvironmentVariable("API_PATH_PREFIX");
    if (!string.IsNullOrEmpty(pathPrefix))
        options.DocumentFilter<PathPrefixInsertDocumentFilter>(pathPrefix);
});

var app = builder.Build();

app.MapControllers();
app.UseSwagger(options =>
{
    options.RouteTemplate = "{documentName}/oas.{json|yaml}";
});
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/v1/oas.json", "v1");
});

await app.RunAsync();

public class PathPrefixInsertDocumentFilter
    : IDocumentFilter
{

    public PathPrefixInsertDocumentFilter(string prefix)
    {
        this.Prefix = prefix;
    }

    protected virtual string Prefix { get; }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.Keys.ToList();
        foreach (var path in paths)
        {
            var pathToChange = swaggerDoc.Paths[path];
            swaggerDoc.Paths.Remove(path);
            swaggerDoc.Paths.Add($"{this.Prefix}{path}", pathToChange);
        }
    }

}