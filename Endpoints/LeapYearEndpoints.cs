using System.Globalization;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlazorApp.Endpoints;

/// <summary>
/// Содержит REST endpoint для проверки года на високосность.
/// </summary>
public static class LeapYearEndpoints
{
    /// <summary>
    /// Регистрирует GET endpoint, принимающий год в заголовке Year.
    /// </summary>
    /// <param name="endpoints">Маршрутизатор приложения ASP.NET Core.</param>
    /// <returns>Тот же маршрутизатор для возможности дальнейшей настройки.</returns>
    public static IEndpointRouteBuilder MapLeapYearEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/leap-year", CheckLeapYear)
            .WithName("CheckLeapYear")
            .WithSummary("Проверить, является ли год високосным")
            .WithDescription("Читает год из заголовка Year. При пустом или отсутствующем заголовке возвращает пустой JSON-объект.")
            .Produces<EmptyResponse>(StatusCodes.Status200OK)
            .Produces<LeapYearResponse>(StatusCodes.Status200OK);

        return endpoints;
    }

    /// <summary>
    /// Проверяет заголовок Year и возвращает объект с результатом либо пустой объект.
    /// </summary>
    /// <param name="request">Текущий HTTP-запрос.</param>
    /// <returns>Пустой объект для пустого заголовка или результат проверки года.</returns>
    private static Results<Ok<EmptyResponse>, Ok<LeapYearResponse>> CheckLeapYear(HttpRequest request)
    {
        // Отсутствующий или состоящий только из пробелов заголовок означает, что год не задан.
        if (!request.Headers.TryGetValue("Year", out var yearHeader)
            || string.IsNullOrWhiteSpace(yearHeader.ToString()))
        {
            return TypedResults.Ok(new EmptyResponse());
        }

        // BigInteger позволяет проверить делимость даже для очень длинной последовательности цифр.
        // NumberStyles.None запрещает знаки и пробелы: ожидается только введенное пользователем число.
        if (!BigInteger.TryParse(yearHeader.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out var year))
        {
            return TypedResults.Ok(new LeapYearResponse(false));
        }

        // По григорианскому правилу високосен каждый год, кратный 400,
        // либо год, кратный 4, но не кратный 100.
        var isLeapYear = year % 400 == 0 || year % 4 == 0 && year % 100 != 0;

        return TypedResults.Ok(new LeapYearResponse(isLeapYear));
    }
}

/// <summary>
/// Представляет ответ API, когда год не был передан; сериализуется как пустой JSON-объект.
/// </summary>
public sealed record EmptyResponse;

/// <summary>
/// Представляет результат проверки високосности года.
/// </summary>
/// <param name="Leap">Истина, если год високосный; иначе ложь.</param>
public sealed record LeapYearResponse(bool Leap);