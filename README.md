# CSharp_practice_1

Небольшое веб-приложение на Blazor Server для проверки високосности года. В проект также входит REST API: год передается в GET-запросе через HTTP-заголовок `Year`.

## Требования

- .NET 10 SDK
- Git для клонирования репозитория

Проверить установленную версию SDK можно командой:

```powershell
dotnet --version
```

## Локальный запуск

Склонируйте репозиторий и перейдите в его каталог:

```powershell
git clone https://github.com/FlamedKlug/CSharp_practice_1.git
cd CSharp_practice_1
dotnet run --urls http://localhost:5207
```

Откройте веб-клиент в браузере: <http://localhost:5207/>. Чтобы остановить сервер, нажмите `Ctrl+C` в терминале.

## REST API

Endpoint `GET /api/leap-year` принимает год в заголовке `Year`.

```powershell
curl.exe -H "Year: 2024" http://localhost:5207/api/leap-year
```

Для високосного года ответ будет `{"leap":true}`, для обычного — `{"leap":false}`. Если заголовок `Year` отсутствует или пуст, API вернет `{}`.
