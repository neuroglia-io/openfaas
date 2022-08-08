FROM openfaas/of-watchdog:0.7.2 as watchdog

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN apt-get update
RUN apt-get install -y jq


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/web-api/WebApiTemplate.csproj", "src/web-api/"]
RUN dotnet restore "src/web-api/WebApiTemplate.csproj"
COPY . .
WORKDIR "/src/src/web-api"

FROM build AS publish
RUN dotnet publish "WebApiTemplate.csproj" -c Release -r linux-x64 -p:PublishTrimmed=true -p:PublishReadyToRun=true -o /app

FROM base AS final

RUN mkdir -p /home/app

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

RUN addgroup --system app \
    && adduser --system --ingroup app app
RUN chown app /home/app

WORKDIR /home/app
COPY --from=publish /app .

USER app

ENV fprocess="dotnet ./Function.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost"
ENV ASPNETCORE_URLS=http://+:80
ENV exec_timeout="30s"
ENV write_timeout="15s"
ENV read_timeout="15s"
ENV prefix_logs="false"
ENV write_debug="true"

EXPOSE 8080
CMD ["fwatchdog"]