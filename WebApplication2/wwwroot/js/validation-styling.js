// validation-styling.js

$(document).ready(function () {

    // 🔹 On form submit
    $("form").on("submit", function () {

        var form = $(this);

        // trigger validation
        if (!form.valid()) {
            applyValidationStyles(form);
            return false;
        }

        applyValidationStyles(form);
        return true;
    });

    // 🔹 On input change (live validation)
    $("input, select, textarea").on("keyup change", function () {
        var input = $(this);

        if (input.valid()) {
            input.removeClass("is-invalid").addClass("is-valid");
        } else {
            input.removeClass("is-valid").addClass("is-invalid");
        }
    });

    // 🔹 Function to apply styles
    function applyValidationStyles(form) {
        form.find("input, select, textarea").each(function () {
            var input = $(this);

            if (input.valid()) {
                input.removeClass("is-invalid").addClass("is-valid");
            } else {
                input.removeClass("is-valid").addClass("is-invalid");
            }
        });
    }

});