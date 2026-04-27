document.addEventListener("DOMContentLoaded", async function () {
    const badge = document.getElementById("cartCount");
    if (!badge) return;

    try {
        const response = await fetch("/Cart/GetTotalItemInCart", {
            method: "GET",
            credentials: "same-origin"
        });

        if (!response.ok) {
            badge.textContent = "0";
            return;
        }

        const text = await response.text();
        const number = parseInt(text, 10);

        if (isNaN(number)) {
            badge.textContent = "0";
            return;
        }

        badge.textContent = number.toString();
    } catch {
        badge.textContent = "0";
    }
});