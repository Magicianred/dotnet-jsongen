# dotnet-jsongen
---
[![dcvs-devenv MyGet Build Status](https://www.myget.org/BuildSource/Badge/dcvs-devenv?identifier=8078a458-180f-4668-b8cd-8d58056fb351)](https://www.myget.org/)

code gen by razor template with json file

Feed: [https://www.myget.org/F/dcvs-devenv/api/v3/index.json](https://www.myget.org/F/dcvs-devenv/api/v3/index.json)

```
dotnet tool install dotnet-jsongen -g --add-source https://www.myget.org/F/dcvs-devenv/api/v3/index.json

dotnet-json <root-path>
```
