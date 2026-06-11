// Validación y envío del formulario - VERSIÓN CORREGIDA
document.addEventListener('DOMContentLoaded', function () {
    const contactForm = document.getElementById('contact-form');
    const submitButton = document.getElementById('submit-button');
    const buttonText = document.getElementById('button-text');
    const submitSpinner = document.getElementById('submit-spinner');

    // Estos elementos pueden no existir
    const successMessage = document.getElementById('success-message');
    const errorMessage = document.getElementById('error-message');

    contactForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Validar formulario
        if (!contactForm.checkValidity()) {
            e.stopPropagation();
            contactForm.classList.add('was-validated');
            return;
        }

        // Mostrar spinner
        buttonText.textContent = 'Enviando...';
        submitSpinner.style.display = 'inline-block';
        submitButton.disabled = true;

        // Ocultar mensajes anteriores (si existen)
        if (successMessage) successMessage.classList.add('d-none');
        if (errorMessage) errorMessage.classList.add('d-none');

        try {
            // Envío real al servidor
            const formData = new FormData(contactForm);

            const response = await fetch(contactForm.action, {
                method: 'POST',
                body: formData
            });

            const result = await response.json();

            if (result.success) {
                // Éxito
                if (successMessage) {
                    successMessage.classList.remove('d-none');
                    successMessage.textContent = 'Mensaje enviado correctamente. Te contactaremos pronto.';
                } else if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'success',
                        title: '¡Éxito!',
                        text: 'Mensaje enviado correctamente. Te contactaremos pronto.',
                        confirmButtonColor: '#3085d6'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
                    });
                } else {
                    alert('Mensaje enviado correctamente. Te contactaremos pronto.');
                    location.reload();
                }

                // Resetear formulario
                contactForm.reset();
                contactForm.classList.remove('was-validated');

                // Limpiar validaciones
                const inputs = contactForm.querySelectorAll('input, textarea');
                inputs.forEach(input => {
                    input.classList.remove('is-valid', 'is-invalid');
                });
            } else {
                // Error del servidor
                if (errorMessage) {
                    errorMessage.classList.remove('d-none');
                    errorMessage.textContent = result.error || 'Error al enviar el mensaje';
                } else if (typeof Swal !== 'undefined') {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: result.error || 'Error al enviar el mensaje',
                        confirmButtonColor: '#d33'
                    });
                } else {
                    alert('Error: ' + (result.error || 'No se pudo enviar el mensaje'));
                }
            }
        } catch (error) {
            console.error('Error:', error);

            // Error de conexión
            if (errorMessage) {
                errorMessage.classList.remove('d-none');
                errorMessage.textContent = 'Error de conexión. Verifica tu internet.';
            } else if (typeof Swal !== 'undefined') {
                Swal.fire({
                    icon: 'error',
                    title: 'Error de conexión',
                    text: 'No se pudo conectar con el servidor. Verifica tu internet.',
                    confirmButtonColor: '#d33'
                });
            } else {
                alert('Error de conexión. Verifica tu internet.');
            }
        } finally {
            // Restaurar botón
            buttonText.textContent = 'Enviar';
            submitSpinner.style.display = 'none';
            submitButton.disabled = false;
        }
    });

    // Validación en tiempo real
    const formInputs = contactForm.querySelectorAll('input, textarea');
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
    if (emailInput) {
        emailInput.addEventListener('input', function () {
            const emailRegex = new RegExp("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
            if (!emailRegex.test(this.value)) {
                this.setCustomValidity('Por favor ingresa un correo electrónico válido');
            } else {
                this.setCustomValidity('');
            }
        });
    }

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