

RUN mkdir -p /dotnetapp
COPY ./src/roversay /dotnetapp
WORKDIR /dotnetapp
RUN dotnet restore
RUN dotnet build

