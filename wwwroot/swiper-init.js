function initializeSwiper() {
    new Swiper('.swiper-container', {
        loop: false,
        autoplay: {
            delay: 3000, // Automatic sliding delay (in milliseconds)
            disableOnInteraction: false,
        },
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
    });
}