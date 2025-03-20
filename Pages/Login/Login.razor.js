export function CheckBoxRemenber() {
    debugger
    let chck = document.getElementById('checkbox1')
    let email = document.getElementById('email')
    let contrasena = document.getElementById('contrasena')

    if (email.value == '' || contrasena.value == '') {
        chck.checked = false;

        if (chck.checked == false) {
            if (localStorage.getItem("rememberApp")) {
                localStorage.removeItem("rememberApp");
            }
        }
    }
    else if (chck.checked) {
        const myObject = {
            EmailAddress: email.value,
            Password: contrasena.value
         
        };
        localStorage.setItem("rememberApp", JSON.stringify(myObject));
    } else {
        if (localStorage.getItem("rememberApp")) {
            localStorage.removeItem("rememberApp");
        }
    }
}

export function ExistCheck() {
    let chck = document.getElementById('checkbox1')
    if (localStorage.getItem("rememberApp")) {
        chck.checked = true;
    } else {
        chck.checked = false;
    }
}
