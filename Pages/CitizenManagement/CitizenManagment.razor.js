export function DeleteImage(id) {
    let e = document.getElementById(`container-${id}`);
    if (e) {
        e.remove();
    }
}

export function MapDraw(lat, lng, icon) {

    var map = window.map; // Assuming you have already initialized the map
    var leafletIcon = L.icon({
        iconUrl: icon.IconUrl,
        iconSize: [icon.IconSize.X, icon.IconSize.Y],
        iconAnchor: [icon.IconAnchor.X, icon.IconAnchor.Y]
    });

    L.marker([lat, lng], { icon: leafletIcon }).addTo(map);

}
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

/*document.getElementById('modalMap').addEventListener('shown.bs.modal', function () {
    map.invalidateSize();
});*/

//document.getElementById('modalMap').addEventListener('shown.bs.modal', function () {
//    let modal = document.getElementById("modalMap");
//    modal.removeAttribute("aria-hidden");
//    map.invalidateSize();
//});

//document.getElementById('modalMap').addEventListener("hidden.bs.modal", function () {
//    let modal = document.getElementById("modalMap");
//    modal.setAttribute("aria-hidden", "true");
//});
