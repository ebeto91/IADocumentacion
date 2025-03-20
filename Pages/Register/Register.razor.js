export function checkLengthAndTrim() {

    const selectElement = document.getElementById('tipoCedula_input');

    // Add an event listener for change event
    selectElement.addEventListener('change', function (event) {
        var input = document.getElementById("cedula");
        input.value = '';
      
    });
    

    
    

    
}