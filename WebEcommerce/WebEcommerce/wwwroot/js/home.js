document.addEventListener('scroll', function () {
    let scrolled = window.pageYOffset;
    let parallaxSpeed = 0.25;

    document.body.style.backgroundPosition = 'center ' + (scrolled * parallaxSpeed) + 'px';
});