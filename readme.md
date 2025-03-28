# Possari

A tool for managing children's token balances and reward redemptions.

## Setup & Run

```sh
git clone https://github.com/juhenius/possari
cd possari
dotnet ef database update --project ./src/Possari.Infrastructure --startup-project ./src/Possari.WebApi
dotnet run --project ./src/Possari.AppHost
```

## Usage

When running the project

- [Aspire dashboard](https://localhost:17106/)
- [OpenAPI Docs](https://localhost:7010/scalar/v1)

## License

MIT

## Author

Jari Helenius

[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- MARKDOWN LINKS & IMAGES -->

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/jari-helenius-a445478a
