export function changeSizeColumn(columnId) {
    let icon = document.getElementById(`${columnId}-changeSizeColumn`);


    let columna = document.getElementById(`${columnId}`);
    let list = document.getElementById(`${columnId}-list`);
    let header = document.getElementById(`${columnId}-header-list`);
    let tittle = document.getElementById(`${columnId}-header-tittle`);

    if (icon.classList.contains("bi-arrow-left")) {

        icon.classList.add("bi-arrow-right");
        icon.classList.remove("bi-arrow-left");

        //aplicar clase de card-kanban-size-min a columnId
        columna.classList.add("card-kanban-size-min");

        //aplicar clase de d-none a columnId-list
        list.classList.add("d-none");

        // aplicar clase card-header-kanban-min a init-header-list
        header.classList.add("card-header-kanban-min");
        header.classList.remove("text-header-align-center");

        // aplicar clase card-header-kanban-min-position a init-header-tittle
        tittle.classList.add("card-header-kanban-min-position");
    } else {

        icon.classList.remove("bi-arrow-right");
        icon.classList.add("bi-arrow-left");

        //aplicar clase de card-kanban-size-min a columnId
        columna.classList.remove("card-kanban-size-min");

        //aplicar clase de d-none a columnId-list
        list.classList.remove("d-none");

        // aplicar clase card-header-kanban-min a init-header-list
        header.classList.remove("card-header-kanban-min");
        header.classList.add("text-header-align-center");

        // aplicar clase card-header-kanban-min-position a init-header-tittle
        tittle.classList.remove("card-header-kanban-min-position");
    }

   
}

export function alertUser() {
    alert('The button was selected!');
}

export function addHandlers() {
    const btn = document.getElementById("btn");
    btn.addEventListener("click", alertUser);
}