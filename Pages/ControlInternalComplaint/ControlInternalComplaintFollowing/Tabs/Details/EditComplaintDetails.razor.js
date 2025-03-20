var map;
var marker;
export function Init(latitude, longitude) {
    map = L.map('map').setView([latitude, longitude], 15);
    map.options.minZoom = 9
    map.options.maxZoom = 19
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    map.whenReady(() => {

        marker = L.marker([latitude, longitude]).addTo(map);
    });

}
/*document.getElementById('modalMap').addEventListener('shown.bs.modal', function () {
    map.invalidateSize();
});*/

document.getElementById('modalMap').addEventListener('shown.bs.modal', function () {
    let modal = document.getElementById("modalMap");
    modal.removeAttribute("aria-hidden");
    map.invalidateSize();
});

document.getElementById('modalMap').addEventListener("hidden.bs.modal", function () {
    let modal = document.getElementById("modalMap");
    modal.setAttribute("aria-hidden", "true");
});