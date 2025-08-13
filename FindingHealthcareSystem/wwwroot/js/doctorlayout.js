

const navLinks = document.querySelectorAll('.nav-link');
navLinks.forEach(link => {
    link.addEventListener('click', function () {
        navLinks.forEach(item => {
            item.classList.remove('active');
        })
        link.classList.add('active');
    })
})

document.addEventListener('DOMContentLoaded', function () {
    const patientPart = $('#patientPart');
    const patientScrollTo = $('#patientRecords');
    const body = $("html, body");
    patientPart.on('click', function () {
        $(body).animate({
            scrollTop: $(patientScrollTo).offset().top
        }, 500);
    })
})




