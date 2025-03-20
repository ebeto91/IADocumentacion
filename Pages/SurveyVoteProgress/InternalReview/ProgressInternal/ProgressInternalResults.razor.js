function setProgress(id, percent) {
    const circle = document.getElementById(id);
    const text = document.getElementById(`text_${id}`);
    const radius = 45;
    const circumference = 2 * Math.PI * radius;
    const offset = circumference - (percent / 100) * circumference;

    circle.style.strokeDashoffset = offset;
    text.textContent = percent + "%";
}

function animateProgress(id, target, speed = 20) {
    let progress = 0;
    const interval = setInterval(() => {
        let number = target.replace(",", ".")
        let parseNumber = Math.round(Number.parseFloat(number))
        if (progress >= parseNumber) {
            clearInterval(interval);
        } else {
            progress++;
            setProgress(id, progress);
        }
    }, speed);
}

//Main method
window.highlightElement = () => {
    GetCircles();
};

export function GetCircles() {
    let elements = document.querySelectorAll(".circle")
    if (elements.length > 0) {
        elements.forEach(e => {
            let id = e.id;
            let Target = id.split("_");
            let porcentage = Target[2];
            animateProgress(id, porcentage);
        })
    }
}



