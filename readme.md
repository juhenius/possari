# Possari

A tool for managing children's token balances and reward redemptions.

## Setup & Run

```sh
git clone https://github.com/juhenius/possari
cd possari
dotnet ef database update --project ./src/Possari.Infrastructure --startup-project ./src/Possari.WebApi
dotnet run --project ./src/Possari.WebApi
```

## OpenAPI

[API Docs](http://localhost:5043/scalar/v1) when running the project.

## License

MIT

## Author

Jari Helenius

[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- MARKDOWN LINKS & IMAGES -->

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/jari-helenius-a445478a
