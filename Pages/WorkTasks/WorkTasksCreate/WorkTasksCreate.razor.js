var map;
var marker;
export function Init(latitude, longitude, component) {
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

    map.on("click", (event) => {
        map.removeLayer(marker)
        marker = L.marker([event.latlng.lat, event.latlng.lng]).addTo(map);


        var latitude = parse(event.latlng.lat)
        var longitude = parse(event.latlng.lng)

        component.invokeMethodAsync("returnOnClickCoordinates", latitude.toString(), longitude.toString())
    })
}
export function AddedMark(latitude, longitude) {

    map.removeLayer(marker)
    marker = L.marker([latitude, longitude]).addTo(map);

    map.setView({ lat: latitude, lng: longitude }, map.getZoom(), { animation: true })
}

function parse(x) {
    return Number.parseFloat(x).toFixed(6);
}
