// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const menuToggle = document.querySelector(".menu-toggle");
    const nav = document.querySelector(".nav");

    if (menuToggle && nav) {
        menuToggle.addEventListener("click", function () {
            nav.classList.toggle("active"); // Toggle the 'active' class
        });
    }
});
document.querySelector(".menu-toggle").click();
console.log("JavaScript file loaded");
