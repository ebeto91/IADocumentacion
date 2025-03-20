export function addAnimation() {
    let div = document.getElementById(`principal-div-notification-icon`);
    div.classList.add("horizontal-shake");
}

export function removeAnimation() {
    let div = document.getElementById(`principal-div-notification-icon`);
    div.classList.remove("horizontal-shake");
}

