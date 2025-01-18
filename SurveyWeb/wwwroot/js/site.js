// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", () => {
    const numericInput = document.querySelector("#numeric");
    if (numericInput) {
        numericInput.addEventListener("change", () => {
            const value = parseInt(numericInput.value, 10);
            if (value < 1 || value > 10) {
                alert("Vänligen ange ett nummer mellan 1 och 10.");
                numericInput.value = "";
            }
        });
    }
});
