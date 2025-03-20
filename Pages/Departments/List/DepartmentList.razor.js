
export function preventEnterKey(id) {

    let formObj = document.getElementById(id);
    formObj.addEventListener('keypress', function (event) {

        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    })
}
