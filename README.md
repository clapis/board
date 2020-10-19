board
=====

A simple reference app inspired by [Jimmy Bogard](https://github.com/jbogard) and [Steve Smith](https://github.com/ardalis)

- MediatR
- Automapper
- Fluent Validation
- Entity Framework Core
- Layered/Onion/Clean Architecture


### Start

Run Integration Tests
```
> cd tests\Board.IntegrationTests
> dotnet test
```

Run Application
```
> cd src\Board.WebHost
> dotnet run
```

### Docker build

```
> docker build -t [tag] .
> docker run --rm -it -p 5000:80 [tag]
> docker push [tag]
```
