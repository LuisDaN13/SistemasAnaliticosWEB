document.getElementById("contact-form").addEventListener("submit", function (e) {
    e.preventDefault();

    const form = e.target;
    const submitButton = document.getElementById("submit-button");
    const buttonText = document.getElementById("button-text");
    const submitSpinner = document.getElementById("submit-spinner");

    submitButton.disabled = true;
    buttonText.style.display = "none";
    submitSpinner.style.display = "block";

    fetch(form.action, {
        method: "POST",
        body: new FormData(form)
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    icon: "success",
                    title: "Mensaje enviado",
                    text: "Tu mensaje ha sido enviado correctamente.",
                    confirmButtonColor: "#3085d6"
                });

                form.reset();
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Hubo un problema al enviar el mensaje: " + data.error,
                    confirmButtonColor: "#d33"
                });
            }
        })
        .catch(() => {
            Swal.fire({
                icon: "error",
                title: "Error inesperado",
                text: "No se pudo conectar con el servidor.",
                confirmButtonColor: "#d33"
            });
        })
        .finally(() => {
            submitButton.disabled = false;
            buttonText.style.display = "inline";
            submitSpinner.style.display = "none";
        });
});