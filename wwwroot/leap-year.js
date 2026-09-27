// Этот небольшой браузерный клиент отправляет тот же GET-запрос, который доступен внешним API-клиентам.
// Обработчик в фазе перехвата очищает поле до того, как событие получит компонент Blazor.
document.addEventListener("input", (event) => {
    const input = event.target;
    if (!(input instanceof HTMLInputElement) || input.id !== "year-input") {
        return;
    }

    const cursorPosition = input.selectionStart ?? input.value.length;
    const sanitizedPrefix = input.value.slice(0, cursorPosition).replace(/[^0-9]/g, "");
    const sanitizedValue = input.value.replace(/[^0-9]/g, "");

    if (input.value !== sanitizedValue) {
        input.value = sanitizedValue;
        input.setSelectionRange(sanitizedPrefix.length, sanitizedPrefix.length);
    }
}, true);

window.leapYearApi = {
    async check(year) {
        // При пустом поле заголовок Year не добавляется, чтобы сервер вернул пустой JSON-объект.
        const headers = year ? { Year: year } : {};
        const response = await fetch("/api/leap-year", {
            method: "GET",
            headers
        });

        if (!response.ok) {
            throw new Error(`Сервер вернул HTTP ${response.status}`);
        }

        return await response.json();
    }
};