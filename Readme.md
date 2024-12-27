
#### Migration example
```bash
cd infrastructure
dotnet ef migrations add CleaningReservation --startup-project ../MySpot.Api/MySpot.Api.csproj --context MySpotDbContext -o ./DAL/Migrations/
```

#### Run Seq structural logger
```bash
https://hub.docker.com/r/datalust/seq/
```