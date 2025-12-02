// Validación y envío del formulario
document.addEventListener('DOMContentLoaded', function () {
    const contactForm = document.getElementById('contact-form');
    const submitButton = document.getElementById('submit-button');
    const buttonText = document.getElementById('button-text');
    const submitSpinner = document.getElementById('submit-spinner');
    const successMessage = document.getElementById('success-message');
    const errorMessage = document.getElementById('error-message');

    contactForm.addEventListener('submit', function (e) {
        e.preventDefault();

        // Validar formulario
        if (!contactForm.checkValidity()) {
            e.stopPropagation();
            contactForm.classList.add('was-validated');
            return;
        }

        // Mostrar spinner y cambiar texto del botón
        buttonText.textContent = 'Enviando...';
        submitSpinner.style.display = 'inline-block';
        submitButton.disabled = true;

        // Ocultar mensajes anteriores
        successMessage.classList.add('d-none');
        errorMessage.classList.add('d-none');

        // Simular envío del formulario
        setTimeout(function () {
            const success = Math.random() > 0.1;

            if (success) {
                successMessage.classList.remove('d-none');
                contactForm.reset();
                contactForm.classList.remove('was-validated');
            } else {
                errorMessage.classList.remove('d-none');
            }

            // Restaurar botón
            buttonText.textContent = 'Enviar';
            submitSpinner.style.display = 'none';
            submitButton.disabled = false;
        }, 2000);
    });

    // Validación en tiempo real
    const formInputs = contactForm.querySelectorAll('input, textarea, select');
    formInputs.forEach(input => {
        input.addEventListener('input', function () {
            if (this.checkValidity()) {
                this.classList.remove('is-invalid');
                this.classList.add('is-valid');
            } else {
                this.classList.remove('is-valid');
                this.classList.add('is-invalid');
            }
        });

        input.addEventListener('blur', function () {
            if (!this.checkValidity()) {
                this.classList.add('is-invalid');
            }
        });
    });

    // Validación del correo electrónico
    const emailInput = document.getElementById('email');
    emailInput.addEventListener('input', function () {
        const emailRegex = new RegExp("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
        if (!emailRegex.test(this.value)) {
            this.setCustomValidity('Por favor ingresa un correo electrónico válido');
        } else {
            this.setCustomValidity('');
        }
    });

    // Ajustar altura del mapa según pantalla
    function adjustMapHeight() {
        const mapContainer = document.querySelector('.map-container');
        if (mapContainer) {
            if (window.innerWidth >= 1200) {
                mapContainer.style.height = '500px';
            } else if (window.innerWidth >= 992) {
                mapContainer.style.height = '450px';
            } else if (window.innerWidth >= 768) {
                mapContainer.style.height = '400px';
            } else if (window.innerWidth >= 576) {
                mapContainer.style.height = '350px';
            } else {
                mapContainer.style.height = '300px';
            }
        }
    }

    // Ajustar padding del map-wrapper
    function adjustMapPadding() {
        const mapWrapper = document.querySelector('.map-wrapper');
        if (mapWrapper) {
            if (window.innerWidth >= 1200) {
                mapWrapper.style.padding = '0 2rem';
            } else if (window.innerWidth >= 768) {
                mapWrapper.style.padding = '0 1rem';
            } else {
                mapWrapper.style.padding = '0 0.5rem';
            }
        }
    }

    // Ejecutar al cargar
    adjustMapHeight();
    adjustMapPadding();

    // Ejecutar al redimensionar
    window.addEventListener('resize', function () {
        adjustMapHeight();
        adjustMapPadding();
    });
});
