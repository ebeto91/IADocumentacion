document.addEventListener("DOMContentLoaded", function () {
    toggleMenu();
    console.log('teststest')

    window.addEventListener('resize', function () {
        if (window.innerWidth >= 991) {

            let e = document.querySelector('.menu-open');
            if (e != null) {
                e.classList.remove('menu-open')
            }
        }
    });
}); 

function toggleMenu() {
    const sidebar = document.getElementById('sidebar');
    if (sidebar != null) {
        sidebar.classList.toggle('menu-open');
    }
}