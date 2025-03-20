document.querySelector(".menu-container-custom").addEventListener("mouseover", () => {
    const dropdown = document.querySelector(".dropdown-menu-custom");
    const isVisible = dropdown.style.display === "flex";
    dropdown.style.display = isVisible ? "none" : "flex";
});