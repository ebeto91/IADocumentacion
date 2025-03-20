
var map;
var marker;
export function Init(latitude, longitude, component) {
    setTimeout(() => {
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

        map.invalidateSize()
    }, "1000");

}

