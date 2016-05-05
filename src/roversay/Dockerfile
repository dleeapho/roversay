FROM msimons/dotnet:slim

RUN mkdir -p /dotnetapp
COPY ./bin/Debug/netcoreapp1.0/debian.8-x64/publish /dotnetapp
WORKDIR /dotnetapp
ENTRYPOINT ["./roversay"]


