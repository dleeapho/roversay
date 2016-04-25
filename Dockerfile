FROM debtest
RUN mkdir -p /dotnetapp
COPY ./src/whalesay /dotnetapp
WORKDIR /dotnetapp
RUN dotnet restore
RUN dotnet build
#ENTRYPOINT ["dotnet", "run"]
